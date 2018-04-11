Device Info Readme

## Android Current Activity Setup

This plugin uses the [Current Activity Plugin](https://github.com/jamesmontemagno/CurrentActivityPlugin/blob/master/README.md) to get access to the current Android Activity. Be sure to complete the full setup if a MainApplication.cs file was not automatically added to your application. Please fully read through the [Current Activity Plugin Documentation](https://github.com/jamesmontemagno/CurrentActivityPlugin/blob/master/README.md). At an absolute minimum you must set the following in your Activity's OnCreate method:

```csharp
Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, bundle);
```

It is highly recommended that you use a custom Application that are outlined in the Current Activity Plugin Documentation](https://github.com/jamesmontemagno/CurrentActivityPlugin/blob/master/README.md)
