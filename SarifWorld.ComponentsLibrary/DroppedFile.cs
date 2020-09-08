namespace SarifWorld.ComponentsLibrary
{
    public class DroppedFile
    {
        public string Name { get; set; }

        public string Text { get; set; }

        public DroppedFile(string name, string text)
        {
            Name = name;
            Text = text;
        }
    }
}
