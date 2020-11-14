using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Configuration;
using AkkaClusterExample.Protocol;
using AkkaClusterExample.Worker.Actors;
using Petabridge.Cmd.Host;

namespace AkkaClusterExample.Worker
{
    /// <summary>
    /// The Worker service contains actual actors.
    /// </summary>
    public class WorkerService
    {
        private readonly string _actorSystemName;
        private readonly Config _akkaConfig;

        public WorkerService(string actorSystemName, Config akkaConfig)
        {
            _actorSystemName = actorSystemName;
            _akkaConfig = akkaConfig;
        }

        private ActorSystem _system;
        public Task TerminationHandle => _system.WhenTerminated;

        //Creating a new WorkerService creates a new ActorSystem.
        public void Start(CancellationToken token)
        {
            _system = ActorSystem.Create(_actorSystemName, _akkaConfig);

            var cmd = PetabridgeCmd.Get(_system);
            cmd.RegisterCommandPalette(Petabridge.Cmd.Cluster.ClusterCommands.Instance);
            cmd.RegisterCommandPalette(Petabridge.Cmd.Cluster.Sharding.ClusterShardingCommands.Instance);
            // Register custom cmd commands here
            cmd.Start();

            DistributedPubSub.Get(_system);

            StartActors();

            token.Register(StopAsync);
        }

        private void StartActors()
        {
            //Start a shared actor definition.
            ActorPaths.BalanceActor.Start(_system, BalanceActor.Props());
        }

        private async void StopAsync()
        {
            //await CoordinatedShutdown.Get(_system).Run(CoordinatedShutdown.ClrExitReason.Instance);
        }
    }
}