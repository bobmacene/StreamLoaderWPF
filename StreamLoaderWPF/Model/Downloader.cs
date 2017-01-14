using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using OpenQA.Selenium.Chrome;

namespace StreamDownloader
{
    public class DownLoader
    {
        public void LoadWebStream(string streamUrl, string loadPath)
        {
            var webReq = WebRequest.Create(streamUrl);

            using (var webResponse = webReq.GetResponse().GetResponseStream())
            {
                var memory = new MemoryStream();

                webResponse?.CopyTo(memory);

                File.WriteAllBytes(loadPath, memory.ToArray());
            }
        }

        public string GetPageSource(string chromeDriverPath, string mixCloudUrl)
        {
            Environment.SetEnvironmentVariable("webdriver.chrome.driver", chromeDriverPath);

            var driver = new ChromeDriver();

            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(3));

            driver.Navigate().GoToUrl(mixCloudUrl);

            return driver.PageSource;
        }

        public string GetStreamUrl(string html, string pattern)
        {
            var regex = new Regex(pattern);

            var match = regex.Match(html);

            return match.Groups[1].Value;
        }

    }
}