using System.Threading.Tasks;
using Bunit;
using Bunit.TestDoubles.JSInterop;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Xunit;
using static Bunit.ComponentParameterFactory;

namespace SarifWorld.ComponentsLibrary
{
    public class DropZoneTests : TestContext
    {
        public DropZoneTests()
        {
            Services.AddLogging();
            Services.AddLocalization();
            Services.AddMockJSRuntime();
        }

        [Fact]
        public void DropZone_HasDefaultParameterValues()
        {
            IRenderedComponent<DropZone> cut = RenderComponent<DropZone>();
            DropZone dropZone = cut.Instance;

            IStringLocalizer<DropZone> localizer = Services.GetService<IStringLocalizer<DropZone>>();
            dropZone.DefaultLabel.Should().Be(localizer["DefaultLabel"]);
            dropZone.BusyLabel.Should().Be(localizer["BusyLabel"]);
            dropZone.CompleteLabel.Should().Be(localizer["CompleteLabel"]);

            dropZone.Id.Should().Be(DropZone.DefaultId);
            dropZone.CompleteLabelDisplayTime.Should().Be(DropZone.DefaultCompleteLabelDisplayTime);
            dropZone.AllowMultiple.Should().Be(DropZone.DefaultAllowMultiple);
        }

        [Fact]
        public void DropZone_AcceptsParameters()
        {
            // Ensure that these strings don't match the real (localized) strings.
            const string TestDefaultLabel = "The default label";
            const string TestBusyLabel = "The busy label";
            const string TestCompleteLabel = "The complete label";

            // Ensure that these values are different from the defaults.
            const string TestId = DropZone.DefaultId + "x";
            const bool TestAllowMultiple = !DropZone.DefaultAllowMultiple;
            const int TestCompleteLabelDisplayTime = DropZone.DefaultCompleteLabelDisplayTime + 1000;

            IRenderedComponent<DropZone> cut = RenderComponent<DropZone>(
                (nameof(DropZone.Id), TestId),
                (nameof(DropZone.DefaultLabel), TestDefaultLabel),
                (nameof(DropZone.BusyLabel), TestBusyLabel),
                (nameof(DropZone.CompleteLabel), TestCompleteLabel),
                (nameof(DropZone.CompleteLabelDisplayTime), TestCompleteLabelDisplayTime),
                (nameof(DropZone.AllowMultiple), false));
            DropZone dropZone = cut.Instance;

            dropZone.DefaultLabel.Should().Be(TestDefaultLabel);
            dropZone.BusyLabel.Should().Be(TestBusyLabel);
            dropZone.CompleteLabel.Should().Be(TestCompleteLabel);

            dropZone.CompleteLabelDisplayTime.Should().Be(TestCompleteLabelDisplayTime);
            dropZone.AllowMultiple.Should().Be(TestAllowMultiple);
        }

        // This test does not exercise the plumbing where you drop a file onto the DropZone,
        // the DropZone's _JavaScript_ ondrop handler is triggered, and it calls back on
        // DropZone's C# handler. It exercises only that C# handler, which in turn calls
        // back on the handler that the _containing_ component registered.
        [Fact]
        public async Task DropZone_CallsBackOnDroppedFileHandler()
        {
            const string TestFileName = "test.txt";
            const string TestFileText = "Hello, world!";

            DroppedFile droppedFile = null;

            IRenderedComponent<DropZone> cut = RenderComponent<DropZone>(
                EventCallback(
                    nameof(DropZone.OnFileDropped), 
                    (DroppedFile arg) => { droppedFile = arg; }));

            DropZone dropZone = cut.Instance;
            await cut.InvokeAsync(
                () => dropZone.HandleDroppedFile(TestFileName, TestFileText));

            droppedFile.Name.Should().Be(TestFileName);
            droppedFile.Text.Should().Be(TestFileText);
        }
    }
}
