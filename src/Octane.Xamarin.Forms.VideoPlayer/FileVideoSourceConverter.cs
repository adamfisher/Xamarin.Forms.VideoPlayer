using System;
using Xamarin.Forms;

namespace Octane.Xamarin.Forms.VideoPlayer
{
    /// <summary>
    /// Converts a source type into a FileVideoSource.
    /// </summary>
    internal class FileVideoSourceConverter : TypeConverter
    {
        public override bool CanConvertFrom(Type sourceType)
        {
            return sourceType == typeof(string);
        }

        /// <summary>
        /// When overriden in a derived class, converts XAML extension syntax into instances of various <see cref="N:Xamarin.Forms" /> types.
        /// </summary>
        public override object ConvertFromInvariantString(string file)
        {
            if (file != null)
                return (FileVideoSource) VideoSource.FromFile(file);

            throw new InvalidOperationException($"Cannot convert file into {typeof(FileVideoSource)}");
        }
    }
}
