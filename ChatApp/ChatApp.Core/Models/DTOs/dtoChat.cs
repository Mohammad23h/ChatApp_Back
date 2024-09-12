using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core.Models.DTOs
{
    public class dtoChat
    {
        [Required]
        public int MaxUsers { get; set; } = 2;
        [AllowNull]
        public string UserId { get; set; } = null;
    }
}
