using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StartEquity.Models
{
    public class Category
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }   
        public List<Company> Companies { get; set; } = new List<Company>();
    }
}

