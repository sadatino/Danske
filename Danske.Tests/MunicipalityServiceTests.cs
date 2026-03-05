using Danske.Application.DTOs;
using Danske.Application.Services;
using Danske.Domain.Aggregates.Municipality;
using Danske.Domain.Aggregates.Tax;
using Danske.Domain.Exceptions;
using Danske.Domain.Interfaces.Repositories;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using System.Net;

namespace Danske.Tests
{
    public class MunicipalityServiceTests
    {
        private readonly Mock<IMunicipalityRepository> _repositoryMock;
        private readonly MunicipalityService _service;

        public MunicipalityServiceTests()
        {
            _repositoryMock = new Mock<IMunicipalityRepository>();
            _service = new MunicipalityService(_repositoryMock.Object);
        }

        [Fact]
        public async Task Get_ExistingMunicipalities_ReturnsAll_WithNoFilters()
        {
            var municipalities = GetMunicipalities();

            _repositoryMock
                .Setup(x => x.GetExistingMunicipalitiesAsync(null, null))
                .ReturnsAsync(municipalities);

            var result = await _service.GetExistingMunicipalities(null, null);

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task Get_MunicipalityByName_ThrowsBusinessException_WhenNotExists()
        {
            var data = GetMunicipalities().BuildMockDbSet().Object;

            _repositoryMock
                .Setup(x => x.GetAll(false))
                .Returns(data);

            var act = async () => await _service.GetMunicipalityByName("ihvdshkjsdkj");

            var exception = await Assert.ThrowsAsync<BusinessException>(act);
            exception.ErrorCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Add_MunicipalityAsync_Add_WhenNotExists()
        {
            var data = GetMunicipalities().BuildMockDbSet().Object;

            _repositoryMock
                .Setup(x => x.GetAll(true))
                .Returns(data);

            _repositoryMock
                .Setup(x => x.AddAsync(It.IsAny<Municipality>()))
                .Callback<Municipality>(m => m.Id = 67)
                .ReturnsAsync((Municipality m) =>
                {
                    m.Id = 67;
                    return m;
                });

            var dto = new CreateMunicipalityDto
            {
                Name = "Kaunas"
            };

            var result = await _service.AddMunicipalityAsync(dto);

            result.Name.Should().Be("Kaunas");
            result.Id.Should().Be(67);

            _repositoryMock.Verify(x => x.AddAsync(It.IsAny<Municipality>()), Times.Once);
            _repositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Add_MunicipalityAsync_ThrowsBusinessException_WhenAlreadyExists()
        {
            var municipalities = GetMunicipalities();

            var dto = new CreateMunicipalityDto
            {
                Name = "Copenhagen"
            };

            _repositoryMock
                .Setup(x => x.MunicipalityExistsByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((string name) =>
                    municipalities.Any(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                );

            var act = async () => await _service.AddMunicipalityAsync(dto);

            var exception = await Assert.ThrowsAsync<BusinessException>(act);
            exception.ErrorCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task Delete_Municipality_Deletes_WhenExists()
        {
            var municipalities = GetMunicipalities();
            var municipalityToDelete = municipalities.First(x => x.Name.Equals("Copenhagen"));

            _repositoryMock
                .Setup(x => x.GetMunicipalityByNameAsync(municipalityToDelete.Name, false))
                .ReturnsAsync(municipalityToDelete);

            _repositoryMock.Setup(x => x.Delete(It.IsAny<Municipality>()));
            _repositoryMock.Setup(x => x.SaveChangesAsync()).Returns(Task.CompletedTask);

            await _service.DeleteMunicipality(municipalityToDelete.Name);

            _repositoryMock.Verify(x => x.Delete(It.Is<Municipality>(m => m == municipalityToDelete)), Times.Once);
            _repositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Delete_Municipality_ThrowsBusinessException_WhenNotExists()
        {
            var data = GetMunicipalities().BuildMockDbSet().Object;
            _repositoryMock
                .Setup(x => x.GetAll(false))
                .Returns(data);

            var act = async () => await _service.DeleteMunicipality("jngjkbngbjkewgb");

            var exception = await Assert.ThrowsAsync<BusinessException>(act);
            exception.ErrorCode.Should().Be(HttpStatusCode.NotFound);
        }

        private static List<Municipality> GetMunicipalities()
        {
            return new List<Municipality>
            {
                new Municipality
                {
                    Id = 1,
                    Name = "Copenhagen",
                    Taxes = new List<Tax>
                    {
                        new Tax
                        {
                            Id = 1,
                            Rate = 0.15m,
                            TaxType = TaxType.Yearly,
                            StartDate = new DateOnly(2026, 1, 1),
                            EndDate = new DateOnly(2027, 1, 1)
                        },
                        new Tax
                        {
                            Id = 2,
                            Rate = 0.5m,
                            TaxType = TaxType.Monthly,
                            StartDate = new DateOnly(2026, 5, 1),
                            EndDate = new DateOnly(2026, 6, 1)
                        }
                    }
                },
                new Municipality
                {
                    Id = 2,
                    Name = "Vilnius",
                    Taxes = new List<Tax>
                    {
                        new Tax
                        {
                            Id = 3,
                            Rate = 0.25m,
                            TaxType = TaxType.Yearly,
                            StartDate = new DateOnly(2026, 1, 1),
                            EndDate = new DateOnly(2027, 1, 1)
                        }
                    }
                }
            };
        }
    }
}
