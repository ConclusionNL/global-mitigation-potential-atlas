using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Arlanet.Umbraco.Grid.Base
{
    public class BlockGridRenderer
    {
        public virtual string GetSectionClasses(BlockGridRow gridRow)
        {
            return string.Empty;
        }

        public virtual string GetSectionStyle(BlockGridRow gridRow)
        {
            return string.Empty;
        }

        public virtual string GetContainerClasses(BlockGridRow gridRow)
        {
            return string.Empty;
        }

        public virtual string GetContainerStyle(BlockGridRow gridRow)
        {
            return string.Empty;
        }

        public virtual string GetRowClasses(BlockGridRow gridRow)
        {
            return string.Empty;
        }

        public virtual string GetRowStyle(BlockGridRow gridRow)
        {
            return string.Empty;
        }

        public virtual string GetColumnClasses(BlockGridRow gridRow, BlockGridColumn gridColumn)
        {
            return string.Empty;
        }

        public virtual string GetColumnStyle(BlockGridRow gridRow, BlockGridColumn gridColumn)
        {
            return string.Empty;
        }

        public virtual BlockGrid MapBlockGridModel(BlockGridModel blockGridModel)
        {
            var rows = new List<BlockGridRow>();

            foreach (var row in blockGridModel)
            {
                var rowArea = row.Areas.FirstOrDefault();

                if (rowArea == null || !rowArea.Any())
                {
                    continue;
                }

                var columns = new List<BlockGridColumn>();

                foreach (var column in rowArea)
                {
                    var columnArea = column.Areas.FirstOrDefault();

                    if (columnArea == null || !columnArea.Any())
                    {
                        continue;
                    }

                    var controls = new List<BlockGridControl>();

                    foreach (var control in columnArea)
                    {
                        controls.Add(new BlockGridControl
                        {
                            Alias = control.Content.ContentType.Alias,

                            Component = control.Content,
                            Settings = control.Settings
                        });
                    }

                    columns.Add(CreateColumn(column.ColumnSpan, column.Settings, controls));
                }

                rows.Add(CreateRow(columns, row.Settings));
            }

            var blockListGrid = new BlockGrid
            {
                Rows = rows
            };

            return blockListGrid;
        }

        public virtual BlockGridRow CreateRow(List<BlockGridColumn> columns, IPublishedElement settings)
        {
            return new BlockGridRow
            {
                Columns = columns,

                Settings = settings
            };
        }

        public virtual BlockGridColumn CreateColumn(int width, IPublishedElement settings, List<BlockGridControl> controls)
        {
            return new BlockGridColumn
            {
                Width = width,

                Controls = controls,

                Settings = settings
            };
        }
    }
}
