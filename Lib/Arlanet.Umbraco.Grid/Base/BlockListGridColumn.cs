using System.Collections.Generic;

namespace Arlanet.Umbraco.Grid.Base
{
    public class BlockListGridColumn
    {
        public int PreviewId { get; set; }

        public int Width { get; set; }

        public List<BlockListGridControl> Controls { get; set; }

        public object Settings { get; set; }
    }
}