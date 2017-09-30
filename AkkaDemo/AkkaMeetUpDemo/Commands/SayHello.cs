namespace AkkaMeetUpDemo.Commands
{
    internal class SayHello
    {
        public string HelloMessage { get; private set; }

        public SayHello(string message){
            HelloMessage = message;
        }
    }
}