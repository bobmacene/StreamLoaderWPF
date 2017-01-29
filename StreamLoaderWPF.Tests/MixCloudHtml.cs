using System.IO;
using System.Text;


namespace StreamLoaderWPF.Tests
{
    class MixCloudHtml
    {
        public string Html { get; set; }
        private const string path = @"C:\Users\bob\Documents\Visual Studio 2015\Projects\StreamLoaderWPF\StreamLoaderWPF.Tests\Html.txt";

        public MixCloudHtml()
        {
            Html = GetHtml();
        }

        public string GetHtml()
        {
            var lines = File.ReadAllLines(path);

            var str = new StringBuilder();

            foreach(var line in lines)
            {
                str.Append(line);
            }

            return str.ToString();
        }

            
    }
}
