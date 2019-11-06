using System;

namespace AatingApp.API.Dtos
{
    public class MessageForCreateionDtoo
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public DateTime MessageSent { get; set; }
        public string Content { get; set; }

        public MessageForCreateionDtoo()
        {
            MessageSent= DateTime.Now;
        }
    }
}