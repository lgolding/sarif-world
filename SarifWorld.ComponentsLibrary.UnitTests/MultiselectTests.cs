using System.Collections.Generic;
using System.Threading.Tasks;
using Bunit;
using FluentAssertions;
using Xunit;

namespace SarifWorld.ComponentsLibrary.UnitTests
{
    public class MultiselectTests : TestContext
    {
        [Fact]
        public async Task Multiselect_AcceptsASetOfOptions()
        {
            var options = new List<string>
            {
                "one",
                "two"
            };

            IRenderedComponent<Multiselect> cut = RenderComponent<Multiselect>();

            Multiselect multiselect = cut.Instance;
            await cut.InvokeAsync(
                () => multiselect.SetOptions(options));

            multiselect.Options.Should().BeEquivalentTo(options);
        }
    }
}
