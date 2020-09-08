using System.Collections.Generic;
using System.Linq;

namespace SarifWorld.App.Pages
{
    public partial class Validation
    {
        private int NumFiles { get; set; } = 0;

        public void HandleDroppedFiles(IEnumerable<string> paths)
        {
            NumFiles = paths != null ? paths.Count() : 0;
        }
    }
}
