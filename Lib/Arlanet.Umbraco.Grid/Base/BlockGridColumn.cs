namespace Arlanet.Umbraco.Grid.Base
{
    public class BlockGridColumn
    {
        public int PreviewId { get; set; }

        public int Width { get; set; }

        public List<BlockGridControl> Controls { get; set; }

        public object Settings { get; set; }
    }
}