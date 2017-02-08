using ChillPlayer.MarkupExtensions;
using ChillPlayer.Models;
using System.Collections.Generic;
using Octane.Xam.VideoPlayer.Constants;
using Octane.Xam.VideoPlayer.Events;
using Xamarin.Forms;

namespace ChillPlayer.Pages
{
    public partial class VideoPlayerQueuePage : ContentPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPlayerQueuePage" /> class.
        /// </summary>
        /// <param name="pageTitle">The page title.</param>
        /// <param name="playlist">The playlist.</param>
        public VideoPlayerQueuePage(string pageTitle, IEnumerable<PlaylistItem> playlist)
        {
            InitializeComponent();

            Title = pageTitle;
            NavigationPage.SetHasNavigationBar(this, true);
            Playlist.ItemsSource = playlist;
        }

        /// <summary>
        /// Handles the OnPlayerStateChanged event of the VideoPlayer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="VideoPlayerStateChangedEventArgs"/> instance containing the event data.</param>
        private void VideoPlayer_OnPlayerStateChanged(object sender, VideoPlayerStateChangedEventArgs e)
        {
            switch (e.CurrentState)
            {
                case PlayerState.Paused:
                case PlayerState.Prepared:
                case PlayerState.Completed:
                case PlayerState.Initialized:
                    PauseButton.IsVisible = false;
                    PlayButton.IsVisible = true;
                    break;
                default:
                    PlayButton.IsVisible = false;
                    PauseButton.IsVisible = true;
                    break;
            }
        }

		/// <summary>
		/// Detects taps on the playlist.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">The <see cref="ItemTappedEventArgs"/> instance containing the event data.</param>
		/// <exception cref="System.NotImplementedException"></exception>
		private void Playlist_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
            if (e.SelectedItem != null)
            {
                var item = e.SelectedItem as PlaylistItem;

                switch (item.VideoType)
                {
                    case VideoType.YouTube:
                        VideoPlayer.Source = YouTubeVideoIdExtension.Convert(item.VideoPath);
                        break;
                    case VideoType.Vimeo:
                        VideoPlayer.Source = VimeoVideoIdExtension.Convert(item.VideoPath);
                        break;
                    default:
                        VideoPlayer.Source = item.VideoPath;
                        break;
                }

                ((ListView)sender).SelectedItem = null; // de-select the row
            }
        }
    }
}
