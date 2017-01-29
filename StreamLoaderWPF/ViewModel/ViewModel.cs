using System.ComponentModel;
using StreamDownloader;
using System;
using System.IO;
using System.Windows.Input;
using System.Windows;

namespace StreamLoaderWPF
{
    public class ViewModel : INotifyPropertyChanged
    {
        private const string m4a = ".m4a";
        private bool _canExecute;
        public ViewModel()
        {
            ProgressBarVisibility = Visibility.Collapsed;
            _canExecute = true;
        }

        public string HtmlSource { get; set; }

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


        public Action BrowserBackAction { get; set; }
        public Action BrowserForwardAction { get; set; }
        public Action CurrentHtmlAction { get; set; }
        public Action CurrentUrlAction { get; set; }
        public Action TitleAction { get; set; }


        private ICommand _currentUrlCommand;
        public ICommand CurrentUrlCommand
        {
            get { return _currentUrlCommand = (_currentUrlCommand = new CommandHandler(() => CurrentUrlAction(), _canExecute)); }
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

        private ICommand _saveStreamCommand;
        public ICommand SaveStreamCommand
        {
            get
            {
                //ProgressBarVisibility = Visibility.Visible;
                return _saveStreamCommand ?? (_saveStreamCommand = new CommandHandler(() => SaveStream(), _canExecute));
            }

        }

        private ICommand _titleCommand;
        public ICommand TitleCommand
        {
            get { return _titleCommand = (_titleCommand = new CommandHandler(() => GetTitle(), _canExecute)); }
        }

        private Visibility _progressBarVisibility;
        public Visibility ProgressBarVisibility
        {
            get { return _progressBarVisibility;}
            set
            {
                _progressBarVisibility = value;
                NotifyPropertyChanged("ProgressBarVisibility");
            }
        }

        private string GetStreamUrl()
        {
            CurrentHtmlAction();
            var load = new DownLoader();

            string RegexPattern = "(https://s.*.m4a)";

            return StreamUrl = load.GetPattern(HtmlSource, RegexPattern);
        }       

        public string GetTitle()
        {
            CurrentHtmlAction();

            string RegexPattern = "</style><title>(.*)</title><meta name=";

            return Title = new DownLoader().GetPattern(HtmlSource, RegexPattern);
        }

        private string savePath;
        public void SaveStream()
        {
            ProgressBarVisibility = Visibility.Visible;
            CurrentHtmlAction();
            
            var musicFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            TitleAction();
            savePath = Path.Combine(musicFolderPath, "Music", _title);

            CurrentUrlAction();
            new DownLoader().LoadWebStream(_streamUrl, savePath);
        }

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
