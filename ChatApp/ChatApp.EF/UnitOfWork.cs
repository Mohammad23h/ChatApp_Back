using ChatApp.Core.Interfaces;
using ChatApp.Core.Models;
using ChatApp.EF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChatApp.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IBaseRepository<Chat> Chats { get; private set; }
        public IBaseRepository<Message> Messages { get; private set; }
        public IBaseRepository<ChatUser> ChatUsers { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            //Chats = new BaseRepository<Chat>(_context);
            Chats = new ChatRepository(_context);
            Messages = new MessageRepository(_context);
            ChatUsers = new ChatUserRepository(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
