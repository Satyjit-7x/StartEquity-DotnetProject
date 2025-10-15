using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace StartEquity.Models
{
    public class Transfer
    {
        public string Id { get; set; }

        public string CompanyId { get; set; }
        public Company Company { get; set; }

        public string FromInvestorId { get; set; }
        public Investor FromInvestor { get; set; }

        public string ToInvestorId { get; set; } 
        public Investor ToInvestor { get; set; }

        public string ShareOfferId { get; set; }

        [Column(TypeName = "decimal(12,6)")]
        public decimal EquityPercent { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } 

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
        public DateTime TransferDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending";
        public bool Approved { get; set; } = false;
        public DateTime? ApprovedAt { get; set; }
    }

}

