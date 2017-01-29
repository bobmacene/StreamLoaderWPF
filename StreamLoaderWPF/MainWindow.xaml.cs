using System;
using System.Windows;
using mshtml;
using System.IO;
using Awesomium.Core;

namespace StreamLoaderWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var viewModel = new ViewModel();
            DataContext = viewModel;

            if (viewModel.BrowserBackAction == null) viewModel.BrowserBackAction = new Action(() => GoBack());
            if (viewModel.BrowserForwardAction == null) viewModel.BrowserForwardAction = new Action(() => GoFoward());
            if (viewModel.CurrentHtmlAction == null) viewModel.CurrentHtmlAction = new Action(() => GetHtml(viewModel));
            if (viewModel.CurrentUrlAction == null) viewModel.CurrentUrlAction = new Action(() => GetCurrentUrl(viewModel));
            if (viewModel.TitleAction == null) viewModel.TitleAction = new Action(() => viewModel.GetTitle());
        }

        private void GetCurrentUrl(ViewModel viewModel)
        {
            viewModel.CurrentUrl = browserXaml.Source.ToString();
        }

        private void GetHtml(ViewModel viewModel)
        {
            var doc = browserXaml.Document as HTMLDocument;
            if (doc != null) viewModel.HtmlSource = doc.documentElement.innerHTML;
        }

        private void GoBack()
        {
            if (browserXaml.CanGoBack) browserXaml.GoBack();
        }

        private void GoFoward()
        {
            if (browserXaml.CanGoForward) browserXaml.GoForward();
        }

        private void bar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            bar.Visibility = Visibility.Visible;

            bar.Value = e.NewValue;
        }
    }
}
