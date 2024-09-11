using ChatApp.Core.Interfaces;
using ChatApp.Core.Models;
using ChatApp.Core.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Claims;
using PusherServer;
using ChatApp.EF.Migrations;
using System.Data.SqlTypes;
using System;
//using System.Web.Mvc;


namespace ChatApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Pusher _pusher;


        public ChatsController(IUnitOfWork unitOfWork, Pusher pusher)
        {
            _unitOfWork = unitOfWork;
            _pusher = pusher;
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var chat =
                _unitOfWork.Chats.Find(b => b.ChatId == id , new[] {"chatUsers>user"});
                //_unitOfWork.Chats.Find(b => b.ChatId == id, new[] {"chatUsers"} );
            return Ok(chat);
        }
        [HttpGet("MessagesOfChat/{id}")]
        public async Task<IActionResult> GetMessages(int id)
        {
            try
            {
                Console.WriteLine("Starting Fetching messages");
                var thismessages = await _unitOfWork.Messages.FindAllAsync(b => b.ChatId == id);
                Console.WriteLine("Messages Fetched Successfully");
                Console.WriteLine("messages count is " + thismessages.Count());
                if (thismessages == null)
                {
                    Console.WriteLine("messages is null");
                    return BadRequest(new { message = "there aren't any message here" });
                }
                Console.WriteLine("messages isn't null here");
                return Ok(thismessages);
        }
            catch(Exception e1)
            {
                Console.WriteLine($"exeption is {e1}");
                return BadRequest(new { message = "ther are an exeption" });
            }
            
        }
        
        [HttpPost("InsertMessage")]
        public async Task<IActionResult> InsertMessage(dtoMessage dtomessage)
        {
            var chat = _unitOfWork.Chats.GetById(dtomessage.ChatId);
            var senderChat = _unitOfWork.ChatUsers.Find1(b =>
                b.ChatId == dtomessage.ChatId &
                b.UserId == dtomessage.SenderId);

            if (chat == null)
                return BadRequest(new { message = "ChatId is invalid" });

            if (senderChat == null)
                return BadRequest(new { message = "you can't send messages at this chat" });

            Message message = new()
            {
                ChatId = dtomessage.ChatId,
                SenderId = dtomessage.SenderId,
                Content = dtomessage.Content
            };
            await _unitOfWork.Messages.InsertAsync(message);

            int c = _unitOfWork.Complete();
            if (c > 0)
                return Ok(new { message = "the insert function has been succeed" });
            return BadRequest(new { message = " the insert function has been failed" });
        }
        
        /*
        [HttpGet("GetName")]
        public IActionResult GetName()
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var name = User.Identity.Name;
            return Ok(id + " " + name);
        }
        */


        [HttpGet("Async/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            return Ok(await _unitOfWork.Chats.GetByIdAsync(id));
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            return Ok(_unitOfWork.Chats.GetAll());
        }
        
        [HttpGet("Find")]
        public IActionResult GetByName(string name , string userId)
        {
            var chatuserfind = _unitOfWork.ChatUsers.FindAll(b => b.user.UserName.Contains(name) , new[] {"user"});
            var chatuserthis = _unitOfWork.ChatUsers.FindAll(b => b.UserId == userId);
            var goalchats = _unitOfWork.Chats.FindAll(b => 
                chatuserfind.Any(c => c.ChatId == b.ChatId) & 
                chatuserthis.Any(x => x.ChatId == b.ChatId));
            if (goalchats.IsNullOrEmpty())
                return NotFound();
            return Ok(goalchats);

            //var chats = _unitOfWork.Chats.FindAll(b =>
            //    b.chatUsers.Any(c => c.user.UserName.Contains(name)) & b.chatUsers.Any(z => z.UserId == userId), new[] { "Author" });

        }
        
        
        /*
        [HttpGet("GetAllWithAuthors")]
        public IActionResult GetAllWithAuthors()
        {
            return Ok(_unitOfWork.chats.FindAll(b => b.Title.Contains("The"), new[] { "Author" }));
        }
        */
        [HttpPost("Insert")]
        public async Task<IActionResult> Insert(dtoChat dtochat)
        {
            Chat chat = new()
            {
                MaxUsers = dtochat.MaxUsers
            }; 
            var thischat = await _unitOfWork.Chats.InsertAsync(chat);
            
            var c = _unitOfWork.Complete();
            if (c == 0)
                return BadRequest(new { message = " the insert function has been failed" });           

            dtoChatUser dto1 = new()
            {
                ChatId = thischat.ChatId,
                UserId = "09861034-b570-4301-b1bd-9dccecc0a16d"
            };
            var result1 = await InsertUser(dto1);


            if (dtochat.UserId != null)
            {
                dtoChatUser dto2 = new()
                {
                    ChatId = thischat.ChatId,
                    UserId = dtochat.UserId
                };
                var result2 = await InsertUser(dto2);
            }
            _unitOfWork.Complete();
            
            
            // تحقق من أن chatId صالح وتحقق من وجود المحادثة
            // هنا يمكنك التحقق من قاعدة البيانات إذا كانت المحادثة موجودة
            // على سبيل المثال:
            // var chat = _dbContext.Chats.Find(chatId);
            // if (chat == null) return NotFound();

            // إعداد قناة جديدة للمحادثة باستخدام ChatId
            
            //var thischat = _unitOfWork.Chats.FindLast(b =>b.ChatId);
            string channelName = $"chat-{thischat.ChatId}";

            // إرسال إشعار أو حدث عند إنشاء القناة إذا لزم الأمر
            var result = await _pusher.TriggerAsync(
                channelName,     // اسم القناة يكون بناءً على ChatId
                "channel-created", // الحدث الذي ترغب في إرساله
                new { message = $"Channel {channelName} created successfully" }
            );

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(new { message = $"Channel {channelName} created successfully" });
            }

            return StatusCode((int)result.StatusCode, new { message = "Failed to create channel" });                       
        }

        [HttpPost("InsertUser")]
        public async Task<IActionResult> InsertUser(dtoChatUser dtochatuser)
        {
            var s = _unitOfWork.ChatUsers.Find1(b => b.UserId == dtochatuser.UserId & b.ChatId == dtochatuser.ChatId);
            var currentChat = _unitOfWork.Chats.Find1(b => b.ChatId == dtochatuser.ChatId , new[] {"chatUsers"});
            
            if(s == null & (currentChat.chatUsers.IsNullOrEmpty() || currentChat.MaxUsers > currentChat.chatUsers.Count()))
            {
                ChatUser chatuser = new()
                {
                    ChatId = dtochatuser.ChatId,
                    UserId = dtochatuser.UserId
                };

                await _unitOfWork.ChatUsers.InsertAsync(chatuser);
                var c = _unitOfWork.Complete();
                if (c > 0)
                    return Ok(new { message = "the insert function has been succeed" });
                return BadRequest(new { message = " the insert function has been failed" });
            }
            return BadRequest(new { message = "the chat is locked or the item had already inserted indatabase" });
            
        }




        
        /*
        // تابع لإنشاء قناة جديدة لكل محادثة
        [HttpPost("create-chat-channel")]
        public async Task<IActionResult> CreateChatChannel([FromBody] int chatId)
        {
            // تحقق من أن chatId صالح وتحقق من وجود المحادثة
            // هنا يمكنك التحقق من قاعدة البيانات إذا كانت المحادثة موجودة
            // على سبيل المثال:
            // var chat = _dbContext.Chats.Find(chatId);
            // if (chat == null) return NotFound();

            // إعداد قناة جديدة للمحادثة باستخدام ChatId
            string channelName = $"chat-{chatId}";

            // إرسال إشعار أو حدث عند إنشاء القناة إذا لزم الأمر
            var result = await _pusher.TriggerAsync(
                channelName,     // اسم القناة يكون بناءً على ChatId
                "channel-created", // الحدث الذي ترغب في إرساله
                new { message = $"Channel {channelName} created successfully" }
            );

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(new { message = $"Channel {channelName} created successfully" });
            }

            return StatusCode((int)result.StatusCode, new { message = "Failed to create channel" });
        }
        */


        // تابع لإرسال رسالة إلى القناة الخاصة بالمحادثة
        [HttpPost("send-message")]
        public async Task<IActionResult> SendMessage(dtoMessage dtomessage)
        {
            var chat = _unitOfWork.Chats.GetById(dtomessage.ChatId);
            if (chat == null)
                return BadRequest(new { message = "ChatId is invalid" });


            var senderChat = _unitOfWork.ChatUsers.Find1(b =>
                b.ChatId == dtomessage.ChatId &
                b.UserId == dtomessage.SenderId);
            if (senderChat == null)
                return BadRequest(new { message = "you can't send messages at this chat" });

            Message message = new()
            {
                ChatId = dtomessage.ChatId,
                SenderId = dtomessage.SenderId,
                Content = dtomessage.Content
            };
            await _unitOfWork.Messages.InsertAsync(message);

            int c = _unitOfWork.Complete();
            if (c == 0)
                return BadRequest(new { message = " the insert function has been failed" });


            // إرسال رسالة إلى القناة الخاصة بالمحادثة
            var result = await _pusher.TriggerAsync(
                $"chat-{message.ChatId}", // قناة المحادثة
                "new-message",            // اسم الحدث
                new { message = message.Content, sender = message.SenderId }
            );

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok(new { message = "Message sent successfully" });
            }

            return StatusCode((int)result.StatusCode, new { message = "Failed to send message" });
        }
    }
}
