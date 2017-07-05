## Device Information Plugin for Xamarin and Windows

Simple way of getting common device information in Xamarin.iOS, Xamarin.Android, Windows, and Xamarin.Forms projects.

### Setup
* Available on NuGet: http://www.nuget.org/packages/Xam.Plugin.DeviceInfo [![NuGet](https://img.shields.io/nuget/v/Xam.Plugin.DeviceInfo.svg?label=NuGet)](https://www.nuget.org/packages/Xam.Plugin.DeviceInfo/)
* Install into your PCL project and Client projects.

Build status: [![Build status](https://ci.appveyor.com/api/projects/status/9y9lk3jjnxjo3tsd?svg=true)](https://ci.appveyor.com/project/JamesMontemagno/deviceinfoplugin)

**Platform Support**

|Platform|Version|
| ------------------- | :------------------: |
|Xamarin.iOS|iOS 7+|
|Xamarin.Android|API 10+|
|Windows 10 UWP|10+|
|Xamarin.Mac|All|
|Xamarin.Mac|All|
|watchOS|All|
|tvOS|All|


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


**Version**
```csharp
/// <summary>
/// Get the version of the Operating System
/// </summary>
string Version { get; }
```

Returns the specific version number of the OS such as:
* iOS: 8.1
* Android: 4.4.4
* Windows Phone: 8.10.14219.0
* WinRT: always 8.1 until there is a work around

**Platform**
```csharp
/// <summary>
/// Get the platform of the device
/// </summary>
Platform Platform { get; }
```

Returns the Platform Enum of:
```csharp
public enum Platform
{
  Android,
  iOS,
  WindowsPhone,
  Windows
}
```


#### Contributions
Contributions are welcome! If you find a bug please report it and if you want a feature please report it.

If you want to contribute code please file an issue and create a branch off of the current dev branch and file a pull request.

#### License
Under MIT, see LICENSE file.
