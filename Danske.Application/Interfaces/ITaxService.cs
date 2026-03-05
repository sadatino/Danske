
using Danske.Application.DTOs;

namespace Danske.Application.Interfaces
{
    public interface ITaxService
    {
        Task AddUpdateTaxRecordsAsync(CreateTaxesDto dto);

        Task RemoveTaxByIdAsync(int taxId);
    }
}
