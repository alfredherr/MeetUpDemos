# Akka.Net

Start in [Program.cs](https://github.com/alfredherr/MeetUpDemos/blob/master/AkkaDemo/AkkaMeetUpDemo/Program.cs)

## Example #1) Simple message
```C#
DemoSupervisor.Tell(new SayHello("Hello!"));
```
## Example #2) Command resulting in events which are persisted 
```C#
DemoSupervisor.Tell(new PersistThisMessage(message: "This is very cool!"));
```
## Example #3) Processes: WorkerActor
```C#
DemoSupervisor.Tell(new WorkXTimes(10));
```
## Example 4) Processes: Parallelized Workers  
In [DemoSupervisor](https://github.com/alfredherr/MeetUpDemos/blob/master/AkkaDemo/AkkaMeetUpDemo/Actors/DemoSupervisor.cs)
```C#
Command<WorkXTimes>(cmd =>
            {
                /* Single worker */
                //SpawnMyWorker(cmd);

                /* One worker per job */
                SpawnAWorkerPerX(cmd);
            });
```