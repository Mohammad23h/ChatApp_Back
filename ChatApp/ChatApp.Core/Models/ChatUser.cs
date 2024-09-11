using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatApp.Core.Models
{
    public class ChatUser
    {
        [Key] 
        public int ChatUserId { get; set; }

        [Required,ForeignKey("chat")]
        public int ChatId { get; set; }
        [JsonIgnore]
        public Chat chat { get; set; }

        [Required,ForeignKey("user")]
        public string UserId { get; set; }
        public AppUser user { get; set; }

        
    }
}
