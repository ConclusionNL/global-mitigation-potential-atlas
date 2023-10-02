using System;
using System.Collections.Generic;
using System.Linq;
using Arlanet.Umbraco.Grid.Other;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Blocks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace Arlanet.Umbraco.Grid.Base
{
    public class BlockListGridRenderer
    {
        private readonly IPublishedValueFallback _publishedValueFallback;
        private readonly BlockEditorConverter _blockEditorConverter;

        private Guid _columnSettingsKey;
        private Guid _columnSettingsContentKey;

        public BlockListGridRenderer(
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
            _columnSettingsKey = Guid.NewGuid();
            //_columnSettingsContentKey = columnSettingsContentType.Key;
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

        public virtual BlockListGrid MapBlockListModel(BlockListModel blockListModel)
        {
            var rows = blockListModel
                .Select(x => new
                {
                    Data = x.Content
                        .GetProperty("data")
                        .Value(_publishedValueFallback) as string,
                    Components = x.Content
                        .GetProperty("controls")
                        .Value(_publishedValueFallback) as BlockListModel,
                    x.Settings
                })
                .ToList();

            var rowViewModels = new List<BlockListGridRow>();

            foreach (var row in rows)
            {
                var rowData = JsonConvert.DeserializeObject<RowData>(row.Data);

                if (rowData?.Columns == null)
                {
                    continue;
                }

                var columns = rowData.Columns
                    .Select(column =>
                    {
                        if (column.Blocks == null)
                        {
                            return null;
                        }

                        var settings = CreateSettings(column.Settings);

                        var controls = column.Blocks
                            .Select(y =>
                            {
                                var udi = UdiParser.Parse(y);
                                var component = row.Components.FirstOrDefault(z => z.ContentUdi == udi);

                                if (component == null)
                                {
                                    return null;
                                }

                                return new BlockListGridControl
                                {
                                    Render = component.Content.ContentType.Alias,

                                    Component = component.Content,
                                    Settings = component.Settings
                                };
                            })
                            .Where(y => y != null)
                            .ToList();

                        return CreateColumn(column.Width, settings, controls);
                    })
                    .Where(x => x != null)
                    .ToList();

                rowViewModels.Add(CreateRow(columns, row.Settings));
            }

            var blockListGrid = new BlockListGrid
            {
                Rows = rowViewModels
            };

            return blockListGrid;
        }

        private IPublishedElement CreateSettings(Dictionary<string, object> properties)
        {
            properties.Add("key", _columnSettingsKey);

            var blockItemData = new BlockItemData
            {
                ContentTypeKey = _columnSettingsContentKey,
                RawPropertyValues = properties
            };

            //TODO: Figure out how to trigger an Element change for this block?
            return _blockEditorConverter.ConvertToElement(blockItemData, PropertyCacheLevel.Snapshot, false);
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
