using Umbraco.Cms.Core.Models.PublishedContent;

namespace Arlanet.Umbraco.Grid.Base
{
    public class BlockGridControl
    {
        public int PreviewId { get; set; }

        public string Alias { get; set; }

        public IPublishedElement Component { get; set; }
        public IPublishedElement Settings { get; set; }
    }
}