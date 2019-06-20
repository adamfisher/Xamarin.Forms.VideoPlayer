using System.Threading;
using System.Threading.Tasks;

namespace Octane.Xamarin.Forms.VideoPlayer.Interfaces
{
    public interface IVideoSourceHandler
    {
        /// <summary>
        /// Loads the video from the specified source.
        /// </summary>
        /// <param name="source">The source of the video file.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The path to the video file.</returns>
        Task<string> LoadVideoAsync(VideoSource source, CancellationToken cancellationToken);
    }
}
