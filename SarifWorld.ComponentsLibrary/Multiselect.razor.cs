using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace SarifWorld.ComponentsLibrary
{
    public partial class Multiselect
    {
        [Parameter]
        public List<string> Options { get; set; } = new List<string>();

        public void SetOptions(List<string> options)
        {
            Options = options;
            StateHasChanged();
        }
    }
}
