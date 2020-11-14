using AkkaClusterExample.Shared;

namespace AkkaClusterExample.Protocol
{
    public static class ActorPaths
    {
        public static readonly ActorMetaData PubSubMediator = new ActorMetaData("distributedPubSubMediator", isSystemActor: true);

        public static readonly DistributedShardedActorMetaData<BalanceMessageExtractor> BalanceActor =
            new DistributedShardedActorMetaData<BalanceMessageExtractor>("balance", ActorRoles.Worker);
    }
}