using System.Collections.Generic;
using StartEquity.Models;

namespace StartEquity.Repositories
{
    public interface ITransferRepository
    {
        List<Transfer> GetAll();
        Transfer GetById(string id);
        List<Transfer> GetByInvestorId(string investorId);
        void Insert(Transfer t);
        void Update(Transfer t);
        void Delete(Transfer t);
    }
}
