namespace Octane.Xamarin.Forms.VideoPlayer.Constants
{
    /// <summary>
    /// Represents the current state of the video player.
    /// </summary>
    public enum PlayerState
    {
        /// <summary>
        /// The idle state is the default state of a newly created video player.
        /// </summary>
        Idle,

        /// <summary>
        /// The video player enters this state when a video source has been specified for playback.
        /// </summary>
        Initialized,

        /// <summary>
        /// The video player is ready to begin playback.
        /// </summary>
        Prepared,

        /// <summary>
        /// Video playback is currently active.
        /// </summary>
        Playing,

        /// <summary>
        /// Video playback is paused.
        /// </summary>
        Paused,

        /// <summary>
        /// Video playback is complete.
        /// </summary>
        Completed,

        /// <summary>
        /// The video player has experienced an error.
        /// </summary>
        Error
    }
}
