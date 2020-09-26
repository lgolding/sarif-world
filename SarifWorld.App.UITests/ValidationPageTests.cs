using FluentAssertions;
using OpenQA.Selenium;
using SarifWorld.TestUtilities;
using SeleniumExtras.WaitHelpers;
using Xunit;

namespace SarifWorld.App
{
    [Trait(TestTraits.Category, TestCategories.UITest)]
    public class ValidationPageTests : PageTestBase
    {
        public ValidationPageTests() : base(RelativePageUris.Validation) { }

        [Fact]
        [Trait(TestTraits.Category, TestCategories.Smoke)]
        public void ValidationPage_ShouldHaveCorrectTitleAndUrl()
        {
            var stringResources = new ResourceStrings(typeof(Pages.Validation));
            string expectedTitle = stringResources["PageTitle"];

            Driver.Title.Should().Be(WebPageTitle);
            Driver.Url.Should().Be(PageUri);

            Wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.ClassName("page-title"), expectedTitle));
        }
    }
}
