using System.Collections.Generic;
using StartEquity.Models;

namespace StartEquity.Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        Category GetById(string id);
        void Insert(Category c);
        void Update(Category c);
        void Delete(Category c);
    }
}
