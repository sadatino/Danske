using Danske.Application.DTOs;
using Danske.Domain.Aggregates.Municipality;
using Danske.Domain.Aggregates.Tax;

namespace Danske.Application.Interfaces
{
    public interface ITaxStrategy
    {
        TaxType TaxType { get; }

        void Handle(Municipality municipality, CreateTaxDto taxDto);
    }
}
