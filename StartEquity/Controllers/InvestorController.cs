using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StartEquity.Models;
using StartEquity.Repositories;
using StartEquity.Services;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace StartEquity.Controllers
{
    [Authorize(Roles = "Investor")]
    public class InvestorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public InvestorController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var investor = _unitOfWork.Investors.GetByUserId(userId);
            
            if (investor == null)
            {
                investor = new Investor
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    Balance = 10000, 
                    CreatedAt = DateTime.UtcNow
                };
                _unitOfWork.Investors.Insert(investor);
                _unitOfWork.SaveChanges();
            }

            var categories = _unitOfWork.Categories.GetAll();
            var companies = _unitOfWork.Companies.GetAll();
            
            ViewBag.Categories = categories;
            ViewBag.Companies = companies;
            ViewBag.Investor = investor;
            
            return View();
        }

        [HttpGet]
        public IActionResult Category(string categoryId)
        {
            var companies = _unitOfWork.Companies.GetByCategory(categoryId);
            var category = _unitOfWork.Categories.GetById(categoryId);
            
            ViewBag.Category = category;
            ViewBag.Companies = companies;
            
            return View();
        }

        [HttpPost]
        public IActionResult Invest(string companyId, decimal amount)
        {
            var userId = _userManager.GetUserId(User);
            var investor = _unitOfWork.Investors.GetByUserId(userId);
            var company = _unitOfWork.Companies.GetById(companyId);
            
            if (investor == null || company == null)
            {
                TempData["Error"] = "Invalid investor or company.";
                return RedirectToAction("Index");
            }

            if (amount <= 0)
            {
                TempData["Error"] = "Investment amount must be greater than zero.";
                return RedirectToAction("Index");
            }

            if (!(company.CanRaiseFunds && company.CurrentRound != null))
            {
                TempData["Error"] = "This company is not open for funding right now.";
                return RedirectToAction("Index");
            }

            if (investor.Balance < amount)
            {
                TempData["Error"] = "Insufficient balance to make this investment.";
                return RedirectToAction("Index");
            }

            if (investor.Balance >= amount && company.CanRaiseFunds && company.CurrentRound != null)
            {
                var remainingAmount = company.CurrentRound.TargetAmount - company.CurrentRound.RaisedAmount;
                if (amount > remainingAmount)
                {
                    TempData["Error"] = "Investment amount cannot be greater than the remaining amount to be raised in this round.";
                    return RedirectToAction("Index");
                }

                var equityPercent = (amount / company.CurrentRound.ValuationAtRound) * 100;
                
                var investment = new Investment
                {
                    Id = Guid.NewGuid().ToString(),
                    InvestorId = investor.Id,
                    CompanyId = companyId,
                    RoundId = company.CurrentRoundId,
                    Amount = amount,
                    EquityPercent = equityPercent,
                    InvestmentDate = DateTime.UtcNow
                };

                investor.Balance -= amount;
                company.TotalRaised += amount;
                company.CurrentRound.RaisedAmount += amount;

                _unitOfWork.Investments.Insert(investment);
                _unitOfWork.Investors.Update(investor);
                _unitOfWork.Companies.Update(company);
                _unitOfWork.SaveChanges();

                TempData["Success"] = "Investment placed successfully.";
            }
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Transfers()
        {
            var userId = _userManager.GetUserId(User);
            var investor = _unitOfWork.Investors.GetByUserId(userId);
            
            var transfers = _unitOfWork.Transfers.GetByInvestorId(investor.Id);
            
            ViewBag.Transfers = transfers;
            ViewBag.Investor = investor;
            
            return View();
        }

        [HttpPost]
        public IActionResult CreateTransfer(string toInvestorEmail, decimal equityPercent, decimal price)
        {
            var userId = _userManager.GetUserId(User);
            var fromInvestor = _unitOfWork.Investors.GetByUserId(userId);
            
            var toInvestor = _unitOfWork.Investors.GetByEmail(toInvestorEmail);
            if (toInvestor == null)
            {
                ModelState.AddModelError("", "Investor with that email not found.");
                return RedirectToAction("Transfers");
            }
            
            var transfer = new Transfer
            {
                Id = Guid.NewGuid().ToString(),
                FromInvestorId = fromInvestor.Id,
                ToInvestorId = toInvestor.Id,
                EquityPercent = equityPercent,
                Price = price,
                TransferDate = DateTime.UtcNow,
                Status = "Pending"
            };

            _unitOfWork.Transfers.Insert(transfer);
            _unitOfWork.SaveChanges();
            
            return RedirectToAction("Transfers");
        }

        public IActionResult Portfolio()
        {
            var userId = _userManager.GetUserId(User);
            var investor = _unitOfWork.Investors.GetByUserId(userId);
            
            if (investor == null)
            {
                return RedirectToAction("Index");
            }

            var investments = _unitOfWork.Investments.GetByInvestorId(investor.Id);
            
            ViewBag.Investments = investments;
            ViewBag.Investor = investor;
            
            return View();
        }
    }
}
