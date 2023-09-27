using Arlanet.Umbraco.Grid.Base;
using Marketing.Core.Models.Umbraco;
using Marketing.Core.Services;

namespace Marketing.Core.Grid.Controls
{
    public abstract class SonicGridControl: GridControl
    {
        private readonly IRequestService _requestService;
        public Home Home { get; set; }
        public string Culture { get; set; }

        protected SonicGridControl(IRequestService requestService)
        {
            _requestService = requestService;
        }

        public void Initialize()
        {
            Home = _requestService.GetCacheItem<Home>(Constants.ApplicationCaches.RequestCache.Keys.Home);
            Culture = _requestService.GetCacheItem<string>(Constants.ApplicationCaches.RequestCache.Keys.Culture);
        }
    }
}
