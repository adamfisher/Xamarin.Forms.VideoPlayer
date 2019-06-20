using System;
using Xamarin.Forms;

namespace Octane.Xamarin.Forms.VideoPlayer.Diagnostics
{
    /// <summary>
    /// A cross-platform log implementation.
    /// </summary>
    internal static class Log
    {
        #region Info

        /// <summary>
        /// Informations the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Info(string message)
        {
            var logger = DependencyService.Get<ILogger>();
            logger?.Info(message);
        }

        #endregion

        #region Warn

        /// <summary>
        /// Warns the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Warn(string message)
        {
            var logger = DependencyService.Get<ILogger>();
            logger?.Warn(message);
        }

        #endregion

        #region Error

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void Error(string message)
        {
            var logger = DependencyService.Get<ILogger>();
            logger?.Error(message);
        }

        /// <summary>
        /// Errors the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public static void Error(Exception exception)
        {
            var logger = DependencyService.Get<ILogger>();
            logger?.Error(exception);
        }

        /// <summary>
        /// Errors the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The message.</param>
        public static void Error(Exception exception, string message)
        {
            var logger = DependencyService.Get<ILogger>();
            logger?.Error(exception, message);
        }

        #endregion
    }
}
