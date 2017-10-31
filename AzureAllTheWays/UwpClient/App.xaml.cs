using Windows.ApplicationModel.Activation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using System;
using UwpClient.Helpers;

namespace UwpClient
{
    sealed partial class App : Application
    {
        private AuthMsaHelper _authHelper;

        public App()
        {
            _authHelper = new AuthMsaHelper();
            _authHelper.LoginComplete += _authHelper_LoginComplete;
            _authHelper.LoginFailed += _authHelper_LoginFailed;
        }

        private void _authHelper_LoginComplete(object sender, EventArgs e)
        {
            if (Window.Current.Content is Views.ShellPage p)
            {
                Window.Current.Activate();
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

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.Launch)
            {
                _authHelper.Login();
            }
        }
    }
}
