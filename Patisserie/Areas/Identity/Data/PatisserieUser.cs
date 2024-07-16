using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Patisserie.Models;

namespace Patisserie.Areas.Identity.Data;

// Add profile data for application users by adding properties to the PatisserieUser class
public class PatisserieUser : IdentityUser
{
    public ICollection<UserProduct>? Products { get; set; }
}

