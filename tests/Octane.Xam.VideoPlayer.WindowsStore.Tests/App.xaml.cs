using Windows.ApplicationModel.Activation;
using Xamarin.Forms;
using Application = Windows.UI.Xaml.Application;

namespace Octane.Xam.VideoPlayer.WindowsStore.Tests
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Forms.Init(args);
            base.OnLaunched(args);
        }
    }
}
