using System.Threading;
using System.Threading.Tasks;
using Octane.Xamarin.Forms.VideoPlayer.Interfaces;

namespace Octane.Xamarin.Forms.VideoPlayer.Android.SourceHandlers
{
    public sealed class UriVideoSourceHandler : IVideoSourceHandler
    {
        /// <summary>
        /// Loads the video from the specified source.
        /// </summary>
        /// <param name="source">The source of the video file.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The path to the video file.</returns>
        public async Task<string> LoadVideoAsync(VideoSource source, CancellationToken cancellationToken = default(CancellationToken))
        {
            string path = null;
            var uriVideoSource = source as UriVideoSource;

            if (uriVideoSource?.Uri != null)
            {
				path = uriVideoSource.Uri.AbsoluteUri;
            }

            return await Task.FromResult(path);
        }
    }
}
