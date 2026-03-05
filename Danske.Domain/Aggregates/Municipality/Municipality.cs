namespace Danske.Domain.Aggregates.Municipality
{
    public class Municipality
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public ICollection<Tax.Tax> Taxes { get; set; } = [];
    }
}
