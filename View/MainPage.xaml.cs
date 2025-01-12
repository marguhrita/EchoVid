namespace EchoVid.View;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void TikTokLogin_Clicked(object sender, EventArgs e)
    {
        Console.WriteLine("hi");

        if (BindingContext is Model.Platforms.TikTok platform)
        {
            platform.Authenticate();
            // Navigate to the specified URL in the system browser.
            await Launcher.Default.OpenAsync(platform.OAUTH_Endpoint);
        }
    }


}
