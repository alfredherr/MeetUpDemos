using System;
using System.Collections.Generic;
using Akka.Event;
using Akka.Monitoring;
using Akka.Persistence;
using AkkaMeetUpDemo.Commands;
using AkkaMeetUpDemo.Events;

namespace AkkaMeetUpDemo.Actors
{
    public class DemoWorker : ReceivePersistentActor
    {
        List<string> _ActorState = new List<string>();

        public DemoWorker()
        {
            Recover<string>(msg => _ActorState.Add(msg));

            Command<DoThisWorkYou>(cmd => IDoTheWork(cmd));
        
        }

        private void IDoTheWork(DoThisWorkYou cmd)
		{
            Persist("Got Asked to work", s => 
            {
                _ActorState.Add("Got Asked to work");       
            });

            // Notify my monitoring system +1 message
			Monitor();

            string name = Self.Path.Name;
            for (int i = 1; i <= cmd.TimesToRun; i++)
            {
				Console.ForegroundColor = ConsoleColor.White;
				Console.WriteLine($"I'm {name}, and I'm doing work #{(i)}");
			}

            Context.Parent.Tell(new SeeMyWorkResults(), Self); 
              
        }

        public override string PersistenceId => Self.Path.Name;

		readonly ILoggingAdapter _log = Logging.GetLogger(Context);

		protected override void PostStop()
		{
			Context.IncrementActorStopped();
		}
		protected override void PreStart()
		{
			Context.IncrementActorCreated();
		}
		private void Monitor()
		{
			Context.IncrementMessagesReceived();
		}
	}
}
