using Danske.Application.DTOs;

namespace Danske.Application.Interfaces
{
    public interface ITaxRateService
    {
        Task<TaxRateResult> GetApplicableTaxRateAsync(string municipalityName, DateOnly date);
    }
}
