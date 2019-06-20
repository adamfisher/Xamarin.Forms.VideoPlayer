namespace Octane.Xamarin.Forms.VideoPlayer.Interfaces
{
	/// <summary>
	/// Provides a common set of properties and operations for platform specific video player renderers.
	/// </summary>
	internal interface IVideoPlayerRenderer
	{
        #region Methods

        /// <summary>
        /// Plays the video.
        /// </summary>
        void Play();

		/// <summary>
		/// Determines if the video player instance can play.
		/// </summary>
		///   <c>true</c> if this instance can play; otherwise, <c>false</c>.
		bool CanPlay();

		/// <summary>
		/// Pauses the video.
		/// </summary>
		void Pause();

		/// <summary>
		/// Determines if the video player instance can pause.
		/// </summary>
		///   <c>true</c> if this instance can pause; otherwise, <c>false</c>.
		bool CanPause();

		/// <summary>
		/// Stops the video.
		/// </summary>
		void Stop();

		/// <summary>
		/// Determines if the video player instance can stop.
		/// </summary>
		///   <c>true</c> if this instance can stop; otherwise, <c>false</c>.
		bool CanStop();

        /// <summary>
        /// Seeks a specific number of seconds into the playback stream.
        /// </summary>
        /// <param name="seekTime">The seek time.</param>
        void Seek(int seekTime);

		/// <summary>
		/// Determines if the video player instance can seek.
		/// </summary>
		/// <param name="time">The time in seconds.</param>
		/// <returns></returns>
		/// <c>true</c> if this instance can stop; otherwise, <c>false</c>.
		bool CanSeek(int time);

	    #endregion
	}
}
