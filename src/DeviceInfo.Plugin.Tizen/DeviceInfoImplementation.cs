using Plugin.DeviceInfo.Abstractions;
using System;

namespace Plugin.DeviceInfo
{
	/// <summary>
	/// Implementation for Feature
	/// </summary>
	public class DeviceInfoImplementation : IDeviceInfo
	{
		/// <summary>
		/// Get the name of the device
		/// </summary>
		public string Manufacturer
		{
			get
			{
				Tizen.System.Information.TryGetValue<string>("http://tizen.org/system/manufacturer", out var manufacturer);
				return manufacturer;
			}
		}

		/// <summary>
		/// Get the name of the device
		/// </summary>
		public string DeviceName
		{
			get
			{
				Tizen.System.Information.TryGetValue<string>("http://tizen.org/setting/device_name", out var name);
				return name;
			}
		}

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
				Tizen.System.Information.TryGetValue<string>("tizen.org/system/model_name", out var version);
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
				Tizen.System.Information.TryGetValue<string>("tizen.org/feature/platform.version", out var version);
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
				Tizen.System.Information.TryGetValue<string>("tizen.org/feature/profile", out var profile);
				if (profile == null)
					return Idiom.Unknown;

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
				Tizen.System.Information.TryGetValue<string>("tizen.org/feature/platform.core.cpu.arch", out var arch);
				Tizen.System.Information.TryGetValue<bool>("tizen.org/feature/platform.core.cpu.arch.armv7", out var armv7);
				Tizen.System.Information.TryGetValue<bool>("tizen.org/feature/platform.core.cpu.arch.x86", out var x86);
				if (arch != null && arch.Equals("armv7") && armv7 && !x86)
				{
					return true;
				}
				else if (arch != null && arch.Equals("x86") && !armv7 && x86)
				{
					return false;
				}
				else
					return false;
			}
		}

        public string NetworkCarrier => "";
	}
}
