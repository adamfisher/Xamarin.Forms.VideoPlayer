using System;
using Octane.Xamarin.Forms.VideoPlayer.Constants;

namespace Octane.Xamarin.Forms.VideoPlayer.Events
{
    /// <summary>
    /// Contains information about video player state transitions.
    /// </summary>
    public class VideoPlayerStateChangedEventArgs : VideoPlayerEventArgs
    {
        #region Properties

        /// <summary>
        /// The current state of the video player.
        /// </summary>
        /// <value>
        /// The state of the video player.
        /// </value>
        public PlayerState CurrentState { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPlayerStateChangedEventArgs"/> class.
        /// </summary>
        /// <param name="currentState">State of the current.</param>
        public VideoPlayerStateChangedEventArgs(PlayerState currentState)
        {
            CurrentState = currentState;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPlayerEventArgs" /> class.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="rate">The rate of playback.</param>
        /// <param name="currentState">State of the current.</param>
        public VideoPlayerStateChangedEventArgs(TimeSpan currentTime, TimeSpan duration, float rate, PlayerState currentState) 
            : base(currentTime, duration, rate)
        {
            CurrentState = currentState;
        }

        public VideoPlayerStateChangedEventArgs(VideoPlayerEventArgs videoPlayerEventArgs, PlayerState currentState)
            : this(videoPlayerEventArgs.CurrentTime, videoPlayerEventArgs.Duration, videoPlayerEventArgs.Rate, currentState)
        {
        }

        #endregion
    }
}
