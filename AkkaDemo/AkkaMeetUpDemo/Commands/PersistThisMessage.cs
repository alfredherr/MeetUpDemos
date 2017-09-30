using System;
namespace AkkaMeetUpDemo.Commands
{
    public class PersistThisMessage
    {
		public string Message { get; private set; }

		public PersistThisMessage(string message)
        {
            Message = message;
        }
    }
}
