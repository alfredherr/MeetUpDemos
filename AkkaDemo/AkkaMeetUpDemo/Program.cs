using System;
using Akka.Configuration;
using Akka.Actor;
using Akka.Monitoring;
using Akka.Monitoring.StatsD;
using AkkaMeetUpDemo.Actors;
using AkkaMeetUpDemo.Commands;

namespace AkkaMeetUpDemo
{
    class Program
    {
        static void Main(string[] args)
        {
			Console.WriteLine("*********************************************");
			Console.WriteLine("***************** Akka Demo! ****************");
			Console.WriteLine("*********************************************");

            // Create the Actor System
            ActorSystem demoSystem = ActorSystem.Create("DemoSystem", getConfiguration());

            // Create an actor wihtin that system
			IActorRef DemoSupervisor = demoSystem.ActorOf(Props.Create<DemoSupervisor>(), name: "DemoSupervisor");

            // Add monitoring to the system
			ActorMonitoringExtension.RegisterMonitor(demoSystem, new ActorStatsDMonitor(host: "localhost", port: 8125, prefix: "akka-demo"));

			// Send messages, asynchronously 

            /* Example #1) Simple message */
            //DemoSupervisor.Tell(new SayHello("Hello!"));

            /* Example #2) Command resulting in events which are persisted  */
            //DemoSupervisor.Tell(new PersistThisMessage(message: "This is very cool!"));
			
            /* Examples #3 & #4) Processes  */
            //DemoSupervisor.Tell(new WorkXTimes(10));

			Console.ReadLine();
		}

        public static Config getConfiguration()
        {
            return ConfigurationFactory.ParseString($@" 

                akka.actor.debug.lifecycle = on
                akka.actor.debug.unhandled = on
                
                akka.loglevel = DEBUG
                akka.loggers=[""Akka.Logger.NLog.NLogLogger, Akka.Logger.NLog""]
                
                akka.actor.serializers {{ hyperion = ""Akka.Serialization.HyperionSerializer, Akka.Serialization.Hyperion""}}
                akka.actor.serialization-bindings {{ ""System.Object"" = hyperion }}
                 
                akka.suppress-json-serializer-warning = on
                       
                ## Postgresql         
                #akka.persistence.journal.plugin = ""akka.persistence.journal.postgresql""
                #akka.persistence.journal.postgresql.class = ""Akka.Persistence.PostgreSql.Journal.PostgreSqlJournal, Akka.Persistence.PostgreSql""
                #akka.persistence.journal.postgresql.plugin-dispatcher = ""akka.actor.default-dispatcher""
                #akka.persistence.journal.postgresql.connection-timeout = 30s
                #akka.persistence.journal.postgresql.table-name = event_journal
                #akka.persistence.journal.postgresql.metadata-table-name = journal_metadata
                #akka.persistence.journal.postgresql.auto-initialize = on
                #akka.persistence.journal.postgresql.timestamp-provider = ""Akka.Persistence.Sql.Common.Journal.DefaultTimestampProvider, Akka.Persistence.Sql.Common""
                #akka.persistence.journal.postgresql.connection-string = ""host=localhost port=5432 dbname=akka_demo user=postgres password:akka_demo""
                #akka.persistence.journal.postgresql.stored-as = JSON                

                #akka.persistence.snapshot-store.pugin = ""akka.persistence.snapshot-store.postgresql""
                #akka.persistence.snapshot-store.postgresql.class =""Akka.Persistence.PostgreSql.Snapshot.PostgreSqlSnapshotStore, Akka.Persistence.PostgreSql""                
                #akka.persistence.snapshot-store.postgresql.plugin-dispatcher = ""akka.actor.default-dispatcher""
                #akka.persistence.snapshot-store.postgresql.connection-string = ""host=localhost port=5432 dbname=akka_demo user=postgres password:akka_demo""
                #akka.persistence.snapshot-store.postgresql.connection-timeout = 30s
                #akka.persistence.snapshot-store.postgresql.table-name = snapshot_store
                #akka.persistence.snapshot-store.postgresql.auto-initialize = on
                #akka.persistence.snapshot-store.postgresql.stored-as = JSON

                ## SqLite
                akka.persistence.journal.plugin = ""akka.persistence.journal.sqlite""
                akka.persistence.journal.sqlite.class = ""Akka.Persistence.Sqlite.Journal.SqliteJournal, Akka.Persistence.Sqlite""
                akka.persistence.journal.sqlite.plugin-dispatcher = ""akka.actor.default-dispatcher""
                akka.persistence.journal.sqlite.connection-timeout = 30s
                akka.persistence.journal.sqlite.table-name = event_journal
                akka.persistence.journal.sqlite.metadata-table-name = journal_metadata
                akka.persistence.journal.sqlite.auto-initialize = on
                akka.persistence.journal.sqlite.timestamp-provider = ""Akka.Persistence.Sql.Common.Journal.DefaultTimestampProvider, Akka.Persistence.Sql.Common""
                akka.persistence.journal.sqlite.connection-string = ""Data Source=/Users/alfredherr/Projects/AkkaMeetUpDemo/AkkaMeetUpDemo/akka_demo.db""

                akka.persistence.snapshot-store.plugin = ""akka.persistence.snapshot-store.sqlite""
                akka.persistence.snapshot-store.sqlite.connection-string = ""Data Source=/Users/alfredherr/Projects/AkkaMeetUpDemo/AkkaMeetUpDemo/akka_demo.db""
                akka.persistence.snapshot-store.sqlite.class = ""Akka.Persistence.Sqlite.Snapshot.SqliteSnapshotStore, Akka.Persistence.Sqlite""
                akka.persistence.snapshot-store.sqlite.plugin-dispatcher = ""akka.actor.default-dispatcher""
                akka.persistence.snapshot-store.sqlite.connection-timeout = 30s
                akka.persistence.snapshot-store.sqlite.table-name = snapshot_store
                akka.persistence.snapshot-store.sqlite.auto-initialize = on

                #akka.actor.provider = ""Akka.Cluster.ClusterActorRefProvider, Akka.Cluster""
                #akka.remote.log-remote-lifecycle-events = DEBUG
                #akka.remote.dot-netty.tcp.hostname = ""127.0.0.1""
                #akka.remote.dot-netty.tcp.port = 0
                #akka.cluster.seed-nodes = [""akka.tcp://demo-system@127.0.0.1:4053""] 
                #akka.cluster.roles = [concord]
           ");

        }
	}
}
