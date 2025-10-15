using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StartEquity.Models;
using StartEquity.Repositories;
using StartEquity.Services;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace StartEquity.Controllers
{
    [Authorize(Roles = "Owner")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public CompanyController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var companies = _unitOfWork.Companies.GetByOwnerId(userId);
            
            ViewBag.Companies = companies;
            
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            var categories = _unitOfWork.Categories.GetAll();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Company company)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                company.OwnerId = userId;
                company.Id = Guid.NewGuid().ToString();
                
                _unitOfWork.Companies.Insert(company);
                _unitOfWork.SaveChanges();
                
                return RedirectToAction("Index");
            }
            
            var categories = _unitOfWork.Categories.GetAll();
            ViewBag.Categories = categories;
            return View(company);
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            var company = _unitOfWork.Companies.GetById(id);
            if (company == null) return NotFound();
            
            var investments = _unitOfWork.Investments.GetByCompanyId(id);
            
            ViewBag.Investments = investments;
            
            return View(company);
        }

        [HttpGet]
        public IActionResult StartRound(string companyId)
        {
            var company = _unitOfWork.Companies.GetById(companyId);
            if (company == null) return NotFound();
            
            var totalOfferedSoFar = company.Rounds.Sum(r => r.EquityTargetPercent);
            var remainingEquity = 100m - totalOfferedSoFar;
            if (remainingEquity < 0m) remainingEquity = 0m;
            ViewBag.RemainingEquity = remainingEquity;
            ViewBag.OwnerOwnership = remainingEquity;

            return View(company);
        }

        [HttpPost]
        public IActionResult StartRound(string companyId, decimal valuation, decimal equityTarget)
        {
            var company = _unitOfWork.Companies.GetById(companyId);
            if (company == null) return NotFound();
            
            if (company.CurrentRound != null && company.CurrentRound.RaisedAmount < company.CurrentRound.ValuationAtRound * (company.CurrentRound.EquityTargetPercent / 100))
            {
                ModelState.AddModelError("", "Current round must be fully funded before starting a new round.");
                return View(company);
            }

            var totalOfferedSoFar = company.Rounds.Sum(r => r.EquityTargetPercent);
            var remainingEquity = 100m - totalOfferedSoFar;
            if (remainingEquity < 0m) remainingEquity = 0m;

            if (equityTarget > remainingEquity)
            {
                ModelState.AddModelError("", $"Equity target cannot exceed remaining available equity of {remainingEquity:F2}%.");
                return View(company);
            }
            
            var round = new Round
            {
                Id = Guid.NewGuid().ToString(),
                CompanyId = companyId,
                SequenceNumber = company.Rounds.Count + 1,
                ValuationAtRound = valuation,
                EquityTargetPercent = equityTarget,
                RaisedAmount = 0,
                StartDate = DateTime.UtcNow,
                Status = "Open"
            };
            
            company.CurrentRoundId = round.Id;
            company.CurrentRound = round;
            
            _unitOfWork.Rounds.Insert(round);
            _unitOfWork.Companies.Update(company);
            _unitOfWork.SaveChanges();
            
            return RedirectToAction("Details", new { id = companyId });
        }
    }
}
