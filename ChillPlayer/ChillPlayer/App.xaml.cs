using ChillPlayer.Pages;
using Xamarin.Forms;

namespace ChillPlayer
{
    /// <summary>
    /// Application starting point.
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainMenuPage());
        }
    }
}
