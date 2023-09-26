using System.Collections.Generic;

namespace Arlanet.Umbraco.Grid.Base
{
    public class BlockListGrid
    {
        public bool Preview { get; set; }

        public List<BlockListGridRow> Rows { get; set; }
    }
}