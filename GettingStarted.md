# Getting Started with Video

<img src="http://i.imgur.com/CrqQVhP.png" />
<br/>
## Installation

### 1. Get Your License Key
After purchasing the component, you will need to fill out a short form here to obtain your license key: **http://goo.gl/forms/nIqUXVz1Im**. You should receive a response the same business day.

### 2. Initialization

When adding a video player to a Xamarin.Forms application, **Octane.Xam.VideoPlayer** is a a separate component package that you should add to every project in the solution using the Component Store Manager (see [Walkthrough: Including a Component in Your Project](https://developer.xamarin.com/guides/cross-platform/application_fundamentals/components_walkthrough)).

After installing the component package, insert the following initialization code in each native application project:

```csharp
FormsVideoPlayer.Init("LICENSE_KEY_HERE");
```

This call should be made after the `Xamarin.Forms.Forms.Init()` method call. The Xamarin.Forms templates have this call in the following files for each platform:

- **iOS** - `AppDelegate.cs` file, in the FinishedLaunching method.
- **Android** - `MainActivity.cs` file, in the OnCreate method.
- **Windows Phone 8 (Silverlight)** - `MainPage.xaml.cs` file, in the MainPage constructor.

Once the component package has been added and the initialization method called inside each applcation, Octane.Xam.VideoPlayer APIs can be used in the common PCL or Shared Project code.

### 3. Create a Video Player in XAML

The snippet below shows the most basic example of using the video player. Be sure to check out the Chill Player example application for more advanced example usage as well as all of the documentation that follows. 

```XML
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:o="clr-namespace:Octane.Xam.VideoPlayer;assembly=Octane.Xam.VideoPlayer"
             x:Class="VideoPlayerSamples.VideoPlayerBasicExamplePage"
             Title="Basic Video Player">

    <o:VideoPlayer Source="http://vjs.zencdn.net/v/oceans.mp4" />

</ContentPage>
```

**When developing on Android**, you will need to use a real device to debug your application since the emulator is likely not powerful enough for video playback.

<br/>
## VideoPlayer XAML Control

Shown below are the complete set of options available with the VideoPlayer control and the possible values separated by the bar symbol. All properties of the player are documented extremely well in code comments if you need more information on a specific property. Due to the vastly different nature of video playback on each platform, the properties and events below are the common ones that all platforms share.

The `Source` property takes a `VideoSource` which behaves very similarly to the Xamarin Forms <a href="https://developer.xamarin.com/guides/cross-platform/xamarin-forms/working-with/images/" target="_blank">ImageSource</a>. The same behavior and logic described in that documentation can also be applied here by using `VideoSource` instead of `ImageSource`.

```XAML
<Octane:VideoPlayer
	
	AutoPlay="True|False"
	DisplayControls="True|False"
	FillMode="Resize|ResizeAspect|ResizeAspectFill"
	TimeElapsedInterval="0,1,2,3..."
	Repeat="True|False"
	Volume="..."
	Source="http://..."
	
	Playing="VideoPlayer_OnPlaying"
	Paused="VideoPlayer_OnStopped"
	TimeElapsed="VideoPlayer_OnTimeElapsed"
	Completed="VideoPlayer_OnCompleted"
	Failed="VideoPlayer_OnFailed"
	PlayerStateChanged="VideoPlayer_OnPlayerStateChanged"
	/>
```

<br/>
## Player Properties

Common video player operations are described here.

| Property            | Description                                                                                                                                   |
|---------------------|-----------------------------------------------------------------------------------------------------------------------------------------------|
| AutoPlay            | Specifies that the video will start playing as soon as it is ready. Default value is False.                                                   |
| DisplayControls     | Specifies that video controls should be displayed (such as a play and pause button). Default value is True.                                   |
| FillMode            | Defines how the video content is displayed within the player layer's bounds. Default value is ResizeAspect.                                   |
| TimeElapsedInterval | The time interval in seconds for the TimeElapsed event firing to occur. Default value is 0 seconds which means TimeElapsed will not fire.     |
| Repeat              | Repeat video when playback is complete. Default is value False.                                                                               |
| Volume              | The volume level of the current media stream. The default is the value currently assigned to the system volume.	  							  |
| Source              | A local file path or remote URL to a video file.                                                                                              |
| CurrentTime         | A read-only bindable playback time for the current video.                                                                                     |
| State         	  | A read-only bindable property indicating the current state of the video player (e.g. Idle, Prepared, Playing, etc.)                           |
| IsLoading        	  | A read-only bindable property indicating the video is loading for playback.										                              |
<br/>
## Playback Events

This component gives you the ability to subscribe to a wide variety of events that typically occur during video playback with the event handlers below. The EventArgs that are passed to each event firing contain a wealth of information about the current state of the player (current time, total duration, volume level, etc.).

| Event              	| Description                                                                                                               	|
|--------------------	|---------------------------------------------------------------------------------------------------------------------------	|
| Playing            	| Notification the playback rate has changed to a non-zero rate due to the user pressing play or some other action.         	|
| Paused             	| Notification the playback rate has changed to zero due to the user pressing pause or some other action.                   	|
| TimeElapsed        	| Notification the specified number of seconds configured in the `VideoPlayer.TimeElapsedInterval` have passed.             	|
| Completed          	| Notification fires when the video player has reached the end of the playback stream.                                      	|
| Failed             	| Notification fires when the video player experiences an error during playback or during initialization of the media file. 	|
| PlayerStateChanged 	| Event notification fires when the video player's internal state machine changes state.                                    	|

<br/>
## Commands 

It's also possible to bind other Xamarin Forms controls such as buttons to common video player operations noted below. This could be useful in situations where you want to create your own cross-platform buttons for these operations and perhaps set `DisplayControls="False"` on the video player.

| Command      	| Description                          	|
|--------------	|------------------------------------	|
| PlayCommand  	| Begins playback of the media file. 	|
| PauseCommand 	| Stops playback of the media file.  	|
| SeekCommand  	| Seeks a specific number of seconds forward or backward in the playback stream. This command takes an integer command parameter equal to the number of seconds forward (positive number) or backward (negative number) to seek. |

<br/>
## Compatible Video Playback Streams

You will want to make sure the video files you play are compatible with the mobile platforms you are targeting. <a href="http://developer.android.com/guide/appendix/media-formats.html" target="_blank">Android</a>, <a href="https://developer.apple.com/library/ios/documentation/Miscellaneous/Conceptual/iPhoneOSTechOverview/MediaLayer/MediaLayer.html" target="_blank">iOS</a>, and <a href="https://msdn.microsoft.com/en-us/library/windows/apps/Ff462087(v=VS.105).aspx" target="_blank">Windows Phone</a> all have documentation on the media formats and baseline encoding parameters they support.

This video player component requires **a valid URL to a video file with a file extension (for example: .mp4, .webm, .avi)**. If you want to use a video codec that works on the majority of devices, use H.264-encoded video in MP4 files.


### Streaming Video From Popular Sites like YouTube and Vimeo

Without some help, the native video players on mobile devices are unable to stream URLs directly from popular video hosting sites like YouTube and Vimeo. There is a little more work involved to get the correct URL for playback. **The XAML markup extensions for Vimeo and YouTube shown below are provided as a convenience in the Chill Player sample app and are not guaranteed to be as reliable as integrating the YouTube or Vimeo APIs into your application. They are provided as experimental functionality.**

<br/>
## XAML Markup Extensions

The demo app (Chill Player) comes with several useful extensions methods to make your XAML development more enjoyable.

| Markup Extension       	| Description                                                             	|
|------------------------	|-------------------------------------------------------------------------	|
| VideoResourceExtension 	| Constructs a VideoSource from a video resource embedded in a PCL.<br/>**Example:** `<o:VideoPlayer Source="{e:VideoResourceExtension demo.mp4 }" />`	|
| YouTubeVideoId         	| Converts a YouTube video ID into a compatible stream URL for playback.<br/>**Example:** `<o:VideoPlayer Source="{e:YouTubeVideoId vu4WK-FMyIg }" />` 		|
| VimeoVideoId           	| Converts a Vimeo video ID into a compatible stream URL for playback.<br/>**Example:** `<o:VideoPlayer Source="{e:VimeoVideoId 73901538 }" />`    			|

<br/>You can access these markup extensions by adding the following namespace to your XAML page:

`xmlns:me="clr-namespace:ChillPlayer.MarkupExtensions;assembly=ChillPlayer"`

<br/>
## App Permissions

Depending on what you are using the video player for, you may need to add some of the following permissions to your manifest files for each respective platform.

> ### Streaming Video
> If you are using the video player to stream network-based content, your application must request network access.

> | Platform | Manifest Declaration |
> |----------|----------------------|
> | Android  | In AndroidManifest.xml:<br/>```<uses-permission android:name="android.permission.INTERNET" />```	|
> | iOS		 | iOS 9+ requires permission to stream basic http (non-https) content due to Apple's new App Transport Security (ATS). The below convenience snippet can be placed in the info.plist to allow all http based URLs through but you will want to **add permissions for specific domains to prevent app store rejection.**<br/> ```<key>NSAppTransportSecurity</key><dict><key>NSAllowsArbitraryLoads</key><true/></dict>```|

> ### Wake Lock Permission
> If your player application needs to keep the screen from dimming or the processor from sleeping.

> | Platform | Manifest Declaration |
> |----------|----------------------|
> | Android  | In AndroidManifest.xml:<br/>```<uses-permission android:name="android.permission.WAKE_LOCK" />``` |

<br/>
## Useful Resources

- <a href="http://videohive.net/?ref=octanesoftware" target="_blank">VideoHive</a>: 215,653 Royalty Free Video Files From $4
- <a href="http://www.dreamstime.com/stock-video-footage#res11336177" target="_blank">Dreamstime</a>: Professional stock video

<br/>
## Bug Queue

If you have a feature request, enhancement, or need to report a bug with this component, please **<a href="https://bitbucket.org/OctaneSoftware/octane.xam.videoplayer/issues" target="_blank">open a new issue</a>** in the issue tracker.