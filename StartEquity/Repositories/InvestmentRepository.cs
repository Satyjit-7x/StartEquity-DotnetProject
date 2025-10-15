using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StartEquity.Models;

namespace StartEquity.Repositories
{
    public class InvestmentRepository : IInvestmentRepository
    {
        private readonly AppDbContext _context;

        public InvestmentRepository(AppDbContext context)
        {
            _context = context;
        }

        

        public void Update(Investment inv) => _context.Investments.Update(inv);

        public void Delete(Investment inv) => _context.Investments.Remove(inv);
        
        public List<Investment> GetAll() => 
            _context.Investments
                .Include(i => i.Investor)
                    .ThenInclude(inv => inv.User)
                .Include(i => i.Company)
                .Include(i => i.Round)
                .ToList();

        public Investment GetById(string id) => 
            _context.Investments
                .Include(i => i.Investor)
                    .ThenInclude(inv => inv.User)
                .Include(i => i.Company)
                .Include(i => i.Round)
                .FirstOrDefault(i => i.Id == id);

        public List<Investment> GetByCompanyId(string companyId) =>
            _context.Investments
                .Include(i => i.Investor)
                    .ThenInclude(inv => inv.User)
                .Include(i => i.Company)
                .Include(i => i.Round)
                .Where(i => i.CompanyId == companyId)
                .ToList();

        public List<Investment> GetByInvestorId(string investorId) =>
            _context.Investments
                .Include(i => i.Investor)
                    .ThenInclude(inv => inv.User)
                .Include(i => i.Company)
                .Include(i => i.Round)
                .Where(i => i.InvestorId == investorId)
                .ToList();

        public void Insert(Investment inv)
        {
            if (string.IsNullOrEmpty(inv.Id))
                inv.Id = Guid.NewGuid().ToString();
            _context.Investments.Add(inv);
        }
    }
}
