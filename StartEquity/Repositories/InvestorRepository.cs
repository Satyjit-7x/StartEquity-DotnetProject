using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StartEquity.Models;

namespace StartEquity.Repositories
{
    public class InvestorRepository : IInvestorRepository
    {
        private readonly AppDbContext _context;

        public InvestorRepository(AppDbContext context)
        {
            _context = context;
        }

        

        public void Update(Investor i) => _context.Investors.Update(i);

        public void Delete(Investor i) => _context.Investors.Remove(i);
        
        public List<Investor> GetAll() => 
            _context.Investors
                .Include(i => i.User)
                .ToList();

        public Investor GetById(string id) => 
            _context.Investors
                .Include(i => i.User)
                .FirstOrDefault(i => i.Id == id);

        public Investor GetByUserId(string userId) =>
            _context.Investors
                .Include(i => i.User)
                .FirstOrDefault(i => i.UserId == userId);

        public Investor GetByEmail(string email) =>
            _context.Investors
                .Include(i => i.User)
                .FirstOrDefault(i => i.User.Email == email);

        public void Insert(Investor i)
        {
            if (string.IsNullOrEmpty(i.Id))
                i.Id = Guid.NewGuid().ToString();
            _context.Investors.Add(i);
        }
    }
}
