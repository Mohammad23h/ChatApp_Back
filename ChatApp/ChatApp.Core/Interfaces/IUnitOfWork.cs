using ChatApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChatApp.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Chat> Chats { get; }
        
        IBaseRepository<Message> Messages { get; }
        IBaseRepository<ChatUser> ChatUsers { get; }
        int Complete();
    }
}
