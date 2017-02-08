using Xamarin.Forms;

namespace ChillPlayer.Pages
{
    /// <summary>
    /// Example of a video player that expands to fill the entire page.
    /// </summary>
    public partial class FullScreenVideoPlayerPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FullScreenVideoPlayerPage"/> class.
        /// </summary>
        public FullScreenVideoPlayerPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When overridden, allows application developers to customize behavior immediately prior to the <see cref="T:Xamarin.Forms.Page" /> becoming visible.
        /// </summary>
        /// <remarks>
        /// To be added.
        /// </remarks>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            videoPlayer.Play();

            // We need to hide the main menu splash screen video when navigating to a new page
            // due to the way Xamarin Forms layers pages on Android.
            if (Device.OS == TargetPlatform.WinPhone)
                videoPlayer.IsVisible = true;
        }

        /// <summary>
        /// When overridden, allows the application developer to customize behavior as the <see cref="T:Xamarin.Forms.Page" /> disappears.
        /// </summary>
        /// <remarks>
        /// To be added.
        /// </remarks>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            videoPlayer.Pause();

            // We need to hide the main menu splash screen video when navigating to a new page
            // due to the way Xamarin Forms layers pages on Android.
            if (Device.OS == TargetPlatform.WinPhone)
                videoPlayer.IsVisible = false;
        }
    }
}
