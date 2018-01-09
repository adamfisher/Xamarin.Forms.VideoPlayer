using Foundation;
using Octane.Xamarin.Forms.VideoPlayer.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace ChillPlayer.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();
            FormsVideoPlayer.Init();
            LoadApplication(new App());
            return base.FinishedLaunching(app, options);
        }
    }
}
