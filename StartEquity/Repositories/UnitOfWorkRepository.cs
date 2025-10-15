using StartEquity.Models;

namespace StartEquity.Repositories
{
    public class UnitOfWorkRepository : IUnitOfWork
    {
        private readonly AppDbContext _context;

        private ICompanyRepository _companyRepo;
        private ICategoryRepository _categoryRepo;
        private IInvestorRepository _investorRepo;
        private IRoundRepository _roundRepo;
        private IInvestmentRepository _investmentRepo;
        private ITransferRepository _transferRepo;
        private IShareOfferRepository _shareOfferRepo;

        public UnitOfWorkRepository(AppDbContext context)
        {
            _context = context;
        }

        public ICompanyRepository Companies
        {
            get
            {
                return _companyRepo = _companyRepo ?? new CompanyRepository(_context);
            }
        }

        public ICategoryRepository Categories
        {
            get
            {
                return _categoryRepo = _categoryRepo ?? new CategoryRepository(_context);
            }
        }

        public IInvestorRepository Investors
        {
            get
            {
                return _investorRepo = _investorRepo ?? new InvestorRepository(_context);
            }
        }

        public IRoundRepository Rounds
        {
            get
            {
                return _roundRepo = _roundRepo ?? new RoundRepository(_context);
            }
        }

        public IInvestmentRepository Investments
        {
            get
            {
                return _investmentRepo = _investmentRepo ?? new InvestmentRepository(_context);
            }
        }

        public ITransferRepository Transfers
        {
            get
            {
                return _transferRepo = _transferRepo ?? new TransferRepository(_context);
            }
        }

        public IShareOfferRepository ShareOffers
        {
            get
            {
                return _shareOfferRepo = _shareOfferRepo ?? new ShareOfferRepository(_context);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
