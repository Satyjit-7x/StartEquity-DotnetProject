using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StartEquity.Models;

namespace StartEquity.Repositories
{
    public class RoundRepository : IRoundRepository
    {
        private readonly AppDbContext _context;

        public RoundRepository(AppDbContext context)
        {
            _context = context;
        }

        

        public void Update(Round r) => _context.Rounds.Update(r);

        public void Delete(Round r) => _context.Rounds.Remove(r);
        
        public List<Round> GetAll() => 
            _context.Rounds
                .Include(r => r.Company)
                .ToList();

        public Round GetById(string id) => 
            _context.Rounds
                .Include(r => r.Company)
                .FirstOrDefault(r => r.Id == id);

        public void Insert(Round r)
        {
            if (string.IsNullOrEmpty(r.Id))
                r.Id = Guid.NewGuid().ToString();
            _context.Rounds.Add(r);
        }
    }
}
