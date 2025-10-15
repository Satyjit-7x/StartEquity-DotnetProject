using StartEquity.Models;

namespace StartEquity.Repositories
{ 
    public interface IUnitOfWork
    {
        ICompanyRepository Companies { get; }
        ICategoryRepository Categories { get; }
        IInvestorRepository Investors { get; }
        IRoundRepository Rounds { get; }
        IInvestmentRepository Investments { get; }
        ITransferRepository Transfers { get; }
        IShareOfferRepository ShareOffers { get; }

        void Save();
        void SaveChanges();
    }
}
