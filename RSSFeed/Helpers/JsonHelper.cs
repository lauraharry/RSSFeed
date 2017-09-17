using Newtonsoft.Json;
using System.IO;

namespace RSSFeed
{
    static class JsonHelper
    {
        public static FeedModel LoadJson(string filename)
        {
            FeedModel feed = null;
            using (StreamReader file = File.OpenText(@filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                feed = (FeedModel)serializer.Deserialize(file, typeof(FeedModel));
            }
            return feed;
        }

        public static void SaveJsonFile(string directory, string fileName, string jsonFile)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            using (var fileStream = new FileStream(string.Format(directory + "\\{0}.json", fileName), FileMode.OpenOrCreate))

            using (StreamWriter str = new StreamWriter(fileStream))
            {
                str.Write(jsonFile);
            }
        }
    }
}
