using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Windows.System.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Octane.Xamarin.Forms.VideoPlayer;
using Octane.Xamarin.Forms.VideoPlayer.Constants;
using Octane.Xamarin.Forms.VideoPlayer.Diagnostics;
using Octane.Xamarin.Forms.VideoPlayer.Events;
using Octane.Xamarin.Forms.VideoPlayer.Interfaces;
using Octane.Xamarin.Forms.VideoPlayer.UWP.Renderers;
using Octane.Xamarin.Forms.VideoPlayer.UWP.SourceHandlers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(VideoPlayer), typeof(VideoPlayerRenderer))]

namespace Octane.Xamarin.Forms.VideoPlayer.UWP.Renderers
{
    /// <summary>
    /// A custom renderer for the native platform video player.
    /// </summary>
    public class VideoPlayerRenderer : ViewRenderer<VideoPlayer, MediaElement>, IVideoPlayerRenderer
    {
        #region Fields

        /// <summary>
        /// Determines if this instance has been disposed.
        /// </summary>
        private bool _isDisposed;

        /// <summary>
        /// Prevents the display from being deactivated when user action is no longer detected.
        /// </summary>
        private DisplayRequest _appDisplayRequest;

        /// <summary>
        /// The timer used to control CurrentTime refreshing.
        /// </summary>
        private DispatcherTimer _timer;

        #endregion

        #region IVideoPlayerRenderer

        /// <summary>
        /// Plays the video.
        /// </summary>
        public virtual void Play()
        {
            if (CanPlay())
            {
                Control?.Play();
            }
        }

        /// <summary>
        /// Determines if the video player instance can play.
        /// </summary>
        /// <returns></returns>
        /// <c>true</c> if this instance can play; otherwise, <c>false</c>.
        public virtual bool CanPlay()
        {
            var control = Control;
            return control != null && (control.CanSeek || control.CanPause);
        }

        /// <summary>
        /// Pauses the video.
        /// </summary>
        public virtual void Pause()
        {
            if (CanPause())
            {
                Control?.Pause();
            }
        }

        /// <summary>
        /// Determines if the video player instance can pause.
        /// </summary>
        /// <returns></returns>
        /// <c>true</c> if this instance can pause; otherwise, <c>false</c>.
        public virtual bool CanPause()
        {
            var control = Control;
            return control != null && control.CanPause;
        }

        /// <summary>
        /// Stops the video.
        /// </summary>
        public virtual void Stop()
        {
            if (CanStop())
            {
                Control?.Stop();
            }
        }

        /// <summary>
        /// Determines if the video player instance can stop.
        /// </summary>
        /// <returns></returns>
        /// <c>true</c> if this instance can stop; otherwise, <c>false</c>.
        public virtual bool CanStop()
        {
            var control = Control;
            return control != null && control.CurrentState == MediaElementState.Playing;
        }

        /// <summary>
        /// Seeks a specific number of seconds into the playback stream.
        /// </summary>
        /// <param name="time">The time in seconds.</param>
        public virtual void Seek(int time)
        {
            if (CanSeek(time))
            {
                var control = Control;

                if (control != null)
                {
                    var currentTime = control.Position;
                    var targetTime = currentTime + TimeSpan.FromSeconds(time);
                    Log.Info($"SEEK: CurrentTime={currentTime}; NewTime={targetTime}");
                    control.Position = targetTime;
                    //Element.SetValue(VideoPlayer.CurrentTimePropertyKey, Control.Position);         
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
            var control = Control;
            var absoluteTime = Math.Abs(time);
            return control != null &&
                ((time > 0 && control.CanSeek && (control.Position.Add(TimeSpan.FromSeconds(absoluteTime)) <= control.NaturalDuration))
                || (time < 0 && control.CanSeek && (control.Position.Subtract(TimeSpan.FromSeconds(absoluteTime)) >= TimeSpan.Zero)));
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

            if (e.NewElement == null)
                return;

            if (Control == null)
            {
                SetNativeControl(new MediaElement());
                Control.Name = $"VideoPlayer-{Guid.NewGuid()}";
            }
            
            UpdateRepeat();
            UpdateDisplayControls();
            UpdateVolume();
            UpdateFillMode();
            UpdateTimeElapsedInterval();
            UpdateAutoPlay();
            UpdateRepeat();
            UpdateVisibility();
            RegisterEvents();
            Element.NativeRenderer = this;
            await UpdateSource();
        }

        /// <summary>
        /// Called when [element property changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        protected override async void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Element == null || Control == null) return;

            if (e.PropertyName == VideoPlayer.AutoPlayProperty.PropertyName)
            {
                UpdateAutoPlay();
            }
            else if (e.PropertyName == VideoPlayer.DisplayControlsProperty.PropertyName)
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
            else if (e.PropertyName == VideoPlayer.RepeatProperty.PropertyName)
            {
                UpdateRepeat();
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
            _timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (sender, e) => Element?.SetValue(VideoPlayer.CurrentTimePropertyKey, Control?.Position);

            Control.MediaOpened += (sender, args) =>
            {
                Element.SetValue(VideoPlayer.IsLoadingPropertyKey, false);
                Element.OnPlayerStateChanged(CreateVideoPlayerStateChangedEventArgs(PlayerState.Prepared));
            };

            Control.BufferingProgressChanged += (sender, args) => Element.SetValue(VideoPlayer.IsLoadingPropertyKey, Control.BufferingProgress < 1);

            Control.CurrentStateChanged += DisplayKeepAlive;

            Control.CurrentStateChanged += (sender, e) =>
            {
                switch (Control.CurrentState)
                {
                    case MediaElementState.Playing:
                        _timer.Start();
                        Element.OnPlaying(CreateVideoPlayerEventArgs());
                        break;

                    case MediaElementState.Paused:
                    case MediaElementState.Stopped:
                        _timer.Stop();
                        Element.OnPaused(CreateVideoPlayerEventArgs());
                        break;
                }
            };

            Control.MediaEnded += (sender, args) => {
                _timer.Stop();
                Element.OnCompleted(CreateVideoPlayerEventArgs());
            };

            Control.MediaFailed += (sender, args) =>
            {
                Element.SetValue(VideoPlayer.IsLoadingPropertyKey, false);
                Element.OnFailed(CreateVideoPlayerErrorEventArgs(args));
            };
        }

        /// <summary>
        /// Prevents the display from being deactivated when user action is no longer detected, such as when an app is playing video.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void DisplayKeepAlive(object sender, RoutedEventArgs args)
        {
            if (sender is MediaElement mediaElement && mediaElement.IsAudioOnly == false)
            {
                if (mediaElement.CurrentState == MediaElementState.Playing)
                {
                    if (_appDisplayRequest == null)
                    {
                        // This call creates an instance of the DisplayRequest object. 
                        _appDisplayRequest = new DisplayRequest();
                        _appDisplayRequest.RequestActive();
                    }
                }
                else // CurrentState is Buffering, Closed, Opening, Paused, or Stopped. 
                {
                    if (_appDisplayRequest != null)
                    {
                        // Deactivate the display request and set the var to null.
                        _appDisplayRequest.RequestRelease();
                        _appDisplayRequest = null;
                    }
                }
            }
        }

        /// <summary>
        /// Indicates whether media will begin playback immediately when the source is set.
        /// </summary>
        private void UpdateAutoPlay()
        {
            Control.AutoPlay = Element.AutoPlay;
        }

        /// <summary>
        /// Updates the display controls property on the native player.
        /// </summary>
        private void UpdateDisplayControls()
        {
            Control.AreTransportControlsEnabled = Element.DisplayControls;
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
                Control.Volume = nativeVolume;
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
                Element.SetValue(VideoPlayer.IsLoadingPropertyKey, true);
                var source = Element?.Source;
                var videoSourceHandler = VideoSourceHandler.Create(source);

                if (source != null && videoSourceHandler != null)
                {
                    var path = await videoSourceHandler.LoadVideoAsync(source, new CancellationToken());
                    Log.Info($"Video Source: {path}");
                    
                    if (string.IsNullOrEmpty(path))
                    {
                        Control.Source = null;
                    }
                    else
                    {
                        Control.Source = new Uri(path);
                        Element.SetValue(VideoPlayer.CurrentTimePropertyKey, TimeSpan.Zero);
                        Element.OnPlayerStateChanged(CreateVideoPlayerStateChangedEventArgs(PlayerState.Initialized));
                    }
                }
                else
                {
                    Control.Source = null;
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex);
                Element.SetValue(VideoPlayer.IsLoadingPropertyKey, false);
            }
        }

        /// <summary>
        /// Updates the repeat property on the native player.
        /// </summary>
        private void UpdateRepeat()
        {
            Control.IsLooping = Element.Repeat;
        }

        /// <summary>
        /// Updates the time elapsed interval of the video player.
        /// </summary>
        private void UpdateTimeElapsedInterval()
        {
            if (Element.TimeElapsedInterval > 0)
            {
                var timeElapsedIntervalSpan = TimeSpan.FromSeconds(Element.TimeElapsedInterval);
                var marker = new TimelineMarker { Time = timeElapsedIntervalSpan };
                Control.Markers.Add(marker);

                Control.MarkerReached += (sender, args) =>
                {
                    Element.OnTimeElapsed(CreateVideoPlayerEventArgs());
                    Control.Markers.Remove(args.Marker);

                    if (Element.TimeElapsedInterval > 0)
                    {
                        marker = new TimelineMarker {Time = args.Marker.Time.Add(timeElapsedIntervalSpan)};
                        Control.Markers.Add(marker);
                    }
                };
            }
            else
            {
                Control.Markers.Clear();
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
                    Control.Stretch = Stretch.Fill;
                    break;
                case FillMode.ResizeAspect:
                    Control.Stretch = Stretch.Uniform;
                    break;
                case FillMode.ResizeAspectFill:
                    Control.Stretch = Stretch.UniformToFill;
                    break;
                default:
                    Control.Stretch = Stretch.None;
                    break;
            }
        }

        /// <summary>
        /// Updates the visibility of the video player.
        /// </summary>
        private void UpdateVisibility()
        {
            Control.Visibility = Element.IsVisible ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Creates the video player event arguments.
        /// </summary>
        /// <returns>VideoPlayerEventArgs with information populated.</returns>
        private VideoPlayerEventArgs CreateVideoPlayerEventArgs()
        {
            var mediaElement = Control;

            if (mediaElement != null)
            {
                var duration = Control.NaturalDuration.HasTimeSpan ? Control.NaturalDuration.TimeSpan : TimeSpan.Zero;

                return new VideoPlayerEventArgs(
                    Control.Position,
                    duration,
                    (float)Control.PlaybackRate
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

            return videoPlayerEventArgs == null
                ? new VideoPlayerStateChangedEventArgs(state)
                : new VideoPlayerStateChangedEventArgs(videoPlayerEventArgs, state);
        }

        /// <summary>
        /// Creates the video player error event arguments.
        /// </summary>
        /// <param name="args">The <see cref="ExceptionRoutedEventArgs"/> instance containing the event data.</param>
        /// <returns>VideoPlayerErrorEventArgs with information populated.</returns>
        private VideoPlayerErrorEventArgs CreateVideoPlayerErrorEventArgs(ExceptionRoutedEventArgs args)
        {
            var videoPlayerEventArgs = CreateVideoPlayerEventArgs();

            return videoPlayerEventArgs == null
                ? new VideoPlayerErrorEventArgs(args.ErrorMessage)
                : new VideoPlayerErrorEventArgs(videoPlayerEventArgs, args.ErrorMessage, args);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
                return;

            MediaElement player;

            if (disposing && Control != null && (player = Control) != null)
            {
                player.Stop();
                player.Source = null;
            }

            _isDisposed = true;
            base.Dispose(disposing);
        }

        #endregion
    }
}
