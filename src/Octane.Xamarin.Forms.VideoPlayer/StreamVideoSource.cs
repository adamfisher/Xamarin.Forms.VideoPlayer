using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Octane.Xamarin.Forms.VideoPlayer
{
    /// <summary>
    /// <see cref="T:Octane.Xamarin.Forms.VideoPlayer.VideoSource"/> that loads a video from a <see cref="T:System.IO.Stream"/>.
    /// </summary>
    public sealed class StreamVideoSource : VideoSource
    {
		/// <summary>
		/// The video container format denoted by the file extension (e.g. Format = "mp4")
		/// </summary>
		/// <value>The containerformat.</value>
		public string Format { get; set; }

        /// <summary>
        /// Backing store for the <see cref="P:Octane.Xamarin.Forms.VideoPlayer.StreamVideoSource.Stream"/> property.
        /// </summary>
        public static readonly BindableProperty StreamProperty = BindableProperty.Create(nameof(Stream), typeof(Func<CancellationToken, Task<Stream>>), typeof(StreamVideoSource), null);

        /// <summary>
        /// Gets or sets the delegate responsible for returning a <see cref="T:System.IO.Stream" /> for the video player.
        /// </summary>
        /// <value>
        /// The stream.
        /// </value>
        public Func<CancellationToken, Task<Stream>> Stream
        {
            get { return (Func<CancellationToken, Task<Stream>>) GetValue(StreamProperty); }
            set { SetValue(StreamProperty, value); }
        }

        /// <summary>
        /// Method that is called when the property that is specified by <paramref name="propertyName" /> is changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (propertyName == StreamProperty.PropertyName)
                OnSourceChanged();
            
            base.OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Gets the stream asynchronously.
        /// </summary>
        /// <param name="userToken">The user token.</param>
        /// <returns></returns>
        internal async Task<Stream> GetStreamAsync(CancellationToken userToken = default(CancellationToken))
        {
            Stream result = null;

            if (Stream != null)
            {
                OnLoadingStarted();
                userToken.Register(CancellationTokenSource.Cancel);

                try
                {
					result = await Stream(CancellationTokenSource.Token);
                    OnLoadingCompleted(false);
                }
                catch (OperationCanceledException)
                {
                    OnLoadingCompleted(true);
                    throw;
                }
            }

            return result;
        }

        /// <summary>
        /// Determines if two video sources are equivalent.
        /// </summary>
        /// <param name="other">The other video source.</param>
        /// <returns>
        /// True if the video sources are equal, false otherwise.
        /// </returns>
        public override bool Equals(VideoSource other)
        {
            return !(other is StreamVideoSource) || ((StreamVideoSource)other).Stream.Equals(Stream);
        }
    }
}
