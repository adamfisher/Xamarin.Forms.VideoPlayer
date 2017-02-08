using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using ChillPlayer.Web;
using Octane.Xam.VideoPlayer;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

// http://www.genyoutube.net/formats-resolution-youtube-videos.html

namespace ChillPlayer.MarkupExtensions
{
    /// <summary>
    /// Converts a YouTube video ID into a direct URL that is playable by the Xamarin Forms VideoPlayer.
    /// </summary>
    [ContentProperty("VideoId")]
    public class YouTubeVideoIdExtension : IMarkupExtension
    {
        #region Properties

        /// <summary>
        /// The video identifier associated with the video stream.
        /// </summary>
        /// <value>
        /// The video identifier.
        /// </value>
        public string VideoId { get; set; }

        #endregion

        #region IMarkupExtension

        /// <summary>
        /// Provides the value.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            try
            {
                Debug.WriteLine($"Acquiring YouTube stream source URL from VideoId='{VideoId}'...");
                var videoInfoUrl = $"http://www.youtube.com/get_video_info?video_id={VideoId}";

                using (var client = new HttpClient())
                {
                    var videoPageContent = client.GetStringAsync(videoInfoUrl).Result;
                    var videoParameters = HttpUtility.ParseQueryString(videoPageContent);
                    var encodedStreamsDelimited = WebUtility.HtmlDecode(videoParameters["url_encoded_fmt_stream_map"]);
                    var encodedStreams = encodedStreamsDelimited.Split(',');
                    var streams = encodedStreams.Select(HttpUtility.ParseQueryString);

                    var streamsByPriority = streams
                        .OrderBy(s =>
                        {
                            var type = s["type"];
                            if (type.Contains("video/mp4")) return 10;
                            if (type.Contains("video/3gpp")) return 20;
                            if (type.Contains("video/x-flv")) return 30;
                            if (type.Contains("video/webm")) return 40;
                            return int.MaxValue;
                        })
                        .ThenBy(s =>
                        {
                            var quality = s["quality"];

                            switch (Device.Idiom)
                            {
                                case TargetIdiom.Phone:
                                    return Array.IndexOf(new[] { "medium", "high", "small" }, quality);
                                default:
                                    return Array.IndexOf(new[] { "high", "medium", "small" }, quality);
                            }
                        })
                        .FirstOrDefault();

                    Debug.WriteLine($"Stream URL: {streamsByPriority["url"]}");
					return VideoSource.FromUri(streamsByPriority["url"]);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error occured while attempting to convert YouTube video ID into a remote stream path.");
                Debug.WriteLine(ex);
            }

            return null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Convert the specified video ID into a streamable YouTube URL.
        /// </summary>
        /// <param name="videoId">Video identifier.</param>
        /// <returns></returns>
        public static VideoSource Convert(string videoId)
        {
            var markupExtension = new YouTubeVideoIdExtension { VideoId = videoId };
            return (VideoSource)markupExtension.ProvideValue(null);
        }

        #endregion
    }
}
