using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace StartEquity.Models
{
    public class ShareOffer
    {
        public string Id { get; set; }

        public string CompanyId { get; set; }
        public Company Company { get; set; }

        public string SellerId { get; set; }
        public Investor Seller { get; set; }

        public string InvestmentId { get; set; } 
        public Investment Investment { get; set; }

        [Column(TypeName = "decimal(12,6)")]
        public decimal SharePercentage { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AskingPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentMarketPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalValue { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? SoldAt { get; set; }

        public string Status { get; set; } = "Available";

        public string BuyerId { get; set; } 
        public Investor Buyer { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
