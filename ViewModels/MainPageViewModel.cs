using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EchoVid.Model.Platforms;

namespace EchoVid.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly TikTok _tikTokApiService;

        public MainPageViewModel()
        {
            _tikTokApiService = new TikTok();
        }

        private string loginStatus;
        public string LoginStatus
        {
            get => loginStatus;
            set => SetProperty(ref loginStatus, value);
        }

        private string responseCode;
        public string ResponseCode
        {
            get => responseCode;
            set => SetProperty(ref responseCode, value);
        }


        [RelayCommand]
        private async Task FetchLoginInformation()
        {
            var tikTokResponse = await _tikTokApiService.Authenticate();
            if (tikTokResponse != false)
            {
                loginStatus = "Logged in!";
            }
        }
    }
}
