using Android.App;
using Android.Content.PM;
using Android.OS;
using Octane.Xamarin.Forms.VideoPlayer;
using Octane.Xamarin.Forms.VideoPlayer.Android;
using Xamarin.Forms;

namespace ChillPlayer.Droid
{
    [Activity(Label = "Octane.Xamarin.Forms.VideoPlayer", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Forms.Init(this, bundle);
            FormsVideoPlayer.Init();
            LoadApplication(new App());
        }
    }
}

