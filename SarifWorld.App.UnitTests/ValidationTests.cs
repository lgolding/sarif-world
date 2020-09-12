using System;
using Bunit;
using Bunit.TestDoubles.JSInterop;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SarifWorld.App.Pages;
using SarifWorld.App.Services;
using SarifWorld.ComponentsLibrary;
using Xunit;

namespace SarifWorld.App.UnitTests
{
    public class ValidationTests : TestContext
    {
        [Fact]
        public void ValidateDroppedFile_WhenAnExceptionIsThrown_ShowsTheExceptionMessage()
        {
            const string TestFileName = "a.sarif";
            const string TestFileText = "{}";
            const string TestExceptionMessage = "catastrophe";

            Mock<ISarifValidationService> mockValidationService = new Mock<ISarifValidationService>();
            mockValidationService
                .Setup(mock => mock.ValidateFile(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new InvalidOperationException(TestExceptionMessage));

            Services.AddSingleton(mockValidationService.Object);
            Services.AddLocalization();
            Services.AddLogging();
            Services.AddMockJSRuntime();

            IRenderedComponent<Validation> cut = RenderComponent<Validation>();
            Validation page = cut.Instance;

            var droppedFile = new DroppedFile(TestFileName, TestFileText);
            cut.InvokeAsync(() => page.ValidateDroppedFile(droppedFile));

            page.Alert.Message.Should().Be(TestExceptionMessage);
        }
    }
}
