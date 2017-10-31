using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UwpClient.Models;
using UwpClient.Services;
using Windows.Foundation;
using Windows.Security.Authentication.Web;
using Windows.Security.Authentication.Web.Core;
using Windows.Security.Authentication.Web.Provider;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.UI.ApplicationSettings;

namespace UwpClient.Helpers
{
    // https://cloudblogs.microsoft.com/enterprisemobility/2015/08/03/develop-windows-universal-apps-with-azure-ad-and-the-windows-10-identity-api/
    // If you are targeting Windows 10’s Universal Windows Platform, use the WebAccountManager.
    // https://docs.microsoft.com/en-us/windows/uwp/security/web-account-manager

    class AuthMsaHelper : IDisposable
    {
        private SettingsService _settings;

        public AuthMsaHelper()
        {
            AccountsSettingsPane.GetForCurrentView().AccountCommandsRequested += BuildPaneAsync;
            _settings = new SettingsService();
        }

        public void Dispose()
        {
            AccountsSettingsPane.GetForCurrentView().AccountCommandsRequested -= BuildPaneAsync;
        }

        public User User { get; private set; }

        public event EventHandler LoginComplete;

        private void RaiseLoginComplete()
        {
            Debug.WriteLine($"LoginComplete: {User.UserName}");
            LoginComplete?.Invoke(this, EventArgs.Empty);
        }

        public event TypedEventHandler<object, WebTokenRequestStatus> LoginFailed;

        private void RaiseLoginFailed(WebTokenRequestStatus e)
        {
            Debug.WriteLine($"LoginFailed: {e}");
            LoginFailed?.Invoke(this, e);
        }

        public async void Login()
        {
            if (await TryGetTokenSilentlyAsync())
            {
                LoginComplete?.Invoke(this, EventArgs.Empty);
                return;
            }
            else
            {
                AccountsSettingsPane.Show();
            }
        }

        private async void BuildPaneAsync(AccountsSettingsPane sender, AccountsSettingsPaneCommandsRequestedEventArgs e)
        {
            var deferral = e.GetDeferral();
            try
            {
                var provider = await WebAuthenticationCoreManager.FindAccountProviderAsync("https://login.microsoft.com", "consumers");
                var command = new WebAccountProviderCommand(provider, GetMsaTokenAsync);
                e.WebAccountProviderCommands.Add(command);
            }
            finally
            {
                deferral.Complete();
            }
        }

        private async void GetMsaTokenAsync(WebAccountProviderCommand command)
        {
            var request = new WebTokenRequest(command.WebAccountProvider, "wl.basic");
            var result = await WebAuthenticationCoreManager.RequestTokenAsync(request);
            if (result.ResponseStatus == WebTokenRequestStatus.Success)
            {
                await ProcessSuccessAsync(result);
                RaiseLoginComplete();
            }
            else
            {
                // If you receive an error when requesting a token, make sure you've associated your app with the Store as described in step one. Your app won't be able to get a token if you skipped this step.
                RaiseLoginFailed(result.ResponseStatus);
            }
        }

        private async Task ProcessSuccessAsync(WebTokenRequestResult result)
        {
            var token = result.ResponseData[0].Token;
            var account = result.ResponseData[0].WebAccount;
            await SetUser(account);
            PersistCache(account);
        }

        private async Task SetUser(WebAccount account)
        {
            User = new User
            {
                Id = account.Id,
                State = account.State,
                UserName = account.UserName,
                Picture = await account.WebAccountProvider.User.GetPictureAsync(Windows.System.UserPictureSize.Size208x208),
            };
        }

        private void PersistCache(WebAccount account)
        {
            CurrentUserProviderId = account.WebAccountProvider.Id;
            CurrentUserId = account.Id;
        }

        private string CurrentUserProviderId
        {
            get => ApplicationData.Current.LocalSettings.Values[nameof(CurrentUserProviderId)]?.ToString();
            set => ApplicationData.Current.LocalSettings.Values[nameof(CurrentUserProviderId)] = value;
        }

        private string CurrentUserId
        {
            get => ApplicationData.Current.LocalSettings.Values[nameof(CurrentUserId)]?.ToString();
            set => ApplicationData.Current.LocalSettings.Values[nameof(CurrentUserId)] = value;
        }

        private async Task<bool> TryGetTokenSilentlyAsync()
        {
            if (null == CurrentUserProviderId || null == CurrentUserId)
            {
                return false;
            }

            var provider = await WebAuthenticationCoreManager.FindAccountProviderAsync(CurrentUserProviderId);
            var account = await WebAuthenticationCoreManager.FindAccountAsync(provider, CurrentUserId);
            var request = new WebTokenRequest(provider, "wl.basic");
            var result = await WebAuthenticationCoreManager.GetTokenSilentlyAsync(request, account);

            if (result.ResponseStatus == WebTokenRequestStatus.UserInteractionRequired)
            {
                return false;
            }
            else if (result.ResponseStatus == WebTokenRequestStatus.Success)
            {
                await ProcessSuccessAsync(result);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
