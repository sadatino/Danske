
using System.ComponentModel.DataAnnotations;

namespace Danske.Application.DTOs
{
    public class CreateMunicipalityDto
    {
        [Required]
        public required string Name { get; set; }
    }
}
