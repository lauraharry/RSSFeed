using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;

namespace RSSFeed
{
    static class FeedHelper
    {
        public static SyndicationFeed GetFeedItems(string url)
        {
            // Get RSS feed
            XmlReader reader = XmlReader.Create(url);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();

            return feed;
        }

        public static FeedModel ToFeedModel(SyndicationFeed feed)
        {
            var items = feed.Items.Select(x => new FeedItem
            {
                Title = x.Title.Text.ToString(),
                Description = x.Summary.Text.ToString(),
                PubDate = x.PublishDate.DateTime,
                Link = x.Links.First().Uri.ToString()
            });

            var feedModel = new FeedModel
            {
                Title = feed.Title.Text.ToString(),
                Description = feed.Description.Text.ToString(),
                Items = items.ToList(),
                Link = feed.Links.First().Uri.ToString()
            };

            return feedModel;
        }

    }
}
