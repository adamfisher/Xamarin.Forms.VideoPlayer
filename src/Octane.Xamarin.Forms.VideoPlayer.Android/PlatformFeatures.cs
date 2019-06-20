using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Android.App;
using Octane.Xamarin.Forms.VideoPlayer.Interfaces;
using Application = Android.App.Application;

namespace Octane.Xamarin.Forms.VideoPlayer.Android
{
    /// <summary>
    /// Platform native methods needed in cross-platform code.
    /// </summary>
    /// <seealso cref="IPlatformFeatures" />
    internal class PlatformFeatures : IPlatformFeatures
    {
        #region Properties

        /// <summary>
        /// Gets the name of the package.
        /// </summary>
        /// <value>
        /// The name of the package.
        /// </value>
        public string PackageName => Application.Context.PackageName;

        #endregion

        #region Methods

        /// <summary>
        /// Hashes the specified value using SHA-1.
        /// </summary>
        /// <param name="value">The value to hash.</param>
        /// <returns>
        /// A base-64 encoded SHA-1 hash.
        /// </returns>
        public string HashSha1(string value)
        {
            var hash = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(value));
            return string.Join(string.Empty, hash.Select(b => b.ToString("x2")).ToArray());
        }

        /// <summary>
        /// Exits the mobile application.
        /// </summary>
        public void Exit()
        {
            var activity = global::Xamarin.Forms.Forms.Context as Activity;
            activity?.FinishAffinity();
        }

        #endregion
    }
}