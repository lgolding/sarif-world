using Xunit;
using FluentAssertions;
using Microsoft.CodeAnalysis.Sarif;
using Moq;
using Microsoft.Extensions.Localization;
using SarifWorld.App.Pages;
using SarifWorld.App.Models;

namespace SarifWorld.App.Services
{
    public class SarifValidationServiceTests
    {
        [Fact]
        public void SarifValidationService_WhenInputIsNotASarifFile_ReportsAnError()
        {
            const string FileName = "test.not-sarif";
            const string ExpectedMessage = "test.not-sarif is not a SARIF file.";

            var mockFileSystem = new Mock<IFileSystem>();
            var mockLocalizer = new Mock<ILocalizationWrapper<Validation>>();
            mockLocalizer
                .Setup(mock => mock.GetString(SarifValidationService.ErrorNotASarifFile, FileName))
                .Returns(new LocalizedString(SarifValidationService.ErrorNotASarifFile, ExpectedMessage));

            var service = new SarifValidationService(mockFileSystem.Object, mockLocalizer.Object);

            ValidationResult validationResult = service.ValidateFile(FileName, fileContents: string.Empty);

            validationResult.ValidationLog.Should().BeNull();
            validationResult.ErrorMessage.Should().Be($"{FileName} is not a SARIF file.");
        }
    }
}
