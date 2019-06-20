namespace Octane.Xamarin.Forms.VideoPlayer.Interfaces
{
    internal interface IPlatformFeatures
    {
        #region Properties

        /// <summary>
        /// Gets the name of the package.
        /// </summary>
        /// <value>
        /// The name of the package.
        /// </value>
        string PackageName { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Hashes the specified value using SHA-1.
        /// </summary>
        /// <param name="value">The value to hash.</param>
        /// <returns>
        /// A base-64 encoded SHA-1 hash.
        /// </returns>
        string HashSha1(string value);

        /// <summary>
        /// Exits the mobile application.
        /// </summary>
        void Exit();

        #endregion
    }
}
