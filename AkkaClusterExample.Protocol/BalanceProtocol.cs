using AkkaClusterExample.Shared.Domain;

namespace AkkaClusterExample.Protocol
{
    /// <summary>
    /// Messages relating to BalanceActor.
    /// </summary>
    public static class BalanceProtocol
    {
        public abstract class BalanceMessage
        {
            public abstract int CustomerId { get; }
        }

        public class AddBalance : BalanceMessage
        {
            public override int CustomerId { get; }
            public float Amount { get; }

            public AddBalance(int customerId, float amount)
            {
                CustomerId = customerId;
                Amount = amount;
            }
        }

        public abstract class BalanceCreationResponseMessage : BalanceMessage
        {
        }

        public class FindBalance : BalanceMessage
        {
            public FindBalance(int customerId)
            {
                CustomerId = customerId;
            }

            public override int CustomerId { get; }
        }

        public abstract class FindBalanceResponseMessage : BalanceMessage
        {
        }

        public class FoundBalanceResponse : FindBalanceResponseMessage
        {
            public override int CustomerId => Balance.CustomerId;
            public Balance Balance { get; }

            public FoundBalanceResponse(Balance balance)
            {
                Balance = balance;
            }
        }

        public class CouldNotFindBalanceResponse : FindBalanceResponseMessage
        {
            public override int CustomerId { get; }

            public CouldNotFindBalanceResponse(int customerId)
            {
                CustomerId = customerId;
            }
        }

        public class BalanceAdded : BalanceCreationResponseMessage
        {
            public override int CustomerId => Balance.CustomerId;
            public Balance Balance { get; }

            public BalanceAdded(Balance balance)
            {
                Balance = balance;
            }
        }

        public class BalanceAlreadyAdded : BalanceCreationResponseMessage
        {
            public override int CustomerId => Balance.CustomerId;
            public Balance Balance { get; }

            public BalanceAlreadyAdded(Balance balance)
            {
                Balance = balance;
            }
        }

        public class DoDeposit : BalanceMessage
        {
            public override int CustomerId { get; }
            public float Amount { get; }

            public DoDeposit(int customerId, float amount)
            {
                CustomerId = customerId;
                Amount = amount;
            }
        }

        public abstract class DoDepositResponseMessage : BalanceMessage
        {
        }

        public class DepositAdded : DoDepositResponseMessage
        {
            public override int CustomerId => Balance.CustomerId;
            public Balance Balance { get; }

            public DepositAdded(Balance balance)
            {
                Balance = balance;
            }
        }



        public class DoWithdraw : BalanceMessage
        {
            public override int CustomerId { get; }
            public float Amount { get; }

            public DoWithdraw(int customerId, float amount)
            {
                CustomerId = customerId;
                Amount = amount;
            }
        }

        public abstract class DoWithdrawResponseMessage : BalanceMessage
        {
        }

        public class WithdrawAdded : DoWithdrawResponseMessage
        {
            public override int CustomerId => Balance.CustomerId;
            public Balance Balance { get; }

            public WithdrawAdded(Balance balance)
            {
                Balance = balance;
            }
        }
    }
}