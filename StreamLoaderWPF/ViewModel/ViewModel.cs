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

        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }

        private string GetStreamName(string url)
        {
            if (_streamUrl.EndsWith("/")) _streamUrl = _streamUrl.Substring(0, _streamUrl.Length - 1);

            var index = url.LastIndexOf('/') + 1;

            return _streamUrl.Substring(index, _streamUrl.Length - index) + m4a;
        }

        private string _currentUrl;
        public string CurrentUrl
        {
            get
            {
                return _currentUrl;
            }
            set
            {
                _currentUrl = value;
                NotifyPropertyChanged("CurrentUrl");
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

        public Action BrowserBackAction { get; set; }
        public Action BrowserForwardAction { get; set; }
        public Action StreamAction { get; set; }
        public Action CurrentUrlAction { get; set; }
        public Action TitleAction { get; set; }

        private ICommand _currentUrlCommand;
        public ICommand CurrentUrlCommand
        {
            get { return _currentUrlCommand = (_currentUrlCommand = new CommandHandler(() => CurrentUrlAction(), _canExecute)); }
        }

        public string HtmlSource { get; set; }

        private ICommand _streamCommand;
        public ICommand StreamCommand
        {
            get { return _streamCommand ?? (_streamCommand = new CommandHandler(() => StreamAction(), _canExecute));  }
        }

        private ICommand _browserForward;
        public ICommand BrowserForwardCommand
        {
            get { return _browserForward ?? (_browserForward = new CommandHandler(() => BrowserForwardAction(), _canExecute)); }
        }

        private ICommand _browserBack;
        public ICommand BrowserBackCommand
        {
            get { return _browserBack ?? (_browserBack = new CommandHandler(() => BrowserBackAction(), _canExecute)); }
        }

        private ICommand _getStreamUrlCommand;
        public ICommand GetStreamUrlCommand
        {
            get { return _getStreamUrlCommand = (_getStreamUrlCommand = new CommandHandler(() => GetStreamUrl(), _canExecute)); }
        }

        private string _streamUrl;
        public string StreamUrl
        {
            get { return _streamUrl; }
            set
            {
                _streamUrl = value;
                NotifyPropertyChanged("StreamUrl");
            }
        }

        private string GetStreamUrl()
        {
            StreamAction();
            var load = new DownLoader();

            string RegexPattern = "(https://s.*.m4a)";

            return StreamUrl = load.GetPattern(HtmlSource, RegexPattern);
        }

        private ICommand _titleCommand;
        public ICommand TitleCommand
        {
            get { return _titleCommand = (_titleCommand = new CommandHandler(() => GetTitle(), _canExecute)); }
        }

        public string GetTitle()
        {
            StreamAction();
            var load = new DownLoader();

            string RegexPattern = "</style><title>(.*)</title><meta name=";

            return Title = load.GetPattern(HtmlSource, RegexPattern);
        }

        public void SaveStream()
        {
            var load = new DownLoader();

            StreamAction();
            string RegexPattern = "(https://s.*.m4a)";
            var streamUrl = load.GetPattern(HtmlSource, RegexPattern);

            var musicFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            savePath = Path.Combine(musicFolderPath, "Music", GetStreamName(streamUrl));
            load.LoadWebStream(streamUrl, savePath);
        }

        private ICommand _saveStreamCommand;
        public ICommand SaveStreamCommand
        {
            get { return _saveStreamCommand ?? (_saveStreamCommand = new CommandHandler(() => SaveStream(), _canExecute)); }

        }

        private string savePath;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
/*      
 *       //public void DownloadStream()
        //{
        //    var downLoadr = new DownLoader();
        //    var pageSource = downLoadr.GetPageSource(_chromeDriverPath, StreamUrl);

        //    string RegexPattern = "(https://s.*.m4a)";
        //    var streamUrl = downLoadr.GetStreamUrl(pageSource, RegexPattern);

        //    if (streamUrl == null)
        //    {
        //        RegexPattern = "(https://a.*.mp3)";
        //        streamUrl = downLoadr.GetStreamUrl(pageSource, RegexPattern);
        //    }

        //    DownloadText = "Downloading: " + streamUrl;

        //    var musicFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        //    savePath = Path.Combine(musicFolderPath, "Music", _streamName);
        //    downLoadr.LoadWebStream(streamUrl, savePath);

        //    DownloadText = "Downloaded: " + streamUrl;
        //}
 *      
 *      
 *        //private ICommand _loadStream;
        //private string _chromeDriverPath;

        //public ICommand LoadStream
        //{
        //    get
        //    {
        //        _chromeDriverPath = Path.GetDirectoryName(
        //        System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);

        //        return _loadStream ?? (_loadStream = new CommandHandler(() => DownloadStream(), _canExecute));
        //    }
        //}*/
