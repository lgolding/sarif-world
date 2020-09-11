using Bunit;
using FluentAssertions;
using Xunit;

namespace SarifWorld.ComponentsLibrary.UnitTests
{
    public class AlertTests : TestContext
    {
        private const string TestMessage = "Test message.";

        [Fact]
        public void Alert_IsInitiallyHidden()
        {
            var alert = new Alert();

            alert.DisplayClass.Should().Be(Alert.NoDisplay);
        }

        [Fact]
        public void ShowMessage_ShowsAlertWithMessageClass()
        {
            IRenderedComponent<Alert> cut = RenderComponent<Alert>();
            Alert alert = cut.Instance;

            cut.InvokeAsync(() => alert.ShowMessage(TestMessage));

            alert.Message.Should().Be(TestMessage);
            alert.DisplayClass.Should().Be(Alert.BlockDisplay);
            alert.AlertClass.Should().Be(Alert.MessageAlert);
        }

        [Fact]
        public void ShowError_ShowsAlertWithErrorClass()
        {
            IRenderedComponent<Alert> cut = RenderComponent<Alert>();
            Alert alert = cut.Instance;

            cut.InvokeAsync(() => alert.ShowError(TestMessage));

            alert.Message.Should().Be(TestMessage);
            alert.DisplayClass.Should().Be(Alert.BlockDisplay);
            alert.AlertClass.Should().Be(Alert.ErrorAlert);
        }

        [Fact]
        public void Hide_HidesAlert()
        {
            IRenderedComponent<Alert> cut = RenderComponent<Alert>();
            Alert alert = cut.Instance;

            cut.InvokeAsync(() => alert.Hide());

            alert.DisplayClass.Should().Be(Alert.NoDisplay);
        }
    }
}
