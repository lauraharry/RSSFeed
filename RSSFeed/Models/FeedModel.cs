using System.Collections.Generic;

namespace RSSFeed
{
    public class FeedModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public List<FeedItem> Items { get; set; }
    }
}
