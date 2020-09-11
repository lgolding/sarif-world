using Bunit;
using Bunit.TestDoubles.JSInterop;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Xunit;

namespace SarifWorld.ComponentsLibrary.UnitTests
{
    public class DropZoneTests : TestContext
    {
        [Fact]
        public void DropZone_HasDefaultLabels()
        {
            Services.AddLogging();
            Services.AddLocalization();
            Services.AddMockJSRuntime();

            IRenderedComponent<DropZone> cut = RenderComponent<DropZone>();
            DropZone dropZone = cut.Instance;

            IStringLocalizer<DropZone> localizer = Services.GetService<IStringLocalizer<DropZone>>();
            dropZone.DefaultLabel.Should().Be(localizer["DefaultLabel"]);
        }
    }
}
