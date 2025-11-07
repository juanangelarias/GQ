using GQ.Api.GraphQl.Mutations;
using GQ.Entities;

namespace GQ.Api.GraphQl.Subscriptions;

public class Subscription
{
    [Subscribe]
    [Topic(nameof(Mutation.SetProduct))]
    public Product OnProductChanged([EventMessage] Product productChanged) => productChanged;
}