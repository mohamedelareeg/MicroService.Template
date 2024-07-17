using BuildingBlocks.Primitives;
using BuildingBlocks.Results;

namespace MicroService.Template.Catalog.Api.ValueObjects
{
    public class Price : ValueObject
    {
        public decimal Amount { get; }
        public Price()
        {
            
        }
        private Price(decimal amount)
        {
            Amount = amount;
        }

        public static Result<Price> Create(decimal amount)
        {
            if (amount < 0)
            {
                return Result.Failure<Price>("Price.Create", "Price cannot be negative.");
            }
            return new Price(amount);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Amount;
        }
    }
}
