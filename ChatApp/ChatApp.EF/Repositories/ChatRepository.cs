using ChatApp.Core.Interfaces;
using ChatApp.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.EF.Repositories
{
    public class ChatRepository : BaseRepository<Chat> //: IRepository<Chat>
    {

        //protected ApplicationDbContext __context;
        public ChatRepository(ApplicationDbContext context) : base(context)
        {

        }
        public override Chat GetByIdWith(int id)
        {
            var entity = _context.Chats.Include(b => b.chatUsers).ThenInclude(c => c.user).FirstOrDefault(b => b.ChatId == id);
            return entity;
        }
        public override Chat GetByIdWith(int id, string[] includes)
        {
            Chat chat;
            if (includes != null)
                foreach (var include in includes)
                {
                    int index = include.IndexOf(">");
                    if (index != -1)
                    {
                        string word1 = include.Substring(0, index);
                        string word2 = include.Substring(++index);
                        Console.WriteLine(word1);
                        Console.WriteLine(word2);
                        try
                        {
                            chat = _context.Chats.Include(word1).Include($"{word1}.{word2}").FirstOrDefault(b => b.ChatId == id);
                        }
                        catch
                        {
                            Console.WriteLine("failed to load 2");
                            chat = _context.Chats.FirstOrDefault(b => b.ChatId == id);
                        }
                        
                    }
                    else
                    {
                        Console.WriteLine("failed");
                        chat = _context.Chats.Include(include).FirstOrDefault(b => b.ChatId == id);
                    }
                    return chat;    
                }
                    
            

            var entity = _context.Chats.Include(b => b.chatUsers).ThenInclude(c => c.user).FirstOrDefault(b => b.ChatId == id);
            return entity;
        }
    }
}
