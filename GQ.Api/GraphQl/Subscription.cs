using GQ.Entities;

namespace GQ.Api.GraphQl;

public class Subscription
{
    [Subscribe]
    [Topic(nameof(Mutation.SetProduct))]
    public Product OnProductChanged([EventMessage] Product productChanged) => productChanged;
}