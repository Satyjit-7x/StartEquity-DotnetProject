using StartEquity.Models;
using Microsoft.AspNetCore.Identity;

namespace StartEquity.Services
{
    public interface IAuthService
    {
        AppUser Register(string email, string password, string fullName, string role);
        SignInResult Login(string email, string password, bool rememberMe);
        void Logout();
        AppUser GetCurrentUser();
        bool IsInRole(string role);
        Investor CreateInvestorProfile(string userId);
        Company CreateCompanyProfile(string userId, string companyName, string description, string categoryId);
        Investor GetInvestorByEmail(string email);
    }
}
