using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Core.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }


        [Required,ForeignKey("chat")]
        public int ChatId { get; set; }
        public Chat chat { get; set; }

        [Required,ForeignKey("sender")]
        public string SenderId { get; set; }
        public AppUser sender { get; set; }

        [Required, AllowNull]
        public string Content { get; set; }

        [Required, DisallowNull]
        public DateTime SendDate { get; set; } = DateTime.UtcNow;
    }
}
