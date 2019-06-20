using System;
using Xamarin.Forms;

namespace Octane.Xamarin.Forms.VideoPlayer
{
    public class VideoSourceConverter : TypeConverter
    {
        /// <summary>
        /// Determines whether this instance [can convert from] the specified source type.
        /// </summary>
        /// <param name="sourceType">Type of the source.</param>
        /// <returns></returns>
        public override bool CanConvertFrom(Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFromInvariantString(string value)
        {
            if (value == null)
                return null;

            Uri result;

            if (!Uri.TryCreate(value, UriKind.Absolute, out result) || result.Scheme == "file")
                return VideoSource.FromFile(value);

            return VideoSource.FromUri(result);
        }
    }
}
