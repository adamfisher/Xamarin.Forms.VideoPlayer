using Android.Content;

namespace Octane.Xamarin.Forms.VideoPlayer.Android.ExtensionMethods
{
    public static class ContextExtensions
    {
        /// <summary>
        /// Gets the height of the screen in pixels.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static float GetHeightInPixels(this Context context)
        {
            return context.Resources.DisplayMetrics.HeightPixels;
        }

        /// <summary>
        /// Gets the width of the screen in pixels.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static float GetWidthInPixels(this Context context)
        {
            return context.Resources.DisplayMetrics.WidthPixels;
        }

        /// <summary>
        /// Gets the height of the screen in dp.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static int GetHeighInDp(this Context context)
        {
            var heightInPixels = context.GetHeightInPixels();
            var scale = context.Resources.DisplayMetrics.Density;
            return (int)(heightInPixels / scale + 0.5f);
        }

        /// <summary>
        /// Gets the width of the screen in dp.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static int GetWidthInDp(Context context)
        {
            var widthInPixels = context.GetWidthInPixels();
            var scale = context.Resources.DisplayMetrics.Density;
            return (int)(widthInPixels / scale + 0.5f);
        }
    }
}