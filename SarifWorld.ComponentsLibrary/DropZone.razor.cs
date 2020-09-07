using Microsoft.AspNetCore.Components;

namespace SarifWorld.ComponentsLibrary
{
    public partial class DropZone
    {
        [Parameter]
        public string Label { get; set; } = string.Empty;
    }
}
