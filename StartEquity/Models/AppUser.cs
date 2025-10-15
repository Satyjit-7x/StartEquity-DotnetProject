using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace StartEquity.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string Role { get; set; } 
        
        [MaxLength(100)]
        public string FullName { get; set; }
        
        public bool IsActive { get; set; } = true;
    }
}
