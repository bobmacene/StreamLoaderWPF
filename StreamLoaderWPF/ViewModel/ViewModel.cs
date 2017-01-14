using System.ComponentModel;
using StreamDownloader;
using System;
using System.IO;
using System.Windows.Input;

namespace StreamLoaderWPF
{
    public class ViewModel : INotifyPropertyChanged
    {
        private const string m4a = ".m4a";
        private bool _canExecute;
        public ViewModel()
        {
            _canExecute = true;
        }

        private string _streamUrl;
        public string StreamUrl
        {
            get { return _streamUrl; }
            set
            {
                _streamUrl = value;

                if (_streamUrl.EndsWith("/")) _streamUrl = _streamUrl.Substring(0, _streamUrl.Length - 1);

                var index = _streamUrl.LastIndexOf('/') + 1;

                StreamName = _streamUrl.Substring(index, _streamUrl.Length - index) + m4a;
                
                NotifyPropertyChanged("StreamUrl");
            }
        }

        private string _streamName;
        public string StreamName
        {
            get { return _streamName; }
            set
            {
                _streamName = value;
                NotifyPropertyChanged("StreamName");
            }
        }

        private string _downloadingText;
        public string DownloadText
        {
            get { return _downloadingText; }
            set
            {
                _downloadingText = value;
                NotifyPropertyChanged("DownloadText");
            }
        }

        private ICommand _loadStream;
        private string _chromeDriverPath;  
        public ICommand LoadStream
        {
            get
            {
                _chromeDriverPath = Path.GetDirectoryName(
                System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);

                return _loadStream ?? (_loadStream = new CommandHandler(() => Downloadstream(), _canExecute));
            }
        }

        private string savePath;
        public void Downloadstream()
        {
            var downLoadr = new DownLoader();
            var pageSource = downLoadr.GetPageSource(_chromeDriverPath, StreamUrl);

            const string RegexPattern = "(https://s.*.m4a)";
            var streamUrl = downLoadr.GetStreamUrl(pageSource, RegexPattern);

            DownloadText = "Downloading: " + streamUrl;

            var musicFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            savePath = Path.Combine(musicFolderPath, "Music", _streamName);
            downLoadr.LoadWebStream(streamUrl, savePath);

            DownloadText = "Downloaded: " + streamUrl;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
