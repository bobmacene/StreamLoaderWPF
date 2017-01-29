
using NUnit.Framework;

namespace StreamLoaderWPF.Tests
{
    [TestFixture]
    public class UnitTest1
    {
        [SetUp]
        public void SetUp()
        {
            mixCloudHtml = new MixCloudHtml();
        }

        MixCloudHtml mixCloudHtml;

        [Test]
        public void GetNameTest()
        {
            mixCloudHtml = new MixCloudHtml();

         
            //var htmlDocument = new HtmlAgilityPack.HtmlDocument();
            //htmlDocument.LoadHtml(mixCloudHtml.Html);

            //var name = htmlDocument.Load(;

            

        }
    }
}
