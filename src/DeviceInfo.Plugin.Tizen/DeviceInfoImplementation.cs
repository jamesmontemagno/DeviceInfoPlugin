using Plugin.DeviceInfo.Abstractions;
using System;

namespace Plugin.DeviceInfo
{
	/// <summary>
	/// Implementation for Feature
	/// </summary>
	public class DeviceInfoImplementation : IDeviceInfo
	{
		public string GenerateAppId(bool usingPhoneId = false, string prefix = null, string suffix = null)
		{
			var appId = "";

			if (!string.IsNullOrEmpty(prefix))
				appId += prefix;

			appId += Guid.NewGuid().ToString();

			if (usingPhoneId)
				appId += Id;

			if (!string.IsNullOrEmpty(suffix))
				appId += suffix;

			return appId;
		}

		/// <summary>
		/// Returns the unique identifier of the device if supported
		/// </summary>
		public string Id => string.Empty;

		/// <summary>
		/// Returns the model of the device
		/// </summary>
		public string Model
		{
			get
			{
				string version = null;
				Tizen.System.Information.TryGetValue<string>("tizen.org/system/model_name", out version);
				return version;
			}
		}

		/// <summary>
		/// Returns the version number as a string
		/// </summary>
		public string Version
		{
			get
			{
				string version = null;
				Tizen.System.Information.TryGetValue<string>("tizen.org/feature/platform.version", out version);
				return version;
			}
		}

		public Version VersionNumber
		{
			get
			{
				try
				{
					return new Version(Version);
				}
				catch
				{
					return new Version();
				}
			}
		}


		/// <summary>
		/// Returns the current build of the app, as defined in the PList, e.g. "4300".
		/// </summary>
		/// <value>The current build.</value>
		public string AppBuild => string.Empty;

		/// <summary>
		/// Returns the current version of the app, as defined in the PList, e.g. "4.3".
		/// </summary>
		/// <value>The current version.</value>
		public string AppVersion
		{
			get
			{
				try
				{
					var packageId = Tizen.Applications.Application.Current.ApplicationInfo.PackageId;
					return Tizen.Applications.PackageManager.GetPackage(packageId).Version;
				}
				catch
				{
					return string.Empty;
				}
			}
		}

		/// <summary>
		/// Returns platform of device
		/// </summary>
		public Platform Platform => Platform.Tizen;

		/// <summary>
		/// Returns the idiom type of the device
		/// </summary>
		public Idiom Idiom
		{
			get
			{
				string profile = null;
				Tizen.System.Information.TryGetValue<string>("tizen.org/feature/profile", out profile);
				if (profile.StartsWith("m") || profile.StartsWith("M"))
				{
					return Idiom.Phone;
				}
				else if (profile.StartsWith("w") || profile.StartsWith("W"))
				{
					return Idiom.Watch;
				}
				else if (profile.StartsWith("t") || profile.StartsWith("T"))
				{
					return Idiom.TV;
				}
				else if (profile.StartsWith("i") || profile.StartsWith("I"))
				{
					return Idiom.Car;
				}
				else
					return Idiom.Unknown;
			}
		}

		/// <summary>
		/// Checks whether this is a real device or an emulator/simulator
		/// </summary>
		public bool IsDevice
		{
			get
			{
				string arch = null;
				Tizen.System.Information.TryGetValue<string>("tizen.org/feature/platform.core.cpu.arch", out arch);
				bool armv7 = false;
				Tizen.System.Information.TryGetValue<bool>("tizen.org/feature/platform.core.cpu.arch.armv7", out armv7);
				bool x86 = false;
				Tizen.System.Information.TryGetValue<bool>("tizen.org/feature/platform.core.cpu.arch.x86", out x86);
				if (arch.Equals("armv7") && armv7 && !x86)
				{
					return true;
				}
				else if (arch.Equals("x86") && !armv7 && x86)
				{
					return false;
				}
				else
					return false;
			}
		}
	}
}
