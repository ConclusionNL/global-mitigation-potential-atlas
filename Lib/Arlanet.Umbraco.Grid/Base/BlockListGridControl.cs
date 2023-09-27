using Umbraco.Cms.Core.Models.PublishedContent;

namespace Arlanet.Umbraco.Grid.Base
{
    public class BlockListGridControl
    {
        public int PreviewId { get; set; }

        public string Render { get; set; }

        public IPublishedElement Component { get; set; }
        public IPublishedElement Settings { get; set; }
    }
}