using Danske.Application.Interfaces;
using Danske.Domain.Aggregates.Tax;
using Danske.Domain.Exceptions;
using System.Net;

namespace Danske.Application.Services.TaxStrategy
{
    public class TaxStrategyResolver
    {
        private readonly Dictionary<TaxType, ITaxStrategy> _strategies;

        public TaxStrategyResolver(IEnumerable<ITaxStrategy> strategies)
        {
            _strategies = strategies.ToDictionary(s => s.TaxType);
        }

        public ITaxStrategy Resolve(TaxType taxType)
        {
            if (!_strategies.TryGetValue(taxType, out var strategy))
            {
                throw new BusinessException("unsupported tax type", HttpStatusCode.BadRequest);
            }

            return strategy;
        }
    }
}
