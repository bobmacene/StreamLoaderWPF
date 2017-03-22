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
        private BackgroundWorker _worker;
        private int _progressPercentage;
        private string Output;
        private bool _startEnabled = true;
        private bool _cancelEnabled = false;

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
            CurrentHtmlAction();
            CurrentUrlAction();

            _worker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };

            _worker.DoWork += Worker_DoWork;
            //_worker.ProgressChanged += BackgroundWorker_ProgressChanged;
            _worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

            if (_worker.IsBusy)
            {
                _worker.CancelAsync();
            }


            Output = string.Empty;
            
            if(!_worker.IsBusy)
            {
                _worker.RunWorkerAsync();
            }

            _startEnabled = !_worker.IsBusy;
            _cancelEnabled = _worker.IsBusy;
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            ProgressBarVisibility = Visibility.Visible;
           

            var musicFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            TitleAction();
            savePath = Path.Combine(musicFolderPath, "Music", _title);

            new DownLoader().LoadWebStream(_streamUrl, savePath);

            ProgressBarVisibility = Visibility.Hidden;
        }


        public void CancelDownload()
        {
            _worker.CancelAsync();
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null) Output = e.Error.Message;
            else if (e.Cancelled) Output = "Cancelled";
            else
            {
                Output = e.Result.ToString();
                _progressPercentage = 0;
            }

            _startEnabled = !_worker.IsBusy;
            _cancelEnabled = _worker.IsBusy;

        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}