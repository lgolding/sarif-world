using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace SarifWorld.ComponentsLibrary
{
    public partial class DropZone : IDisposable
    {
        [Parameter]
        public string Label { get; set; } = string.Empty;

        // If you have more than one dropZone on a page, provide each with its own
        // Id parameter.
        [Parameter]
        public string Id { get; set; } = "dropZone";

        [Parameter]
        public EventCallback<IEnumerable<string>> OnFilesDropped { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        private DotNetObjectReference<DropZone> thisReference;

        protected override void OnInitialized()
        {
            this.thisReference = DotNetObjectReference.Create(this);
            JSRuntime.InvokeVoidAsync("setCallbackTarget", this.Id, this.thisReference);
        }

        [JSInvokable]
        public void HandleDroppedFiles(int count, string name, string text)
        {
            OnFilesDropped.InvokeAsync(new List<string> { name });
        }

        public void Dispose()
        {
            if (this.thisReference != null)
            {
                this.thisReference.Dispose();
                this.thisReference = null;
            }
        }
    }
}
