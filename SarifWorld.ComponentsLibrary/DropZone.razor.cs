using Microsoft.AspNetCore.Components;

namespace SarifWorld.ComponentsLibrary
{
    public partial class DropZone
    {
        [Parameter]
        public string Label { get; set; } = string.Empty;

        // If you have more than one dropZone on a page, provide each with its own
        // Id parameter.
        [Parameter]
        public string Id { get; set; } = "dropZone";
    }
}
