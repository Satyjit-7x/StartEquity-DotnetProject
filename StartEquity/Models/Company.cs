using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StartEquity.Models;


namespace StartEquity.Models
{
    public class Company
    {
        public string Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string CategoryId { get; set; }
        public Category Category { get; set; }

        public string CurrentRoundId { get; set; }
        public Round CurrentRound { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalRaised { get; set; } = 0m;

        public string OwnerId { get; set; }
        public AppUser Owner { get; set; }

        public bool IsActive { get; set; } = true;
        public bool CanRaiseFunds { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<Round> Rounds { get; set; } = new List<Round>();
        public List<Investment> Investments { get; set; } = new List<Investment>();
    }
}

