using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Octane.Xam.VideoPlayer;

namespace ChillPlayer.MarkupExtensions
{
    /// <summary>
    /// Loads an embedded resource from the currently executing assembly.
    /// </summary>
    [ContentProperty("Resource")]
    public class VideoResourceExtension : IMarkupExtension
    {
        /// <summary>
        /// The id of the EmbeddedResource to load.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public string Resource { get; set; }

        /// <summary>
        /// Provides the value.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Resource == null)
                return null;

            var videoSource = VideoSource.FromResource(Resource);

            return videoSource;
        }
    }
}