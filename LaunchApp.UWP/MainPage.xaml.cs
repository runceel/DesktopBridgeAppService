using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LaunchApp.UWP
{
    public sealed partial class MainPage : Page
    {
        public static MainPage Current { get; private set; }

        public MainPage()
        {
            this.InitializeComponent();
            Current = this;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
        }

        public async Task SetTextAsync(string text)
        {
            void setText()
            {
                textBlock.Text = text;
            }

            if (Dispatcher.HasThreadAccess)
            {
                setText();
            }
            else
            {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    setText();
                });
            }
        }

        private async void Button_Click2(object sender, RoutedEventArgs e)
        {
            await App.Current.SendNowAsync();
        }
    }
}
