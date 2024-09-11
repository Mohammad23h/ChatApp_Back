using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core.Models.DTOs
{
    public class dtoMessage
    {
        [Required]
        public int ChatId { get; set; }

        [Required]
        public string SenderId { get; set; }

        [Required,AllowNull]
        public string Content { get; set; }
    }
}
