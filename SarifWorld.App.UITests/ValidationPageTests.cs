using FluentAssertions;
using OpenQA.Selenium;
using SarifWorld.App.PageObjectModels;
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
            var validationPage = new ValidationPage(Driver);

            var stringResources = new ResourceStrings(typeof(Pages.Validation));
            string expectedTitle = stringResources["PageTitle"];

            validationPage.Title.Should().Be(WebPageTitle);
            validationPage.Url.Should().Be(PageUri);

            // How can we push the HTML-ness of this statement down into the page object model?
            Wait.Until(ExpectedConditions.TextToBePresentInElementLocated(By.ClassName("page-title"), expectedTitle));
        }
    }
}
