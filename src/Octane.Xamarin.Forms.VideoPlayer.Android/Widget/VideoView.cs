using System;
using System.Collections.Generic;
using Android.Content;
using Android.Media;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using Octane.Xamarin.Forms.VideoPlayer.Android.Constants;
using Octane.Xamarin.Forms.VideoPlayer.Constants;
using Octane.Xamarin.Forms.VideoPlayer.ExtensionMethods;
using Timer = System.Timers.Timer;
using Uri = Android.Net.Uri;

namespace Octane.Xamarin.Forms.VideoPlayer.Android.Widget
{
    /// <summary>
    /// Displays a video file.
    /// </summary>
    [Register("Octane.Xamarin.Forms.VideoPlayer.Android.Widget.VideoView")]
    public class VideoView : global::Android.Widget.VideoView
    {
        #region Properties

        /// <summary>
        /// The platform specific media player object.
        /// </summary>
        public MediaPlayer Player { get; private set; }

        /// <summary>
        /// The media controller.
        /// </summary>
        public MediaController MediaController { get; private set; }

        /// <summary>
        /// Gets or sets the time elapsed interval.
        /// </summary>
        /// <value>
        /// The time elapsed interval.
        /// </value>
        public double TimeElapsedInterval
        {
            get { return _timeElapsedInterval; }
            set
            {
                if (value > 0)
                {
                    _timeElapsedInterval = value;
                    _timer.Interval = _timeElapsedInterval * 1000;
                }
                else
                {
                    _timer?.Stop();
                }
            }
        }

        /// <summary>
        /// Gets the state of the MediaPlayer.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public MediaPlayerStatus Status { get; private set; }

        /// <summary>
        /// Gets or sets the video scaling fill mode of the player on the view surface.
        /// </summary>
        /// <value>
        /// The fill mode.
        /// </value>
        public FillMode FillMode { get; set; }

        #endregion

        #region Fields

        /// <summary>
        /// The timer used to control time elapsed event firings.
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// The time elapsed interval.
        /// </summary>
        private double _timeElapsedInterval;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoView"/> class.
        /// </summary>
        /// <param name="javaReference">The java reference.</param>
        /// <param name="transfer">The transfer.</param>
        public VideoView(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            Init();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoView"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public VideoView(Context context) : base(context)
        {
            Init();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoView"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="attrs">The attrs.</param>
        public VideoView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoView"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="attrs">The attrs.</param>
        /// <param name="defStyle">The definition style.</param>
        public VideoView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Init();
        }

        #endregion

        #region Events

        /// <summary>
        /// Event notification fires when the video player's play button has been pressed.
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// Event notification fires when the video player's play button has been pressed.
        /// </summary>
        public event EventHandler Paused;

        /// <summary>
        /// Event notification fires when the video player's stop button has been pressed.
        /// </summary>
        public event EventHandler Stopped;

        /// <summary>
        /// Event notification fires when the video player's pause or stop button has been pressed.
        /// </summary>
        public event EventHandler TimeElapsed;

        #endregion

        #region Methods

        /// <summary>
        /// Initializes this VideoView instance.
        /// </summary>
        private void Init()
        {
            Status = MediaPlayerStatus.Idle;
            SetMediaController(new MediaController(Context));
            _timer = new Timer();
            _timer.Elapsed += (sender, args) => TimeElapsed.RaiseEvent(this);

            // Register Events
            Prepared += OnPrepared;
            Completion += OnCompletion;
            Error += OnError;
        }

        /// <summary>
        /// Called when the video is prepared for playback.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnPrepared(object sender, EventArgs e)
        {
            Player = sender as MediaPlayer;
            Status = MediaPlayerStatus.Prepared;
        }

        /// <summary>
        /// Called when the video player fails.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="errorEventArgs">The <see cref="MediaPlayer.ErrorEventArgs"/> instance containing the event data.</param>
        private void OnError(object sender, MediaPlayer.ErrorEventArgs errorEventArgs)
        {
            _timer?.Stop();
            Status = MediaPlayerStatus.Error;
        }

        /// <summary>
        /// Called when [completion].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnCompletion(object sender, EventArgs e)
        {
            _timer?.Stop();
            Status = MediaPlayerStatus.PlaybackCompleted;
        }

        /// <summary>
        /// Sets the media controller.
        /// </summary>
        /// <param name="controller">The controller.</param>
        public override void SetMediaController(MediaController controller)
        {
            base.SetMediaController(controller);
            MediaController = controller;
        }

        /// <summary>
        /// Sets the video path.
        /// </summary>
        /// <param name="path">The path.</param>
        public override void SetVideoPath(string path)
        {
            Status = MediaPlayerStatus.Preparing;
            base.SetVideoPath(path);
        }

        /// <summary>
        /// Sets the video URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        public override void SetVideoURI(Uri uri)
        {
            Status = MediaPlayerStatus.Preparing;
            base.SetVideoURI(uri);
        }

        /// <summary>
        /// Sets the video URI.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="headers">The headers.</param>
        public override void SetVideoURI(Uri uri, IDictionary<string, string> headers)
        {
            Status = MediaPlayerStatus.Preparing;
            base.SetVideoURI(uri, headers);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public override void Start()
        {
            base.Start();
            Status = MediaPlayerStatus.Playing;
            Started.RaiseEvent(this);

            if (TimeElapsedInterval > 0)
                _timer?.Start();
        }

        /// <summary>
        /// Stops the playback.
        /// </summary>
        public override void StopPlayback()
        {
            _timer?.Stop();
            base.StopPlayback();
            Status = MediaPlayerStatus.Idle;
            Stopped.RaiseEvent(this);
        }

        /// <summary>
        /// Pauses the video playback.
        /// </summary>
        public override void Pause()
        {
            _timer?.Stop();
            base.Pause();
            Status = MediaPlayerStatus.Paused;
            Paused.RaiseEvent(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Player = null;
                MediaController = null;
                _timer = null;

                Started = null;
                Paused = null;
                Stopped = null;
                TimeElapsed = null;
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}