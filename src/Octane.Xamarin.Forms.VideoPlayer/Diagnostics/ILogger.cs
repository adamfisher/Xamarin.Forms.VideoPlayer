using System;

namespace Octane.Xamarin.Forms.VideoPlayer.Diagnostics
{
    /// <summary>
    /// A native platform logging implementation.
    /// </summary>
    internal interface ILogger
    {
        #region Info

        /// <summary>
        /// Informations the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Info(string message);

        #endregion

        #region Warn

        /// <summary>
        /// Warns the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Warn(string message);

        #endregion

        #region Error

        /// <summary>
        /// Errors the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Error(string message);

        /// <summary>
        /// Errors the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        void Error(Exception exception);

        /// <summary>
        /// Errors the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="message">The message.</param>
        void Error(Exception exception, string message);

        #endregion
    }
}
