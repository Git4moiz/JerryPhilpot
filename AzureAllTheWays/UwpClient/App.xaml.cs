using Windows.ApplicationModel.Activation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using System;
using UwpClient.Helpers;

namespace UwpClient
{
    sealed partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.Launch)
            {
                var auth = new AuthMsaHelper();
                auth.LoginComplete += _authHelper_LoginComplete;
                auth.LoginFailed += _authHelper_LoginFailed;
                auth.Login();
            }
        }

        private void _authHelper_LoginComplete(object sender, EventArgs e)
        {
            if (Window.Current.Content is Views.ShellPage p)
            {
                // empty
            }
            else
            {
                Window.Current.Content = new Views.ShellPage();
                Window.Current.Activate();
            }
        }

        private async void _authHelper_LoginFailed(object sender, Windows.Security.Authentication.Web.Core.WebTokenRequestStatus e)
        {
            await new MessageDialog(e.ToString()).ShowAsync();
        }
    }
}
