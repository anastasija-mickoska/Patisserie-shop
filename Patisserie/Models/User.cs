using Microsoft.AspNetCore.Identity;
using Patisserie.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace Patisserie.Models
{
    public class User : IdentityUser
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}
