using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using AVFoundation;
using AVKit;
using CoreMedia;
using Foundation;
using Octane.Xamarin.Forms.VideoPlayer;
using Octane.Xamarin.Forms.VideoPlayer.Constants;
using Octane.Xamarin.Forms.VideoPlayer.Diagnostics;
using Octane.Xamarin.Forms.VideoPlayer.Events;
using Octane.Xamarin.Forms.VideoPlayer.iOS.Renderers;
using Octane.Xamarin.Forms.VideoPlayer.iOS.SourceHandlers;
using Octane.Xamarin.Forms.VideoPlayer.Interfaces;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(VideoPlayer), typeof(VideoPlayerRenderer))]

namespace Octane.Xamarin.Forms.VideoPlayer.iOS.Renderers
{
        /// <summary>
        /// A custom renderer for the native platform video player.
        /// </summary>
        public class VideoPlayerRenderer : ViewRenderer<VideoPlayer, UIView>, IVideoPlayerRenderer
    {
        #region Fields

        /// <summary>
        /// The periodic time oberserver.
        /// </summary>
        private NSObject _periodicTimeOberserver;

        /// <summary>
        /// The current time observer.
        /// </summary>
        private NSObject _currentTimeObserver;

        /// <summary>
        /// The did play to end time notification observer.
        /// </summary>
        private NSObject _didPlayToEndTimeNotificationObserver;

        /// <summary>
        /// Determines if this instance has been disposed.
        /// </summary>
        private bool _isDisposed;

        /// <summary>
        /// The platform specific media player object.
        /// </summary>
        private AVPlayerViewController _playerControl;

        #endregion

        #region Properties

        /// <summary>
        /// The platform specific media player object.
        /// </summary>
        /// <value>
        /// The AVPlayer controller
        /// </value>
        protected AVPlayerViewController PlayerControl => _playerControl;

        #endregion

        #region IVideoPlayerRenderer

        /// <summary>
        /// Plays the video.
        /// </summary>
        public virtual void Play()
        {
            if (CanPlay())
                _playerControl.Player.Play();
        }

        /// <summary>
        /// Determines if the video player instance can play.
        /// </summary>
        /// <returns></returns>
        /// <c>true</c> if this instance can play; otherwise, <c>false</c>.
        public virtual bool CanPlay()
        {
            return _playerControl?.Player?.Status == AVPlayerStatus.ReadyToPlay;
        }

        /// <summary>
        /// Pauses the video.
        /// </summary>
        public virtual void Pause()
        {
            if (CanPause())
                _playerControl?.Player?.Pause();
        }

        /// <summary>
        /// Determines if the video player instance can pause.
        /// </summary>
        /// <returns></returns>
        /// <c>true</c> if this instance can pause; otherwise, <c>false</c>.
        public virtual bool CanPause()
        {
            return _playerControl?.Player?.Status == AVPlayerStatus.ReadyToPlay;
        }

        /// <summary>
        /// Stops the video.
        /// </summary>
        public virtual void Stop()
        {
            if (CanStop())
                _playerControl?.Player?.Pause();
        }

        /// <summary>
        /// Determines if the video player instance can stop.
        /// </summary>
        /// <returns></returns>
        /// <c>true</c> if this instance can stop; otherwise, <c>false</c>.
        public virtual bool CanStop()
        {
            return _playerControl?.Player?.Status == AVPlayerStatus.ReadyToPlay;
        }

        /// <summary>
        /// Seeks a specific number of seconds into the playback stream.
        /// </summary>
        /// <param name="seekTime">The seek time.</param>
        public virtual void Seek(int seekTime)
        {
            if (CanSeek(seekTime))
            {
                var player = _playerControl?.Player;

                if (player != null)
                {
                    var currentTime = player.CurrentTime.Seconds;
                    var timeScale = player.CurrentItem.Duration.TimeScale;
                    var targetTime = currentTime + seekTime;
                    Log.Info($"SEEK: CurrentTime={currentTime}; TimeScale={timeScale}; NewTime={targetTime}");
                    player.Seek(CMTime.FromSeconds(targetTime, timeScale));
                }
            }
        }

        /// <summary>
        /// Determines if the video player instance can seek.
        /// </summary>
        /// <param name="time">The time in seconds.</param>
        /// <returns></returns>
        /// <c>true</c> if this instance can stop; otherwise, <c>false</c>.
        public virtual bool CanSeek(int time)
        {
            var player = _playerControl?.Player;
            return player != null && (player.CurrentItem.CanStepBackward
                   || player.CurrentItem.CanStepForward);
        }

        #endregion

        #region ViewRenderer Overrides

        /// <summary>
        /// Raises the <see cref="E:ElementChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="ElementChangedEventArgs{VideoPlayer}"/> instance containing the event data.</param>
        protected override async void OnElementChanged(ElementChangedEventArgs<VideoPlayer> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                // Controller Setup
                _playerControl = new AVPlayerViewController
                {
                    View = {
                        Bounds = Bounds,
                        AutoresizingMask = AutoresizingMask
                    },
                    Player = new AVPlayer()
                };

                SetNativeControl(_playerControl.View);
            }

            if (e.NewElement != null)
            {
                // Player Setup
                UpdateDisplayControls();
                UpdateVolume();
                UpdateFillMode();
                UpdateTimeElapsedInterval();
                UpdateVisibility();
                RegisterEvents();
                await UpdateSource(e.OldElement);
                Element.NativeRenderer = this;
            }
        }

        /// <summary>
        /// Called when [element property changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override async void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Element == null || Control == null || _playerControl?.Player == null)
                return;

            if (e.PropertyName == VideoPlayer.DisplayControlsProperty.PropertyName)
            {
                UpdateDisplayControls();
            }
            else if (e.PropertyName == VideoPlayer.FillModeProperty.PropertyName)
            {
                UpdateFillMode();
            }
            else if (e.PropertyName == VideoPlayer.TimeElapsedIntervalProperty.PropertyName)
            {
                UpdateTimeElapsedInterval();
            }
            else if (e.PropertyName == VideoPlayer.VolumeProperty.PropertyName)
            {
                UpdateVolume();
            }
            else if (e.PropertyName == VideoPlayer.SourceProperty.PropertyName)
            {
                await UpdateSource();
            }
            else if (e.PropertyName == VisualElement.IsVisibleProperty.PropertyName)
            {
                UpdateVisibility();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Registers this renderer with native events.
        /// </summary>
        private void RegisterEvents()
        {
            _playerControl.Player.AddObserver(this, (NSString)"status", 0, Handle);
            _playerControl.Player.AddObserver(this, (NSString)"rate", 0, Handle);
        }

        /// <summary>
        /// Event handling for the play to end time notification.
        /// </summary>
        /// <param name="obj">The notification object.</param>
        private void DidPlayToEndTimeNotification(NSNotification obj)
        {
            var playerItem = obj.Object as AVPlayerItem;
            var thisIsMyPlayerItem = playerItem?.Handle == _playerControl.Player.CurrentItem?.Handle;

            if (thisIsMyPlayerItem)
            {
                Element.OnCompleted(CreateVideoPlayerEventArgs());

                if (Element.Repeat)
                {
                    _playerControl.Player.Seek(CMTime.Zero);
                    _playerControl.Player.Play();
                }
            }
        }

        /// <summary>
        /// Uns the register events.
        /// </summary>
        private void UnRegisterEvents()
        {
            _playerControl?.Player?.CurrentItem?.RemoveObserver(FromObject(this), "status");
            _playerControl?.Player?.RemoveObserver(FromObject(this), "status");
            _playerControl?.Player?.RemoveObserver(FromObject(this), "rate");

            if (_didPlayToEndTimeNotificationObserver != null)
                NSNotificationCenter.DefaultCenter.RemoveObserver(_didPlayToEndTimeNotificationObserver);

            if (_currentTimeObserver != null)
                NSNotificationCenter.DefaultCenter.RemoveObserver(_currentTimeObserver);
        }

        /// <summary>
        /// Indicates that the value at the specified keyPath relative to this object has changed.
        /// </summary>
        /// <param name="keyPath">Key-path to use to perform the value lookup.   The keypath consists of a series of lowercase ASCII-strings with no spaces in them separated by dot characters.</param>
        /// <param name="ofObject">The object.</param>
        /// <param name="change">A dictionary that describes the changes that have been made to the value of the property at the key path keyPath relative to object. Entries are described in Change Dictionary Keys.</param>
        /// <param name="context">The value that was provided when the receiver was registered to receive key-value observation notifications.</param>
        /// <remarks>
        /// This method is invoked if you have registered an observer using the <see cref="M:Foundation.NSObject.AddObserver" /> method
        /// </remarks>
        public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
        {
            var videoPlayer = _playerControl?.Player;

            if (string.Equals(keyPath, "status"))
            {
                if (videoPlayer.Status == AVPlayerStatus.ReadyToPlay && videoPlayer.CurrentItem.Status == AVPlayerItemStatus.ReadyToPlay)
                {
                    Element.OnPlayerStateChanged(CreateVideoPlayerStateChangedEventArgs(PlayerState.Prepared));

                    if (Element.AutoPlay && videoPlayer.CurrentItem.CurrentTime == CMTime.Zero)
                        Play();
                }
                else if (videoPlayer?.Status == AVPlayerStatus.Failed)
                {
                    Element.OnFailed(CreateVideoPlayerErrorEventArgs());
                }

                Element.SetValue(VideoPlayer.IsLoadingPropertyKey, false);
            }
            else if (string.Equals(keyPath, "rate"))
            {
                if (videoPlayer.Rate < .000001)
                {
                    var currentTime = videoPlayer.CurrentItem.CurrentTime;
                    var totalTime = videoPlayer.CurrentItem.Duration;

                    // Rate event fires when player is initialized so don't fire paused event at beginning.
                    // We also don't want to fire at the very end of playback since it will wipe out the completed event.
                    if (currentTime != CMTime.Zero && currentTime != totalTime)
                        Element.OnPaused(CreateVideoPlayerEventArgs());
                }
                else
                {
                    Element.OnPlaying(CreateVideoPlayerEventArgs());
                }
            }
            else
            {
                base.ObserveValue(keyPath, ofObject, change, context);
            }
        }

        /// <summary>
        /// Updates the display controls property on the native player.
        /// </summary>
        private void UpdateDisplayControls()
        {
            _playerControl.ShowsPlaybackControls = Element.DisplayControls;
        }

        /// <summary>
        /// Updates the volume level.
        /// </summary>
        private void UpdateVolume()
        {
            var volume = Element.Volume;

            if (volume != int.MinValue)
            {
                var nativeVolume = (float)Math.Min(100, Math.Max(0, volume)) / 100;
                _playerControl.Player.Volume = nativeVolume;
            }
        }

        /// <summary>
        /// Updates the video source property on the native player.
        /// </summary>
        /// <param name="oldElement">The old element.</param>
        private async Task UpdateSource(VideoPlayer oldElement = null)
        {
            try
            {
                var newSource = Element?.Source;

                if (oldElement != null)
                {
                    var oldSource = oldElement.Source;

                    if (!oldSource.Equals(newSource))
                        return;
                }

                Element.SetValue(VideoPlayer.IsLoadingPropertyKey, true);
                var videoSourceHandler = VideoSourceHandler.Create(newSource);
                var path = await videoSourceHandler.LoadVideoAsync(newSource, new CancellationToken());
                Log.Info($"Video Source: {path}");

                if (!string.IsNullOrEmpty(path))
                {
                    if (_currentTimeObserver != null)
                        _playerControl.Player.RemoveTimeObserver(_currentTimeObserver);
                    if (_didPlayToEndTimeNotificationObserver != null)
                        NSNotificationCenter.DefaultCenter.RemoveObserver(_didPlayToEndTimeNotificationObserver);

                    // Update video source.
                    Element.SetValue(VideoPlayer.CurrentTimePropertyKey, TimeSpan.Zero);

                    var pathUrl = newSource is UriVideoSource ? NSUrl.FromString(path) : NSUrl.FromFilename(path);

                    _playerControl.Player.CurrentItem?.RemoveObserver(FromObject(this), "status");

                    _playerControl.Player.ReplaceCurrentItemWithPlayerItem(AVPlayerItem.FromUrl(pathUrl));

                    _playerControl.Player.CurrentItem.AddObserver(this, (NSString)"status", 0, Handle);

                    Element.OnPlayerStateChanged(CreateVideoPlayerStateChangedEventArgs(PlayerState.Initialized));

                    _didPlayToEndTimeNotificationObserver = NSNotificationCenter.DefaultCenter.AddObserver(
                        AVPlayerItem.DidPlayToEndTimeNotification, DidPlayToEndTimeNotification, _playerControl.Player.CurrentItem);

                    _currentTimeObserver = _playerControl.Player.AddPeriodicTimeObserver(CMTime.FromSeconds(1, 1), null,
                        time => Element?.SetValue(VideoPlayer.CurrentTimePropertyKey,
                            double.IsNaN(time.Seconds) ? TimeSpan.Zero : TimeSpan.FromSeconds(time.Seconds)));
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                Element.SetValue(VideoPlayer.IsLoadingPropertyKey, false);
            }
        }

        /// <summary>
        /// Updates the time elapsed interval of the video player.
        /// </summary>
        private void UpdateTimeElapsedInterval()
        {
            if (_periodicTimeOberserver != null)
            {
                _playerControl?.Player?.RemoveTimeObserver(_periodicTimeOberserver);
                _periodicTimeOberserver = null;
            }

            var element = Element;

            if (element != null && Element?.TimeElapsedInterval > 0)
            {
                _periodicTimeOberserver = _playerControl?.Player?.AddPeriodicTimeObserver(CMTime.FromSeconds(element.TimeElapsedInterval, 1), null,
                    time => element.OnTimeElapsed(CreateVideoPlayerEventArgs()));
            }
        }

        /// <summary>
        /// Updates the fill mode property on the native player.
        /// </summary>
        private void UpdateFillMode()
        {
            switch (Element.FillMode)
            {
                case FillMode.Resize:
                    _playerControl.VideoGravity = AVLayerVideoGravity.Resize;
                    break;
                case FillMode.ResizeAspect:
                    _playerControl.VideoGravity = AVLayerVideoGravity.ResizeAspect;
                    break;
                default:
                    _playerControl.VideoGravity = AVLayerVideoGravity.ResizeAspectFill;
                    break;
            }
        }

        /// <summary>
        /// Updates the visibility of the video player.
        /// </summary>
        private void UpdateVisibility()
        {
            Control.Hidden = !Element.IsVisible;
        }

        /// <summary>
        /// Creates the video player event arguments.
        /// </summary>
        /// <returns>VideoPlayerEventArgs with information populated.</returns>
        private VideoPlayerEventArgs CreateVideoPlayerEventArgs()
        {
            var playerItem = _playerControl.Player.CurrentItem;

            if (playerItem != null)
            {
                var currentTime = !double.IsNaN(playerItem.CurrentTime.Seconds) ? playerItem.CurrentTime.Seconds : 0;
                var duration = !double.IsNaN(playerItem.Duration.Seconds) ? playerItem.Duration.Seconds : 0;

                return new VideoPlayerEventArgs(
                    TimeSpan.FromSeconds(currentTime),
                    TimeSpan.FromSeconds(duration),
                    _playerControl.Player.Rate
                );
            }

            return null;
        }

        /// <summary>
        /// Creates the video player state changed event arguments.
        /// </summary>
        /// <param name="state">The current state.</param>
        /// <returns></returns>
        private VideoPlayerStateChangedEventArgs CreateVideoPlayerStateChangedEventArgs(PlayerState state)
        {
            var videoPlayerEventArgs = CreateVideoPlayerEventArgs();
            return new VideoPlayerStateChangedEventArgs(videoPlayerEventArgs, state);
        }

        /// <summary>
        /// Creates the video player error event arguments.
        /// </summary>
        /// <returns>VideoPlayerErrorEventArgs with information populated.</returns>
        private VideoPlayerErrorEventArgs CreateVideoPlayerErrorEventArgs()
        {
            var error = _playerControl.Player.Error;

            if (error != null)
            {
                var errorStatus = Enum.GetName(typeof(AVError), error.Code);
                var videoPlayerEventArgs = CreateVideoPlayerEventArgs();
                return new VideoPlayerErrorEventArgs(videoPlayerEventArgs, errorStatus, error);
            }

            return null;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            AVPlayerViewController playerController;

            if (disposing && Control != null && (playerController = _playerControl) != null)
            {
                UnRegisterEvents();
                playerController.Player?.Pause();
                playerController.Dispose();
            }

            _isDisposed = true;
            base.Dispose(disposing);
        }

        #endregion
    }
}
