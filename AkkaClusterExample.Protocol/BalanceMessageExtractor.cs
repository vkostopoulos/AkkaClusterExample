using Akka.Cluster.Sharding;

namespace AkkaClusterExample.Protocol
{
    public class BalanceMessageExtractor : HashCodeMessageExtractor
    {
        public BalanceMessageExtractor() : base(10)
        {
        }

        public override string EntityId(object message)
        {
            if (message is BalanceProtocol.BalanceMessage a)
            {
                return a.CustomerId.ToString();
            }
            return null;
        }
    }
}
