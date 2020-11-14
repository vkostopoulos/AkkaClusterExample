using Akka.Actor;
using Akka.Cluster.Sharding;
using LanguageExt;

namespace AkkaClusterExample.Shared
{
    public class DistributedShardedActorMetaData<T> : DistributedActorMetaData
        where T : IMessageExtractor, new()
    {
        public DistributedShardedActorMetaData(string name, Option<string> role) : base(name, role)
        {
        }
        public IActorRef StartProxy(ActorSystem system)
        {
            return ClusterSharding.Get(system).StartProxy(
                Name,
                "worker",
                new T()
            );
        }



        public IActorRef Start(ActorSystem system, Props actorProps)
        {
            return ClusterSharding.Get(system).Start(
                Name,
                actorProps,
                ClusterShardingSettings.Create(system).WithRole("worker"),
                new T()
            );
        }

        public IActorRef Start(ActorSystem system, ClusterShardingSettings settings, Props actorProps)
        {
            return ClusterSharding.Get(system).Start(
                Name,
                actorProps,
                settings.WithRole("worker"),
                new T()
            );
        }
    }
}