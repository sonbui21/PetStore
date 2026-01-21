using CardType = Ordering.Domain.AggregatesModel.BuyerAggregate.CardType;

namespace Ordering.API.Infrastructure;

public class OrderingContextSeed : IDbSeeder<OrderingContext>
{
    public async Task SeedAsync(OrderingContext context)
    {
        if (!(await context.CardTypes.AnyAsync()))
        {
            context.CardTypes.AddRange(GetPredefinedCardTypes());

            await context.SaveChangesAsync();
        }

        await context.SaveChangesAsync();
    }

    private static IEnumerable<CardType> GetPredefinedCardTypes()
    {
        yield return new CardType { Id = 1, Name = "Visa" };
        yield return new CardType { Id = 2, Name = "MasterCard" };
    }
}
