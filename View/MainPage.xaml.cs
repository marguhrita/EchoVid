using EchoVid.ViewModels;

namespace EchoVid.View;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = new MainPageViewModel(); // Set the ViewModel as the BindingContext
    
    }

    private async void TikTokLogin_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is Model.Platforms.TikTok platform)
        {
            //await platform.Authenticate();
            //await Launcher.Default.OpenAsync(platform.RequestOauth().ToString());
            
        }
    }


}
