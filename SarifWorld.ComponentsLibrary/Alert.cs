using Microsoft.JSInterop;

namespace SarifWorld.ComponentsLibrary
{
    public class Alert
    {
        private readonly IJSRuntime jsRuntime;

        public Alert(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public void Show(string message)
        {
            this.jsRuntime.InvokeVoidAsync("alert", message);
        }
    }
}
