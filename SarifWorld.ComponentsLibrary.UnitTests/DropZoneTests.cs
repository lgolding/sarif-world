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
        private readonly IRenderedComponent<DropZone> cut;
        private readonly DropZone dropZone;

        public DropZoneTests()
        {
            Services.AddLogging();
            Services.AddLocalization();
            Services.AddMockJSRuntime();

            this.cut = RenderComponent<DropZone>();
            this.dropZone = cut.Instance;
        }

        [Fact]
        public void DropZone_HasDefaultLabels()
        {
            IStringLocalizer<DropZone> localizer = Services.GetService<IStringLocalizer<DropZone>>();
            this.dropZone.DefaultLabel.Should().Be(localizer["DefaultLabel"]);
        }
    }
}
