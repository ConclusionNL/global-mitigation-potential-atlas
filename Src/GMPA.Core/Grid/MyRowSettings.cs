using GMPA.Core.Grid.Enums;

namespace GMPA.Core.Grid
{
    public class MyRowSettings
    {
        //Section
        public MyBackgroundSettings Background { get; set; }
        public string TextColor { get; set; }
        public List<string> CssClasses { get; set; }

        //Container
        public bool IsFullWidth { get; set; }
        public bool IsFullHeight { get; set; }

        //Row
        public string Anchor { get; set; }
        public Alignment HorizontalAlignment { get; set; }
        public Alignment VerticalAlignment { get; set; }
        public Padding PaddingTop { get; set; }
        public Padding PaddingBottom { get; set; }
    }
}
