﻿namespace GMPA.Core.Models.ViewModels
{
    public class CountryViewModel : MainViewModel
    {
        public class NormalBlockListItem
        {
            public string Title { get; set; }
            public string BodyText { get; set; }
            public string Url { get; set; }
            public string ImageUrl { get; set; }
        }

        public class CollapsibleBlockListItem
        {
            public string Title { get; set; }
            public string BodyText { get; set; }
            public string ImageUrl { get; set; }
            public string Id { get; set; }
        }

        public CountryModel Country { get; set; }
        public List<NormalBlockListItem> NormalBlockList { get; set; }
        public List<CollapsibleBlockListItem> CollapsibleBlockList { get; set; }
    }
}
