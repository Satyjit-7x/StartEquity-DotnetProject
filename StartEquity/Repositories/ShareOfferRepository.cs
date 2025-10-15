using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StartEquity.Models;

namespace StartEquity.Repositories
{
    public class ShareOfferRepository : IShareOfferRepository
    {
        private readonly AppDbContext _context;

        public ShareOfferRepository(AppDbContext context)
        {
            _context = context;
        }

        

        public void Update(ShareOffer shareOffer) => _context.ShareOffers.Update(shareOffer);

        public void Delete(ShareOffer shareOffer) => _context.ShareOffers.Remove(shareOffer);
        
        public List<ShareOffer> GetAll() => 
            _context.ShareOffers
                .Include(so => so.Company)
                .Include(so => so.Seller)
                    .ThenInclude(s => s.User)
                .Include(so => so.Buyer)
                    .ThenInclude(b => b.User)
                .Include(so => so.Investment)
                .OrderByDescending(so => so.CreatedAt)
                .ToList();

        public ShareOffer GetById(string id) => 
            _context.ShareOffers
                .Include(so => so.Company)
                .Include(so => so.Seller)
                    .ThenInclude(s => s.User)
                .Include(so => so.Buyer)
                    .ThenInclude(b => b.User)
                .Include(so => so.Investment)
                .FirstOrDefault(so => so.Id == id);

        public List<ShareOffer> GetAvailableOffers() =>
            _context.ShareOffers
                .Include(so => so.Company)
                .Include(so => so.Seller)
                    .ThenInclude(s => s.User)
                .Include(so => so.Investment)
                .Where(so => so.Status == "Available" && so.IsActive)
                .OrderByDescending(so => so.CreatedAt)
                .ToList();

        public List<ShareOffer> GetBySellerId(string sellerId) =>
            _context.ShareOffers
                .Include(so => so.Company)
                .Include(so => so.Seller)
                    .ThenInclude(s => s.User)
                .Include(so => so.Buyer)
                    .ThenInclude(b => b.User)
                .Include(so => so.Investment)
                .Where(so => so.SellerId == sellerId)
                .OrderByDescending(so => so.CreatedAt)
                .ToList();

        public List<ShareOffer> GetByCompanyId(string companyId) =>
            _context.ShareOffers
                .Include(so => so.Company)
                .Include(so => so.Seller)
                    .ThenInclude(s => s.User)
                .Include(so => so.Buyer)
                    .ThenInclude(b => b.User)
                .Include(so => so.Investment)
                .Where(so => so.CompanyId == companyId)
                .OrderByDescending(so => so.CreatedAt)
                .ToList();

        public List<ShareOffer> GetByBuyerId(string buyerId) =>
            _context.ShareOffers
                .Include(so => so.Company)
                .Include(so => so.Seller)
                    .ThenInclude(s => s.User)
                .Include(so => so.Buyer)
                    .ThenInclude(b => b.User)
                .Include(so => so.Investment)
                .Where(so => so.BuyerId == buyerId)
                .OrderByDescending(so => so.SoldAt)
                .ToList();

        public void Insert(ShareOffer shareOffer)
        {
            if (string.IsNullOrEmpty(shareOffer.Id))
                shareOffer.Id = Guid.NewGuid().ToString();
            _context.ShareOffers.Add(shareOffer);
        }
    }
}
