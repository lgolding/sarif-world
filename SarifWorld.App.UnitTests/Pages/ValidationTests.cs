using System;
using System.Collections.Generic;
using Bunit;
using Bunit.TestDoubles.JSInterop;
using FluentAssertions;
using Microsoft.CodeAnalysis.Sarif;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SarifWorld.App.Models;
using SarifWorld.App.Services;
using SarifWorld.ComponentsLibrary;
using Xunit;

namespace SarifWorld.App.Pages
{
    public class ValidationTests : TestContext
    {
        private const string TestFileName = "a.sarif";
        private const string TestFileText = "{}";
        private const string TestErrorMessage = "failure";

        private static readonly DroppedFile s_droppedFile = new DroppedFile(TestFileName, TestFileText);

        private static readonly SarifLog s_successfulValidationLog =
            new SarifLog
            {
                Runs = new List<Run>
                {
                    new Run
                    {
                        Tool = new Tool
                        {
                            Driver = new ToolComponent
                            {
                                Rules = new List<ReportingDescriptor>
                                {
                                    new ReportingDescriptor { Id = "RULE1001" },
                                    new ReportingDescriptor { Id = "RULE1002" },
                                    new ReportingDescriptor { Id = "RULE1003" }
                                }
                            }
                        },
                        Results = new List<Result>
                        {
                            new Result(),
                            new Result()
                        }
                    }
                }
            };

        private static readonly Run s_successfulRun = s_successfulValidationLog.Runs[0];
        private static readonly int s_numResults = s_successfulRun.Results.Count;
        private static readonly int s_numRules = s_successfulRun.Tool.Driver.Rules.Count;

        public ValidationTests()
        {
            Services.AddLocalization();
            Services.AddLogging();
            Services.AddMockJSRuntime();
        }

        [Fact]
        public void ValidateDroppedFile_WhenValidationSucceeds_DisplaysRunInformation()
        {
            var validationResult = new ValidationResult
            {
                ValidationLog = s_successfulValidationLog
            };

            Mock <ISarifValidationService> mockValidationService = new Mock<ISarifValidationService>();
            mockValidationService
                .Setup(mock => mock.ValidateFile(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(validationResult);

            Services.AddSingleton(mockValidationService.Object);

            IRenderedComponent<Validation> cut = RenderComponent<Validation>();
            Validation page = cut.Instance;

            cut.InvokeAsync(() => page.ValidateDroppedFile(s_droppedFile));

            page.Alert.Message.Should().Contain(s_numResults.ToString());
            page.Alert.AlertClass.Should().Be(Alert.MessageAlert);
            page.RulesSelector.Options.Count.Should().Be(s_numRules);
        }

        [Fact]
        public void ValidateDroppedFile_WhenThereAreNoRuleViolations_DisplaysRunInformation()
        {
            SarifLog logWithNoResults = s_successfulValidationLog.DeepClone();
            logWithNoResults.Runs[0].Results = new List<Result>();
            logWithNoResults.Runs[0].Tool.Driver.Rules = null;

            var validationResult = new ValidationResult
            {
                ValidationLog = logWithNoResults
            };

            Mock<ISarifValidationService> mockValidationService = new Mock<ISarifValidationService>();
            mockValidationService
                .Setup(mock => mock.ValidateFile(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(validationResult);

            Services.AddSingleton(mockValidationService.Object);

            IRenderedComponent<Validation> cut = RenderComponent<Validation>();
            Validation page = cut.Instance;

            cut.InvokeAsync(() => page.ValidateDroppedFile(s_droppedFile));

            page.Alert.Message.Should().Contain(0.ToString());
            page.Alert.AlertClass.Should().Be(Alert.MessageAlert);
            page.RulesSelector.Options.Count.Should().Be(0);
        }

    [Fact]
        public void ValidateDroppedFile_WhenValidationFails_DisplaysErrorMessage()
        {
            ValidationResult validationResult = new ValidationResult
            {
                ErrorMessage = TestErrorMessage
            };

            Mock<ISarifValidationService> mockValidationService = new Mock<ISarifValidationService>();
            mockValidationService
                .Setup(mock => mock.ValidateFile(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(validationResult);

            Services.AddSingleton(mockValidationService.Object);

            IRenderedComponent<Validation> cut = RenderComponent<Validation>();
            Validation page = cut.Instance;

            cut.InvokeAsync(() => page.ValidateDroppedFile(s_droppedFile));

            page.Alert.Message.Should().Be(TestErrorMessage);
            page.Alert.AlertClass.Should().Be(Alert.ErrorAlert);
        }

        [Fact]
        public void ValidateDroppedFile_WhenAnExceptionIsThrown_DisplaysExceptionMessage()
        {
            Mock<ISarifValidationService> mockValidationService = new Mock<ISarifValidationService>();
            mockValidationService
                .Setup(mock => mock.ValidateFile(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new InvalidOperationException(TestErrorMessage));

            Services.AddSingleton(mockValidationService.Object);

            IRenderedComponent<Validation> cut = RenderComponent<Validation>();
            Validation page = cut.Instance;

            cut.InvokeAsync(() => page.ValidateDroppedFile(s_droppedFile));

            page.Alert.Message.Should().Be(TestErrorMessage);
            page.Alert.AlertClass.Should().Be(Alert.ErrorAlert);
        }

    }
}
