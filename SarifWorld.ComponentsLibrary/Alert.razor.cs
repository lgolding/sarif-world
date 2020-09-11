using Microsoft.AspNetCore.Components;

namespace SarifWorld.ComponentsLibrary
{
    public partial class Alert
    {
        [Parameter]
        public string Message { get; set; } = string.Empty;
    }
}
