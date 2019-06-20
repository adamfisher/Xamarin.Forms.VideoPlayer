using System;
using System.Diagnostics;
using Octane.Xamarin.Forms.VideoPlayer.Diagnostics;
using Octane.Xamarin.Forms.VideoPlayer.UWP.Diagnostics;
using Exception = System.Exception;

[assembly: Xamarin.Forms.Dependency(typeof(WinPhoneLogger))]

namespace Octane.Xamarin.Forms.VideoPlayer.UWP.Diagnostics
{
    /// <summary>
    /// Platform specific logging implementation.
    /// </summary>
    internal class WinPhoneLogger : ILogger
    {
        #region Fields

        /// <summary>
        /// The prefix used to denote the log entry subject.
        /// </summary>
        private string _logEntryPrefix = "VideoPlayer";

        #endregion

        #region Methods

        /// <summary>
        /// Informations the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Info(string message)
        {
            Debug.WriteLine($"{_logEntryPrefix} [INFO]: {message}");
        }

        /// <summary>
        /// Warns the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Warn(string message)
        {
            Debug.WriteLine($"{_logEntryPrefix} [WARN]: {message}");
        }

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Error(string message)
        {
            Debug.WriteLine($"{_logEntryPrefix} [ERROR]: {message}");
        }

        /// <summary>
        /// Errors the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void Error(Exception exception)
        {
            Debug.WriteLine($"{_logEntryPrefix} [ERROR]: {exception.Message}{Environment.NewLine}{exception.StackTrace}");
        }

        /// <summary>
        /// Errors the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The message.</param>
        public void Error(Exception exception, string message)
        {
            Debug.WriteLine($"{_logEntryPrefix} [ERROR]: {message}{Environment.NewLine}{exception.Message}{Environment.NewLine}{exception.StackTrace}");
        }

        #endregion
    }
}