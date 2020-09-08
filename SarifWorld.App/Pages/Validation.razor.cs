using SarifWorld.ComponentsLibrary;

namespace SarifWorld.App.Pages
{
    public partial class Validation
    {
        private const int HeadLength = 50;

        private int NumFiles { get; set; } = 0;
        private string Name { get; set; } = string.Empty;
        private string Head { get; set; } = string.Empty;

        public void HandleDroppedFile(DroppedFile droppedFile)
        {
            NumFiles++;
            Name = droppedFile.Name;
            Head = droppedFile.Text.Length <= HeadLength
                ? droppedFile.Text
                : droppedFile.Text.Substring(0, HeadLength) + "\u2026";
        }
    }
}
