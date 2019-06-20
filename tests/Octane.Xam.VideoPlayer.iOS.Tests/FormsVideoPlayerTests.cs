using NUnit.Framework;
using Octane.Xam.VideoPlayer.Constants;
using Octane.Xam.VideoPlayer.Licensing;

namespace Octane.Xam.VideoPlayer.iOS.Tests
{
    [TestFixture]
    public class FormsVideoPlayerTests
    {
        [SetUp]
        public void Setup()
        {
            FormsVideoPlayer.IsInitialized = false;
        }

        [Test]
        public void IsInitializedStateChangesWhenCallingInit()
        {
            var initializedBeforeCallingInit = FormsVideoPlayer.IsInitialized;
            FormsVideoPlayer.Init();
            var initializedAfterCallingInit = FormsVideoPlayer.IsInitialized;

            Assert.IsFalse(initializedBeforeCallingInit);
            Assert.IsTrue(initializedAfterCallingInit);
            Assert.AreEqual(VideoPlayerLicense.LicenseType, LicenseType.Trial);
        }

        [Test]
        public void NoLicenseKeyPassedToInit()
        {
            FormsVideoPlayer.Init();
            
            Assert.IsTrue(FormsVideoPlayer.IsInitialized, "FormsVideoPlayer.IsInitialized");
            Assert.AreEqual(VideoPlayerLicense.LicenseType, LicenseType.Trial, "License should be in trial mode.");
        }

        [Test]
        public void ValidLicenseKeyPassedToInit()
        {
            var licenseKey = "AB0FAF4003456ECFE1F606BB343AA306D3D153E9";
            FormsVideoPlayer.Init(licenseKey);

            Assert.IsTrue(FormsVideoPlayer.IsInitialized, "FormsVideoPlayer.IsInitialized");
            Assert.AreEqual(VideoPlayerLicense.LicenseType, LicenseType.Full, "License should be in full mode.");
        }

        [Test]
        public void InvalidLicenseKeyPassedToInit()
        {
            var licenseKey = "07C54D83E0CC806F017B369D33E755E8253E8F95";

            Assert.Catch<VideoPlayerLicenseException>(() => FormsVideoPlayer.Init(licenseKey), "FormsVideoPlayer.Init(licenseKey)");
            Assert.IsFalse(FormsVideoPlayer.IsInitialized, "FormsVideoPlayer.IsInitialized");
            Assert.AreEqual(VideoPlayerLicense.LicenseType, LicenseType.Trial, "License should be in trial mode.");
        }
    }
}