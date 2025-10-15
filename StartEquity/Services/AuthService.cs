using StartEquity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace StartEquity.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;

        public AuthService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IHttpContextAccessor httpContextAccessor,
            AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public AppUser Register(string email, string password, string fullName, string role)
        {
            var user = new AppUser
            {
                UserName = email,
                Email = email,
                FullName = fullName,
                Role = role
            };

            var result = _userManager.CreateAsync(user, password).Result;
            if (result.Succeeded)
            {
                _userManager.AddToRoleAsync(user, role).Wait();
                return user;
            }
            return null;
        }

        public SignInResult Login(string email, string password, bool rememberMe)
        {
            return _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false).Result;
        }

        public void Logout()
        {
            _signInManager.SignOutAsync().Wait();
        }

        public AppUser GetCurrentUser()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return null;
            return _userManager.FindByIdAsync(userId).Result;
        }

        public bool IsInRole(string role)
        {
            var user = GetCurrentUser();
            if (user == null) return false;
            return _userManager.IsInRoleAsync(user, role).Result;
        }

        public Investor CreateInvestorProfile(string userId)
        {
            var investor = new Investor
            {
                Id = System.Guid.NewGuid().ToString(),
                UserId = userId,
                Balance = 10000m
            };

            _context.Investors.Add(investor);
            _context.SaveChanges();
            return investor;
        }

        public Company CreateCompanyProfile(string userId, string companyName, string description, string categoryId)
        {
            var company = new Company
            {
                Id = System.Guid.NewGuid().ToString(),
                Name = companyName,
                Description = description,
                CategoryId = categoryId,
                OwnerId = userId,
                IsActive = true,
                CanRaiseFunds = true
            };

            _context.Companies.Add(company);
            _context.SaveChanges();
            return company;
        }

        public Investor GetInvestorByEmail(string email)
        {
            var user = _userManager.FindByEmailAsync(email).Result;
            if (user == null) return null;
            
            return _context.Investors
                .Include(i => i.User)
                .FirstOrDefault(i => i.UserId == user.Id);
        }
    }
}
