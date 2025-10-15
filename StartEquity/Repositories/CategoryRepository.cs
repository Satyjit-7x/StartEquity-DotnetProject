using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StartEquity.Models;

namespace StartEquity.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        

        public void Update(Category c) => _context.Categories.Update(c);

        public void Delete(Category c) => _context.Categories.Remove(c);
        
        public List<Category> GetAll() => 
            _context.Categories.ToList();

        public Category GetById(string id) => 
            _context.Categories.FirstOrDefault(c => c.Id == id);

        public void Insert(Category c)
        {
            if (string.IsNullOrEmpty(c.Id))
                c.Id = Guid.NewGuid().ToString();
            _context.Categories.Add(c);
        }
    }
}
