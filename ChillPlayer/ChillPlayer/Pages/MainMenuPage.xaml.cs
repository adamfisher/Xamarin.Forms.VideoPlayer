using ChillPlayer.Models;
using Octane.Xam.VideoPlayer.Constants;
using Octane.Xam.VideoPlayer.Licensing;
using System;
using Xamarin.Forms;

namespace ChillPlayer.Pages
{
    public partial class MainMenuPage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainMenuPage"/> class.
        /// </summary>
        public MainMenuPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            if (VideoPlayerLicense.LicenseType == LicenseType.Trial || Device.OS == TargetPlatform.Windows)
            {
                //YouTubeButton.IsVisible = false;
                //VimeoButton.IsVisible = false;
            }
        }

        /// <summary>
        /// Handles the OnClicked event of the button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void FullScreenVideoPlayerPage_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FullScreenVideoPlayerPage());
        }

        /// <summary>
        /// Handles the OnClicked event of the button control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void TopYouTubeVideoButton_OnClick(object sender, EventArgs e)
        {
            Navigation.PushAsync(new VideoPlayerQueuePage("Top YouTube Videos", Data.YouTubePlaylist));
        }

		/// <summary>
		/// Handles the OnClicked event of the button control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
		private void TopVimeoVideoButton_OnClick(object sender, EventArgs e)
		{
            Navigation.PushAsync(new VideoPlayerQueuePage("Top Vimeo Videos", Data.VimeoPlaylist));
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
            VideoPlayer.Play();

            // We need to hide the main menu splash screen video when navigating to a new page
            // due to the way Xamarin Forms layers pages on Android.
            if (Device.OS == TargetPlatform.Android)
                VideoPlayer.IsVisible = true;
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
            VideoPlayer.Pause();

            // We need to hide the main menu splash screen video when navigating to a new page
            // due to the way Xamarin Forms layers pages on Android.
            if (Device.OS == TargetPlatform.Android)
                VideoPlayer.IsVisible = false;
        }
    }
}
