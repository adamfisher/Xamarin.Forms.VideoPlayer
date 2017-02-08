### 1.2.2

- Removed Android resolution code causing exception on OrientationChange.  ([#54](https://bitbucket.org/OctaneSoftware/octane.xam.videoplayer/issues/54/illegalstateexception-on-orientation)) 
- Fixed Android null exception when no source is specified for the video view ([#55](https://bitbucket.org/OctaneSoftware/octane.xam.videoplayer/issues/55/nullreferenceexception-in-onerror-during-a)) 


### 1.2.1

- Tweaked autoplay on iOS to wait for a PlayerItem to be ready to play instead of the player. ([#35](https://bitbucket.org/OctaneSoftware/octane.xam.videoplayer/issues/35/autoplay-not-consistently-working))
- Added check for valid Android MediaPlayer state before computing the player's new dimensions on orientation change ([#33](https://bitbucket.org/OctaneSoftware/octane.xam.videoplayer/issues/33/illegalstateexception-on-android-when))
- Fixed linker issues ([#29](https://bitbucket.org/OctaneSoftware/octane.xam.videoplayer/issues/29/android-error-cannot-find-default)) and ([#32](https://bitbucket.org/OctaneSoftware/octane.xam.videoplayer/issues/32/systemmissingmethodexception-after-adding))


### 1.2.0

- Added support for Xamarin Forms 2.3.2.127
- Fixed Android orientation change issue ([#16](https://bitbucket.org/OctaneSoftware/octane.xam.videoplayer/issues/16/resize-on-orientation-change))
- "Improved" Android video resolution appearing stretched ([#2](https://bitbucket.org/OctaneSoftware/octane.xam.videoplayer/issues/2/android-video-resolution-appears-stretched))
- Fixed iOS crashing when no source set ([#18](https://bitbucket.org/OctaneSoftware/octane.xam.videoplayer/issues/18/app-crashing-when-leaving-page-with-no))
- Fixed bug with Android TimeElapsedInterval not firing properly.
- Fixed bug with Windows platforms TimeElapsedInterval not firing properly.


### 1.1.4

- Fixed license validation check failing on Windows implementation.

### 1.1.3

- Fixed license validation check failing when trying to get the AppId on iOS

### 1.1.2

- Improved license key by simplifying sensitivity checks.