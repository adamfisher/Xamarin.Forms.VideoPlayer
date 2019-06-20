using NUnit.Framework;

namespace Octane.Xam.VideoPlayer.Android.Tests
{
    [TestFixture]
    public class PlatformFeaturesTests
    {
        [Test]
        public void PackageNameTests()
        {
            var platformFeatures = new PlatformFeatures();
            var appIdentifier = platformFeatures.PackageName;

            Assert.AreEqual(appIdentifier, "Octane.Xam.VideoPlayer.Android.Tests",
                "The package name is not coming back as expected.");
        }

        [Test]
        public void HashSha1()
        {
            var platformFeatures = new PlatformFeatures();
            var hashedValue = platformFeatures.HashSha1("com.MyPackage.AppName");

            Assert.AreEqual(hashedValue.ToUpper(), "7604D67F58F960D8A8F2B8558269F649F7904ED8", 
                "Hashing a string does not produce the expected SHA-1 hash.");
        }
    }
}