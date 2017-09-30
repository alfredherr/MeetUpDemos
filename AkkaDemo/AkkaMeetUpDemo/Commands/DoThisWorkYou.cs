using System;
namespace AkkaMeetUpDemo.Commands
{
    public class DoThisWorkYou
    {
        public int TimesToRun { get; private set; }

        public DoThisWorkYou(int iterations)
        {
            TimesToRun = iterations;
        }
    }
}
