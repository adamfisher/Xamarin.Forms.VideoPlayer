using Octane.Xam.VideoPlayer;
using Xamarin.Forms;

namespace ChillPlayer.Models
{
	public class PlaylistItem
	{
		public string Title { get; set; }
		public string Author { get; set; }
		public ImageSource ThumbnailPath { get; set; }
		public string VideoPath { get; set; }
		public VideoType VideoType { get; set; }

		public PlaylistItem (string title, string author, ImageSource thumbnailPath, string videoPath, VideoType videoType)
		{
			Title = title;
			Author = author;
			ThumbnailPath = thumbnailPath;
			VideoPath = videoPath;
			VideoType = videoType;
		}
	}
}

