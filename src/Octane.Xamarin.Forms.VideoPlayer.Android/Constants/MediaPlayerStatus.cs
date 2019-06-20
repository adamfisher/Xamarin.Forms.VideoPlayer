namespace Octane.Xamarin.Forms.VideoPlayer.Android.Constants
{
    /// <summary>
    /// Represents the states of an Android MediaPlayer.
    /// See: http://developer.android.com/images/mediaplayer_state_diagram.gif
    /// </summary>
    public enum MediaPlayerStatus
    {
        Error,
        Idle,
        Preparing,
        Prepared,
        Playing,
        Paused,
        PlaybackCompleted
    }
}