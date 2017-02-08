using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using Octane.Xam.VideoPlayer;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChillPlayer.MarkupExtensions
{
    /// <summary>
    /// Converts a Vimeo video ID into a direct URL that is playable by the Xamarin Forms VideoPlayer.
    /// </summary>
    [ContentProperty("VideoId")]
    public class VimeoVideoIdExtension : IMarkupExtension
    {
        #region Properties

        /// <summary>
        /// Gets or sets the video identifier.
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
                Debug.WriteLine($"Acquiring Vimeo stream source URL from VideoId='{VideoId}'...");
                var videoInfoUrl = $"https://player.vimeo.com/video/{VideoId}/config";

                using (var client = new HttpClient())
                {
                    var videoPageContent = client.GetStringAsync(videoInfoUrl).Result;
                    var videoPageBytes = Encoding.UTF8.GetBytes(videoPageContent);

                    using (var stream = new MemoryStream(videoPageBytes))
                    {
                        var serializer = new DataContractJsonSerializer(typeof(VimeoVideo));
                        var metaData = (VimeoVideo)serializer.ReadObject(stream);
                        var files = metaData.request.files.progressive;

                        // Exact match
                        var url = files.OrderByDescending(s => s.width).Select(s => s.url).FirstOrDefault();

                        Debug.WriteLine($"Stream URL: {url}");
                        return VideoSource.FromUri(url);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error occured while attempting to convert Vimeo video ID into a remote stream path.");
                Debug.WriteLine(ex);
            }

            return null;
        }

        #endregion
        
        #region Methods

        /// <summary>
        /// Convert the specified video ID into a streamable Vimeo URL.
        /// </summary>
        /// <param name="videoId">Video identifier.</param>
        /// <returns></returns>
        public static VideoSource Convert(string videoId) {
		    var markupExtension = new VimeoVideoIdExtension { VideoId = videoId };
		    return (VideoSource) markupExtension.ProvideValue (null);
		}

        #endregion
    }
}
