using System.Threading.Tasks;
using Xamarin.Forms;

namespace Octane.Xamarin.Forms.VideoPlayer
{
    /// <summary>
    /// An <see cref="T:Octane.Xamarin.Forms.VideoPlayer.VideoSource"/> that reads a video from a file.
    /// </summary>
    [TypeConverter(typeof(FileVideoSourceConverter))]
    public class FileVideoSource : VideoSource
    {
        /// <summary>
        /// Backing store for the <see cref="P:Octane.Xamarin.Forms.VideoPlayer.FileVideoSource.File"/> property.
        /// </summary>
        public static readonly BindableProperty FileProperty = BindableProperty.Create(nameof(File), typeof (string), typeof (FileVideoSource), null);

        /// <summary>
        /// Gets or sets the file from which this <see cref="T:Octane.Xamarin.Forms.VideoPlayer.FileVideoSource"/> will load a video.
        /// </summary>
        public string File
        {
            get { return (string) GetValue(FileProperty); }
            set { SetValue(FileProperty, value); }
        }

        /// <summary>
        /// Allows implicit casting from a string.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator FileVideoSource(string file)
        {
            return (FileVideoSource) FromFile(file);
        }

        /// <summary>
        /// Allows implicit casting to a string.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string (FileVideoSource file)
        {
            return file?.File;
        }

        /// <summary>
        /// Method that is called when the property that is specified by <paramref name="propertyName" /> is changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected override void OnPropertyChanged(string propertyName = null)
        {
            if (propertyName == FileProperty.PropertyName)
                OnSourceChanged();

            base.OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Request a cancel of the VideoSource loading.
        /// </summary>
        /// <remarks>
        /// Overriden for FileVideoSource. FileVideoSource are not cancellable, so this will always return
        /// a completed Task with <see langword="false"/> as Result.
        /// </remarks>
        public override Task<bool> Cancel()
        {
            return Task.FromResult(false);
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
            return !(other is FileVideoSource) || ((FileVideoSource)other).File.Equals(File);
        }
    }
}
