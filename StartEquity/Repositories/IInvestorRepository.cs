using System.Collections.Generic;
using StartEquity.Models;

namespace StartEquity.Repositories
{
    public interface IInvestorRepository
    {
        List<Investor> GetAll();
        Investor GetById(string id);
        Investor GetByUserId(string userId);
        Investor GetByEmail(string email);
        void Insert(Investor i);
        void Update(Investor i);
        void Delete(Investor i);
    }
}
