using Octane.Xamarin.Forms.VideoPlayer.Interfaces;

namespace Octane.Xamarin.Forms.VideoPlayer.Android.SourceHandlers
{
    public static class VideoSourceHandler
    {
        /// <summary>
        /// Creates a source handler capable of loading the video resource.
        /// </summary>
        /// <param name="source">The source of the video file.</param>
        /// <returns>A compatible source handler.</returns>
        public static IVideoSourceHandler Create(VideoSource source)
        {
            if (source is FileVideoSource)
                return new FileVideoSourceHandler();

            if(source is StreamVideoSource)
                return new StreamVideoSourceHandler();

            return new UriVideoSourceHandler();
        }
    }
}
