using System.Collections.Generic;

namespace Arlanet.Umbraco.Grid.Base
{
    public class RowData
    {
        public class ColumnData
        {
            public int Width { get; set; }
            public Dictionary<string, object> Settings { get; set; }
            public List<string> Blocks { get; set; }
        }

        public List<ColumnData> Columns { get; set; }
    }
}