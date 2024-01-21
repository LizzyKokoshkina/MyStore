using Braintree;

namespace Core.Payments
{
    public interface IBraintreeConfiguration
    {
        IBraintreeGateway GetGateway();
    }
}
