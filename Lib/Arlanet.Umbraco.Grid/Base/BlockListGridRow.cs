using System.Collections.Generic;

namespace Arlanet.Umbraco.Grid.Base
{
    public class BlockListGridRow
    {
        public int PreviewId { get; set; }

        public List<BlockListGridColumn> Columns { get; set; }

        public object Settings { get; set; }
    }
}