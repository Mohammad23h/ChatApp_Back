using ChatApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.EF.Repositories
{
    public class ChatUserRepository : BaseRepository<ChatUser> //: IRepository<Chat>
    {

        //protected ApplicationDbContext __context;
        public ChatUserRepository(ApplicationDbContext context) : base(context)
        {

        }
        public override ChatUser GetByIdWith(int id)
        {
            var entity = _context.chatUsers.Include(b => b.chat).Include(c => c.user).FirstOrDefault(b => b.ChatId == id);
            return entity;
        }

        public override ChatUser GetByIdWith(int id, string[] includes)
        {
            throw new NotImplementedException();
        }
    }
}
