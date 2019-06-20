using System;
using Xamarin.Forms;
using UriTypeConverter = Xamarin.Forms.UriTypeConverter;

namespace Octane.Xamarin.Forms.VideoPlayer
{
    /// <summary>
    /// A VideoSource that loads a video from a URI.
    /// </summary>
    public sealed class UriVideoSource : VideoSource
    {
        /// <summary>
        /// Backing store for the <see cref="P:Octane.Xamarin.Forms.VideoPlayer.UriVideoSource.Uri"/> property.
        /// </summary>
        public static readonly BindableProperty UriProperty = BindableProperty.Create(nameof(Uri), typeof(Uri), typeof(UriVideoSource), null);

        /// <summary>
        /// Gets or sets the URI for the video to get.
        /// </summary>
        [TypeConverter(typeof(UriTypeConverter))]
        public Uri Uri
        {
            get => (Uri)GetValue(UriProperty);
            set => SetValue(UriProperty, value);
        }

        /// <summary>
        /// Method that is called when the property that is specified by <paramref name="propertyName" /> is changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (propertyName == UriProperty.PropertyName)
                OnSourceChanged();

            base.OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Determines if two video sources are equivalent.
        /// </summary>
        /// <param name="other">The other video source.</param>
        /// <returns>
        /// True if the video sources are equal, false otherwise.
        /// </returns>
        public override bool Equals(VideoSource other)
        {
            return !(other is UriVideoSource) || ((UriVideoSource) other).Uri.Equals(Uri);
        }
    }
}
