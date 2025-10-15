using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StartEquity.Models;
using StartEquity.Repositories;

namespace StartEquity.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDbContext _context;

        public CompanyRepository(AppDbContext context) => _context = context;

        

        public void Update(Company c) => _context.Companies.Update(c);

        public void Delete(Company c) => _context.Companies.Remove(c);
        
        public List<Company> GetAll() => 
            _context.Companies
                .Include(c => c.Category)
                .Include(c => c.CurrentRound)
                .Include(c => c.Owner)
                .ToList();

        public Company GetById(string id) => 
            _context.Companies
                .Include(c => c.Category)
                .Include(c => c.CurrentRound)
                .Include(c => c.Owner)
                .Include(c => c.Rounds)
                .FirstOrDefault(x => x.Id == id);

        public List<Company> GetByCategory(string categoryId) =>
            _context.Companies
                .Include(c => c.Category)
                .Include(c => c.CurrentRound)
                .Where(c => c.CategoryId == categoryId && c.IsActive && c.CanRaiseFunds)
                .ToList();

        public List<Company> GetByOwnerId(string ownerId) =>
            _context.Companies
                .Include(c => c.Category)
                .Include(c => c.CurrentRound)
                .Where(c => c.OwnerId == ownerId)
                .ToList();

        public void Insert(Company c)
        {
            if (string.IsNullOrEmpty(c.Id))
                c.Id = Guid.NewGuid().ToString();
            _context.Companies.Add(c);
        }
    }

}
