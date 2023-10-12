namespace Arlanet.Umbraco.Grid.Base
{
    public class BlockGridRow
    {
        public int PreviewId { get; set; }

        public List<BlockGridColumn> Columns { get; set; }

        public object Settings { get; set; }
    }
}