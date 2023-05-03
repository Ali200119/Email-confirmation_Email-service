using System;
using Microsoft.AspNetCore.Identity;

namespace Elearn.Models
{
	public class AppUser: IdentityUser
	{
        public string FullName { get; set; }
    }
}