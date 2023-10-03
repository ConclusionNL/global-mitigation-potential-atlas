using System.Collections.Generic;
using System.Linq;
using Arlanet.Umbraco.Grid.Other;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Crmf;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace Arlanet.Umbraco.Grid.Base
{
    public class BlockGridRenderer
    {
        private readonly IPublishedValueFallback _publishedValueFallback;
        private readonly BlockEditorConverter _blockEditorConverter;

        public BlockGridRenderer(
            IPublishedValueFallback publishedValueFallback,
            BlockEditorConverter blockEditorConverter,
            IContentTypeService contentTypeService,
            IOptions<BlockListGridSettings> blockListGridSettings
        )
        {
            _publishedValueFallback = publishedValueFallback;
            _blockEditorConverter = blockEditorConverter;

            Initialize(blockListGridSettings.Value, contentTypeService);
        }

        private void Initialize(BlockListGridSettings blockListGridSettings, IContentTypeService contentTypeService)
        {
            //Do nothing
        }

        public virtual string GetSectionClasses(BlockListGridRow gridRow)
        {
            return string.Empty;
        }

        public virtual string GetSectionStyle(BlockListGridRow gridRow)
        {
            return string.Empty;
        }

        public virtual string GetContainerClasses(BlockListGridRow gridRow)
        {
            return string.Empty;
        }

        public virtual string GetContainerStyle(BlockListGridRow gridRow)
        {
            return string.Empty;
        }

        public virtual string GetRowClasses(BlockListGridRow gridRow)
        {
            return string.Empty;
        }

        public virtual string GetRowStyle(BlockListGridRow gridRow)
        {
            return string.Empty;
        }

        public virtual string GetColumnClasses(BlockListGridRow gridRow, BlockListGridColumn gridColumn)
        {
            return string.Empty;
        }

        public virtual string GetColumnStyle(BlockListGridRow gridRow, BlockListGridColumn gridColumn)
        {
            return string.Empty;
        }

        public virtual BlockListGrid MapBlockListModel(BlockGridModel blockGridModel)
        {
            var rows = new List<BlockListGridRow>();

            foreach (var row in blockGridModel)
            {
                var rowArea = row.Areas.FirstOrDefault();

                if (rowArea == null || !rowArea.Any())
                {
                    continue;
                }

                var columns = new List<BlockListGridColumn>();

                foreach (var column in rowArea)
                {
                    var columnArea = column.Areas.FirstOrDefault();

                    if (columnArea == null || !columnArea.Any())
                    {
                        continue;
                    }

                    var controls = new List<BlockListGridControl>();

                    foreach (var control in columnArea)
                    {
                        controls.Add(new BlockListGridControl
                        {
                            Render = control.Content.ContentType.Alias,

                            Component = control.Content,
                            Settings = control.Settings
                        });
                    }

                    columns.Add(CreateColumn(column.ColumnSpan, column.Settings, controls));
                }

                rows.Add(CreateRow(columns, row.Settings));
            }

            var blockListGrid = new BlockListGrid
            {
                Rows = rows
            };

            return blockListGrid;
        }

        public virtual BlockListGridRow CreateRow(List<BlockListGridColumn> columns, IPublishedElement settings)
        {
            return new BlockListGridRow
            {
                Columns = columns,

                Settings = settings
            };
        }

        public virtual BlockListGridColumn CreateColumn(int width, IPublishedElement settings, List<BlockListGridControl> controls)
        {
            return new BlockListGridColumn
            {
                Width = width,

                Controls = controls,

                Settings = settings
            };
        }
    }
}
