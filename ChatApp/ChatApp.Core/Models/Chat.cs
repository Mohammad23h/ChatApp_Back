using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatApp.Core.Models
{
    public class Chat
    {
        [Key]
        public int ChatId { get; set; }
        [Required]
        public int MaxUsers { get; set; } = 2;

        [JsonIgnore]
        public ICollection<Message> messages { get; set; }

        
        public ICollection<ChatUser> chatUsers { get; set; }
    }
}
