using Braintree;

namespace Core.Payments
{
    public class BraintreeConfiguration : IBraintreeConfiguration
    {
        public IBraintreeGateway BraintreeGateway { get; set; }


        public IBraintreeGateway GetGateway()
        {
            if (BraintreeGateway == null)
            {
                BraintreeGateway = CreateGateway();
            }

            return BraintreeGateway;
        }
        private IBraintreeGateway CreateGateway()
        {
            return new BraintreeGateway("Sandbox", "jc9y7bhjff8sf5gc", "4wkrxs54khjs397v", "632dcd9ef51bde80365c13a2cbc8b324");
        }
    }


}
