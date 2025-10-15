using System.Collections.Generic;
using StartEquity.Models;

namespace StartEquity.Repositories
{
    public interface IShareOfferRepository
    {
        List<ShareOffer> GetAll();
        ShareOffer GetById(string id);
        List<ShareOffer> GetAvailableOffers();
        List<ShareOffer> GetBySellerId(string sellerId);
        List<ShareOffer> GetByCompanyId(string companyId);
        List<ShareOffer> GetByBuyerId(string buyerId);
        void Insert(ShareOffer shareOffer);
        void Update(ShareOffer shareOffer);
        void Delete(ShareOffer shareOffer);
    }
}
