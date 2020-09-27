using SarifWorld.App.PageObjectModels;
using SarifWorld.TestUtilities;
using Xunit;

namespace SarifWorld.App
{
    [Trait(TestTraits.Category, TestCategories.UITest)]
    public class ValidationPageTests : PageTestBase
    {
        [Fact]
        [Trait(TestTraits.Category, TestCategories.Smoke)]
        public void ValidationPage_ShouldHaveCorrectTitleAndUrl()
        {
            var validationPage = new ValidationPage(Driver);
            validationPage.NavigateTo();

            validationPage.WaitForExpectedPageTitle();
        }
    }
}
