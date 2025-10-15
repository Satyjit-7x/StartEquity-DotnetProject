using System.Collections.Generic;
using StartEquity.Models;

namespace StartEquity.Repositories
{
    public interface ICompanyRepository
    {
        List<Company> GetAll();
        Company GetById(string id);
        List<Company> GetByCategory(string categoryId);
        List<Company> GetByOwnerId(string ownerId);
        void Insert(Company c);
        void Update(Company c);
        void Delete(Company c);
    }

}
