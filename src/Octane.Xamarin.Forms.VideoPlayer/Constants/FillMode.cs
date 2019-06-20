namespace Octane.Xamarin.Forms.VideoPlayer.Constants
{
    /// <summary>
    /// Define how the video is displayed within a layer’s bounds.
    /// </summary>
    public enum FillMode
    {
        /// <summary>
        /// The video stretches to fill the layer’s bounds.
        /// </summary>
        Resize,

        /// <summary>
        /// The video’s aspect ratio is preserved and fits the video within the layer’s bounds.
        /// </summary>
        ResizeAspect,

        /// <summary>
        /// The video’s aspect ratio is preserved and fills the layer’s bounds.
        /// </summary>
        ResizeAspectFill
    }
}
