using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.Xml;

namespace StartEquity.Models
{
    public class Investor
    {
        public string Id { get; set; }
        
        [Required] 
        public string UserId { get; set; }
        public AppUser User { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; } = 10000m;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<Investment> Investments { get; set; } = new List<Investment>();
        public List<Transfer> TransfersSent { get; set; } = new List<Transfer>();
        public List<Transfer> TransfersReceived { get; set; } = new List<Transfer>();
    }

}

