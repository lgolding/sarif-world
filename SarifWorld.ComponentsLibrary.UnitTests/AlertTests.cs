using Bunit;
using FluentAssertions;
using Xunit;

namespace SarifWorld.ComponentsLibrary.UnitTests
{
    public class AlertTests
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
            using var ctx = new TestContext();
            IRenderedComponent<Alert> component = ctx.RenderComponent<Alert>();
            Alert alert = component.Instance;

            component.InvokeAsync(() => alert.ShowMessage(TestMessage));

            alert.Message.Should().Be(TestMessage);
            alert.DisplayClass.Should().Be(Alert.BlockDisplay);
            alert.AlertClass.Should().Be(Alert.MessageAlert);
        }

        [Fact]
        public void ShowError_ShowsAlertWithErrorClass()
        {
            using var ctx = new TestContext();
            IRenderedComponent<Alert> component = ctx.RenderComponent<Alert>();
            Alert alert = component.Instance;

            component.InvokeAsync(() => alert.ShowError(TestMessage));

            alert.Message.Should().Be(TestMessage);
            alert.DisplayClass.Should().Be(Alert.BlockDisplay);
            alert.AlertClass.Should().Be(Alert.ErrorAlert);
        }
    }
}
