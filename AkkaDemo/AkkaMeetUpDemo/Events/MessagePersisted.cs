using System;
namespace AkkaMeetUpDemo.Events
{
    public class MessagePersisted
    {
		public string Message { get; private set; }

		public MessagePersisted(string msg)
        {
            Message = msg;
        }
    }
}
