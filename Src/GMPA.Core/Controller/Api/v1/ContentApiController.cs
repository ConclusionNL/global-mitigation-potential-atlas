using Arlanet.Umbraco.Grid.Base;
using GMPA.Core.Grid;
using GMPA.Core.Models.ApiModels;
using GMPA.Core.Models.Umbraco;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Web.Common.Controllers;

namespace GMPA.Core.Controller.Api.v1
{
    [Route("api/content")]
    [Produces("application/json")]
    public class ContentApiController : UmbracoApiController
    {
        private readonly IPublishedSnapshotAccessor _publishedSnapshotAccessor;
        private readonly BlockGridRenderer _blockGridRenderer;
        private readonly BlockGridControlResolver _blockGridControlResolver;

        public ContentApiController(
            IPublishedSnapshotAccessor publishedSnapshotAccessor,
            BlockGridRenderer blockGridRenderer,
            BlockGridControlResolver blockGridControlResolver
        )
        {
            _publishedSnapshotAccessor = publishedSnapshotAccessor;
            _blockGridRenderer = blockGridRenderer;
            _blockGridControlResolver = blockGridControlResolver;
        }

        [HttpGet("GetByGuid/{guid}")]
        [ProducesResponseType(typeof(ContentApiModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetContentByGuid(Guid guid)
        {
            _publishedSnapshotAccessor.TryGetPublishedSnapshot(out var publishedSnapshot);

            var content = publishedSnapshot?.Content?.GetById(guid);

            if (content == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }

            //TODO: Support different types

            if (content is IGrid grid)
            {
                var blockGridModel = _blockGridRenderer.MapBlockGridModel(grid.BlockGrid);

                return new JsonResult(new ContentApiModel
                {
                    Guid = content.Key,
                    Type = content.ContentType.Alias,
                    Body = new GridApiModel
                    {
                        Rows = blockGridModel.Rows
                            .Select(x => new GridRowApiModel
                            {
                                Settings = x.Settings as MyRowSettings,
                                Columns = x.Columns
                                    .Select(y => new GridColumnApiModel
                                    {
                                        Width = y.Width,
                                        Settings = x.Settings as MyColumnSettings,
                                        Controls = y.Controls
                                            .Select(z =>
                                            {
                                                var gridControl = _blockGridControlResolver.Get(z.Alias);

                                                if (gridControl == null)
                                                {
                                                    //TODO: Add fallback "missing" control
                                                    return null;
                                                }

                                                var result = gridControl.Render(z);

                                                var headlessResult = (IGridControlApiModel)result.Model;
                                                headlessResult.Alias = z.Alias;

                                                return headlessResult;
                                            })
                                            .ToList()
                                    })
                                    .ToList()
                            })
                            .ToList()
                    }
                });
            }

            return new JsonResult(new ContentApiModel
            {
                Guid = content.Key,
                Type = content.ContentType.Alias,
                Body = null
            });
        }
    }
}
