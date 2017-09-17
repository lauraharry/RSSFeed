using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RSSFeed
{
    class Program
    {
        static void Main(string[] args)
        {
            string feedDirectory = "C:\\Feeds";

            // Get new feed
            var feedModel = FeedHelper.ToFeedModel(FeedHelper.GetFeedItems("http://feeds.bbci.co.uk/news/uk/rss.xml"));

            if (Directory.Exists(feedDirectory))
            {
                // Get previous feeds of the day
                string partialName = DateTime.Now.ToString("yyyy-MM-dd");
                DirectoryInfo directoryToSearch = new DirectoryInfo(@feedDirectory);
                FileInfo[] filesInDir = directoryToSearch.GetFiles("*" + partialName + "*.*");

                // If there are no files for the day then all items are new
                List<FeedItem> newItems = filesInDir.Count() == 0 ? feedModel.Items : new List<FeedItem>();
                List<FeedItem> existingItems = new List<FeedItem>();

                foreach (FileInfo foundFile in filesInDir)
                {
                    existingItems.AddRange(JsonHelper.LoadJson(foundFile.FullName).Items.ToList());
                }
                foreach (FeedItem itm in feedModel.Items)
                {
                    if (!existingItems.Any(x => x.Title == itm.Title && x.Description == itm.Description && x.Link == itm.Link && x.PubDate == itm.PubDate))
                    {
                        newItems.Add(itm);
                    }
                }
                feedModel.Items = newItems;
            }           

            // Convert to Json and Save new feed
            string jsonFile = JsonConvert.SerializeObject(feedModel);
            JsonHelper.SaveJsonFile(feedDirectory, DateTime.Now.ToString("yyyy-MM-dd-hh"), jsonFile);
        }
    }
}
