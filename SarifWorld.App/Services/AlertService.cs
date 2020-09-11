using Microsoft.JSInterop;

namespace SarifWorld.App.Services
{
    public class AlertService : IAlertService
    {
        private readonly IJSRuntime jsRuntime;

        public AlertService(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public void Show(string message)
        {
            this.jsRuntime.InvokeVoidAsync("alert", message);
        }
    }
}
