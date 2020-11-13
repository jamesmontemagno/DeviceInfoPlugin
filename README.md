## Device Information Plugin for Xamarin and Windows

Simple way of getting common device information in Xamarin.iOS, Xamarin.Android, Windows, and Xamarin.Forms projects.

### Setup
* Available on NuGet: http://www.nuget.org/packages/Xam.Plugin.DeviceInfo [![NuGet](https://img.shields.io/nuget/v/Xam.Plugin.DeviceInfo.svg?label=NuGet)](https://www.nuget.org/packages/Xam.Plugin.DeviceInfo/)
* Install into your PCL/netstandard project and Client projects.

Build status: ![Build status](https://jamesmontemagno.visualstudio.com/_apis/public/build/definitions/6b79a378-ddd6-4e31-98ac-a12fcd68644c/20/badge)

### The Future: [Xamarin.Essentials](https://docs.microsoft.com/xamarin/essentials/index?WT.mc_id=friends-0000-jamont)

I have been working on Plugins for Xamarin for a long time now. Through the years I have always wanted to create a single, optimized, and official package from the Xamarin team at Microsoft that could easily be consumed by any application. The time is now with [Xamarin.Essentials](https://docs.microsoft.com/xamarin/essentials/index?WT.mc_id=friends-0000-jamont), which offers over 50 cross-platform native APIs in a single optimized package. I worked on this new library with an amazing team of developers and I highly highly highly recommend you check it out.

I will continue to work and maintain my Plugins, but I do recommend you checkout Xamarin.Essentials to see if it is a great fit your app as it has been for all of mine!

### Platform Support

|Platform|Version|
| ------------------- | :------------------: |
|Xamarin.iOS|iOS 7+|
|Xamarin.Android|API 13+|
|Windows 10 UWP|10+|
|Xamarin.Mac|All|
|Xamarin.Mac|All|
|watchOS|All|
|tvOS|All|
|Tizen|4.0+|


### API Usage

Call **CrossDeviceInfo.Current** from any project or PCL to gain access to APIs.

**GenerateAppId**
Used to generate a unique Id for your app.

```csharp
/// <summary>
/// Generates a an AppId optionally using the PhoneId a prefix and a suffix and a Guid to ensure uniqueness
///
/// The AppId format is as follows {prefix}guid{phoneid}{suffix}, where parts in {} are optional.
/// </summary>
/// <param name="usingPhoneId">Setting this to true adds the device specific id to the AppId (remember to give the app the correct permissions)</param>
/// <param name="prefix">Sets the prefix of the AppId</param>
/// <param name="suffix">Sets the suffix of the AppId</param>
/// <returns></returns>
string GenerateAppId(bool usingPhoneId = false, string prefix = null, string suffix = null);
```


**Id**
This should not be used as a stable ID as each vendor on Android may or may not set it to a different value and on iOS it is changed when applications are uninstalled.

```csharp
/// <summary>
/// This is the device specific Id (remember the correct permissions in your app to use this)
/// </summary>
string Id { get; }
```

**Device Model**
```csharp
/// <summary>
/// Get the model of the device
/// </summary>
string Model { get; }
```


**Manufacturer**
```csharp
/// <summary>
/// Get the name of the device
/// </summary>
string Manufacturer { get; }
```


**DeviceName**
```csharp
/// <summary>
/// Get the name of the device
/// </summary>
string DeviceName { get; }
```


**Version**
```csharp
/// <summary>
/// Gets the version of the operating system as a string
/// </summary>
string Version { get; }
```

Returns the specific version number of the OS such as:
* iOS: 8.1
* Android: 4.4.4
* Windows Phone: 8.10.14219.0
* UWP: 10.0.14393.105
* Tizen: 4.0

**VersionNumber**
```csharp
/// <summary>
/// Gets the version number of the operating system as a Version
/// </summary>
Version VersionNumber { get; }
```

**AppVersion**
```csharp
/// <summary>
/// Returns the current version of the app, as defined in the PList, e.g. "4.3".
/// </summary>
/// <value>The current version.</value>
string AppVersion { get; }
```

**AppBuild**
```csharp
/// <summary>
/// Returns the current build of the app, as defined in the PList, e.g. "4300".
/// </summary>
/// <value>The current build.</value>
string AppBuild { get; }
```

**Platform**
```csharp
/// <summary>
/// Get the platform of the device
/// </summary>
Platform Platform { get; }
```

Returns the Platform enum of:
```csharp
public enum Platform
{
    Android,
    iOS,
    WindowsPhone,
    Windows,
    WindowsTablet,
    SurfaceHub,
    Xbox,
    IoT,
    Unknown,
    tvOS,
    watchOS,
    macOS,
    Tizen
}
```

**Idiom**
```csharp
/// <summary>
/// Get the idom of the device
/// </summary>
Idiom Idiom { get; }
```

Returns the Idiom enum of:
```csharp
public enum Idiom
{
    Unknown,
    Car,
    Desktop,
    Phone,
    Tablet,
    TV,
    Watch
}
```

**IsDevice**
```csharp
/// <summary>
/// Checks whether this is a real device or an emulator/simulator
/// </summary>
bool IsDevice { get; }
```

Returns `true`, if the app is running on a real physical device. `false` is returned if the app is running on an emulator or simulator (whatever applies for the platform).


## Additional Android Setup

This plugin uses the [Current Activity Plugin](https://github.com/jamesmontemagno/CurrentActivityPlugin/blob/master/README.md) to get access to the current Android Activity. Be sure to complete the full setup if a MainApplication.cs file was not automatically added to your application. Please fully read through the [Current Activity Plugin Documentation](https://github.com/jamesmontemagno/CurrentActivityPlugin/blob/master/README.md). At an absolute minimum you must set the following in your Activity's OnCreate method:

```csharp
Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);
```

It is highly recommended that you use a custom Application that are outlined in the Current Activity Plugin Documentation](https://github.com/jamesmontemagno/CurrentActivityPlugin/blob/master/README.md)


#### Contributions
Contributions are welcome! If you find a bug please report it and if you want a feature please report it.

If you want to contribute code please file an issue and create a branch off of the current dev branch and file a pull request.

#### License
Under MIT, see LICENSE file.
