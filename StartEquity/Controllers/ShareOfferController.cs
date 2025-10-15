using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using StartEquity.Models;
using StartEquity.Repositories;

namespace StartEquity.Controllers
{
    [Authorize(Roles = "Investor")]
    public class ShareOfferController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public ShareOfferController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var availableOffers = _unitOfWork.ShareOffers.GetAvailableOffers();
            var companies = _unitOfWork.Companies.GetAll();

            var userId = _userManager.GetUserId(User);
            var me = _unitOfWork.Investors.GetByUserId(userId);
            if (me != null)
            {
                availableOffers = availableOffers.Where(o => o.SellerId != me.Id).ToList();
            }
            
            ViewBag.AvailableOffers = availableOffers;
            ViewBag.Companies = companies;
            
            return View();
        }

        public IActionResult Details(string id)
        {
            var offer = _unitOfWork.ShareOffers.GetById(id);
            if (offer == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var buyer = _unitOfWork.Investors.GetByUserId(userId);
            
            ViewBag.Offer = offer;
            ViewBag.Buyer = buyer;
            ViewBag.IsSeller = buyer != null && offer.SellerId == buyer.Id;
            
            return View();
        }

        [HttpGet]
        public IActionResult Create(string investmentId)
        {
            var investment = _unitOfWork.Investments.GetById(investmentId);
            if (investment == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var seller = _unitOfWork.Investors.GetByUserId(userId);
            
            if (investment.InvestorId != seller.Id)
            {
                return Forbid();
            }

            ViewBag.Investment = investment;
            ViewBag.Seller = seller;
            
            return View();
        }

        [HttpPost]
        public IActionResult Create(string investmentId, decimal sharePercentage, decimal askingPrice, string description = "")
        {
            var investment = _unitOfWork.Investments.GetById(investmentId);
            if (investment == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var seller = _unitOfWork.Investors.GetByUserId(userId);
            
            if (investment.InvestorId != seller.Id)
            {
                return Forbid();
            }

            if (sharePercentage <= 0 || sharePercentage > investment.EquityPercent)
            {
                ModelState.AddModelError("", "Invalid share percentage. You can only sell up to your total equity percentage.");
                ViewBag.Investment = investment;
                ViewBag.Seller = seller;
                return View();
            }

            var company = _unitOfWork.Companies.GetById(investment.CompanyId);
          
            var currentMarketPrice = (company.CurrentRound?.ValuationAtRound ?? 0) / 100m;
            var totalValue = askingPrice * sharePercentage;

            var shareOffer = new ShareOffer
            {
                Id = Guid.NewGuid().ToString(),
                CompanyId = investment.CompanyId,
                SellerId = seller.Id,
                InvestmentId = investment.Id,
                SharePercentage = sharePercentage,
                AskingPrice = askingPrice,
                CurrentMarketPrice = currentMarketPrice,
                TotalValue = totalValue,
                Description = description,
                Status = "Available",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _unitOfWork.ShareOffers.Insert(shareOffer);
            _unitOfWork.SaveChanges();

            return RedirectToAction("MyOffers");
        }

        [HttpPost]
        public IActionResult Buy(string offerId)
        {
            var offer = _unitOfWork.ShareOffers.GetById(offerId);
            if (offer == null || offer.Status != "Available")
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            var buyer = _unitOfWork.Investors.GetByUserId(userId);
            var seller = _unitOfWork.Investors.GetById(offer.SellerId);
            
            if (buyer.Id == seller.Id)
            {
                ModelState.AddModelError("", "You cannot buy your own shares.");
                return RedirectToAction("Details", new { id = offerId });
            }

            if (buyer.Balance < offer.TotalValue)
            {
                ModelState.AddModelError("", "Insufficient balance to complete this purchase.");
                return RedirectToAction("Details", new { id = offerId });
            }

            offer.Status = "Sold";
            offer.BuyerId = buyer.Id;
            offer.SoldAt = DateTime.UtcNow;
            offer.IsActive = false;

            buyer.Balance -= offer.TotalValue;
            seller.Balance += offer.TotalValue;


            var buyerInvestment = new Investment
            {
                Id = Guid.NewGuid().ToString(),
                CompanyId = offer.CompanyId,
                InvestorId = buyer.Id,
                RoundId = offer.Investment.RoundId,
                Amount = offer.TotalValue,
                EquityPercent = offer.SharePercentage,
                InvestmentDate = DateTime.UtcNow,
                IsSecondary = true
            };

            var sellerInvestment = _unitOfWork.Investments.GetById(offer.InvestmentId);
            sellerInvestment.EquityPercent -= offer.SharePercentage;

            var transfer = new Transfer
            {
                Id = Guid.NewGuid().ToString(),
                CompanyId = offer.CompanyId,
                FromInvestorId = seller.Id,
                ToInvestorId = buyer.Id,
                EquityPercent = offer.SharePercentage,
                Price = offer.TotalValue,
                Status = "Completed",
                Approved = true,
                ApprovedAt = DateTime.UtcNow,
                TransferDate = DateTime.UtcNow,
                ShareOfferId = offer.Id
            };

            _unitOfWork.ShareOffers.Update(offer);
            _unitOfWork.Investors.Update(buyer);
            _unitOfWork.Investors.Update(seller);
            _unitOfWork.Investments.Insert(buyerInvestment);
            _unitOfWork.Investments.Update(sellerInvestment);
            _unitOfWork.Transfers.Insert(transfer);
            _unitOfWork.SaveChanges();

            return RedirectToAction("MyPurchases");
        }

        public IActionResult MyOffers()
        {
            var userId = _userManager.GetUserId(User);
            var investor = _unitOfWork.Investors.GetByUserId(userId);
            var myOffers = _unitOfWork.ShareOffers.GetBySellerId(investor.Id);
            
            ViewBag.MyOffers = myOffers;
            ViewBag.Investor = investor;
            
            return View();
        }

        public IActionResult MyPurchases()
        {
            var userId = _userManager.GetUserId(User);
            var investor = _unitOfWork.Investors.GetByUserId(userId);
            var myPurchases = _unitOfWork.ShareOffers.GetByBuyerId(investor.Id);
            
            ViewBag.MyPurchases = myPurchases;
            ViewBag.Investor = investor;
            
            return View();
        }

        [HttpPost]
        public IActionResult Cancel(string offerId)
        {
            var userId = _userManager.GetUserId(User);
            var investor = _unitOfWork.Investors.GetByUserId(userId);
            var offer = _unitOfWork.ShareOffers.GetById(offerId);
            
            if (offer == null || offer.SellerId != investor.Id || offer.Status != "Available")
            {
                return NotFound();
            }

            offer.Status = "Cancelled";
            offer.IsActive = false;
            
            _unitOfWork.ShareOffers.Update(offer);
            _unitOfWork.SaveChanges();

            return RedirectToAction("MyOffers");
        }
    }
}
