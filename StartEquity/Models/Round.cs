using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StartEquity.Models
{
    public class Round
    {
        public string Id { get; set; }
        public string CompanyId { get; set; }
        public Company Company { get; set; }

        public int SequenceNumber { get; set; }  

        [Column(TypeName = "decimal(18,2)")]
        public decimal ValuationAtRound { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal EquityTargetPercent { get; set; }

        [NotMapped]
        public decimal TargetAmount => ValuationAtRound * (EquityTargetPercent / 100m);

        [Column(TypeName = "decimal(18,2)")]
        public decimal RaisedAmount { get; set; } = 0m;

        public bool IsClosed { get; set; } = false;

        public DateTime OpenedAt { get; set; } = DateTime.UtcNow;
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Open";
        public DateTime? ClosedAt { get; set; }

        public List<Investment> Investments { get; set; } = new List<Investment>();
    }


}

