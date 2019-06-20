using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Octane.Xamarin.Forms.VideoPlayer.Interfaces;
using Octane.Xamarin.Forms.VideoPlayer.UWP.Cryptography;

namespace Octane.Xamarin.Forms.VideoPlayer.UWP.SourceHandlers
{
    public sealed class StreamVideoSourceHandler : IVideoSourceHandler
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
            var streamVideoSource = source as StreamVideoSource;

            if (streamVideoSource?.Stream != null)
            {
                using (var stream = await streamVideoSource.GetStreamAsync(cancellationToken).ConfigureAwait(false))
                {
                    var tempFileName = GetFileName(stream, streamVideoSource.Format);
                    var tempDirectory = ApplicationData.Current.TemporaryFolder;

                    try
                    {
                        var tempFile = await tempDirectory.CreateFileAsync(tempFileName, CreationCollisionOption.FailIfExists);

                        using (var reader = new DataReader(stream.AsInputStream()))
                        {
                            await reader.LoadAsync((uint)stream.Length);
                            var buffer = new byte[(int)stream.Length];
                            reader.ReadBytes(buffer);
                            await FileIO.WriteBytesAsync(tempFile, buffer);
                        }
                    }
                    catch (Exception) { }  // Windows Phone is stupid - you have to catch an exception to determine if the file already exists.

                    path = Path.Combine(tempDirectory.Path, tempFileName);
                }
            }

            return path;
        }

        /// <summary>
        /// Computes a file name based on a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
		private string GetFileName(Stream stream, string format)
        {
            var hasher = MD5.Create();
            var hashBytes = hasher.ComputeHash(stream);
            stream.Position = 0;
            var hash = Convert.ToBase64String(hashBytes)
                .Replace("=", string.Empty)
                .Replace("\\", string.Empty)
                .Replace("/", string.Empty);
            return $"{hash}_temp.{format}";
        }
    }
}
