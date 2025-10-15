using StartEquity.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace StartEquity.Services
{
    public class DbSeeder
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbSeeder(AppDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Seed()
        {
            CreateRoles();
            CreateCategories();
            CreateSampleData();
        }

        


        

        

        private void CreateRoles()
        {
            string[] roles = { "Owner", "Investor" };
            
            foreach (var role in roles)
            {
                if (!_roleManager.RoleExistsAsync(role).Result)
                {
                    _roleManager.CreateAsync(new IdentityRole(role)).Wait();
                }
            }
        }


        private void CreateCategories()
        {
            if (!_context.Categories.Any())
            {
                var categories = new[]
                {
                    new Category { Id = System.Guid.NewGuid().ToString(), Name = "EdTech", Description = "Educational Technology" },
                    new Category { Id = System.Guid.NewGuid().ToString(), Name = "FinTech", Description = "Financial Technology" },
                    new Category { Id = System.Guid.NewGuid().ToString(), Name = "Aerospace", Description = "Aerospace and Defense" },
                    new Category { Id = System.Guid.NewGuid().ToString(), Name = "Eyewear", Description = "Eyewear and Vision Care" },
                    new Category { Id = System.Guid.NewGuid().ToString(), Name = "Clothing", Description = "Fashion and Clothing Brands" },
                    new Category { Id = System.Guid.NewGuid().ToString(), Name = "HealthTech", Description = "Healthcare Technology" },
                    new Category { Id = System.Guid.NewGuid().ToString(), Name = "CleanTech", Description = "Clean Technology" }
                };

                foreach (var category in categories)
                {
                    _context.Categories.Add(category);
                }
                
                _context.SaveChanges();
            }
        }

        private void CreateSampleData()
        {
            if (!_context.Companies.Any())
            {
                var categories = _context.Categories.ToList();
                
                var ownerEmail = "owner@startequity.com";
                var ownerUser = _userManager.FindByEmailAsync(ownerEmail).Result;
                
                if (ownerUser == null)
                {
                    ownerUser = new AppUser
                    {
                        UserName = ownerEmail,
                        Email = ownerEmail,
                        FullName = "John Startup",
                        Role = "Owner"
                    };
                    
                    _userManager.CreateAsync(ownerUser, "Owner123!").Wait();
                    _userManager.AddToRoleAsync(ownerUser, "Owner").Wait();
                }

                _context.SaveChanges();
            }

            if (!_context.Investors.Any())
            {
                var investorEmail = "investor@startequity.com";
                var investorUser = _userManager.FindByEmailAsync(investorEmail).Result;
                
                if (investorUser == null)
                {
                    investorUser = new AppUser
                    {
                        UserName = investorEmail,
                        Email = investorEmail,
                        FullName = "Jane Investor",
                        Role = "Investor"
                    };
                    
                    _userManager.CreateAsync(investorUser, "Investor123!").Wait();
                    _userManager.AddToRoleAsync(investorUser, "Investor").Wait();
                }

                var investor = new Investor
                {
                    Id = System.Guid.NewGuid().ToString(),
                    UserId = investorUser.Id,
                    Balance = 10000m 
                };

                _context.Investors.Add(investor);
                _context.SaveChanges();
            }
        }
    }
}
