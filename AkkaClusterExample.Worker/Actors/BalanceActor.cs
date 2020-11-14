using Akka.Actor;
using Akka.Cluster.Sharding;
using AkkaClusterExample.Protocol;
using AkkaClusterExample.Shared.Domain;

namespace AkkaClusterExample.Worker.Actors
{
    public class BalanceActor : ReceiveActor
    {
    private bool _initialized;
    private Balance _balance;
    public BalanceActor()
    {
        Receive<BalanceProtocol.AddBalance>(_ => !_initialized, msg =>
        {
            _initialized = true;
            _balance = new Balance(msg.CustomerId, msg.Amount);
            Sender.Tell(new BalanceProtocol.BalanceAdded(_balance));
        });

        Receive<BalanceProtocol.AddBalance>(_ => Sender.Tell(new BalanceProtocol.BalanceAlreadyAdded(_balance)));

        Receive<BalanceProtocol.FindBalance>(_ => _initialized, _ => Sender.Tell(new BalanceProtocol.FoundBalanceResponse(_balance)));

        Receive<BalanceProtocol.FindBalance>(r => {
            Sender.Tell(new BalanceProtocol.CouldNotFindBalanceResponse(r.CustomerId));
            Context.Parent.Tell(new Passivate(PoisonPill.Instance));
        });

        Receive<BalanceProtocol.DoDeposit>(msg =>
        {
            _balance.Amount += msg.Amount;
            Sender.Tell(new BalanceProtocol.DepositAdded(_balance));
        });

        Receive<BalanceProtocol.DoWithdraw>(msg =>
        {
            _balance.Amount -= msg.Amount;
            Sender.Tell(new BalanceProtocol.WithdrawAdded(_balance));
        });
    }

    public static Props Props()
    {
        return Akka.Actor.Props.Create(() => new BalanceActor());
    }

    }
}
