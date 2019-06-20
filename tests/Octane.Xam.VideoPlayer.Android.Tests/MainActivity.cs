using System.Reflection;
using Android.App;
using Android.OS;
using Xamarin.Android.NUnitLite;
using Xamarin.Forms;

namespace Octane.Xam.VideoPlayer.Android.Tests
{
    [Activity(Label = "Octane.Xam.VideoPlayer.Android.Tests", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : TestSuiteActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            // tests can be inside the main assembly
            AddTest(Assembly.GetExecutingAssembly());
            // or in any reference assemblies
            // AddTest (typeof (Your.Library.TestClass).Assembly);

            Forms.Init(ApplicationContext, bundle);

            // Once you called base.OnCreate(), you cannot add more assemblies.
            base.OnCreate(bundle);
        }
    }
}

