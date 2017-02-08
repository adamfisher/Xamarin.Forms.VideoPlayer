using Android.App;
using Android.Content.PM;
using Android.OS;
using Octane.Xam.VideoPlayer.Android;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace ChillPlayer.Android
{
    [Activity(Label = "Chill Player", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Forms.Init(this, bundle);
			FormsVideoPlayer.Init();

            Forms.ViewInitialized += (sender, e) => {
				if (!string.IsNullOrWhiteSpace(e.View.StyleId)) {
					e.NativeView.ContentDescription = e.View.StyleId;
				}
			};

            LoadApplication(new App());
        }
    }
}

