using FluentAssertions;
using SarifWorld.TestUtilities;
using Xunit;

namespace SarifWorld.App
{
    // Note: If you run these tests as Admin (at least on Windows 10), Chrome crashes with "Aw,
    // snap!". Multiple posters report this and suggest running the tests as non-Admin, but
    // I don't know the root cause.
    [Trait(TestTraits.Category, TestCategories.UITest)]
    public class IndexPageTests : PageTestBase
    {
        [Fact]
        [Trait(TestTraits.Category, TestCategories.Smoke)]
        public void IndexPage_ShouldHaveCorrectTitleAndUrl()
        {
            var stringResources = new ResourceStrings(typeof(Pages.Index));
            string expectedTitle = stringResources["PageTitle"];

            Driver.Title.Should().Be(expectedTitle);
            Driver.Url.Should().Be(ApplicationUri);
        }

        [Fact]
        [Trait(TestTraits.Category, TestCategories.Smoke)]
        public void IndexPage_CanRefresh()
        {
            var stringResources = new ResourceStrings(typeof(Pages.Index));
            string expectedTitle = stringResources["PageTitle"];

            Driver.Navigate().Refresh();

            Driver.Title.Should().Be(expectedTitle);
            Driver.Url.Should().Be(ApplicationUri);
        }
    }
}
