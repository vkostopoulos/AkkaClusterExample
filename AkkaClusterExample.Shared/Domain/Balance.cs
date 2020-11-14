namespace AkkaClusterExample.Shared.Domain
{
    public class Balance
    {
        public Balance(int customerId, float amount)
        {
            Amount = amount;
            CustomerId = customerId;
        }

        public float Amount { get; set; }

        public int CustomerId { get; }
    }
}
