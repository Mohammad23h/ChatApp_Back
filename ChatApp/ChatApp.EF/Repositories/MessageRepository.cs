using ChatApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.EF.Repositories
{
    public class MessageRepository : BaseRepository<Message> //: IRepository<Chat>
    {

        //protected ApplicationDbContext __context;
        public MessageRepository(ApplicationDbContext context) : base(context)
        {

        }

        public override Message GetByIdWith(int id)
        {
            throw new NotImplementedException();
        }

        public override Message GetByIdWith(int id, string[] includes)
        {
            throw new NotImplementedException();
        }
    }
}
