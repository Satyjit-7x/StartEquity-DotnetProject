using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StartEquity.Models;

namespace StartEquity.Repositories
{
    public class TransferRepository : ITransferRepository
    {
        private readonly AppDbContext _context;

        public TransferRepository(AppDbContext context)
        {
            _context = context;
        }

        

        public void Update(Transfer t) => _context.Transfers.Update(t);

        public void Delete(Transfer t) => _context.Transfers.Remove(t);
        
        public List<Transfer> GetAll() => 
            _context.Transfers
                .Include(t => t.FromInvestor)
                .Include(t => t.ToInvestor)
                .ToList();

        public Transfer GetById(string id) => 
            _context.Transfers
                .Include(t => t.FromInvestor)
                .Include(t => t.ToInvestor)
                .FirstOrDefault(t => t.Id == id);

        public List<Transfer> GetByInvestorId(string investorId) =>
            _context.Transfers
                .Include(t => t.FromInvestor)
                .Include(t => t.ToInvestor)
                .Where(t => t.FromInvestorId == investorId || t.ToInvestorId == investorId)
                .ToList();

        public void Insert(Transfer t)
        {
            if (string.IsNullOrEmpty(t.Id))
                t.Id = Guid.NewGuid().ToString();
            _context.Transfers.Add(t);
        }
    }
}
