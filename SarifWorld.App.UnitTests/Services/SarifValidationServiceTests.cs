using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using Microsoft.CodeAnalysis.Sarif;
using Microsoft.Extensions.Localization;
using Moq;
using SarifWorld.App.Models;
using SarifWorld.App.Pages;
using SarifWorld.TestUtilities;
using Xunit;

namespace SarifWorld.App.Services
{
    public class SarifValidationServiceTests
    {
        private static readonly ResourceExtractor s_extractor = new ResourceExtractor(typeof(SarifValidationServiceTests));

        [Fact]
        public void SarifValidationService_WhenInputIsNotASarifFile_ReportsAnError()
        {
            const string FileName = "test.not-sarif";
            const string ExpectedMessage = "test.not-sarif is not a SARIF file.";

            var mockFileSystem = new Mock<IFileSystem>();
            var mockLocalizer = new Mock<ILocalizationWrapper<Validation>>();
            mockLocalizer
                .Setup(_ => _.GetString(SarifValidationService.ErrorNotASarifFile, FileName))
                .Returns(new LocalizedString(SarifValidationService.ErrorNotASarifFile, ExpectedMessage));

            var service = new SarifValidationService(mockFileSystem.Object, mockLocalizer.Object);

            ValidationResult validationResult = service.ValidateFile(FileName, fileContents: string.Empty);

            validationResult.ValidationLog.Should().BeNull();
            validationResult.ErrorMessage.Should().Be($"{FileName} is not a SARIF file.");
        }

        [Fact]
        public void SarifValidationService_WhenInputFileIsASarifFile_ReportsValidationResults()
        {
            const string FileName = "EmptyBraces.sarif";

            // This isn't a valid SARIF file, but that's fine -- the validation service should
            // tell us so, and we'll verify that at the end.
            string fileContents = s_extractor.GetResourceText(FileName);

            // Mocking the file system here is tricky, because the service synthesizes the names
            // of the input and output files from GUIDs. The test relies on the fact that the
            // service first writes the input file (whose name we can capture), and only then
            // invokes the SARIF SDK's ValidateCommand.
            //
            // Compare this setup to a similar setup in the SARIF SDK tests, in
            // src\Test.FunctionalTests.Sarif\Multitool\ValidateCommandTests.cs, in the method
            // ConstructTestOutputFromInputResource. In those tests (which invoke
            // ValidateCommand.Run directly), the input and output file paths are know in advance,
            // so the mock setup can use the known paths directly.
            //
            // In contrast, in this test, the SarifValidationService _creates_ the input file
            // (which it then passes to ValidateCommand.Run), so the test doesn't know in advance
            // what it will be. Therefore:
            //
            //    1) As mentioned above, we have to _capture_ the input file in the first call
            //       to WriteAllText, and
            //    2) We have to _lazily evaluate_ the paths in subsequent calls (which is why
            //       some of the setups use It.Is<string>(...), which takes a lazily evaluated
            //       lambda that closes over the necessary path values.
            //
            // Ah, but it's worse than that. The SDK's SarifLogger, which creates the output file,
            // does so by creating a FileStream to the (real) output file. That is, it does _not_
            // use the mocked file system. So to get the output, we'll have to read the actual file,
            // which means we have to set up the mock to do that.
            bool isInputFile = true;
            string inputFilePath = null;
            string inputFileName = null;
            string inputFileDirectory = null;
            string inputFileContents = null;
            string outputFilePath = null;

            var mockFileSystem = new Mock<IFileSystem>();
            mockFileSystem.Setup(_ => _.WriteAllText(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((string path, string contents) =>
                {
                    if (isInputFile)
                    {
                        inputFilePath = path;
                        inputFileContents = contents;
                        inputFileName = Path.GetFileName(inputFilePath);
                        inputFileDirectory = Path.GetDirectoryName(inputFilePath);
                        isInputFile = false;
                    }
                });

            mockFileSystem
                .Setup(_ => _.DirectoryExists(It.Is<string>(path => path == inputFileDirectory)))
                .Returns(true);
            mockFileSystem
                .Setup(_ => _.GetDirectoriesInDirectory(It.IsAny<string>()))
                .Returns(new string[0]);
            mockFileSystem
                .Setup(_ => _.GetFilesInDirectory(
                    It.Is<string>(directory => directory == inputFileDirectory),
                    It.Is<string>(filter => filter == inputFileName)))
                .Returns(() => new string[] { inputFilePath });
            mockFileSystem
                .Setup(_ => _.ReadAllText(It.Is<string>(path => path == inputFilePath)))
                .Returns(() => inputFileContents);
            mockFileSystem.Setup(_ => _.ReadAllText(It.Is<string>(path => path != inputFilePath )))
                .Callback((string path) =>
                {
                    outputFilePath = path; // Capture the output file path so the .Returns call can read its contents.
                })
                .Returns(() => File.ReadAllText(outputFilePath));

            var mockLocalizer = new Mock<ILocalizationWrapper<Validation>>();

            var service = new SarifValidationService(mockFileSystem.Object, mockLocalizer.Object);

            ValidationResult validationResult = service.ValidateFile(FileName, fileContents);

            // All the results should be JSON1002 (schema validation errors).
            var results = validationResult.ValidationLog.Runs[0].Results;
            results.Should().NotBeEmpty();
            List<string> ruleIds = results.Select(r => r.RuleId).Distinct().ToList();
            ruleIds.Count.Should().Be(1);
            ruleIds[0].Should().Be("JSON1002");

            validationResult.ErrorMessage.Should().BeEmpty();
        }
    }
}
