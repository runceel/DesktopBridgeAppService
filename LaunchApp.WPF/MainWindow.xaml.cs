using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;

namespace LaunchApp.WPF
{
    public partial class MainWindow : Window
    {
        private AppServiceConnection _appServiceConnection;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async Task<bool> ConnectAsync()
        {
            if (_appServiceConnection != null)
            {
                return true;
            }

            var appServiceConnection = new AppServiceConnection();
            appServiceConnection.AppServiceName = "InProcessAppService";
            appServiceConnection.PackageFamilyName = Package.Current.Id.FamilyName;
            appServiceConnection.RequestReceived += AppServiceConnection_RequestReceived;
            var r = await appServiceConnection.OpenAsync() == AppServiceConnectionStatus.Success;
            if (r)
            {
                _appServiceConnection = appServiceConnection;
            }

            return r;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!(await ConnectAsync()))
            {
                MessageBox.Show($"Failed");
                return;
            }

            var res = await _appServiceConnection.SendMessageAsync(new ValueSet
            {
                ["Input"] = inputTextBox.Text,
            });

            logTextBlock.Text = res.Message["Result"] as string;
        }

        private void AppServiceConnection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            void setText()
            {
                logTextBlock.Text = (string)args.Request.Message["Now"];
            }

            if (Dispatcher.CheckAccess())
            {
                setText();
            }
            else
            {
                Dispatcher.Invoke(() => setText());
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await ConnectAsync();
        }
    }
}
