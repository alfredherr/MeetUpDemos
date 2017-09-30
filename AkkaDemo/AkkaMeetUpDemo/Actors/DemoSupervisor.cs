using System;
using System.Collections.Generic;
using Akka.Actor;
using Akka.Event;
using Akka.Monitoring;
using Akka.Persistence;
using AkkaMeetUpDemo.Commands;
using AkkaMeetUpDemo.Events;

namespace AkkaMeetUpDemo.Actors
{
    public class DemoSupervisor : ReceivePersistentActor
    {
		public DemoSupervisor()
        {
            // Persisted actors will apply all stored events to get 
            // the most up-to-date state
            Recover<MessagePersisted>(msg => _ActorState.Add(msg.Message));

            // Commands this actor knows how to process
            Command<SayHello>(cmd => Console.WriteLine(cmd.HelloMessage));

            // Persistence
            Command<PersistThisMessage>(cmd => RunBusinessLogic(cmd));

            //Spawing childern
            Command<WorkXTimes>(cmd =>
            {
                /* Example 3 */
                //SpawnMyWorker(cmd);

                /* Example 4 */
                SpawnAWorkerPerX(cmd);
            });
			
			Command<SeeMyWorkResults>(cmd => ProcessResultsOfWorker(cmd));
        }
		
        List<string> _ActorState = new List<string>();


		private void RunBusinessLogic(PersistThisMessage cmd)
        {
            // Notify my monitoring system +1 message
            Monitor();

            //  Some fancy logic here, then...
            var @event = new MessagePersisted(cmd.Message);

            //  Persist the event(s) generated from processing the logic
            Persist(@event, s =>
            {
                _ActorState.Add(cmd.Message);
            });
        }

		private void SpawnMyWorker(WorkXTimes cmd)
		{
            // Notify my monitoring system +1 message
			Monitor();

			var worker = Context.ActorOf(Props.Create<DemoWorker>(), name: "demoWorker");

			worker.Tell(new DoThisWorkYou(cmd.Xtimes), Self);

			//worker.Tell(PoisonPill.Instance);
		}

		private void SpawnAWorkerPerX(WorkXTimes cmd)
		{
            // Notify my monitoring system +1 message
			Monitor();

            for (int i = 1; i <= cmd.Xtimes; i++)
            {
                string name = $"demoWorker{i.ToString()}";
                var worker = Context.ActorOf(Props.Create<DemoWorker>(), name: name);

				worker.Tell(new DoThisWorkYou(1), Self);
			}		

			//worker.Tell(PoisonPill.Instance);
		}


		private void ProcessResultsOfWorker(SeeMyWorkResults cmd)
		{
			// Notify my monitoring system +1 message
			Monitor();

            //Process results
            Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"{Sender.Path.Name} is done doing the work. A 'thanks' would be nice.");

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
