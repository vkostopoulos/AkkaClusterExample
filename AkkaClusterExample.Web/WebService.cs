using System;
using Akka.Actor;
using Akka.Cluster.Tools.PublishSubscribe;
using Akka.Configuration;
using AkkaClusterExample.Protocol;

namespace AkkaClusterExample.Web
{
    /// <summary>
    /// The Web service allows us to access actors via proxy.
    /// </summary>
    public class WebService : IDisposable
    {
        private ActorSystem _system;

        /// <summary>
        /// Reference To Balance Singleton Proxy
        /// </summary>
        public IActorRef BalanceActor { get; }

        /// <summary>
        ///  Creating a new WebService creates a new ActorSystem.
        /// </summary>
        public WebService(string actorSystemName, Config akkaConfig)
        {
            _system = ActorSystem.Create(actorSystemName, akkaConfig);

            DistributedPubSub.Get(_system);

            //Shard proxy for BalanceActor
            BalanceActor = ActorPaths.BalanceActor.StartProxy(_system);
        }

        private void StopAsync()
        {
            CoordinatedShutdown.Get(_system).Run(CoordinatedShutdown.ClrExitReason.Instance).Wait();
        }

        /// <summary>
        /// Disposes the actorSystem!
        /// </summary>
        public void Dispose()
        {
            Console.WriteLine("STOPPING");
            StopAsync();
        }
    }
}