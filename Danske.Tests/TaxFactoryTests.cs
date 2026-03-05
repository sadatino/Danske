
using Danske.Domain.Aggregates.Tax;
using Danske.Domain.Exceptions;

namespace Danske.Tests
{
    public class TaxFactoryTests
    {
        [Fact]
        public void Create_DailyTaxType_ReturnsTaxWithAllPropertiesSet()
        {
            // Arrange
            var startDate = new DateOnly(2026, 2, 22);
            var endDate = new DateOnly(2026, 2, 23);

            // Act
            var tax = TaxFactory.Create(2, TaxType.Daily, 0.67m, startDate);

            // Assert
            Assert.Equal(2, tax.MunicipalityId);
            Assert.Equal(TaxType.Daily, tax.TaxType);
            Assert.Equal(0.67m, tax.Rate);
            Assert.Equal(startDate, tax.StartDate);
            Assert.Equal(endDate, tax.EndDate);
        }

        [Fact]
        public void Create_WeeklyTaxType_ReturnsTax()
        {
            // Arrange
            var startDate = new DateOnly(2026, 2, 22);
            var endDate = new DateOnly(2026, 3, 01);

            // Act
            var tax = TaxFactory.Create(2, TaxType.Weekly, 0.2m, startDate);

            //Assert
            Assert.Equal(2, tax.MunicipalityId);
            Assert.Equal(TaxType.Weekly, tax.TaxType);
            Assert.Equal(0.2m, tax.Rate);
            Assert.Equal(startDate, tax.StartDate);
            Assert.Equal(endDate, tax.EndDate);
        }

        [Fact]
        public void Create_MonthlyTaxType_StartingOnFirstDay_ReturnsTax()
        {
            // Arrange
            var startDate = new DateOnly(2026, 6, 1);
            var endDate = new DateOnly(2026, 7, 1);

            //Act
            var tax = TaxFactory.Create(1, TaxType.Monthly, 0.2m, startDate);

            // Assert
            Assert.Equal(1, tax.MunicipalityId);
            Assert.Equal(TaxType.Monthly, tax.TaxType);
            Assert.Equal(0.2m, tax.Rate);
            Assert.Equal(startDate, tax.StartDate);
            Assert.Equal(endDate, tax.EndDate);
        }

        [Theory]
        [InlineData(2024, 6, 2)]
        [InlineData(2024, 6, 15)]
        [InlineData(2024, 6, 30)]
        public void Create_MonthlyTaxType_NotOnFirstDay_ThrowsBusinessException(int year, int month, int day)
        {
            var startDate = new DateOnly(year, month, day);

            Assert.Throws<BusinessException>(() => TaxFactory.Create(1, TaxType.Monthly, 0.2m, startDate));
        }

        [Fact]
        public void Create_YearlyTaxType_OnJanuaryFirst_ReturnsTax()
        {
            var startDate = new DateOnly(2026, 1, 1);
            var endDate = new DateOnly(2027, 1, 1);

            var tax = TaxFactory.Create(1, TaxType.Yearly, 0.2m, startDate);

            Assert.Equal(1, tax.MunicipalityId);
            Assert.Equal(TaxType.Yearly, tax.TaxType);
            Assert.Equal(0.2m, tax.Rate);
            Assert.Equal(startDate, tax.StartDate);
            Assert.Equal(endDate, tax.EndDate);
        }

        [Theory]
        [InlineData(2024, 1, 5)]
        [InlineData(2024, 3, 1)]
        [InlineData(2024, 6, 15)]
        [InlineData(2024, 12, 31)]
        public void Create_YearlyTaxType_NotJanuaryFirst_ThrowsBusinessException(int year, int month, int day)
        {
            var startDate = new DateOnly(year, month, day);

            Assert.Throws<BusinessException>(() =>
                TaxFactory.Create(1, TaxType.Yearly, 0.3m, startDate));
        }
    }
}
