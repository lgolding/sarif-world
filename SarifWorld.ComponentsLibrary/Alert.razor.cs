using Microsoft.AspNetCore.Components;

namespace SarifWorld.ComponentsLibrary
{
    public partial class Alert
    {
        internal const string NoDisplay = "d-none";
        internal const string BlockDisplay = "d-block";

        internal const string MessageAlert = "alert-primary";
        internal const string ErrorAlert = "alert-danger";

        [Parameter]
        public string Message { get; set; } = string.Empty;

        [Parameter]
        public string DisplayClass { get; set; } = NoDisplay;

        [Parameter]
        public string AlertClass { get; set; } = MessageAlert;

        public void ShowMessage(string message)
        {
            Show(message, MessageAlert);
        }

        public void ShowError(string message)
        {
            Show(message, ErrorAlert);
        }

        public void Hide()
        {
            DisplayClass = NoDisplay;
        }

        private void Show(string message, string alertClass)
        {
            Message = message;
            DisplayClass = BlockDisplay;
            AlertClass = alertClass;
            StateHasChanged();
        }
    }
}
