using System;

namespace Octane.Xamarin.Forms.VideoPlayer.ExtensionMethods
{
    /// <summary>
    /// EventHandler extension methods.
    /// </summary>
    internal static class EventHandlerExtensions
    {
        /// <summary>
        /// Raises the event.
        /// </summary>
        /// <param name="eventHandler">The event handler.</param>
        /// <param name="sender">The event sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs" /> instance containing the event data.</param>
        public static void RaiseEvent(this EventHandler eventHandler, object sender, EventArgs eventArgs = null)
        {
            eventHandler?.Invoke(sender, eventArgs ?? EventArgs.Empty);
        }

        /// <summary>
        /// Raises the event.
        /// </summary>
        /// <typeparam name="TEventArgs">The type of the event arguments.</typeparam>
        /// <param name="eventHandler">The event handler.</param>
        /// <param name="sender">The event sender.</param>
        /// <param name="eventArgs">The <see cref="EventArgs" /> instance containing the event data.</param>
        public static void RaiseEvent<TEventArgs>(this EventHandler<TEventArgs> eventHandler, object sender, TEventArgs eventArgs = null) where TEventArgs : EventArgs
        {
            eventHandler?.Invoke(sender, eventArgs ?? EventArgs.Empty as TEventArgs);
        }
    }
}
