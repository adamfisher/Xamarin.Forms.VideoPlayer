using System;

namespace Octane.Xamarin.Forms.VideoPlayer.Events
{
    /// <summary>
    /// Represents video player event data.
    /// </summary>
    public class VideoPlayerEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// The current position in the playback timeline.
        /// </summary>
        /// <value>
        /// The player's current position.
        /// </value>
        public TimeSpan CurrentTime { get; }

        /// <summary>
        /// Length of the video file.
        /// </summary>
        /// <value>
        /// The duration.
        /// </value>
        public TimeSpan Duration { get; }

        /// <summary>
        /// The current rate of playback. Examples:
        /// </summary>
        /// <remarks>
        /// <para>
        /// Some media files can support reverse-playback via negative rates.
        /// 0.0 = Stopped
        /// 0.5 = Playing at half speed (slow motion).
        /// 1.0 = Normal playback.
        /// 1.0+ = Fast playback.
        /// </para>
        /// </remarks>
        public float Rate { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPlayerEventArgs" /> class.
        /// </summary>
        public VideoPlayerEventArgs()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPlayerEventArgs" /> class.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="rate">The rate of playback.</param>
        public VideoPlayerEventArgs(TimeSpan currentTime, TimeSpan duration, float rate)
        {
            CurrentTime = currentTime;
            Duration = duration;
            Rate = rate;
        }

        #endregion
    }
}
