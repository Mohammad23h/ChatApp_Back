using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core.Models
{
    public class AppUser : IdentityUser
    {
        [AllowNull]
        public string ResetCode { get; set; } = "null";
        [AllowNull]
        public DateTime ResetCodeExpiryTime { get; set; } = new DateTime(1000, 1, 1);
        //public virtual ICollection<Favurite> Favurites { get; set; }
        //public virtual ProfileInfo ProfileInfo { get; set; }
    }
}
