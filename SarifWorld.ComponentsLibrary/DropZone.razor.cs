using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;

namespace SarifWorld.ComponentsLibrary
{
    public partial class DropZone : IDisposable
    {
        internal const string DefaultId = "dropZone";
        internal const int DefaultCompleteLabelDisplayTime = 0;
        internal const bool DefaultAllowMultiple = true;

        // If you have more than one dropZone on a page, provide each with its own
        // Id parameter.
        [Parameter]
        public string Id { get; set; } = DefaultId;

        [Parameter]
        public string DefaultLabel { get; set; }

        [Parameter]
        public string BusyLabel { get; set; }

        [Parameter]
        public string CompleteLabel { get; set; }

        [Parameter]
        public int CompleteLabelDisplayTime { get; set; } = DefaultCompleteLabelDisplayTime;

        [Parameter]
        public bool AllowMultiple { get; set; } = DefaultAllowMultiple;

        [Parameter]
        public EventCallback<DroppedFile> OnFileDropped { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public IStringLocalizer<DropZone> Localizer { get; set; }

        private DotNetObjectReference<DropZone> thisReference;
        private string label;

        protected override void OnInitialized()
        {
            if (DefaultLabel == null) { DefaultLabel = Localizer["DefaultLabel"]; }
            if (BusyLabel == null) { BusyLabel = Localizer["BusyLabel"]; }
            if (CompleteLabel == null) { CompleteLabel = Localizer["CompleteLabel"]; }
            label = DefaultLabel;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.InvokeVoidAsync("setAlertMessages", Localizer["ErrorMultipleFilesDropped"].Value);

                this.thisReference = DotNetObjectReference.Create(this);
                await JSRuntime.InvokeVoidAsync("setCallbackTarget", Id, this.thisReference, AllowMultiple);
            }

            string textClass =
                label == BusyLabel
                ? "drop-area-text-busy"
                : label == CompleteLabel
                ? "drop-area-text-complete"
                : "drop-area-text-input";

            await JSRuntime.InvokeVoidAsync("setDropZoneClass", Id, textClass);

            await base.OnAfterRenderAsync(firstRender);
        }

        [JSInvokable]
        public async Task HandleDroppedFile(string name, string text)
        {
            label = BusyLabel;
            StateHasChanged();

            await OnFileDropped.InvokeAsync(new DroppedFile(name, text));

            label = CompleteLabel;
            StateHasChanged();

            await Task.Delay(CompleteLabelDisplayTime);

            label = DefaultLabel;
            StateHasChanged();
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
