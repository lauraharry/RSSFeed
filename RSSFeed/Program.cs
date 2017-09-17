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

                foreach (FileInfo foundFile in filesInDir)
                {
                    var existingItems = JsonHelper.LoadJson(foundFile.FullName).Items.ToList();
                    newItems.AddRange(feedModel.Items.Where(y => !existingItems.Any(x => x.Title == y.Title && x.Description == y.Description && x.PubDate == y.PubDate && x.Link == y.Link)));              
                }

                feedModel.Items = newItems;
            }           

            // Convert to Json and Save new feed
            string jsonFile = JsonConvert.SerializeObject(feedModel);
            JsonHelper.SaveJsonFile(feedDirectory, DateTime.Now.ToString("yyyy-MM-dd-hh"), jsonFile);
        }
    }
}
