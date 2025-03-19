using Microsoft.SemanticKernel;
using SimpleFeedReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKDemo
{
    public class NewsPlugin
    {
        [KernelFunction("get_news")]
        [Description("Gets news items for today's date.")]
        [return: Description("A list of current news stories.")]
        public async Task<List<FeedItem>> GetNews(Kernel kernel, string category)
        {
            var reader = new FeedReader();
            return (await reader.RetrieveFeedAsync($"https://rss.nytimes.com/services/xml/rss/nyt/{category}.xml"))
                .Take(5)
                .ToList();
        }
    }
}
