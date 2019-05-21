using Plugin.DeviceInfo.Abstractions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;

namespace Plugin.DeviceInfo
{
	/// <summary>
	/// Implementation for DeviceInfo
	/// </summary>
	public class DeviceInfoImplementation : IDeviceInfo
	{
		public DeviceInfoImplementation()
		{
			var process = Process.GetCurrentProcess();

			AppVersion = process.MainModule.FileVersionInfo.ProductMajorPart + "." + process.MainModule.FileVersionInfo.ProductMinorPart;
			AppBuild = process.MainModule.FileVersionInfo.ProductBuildPart.ToString();
		}

		public string Id { get; } = NetworkInterface
			.GetAllNetworkInterfaces()
			.Where(nic =>
				nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
				nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
			.Select(nic => nic.GetPhysicalAddress().ToString())
			.FirstOrDefault();

		public string Model { get; } = Environment.OSVersion.Platform.ToString();

		public string Manufacturer => Environment.OSVersion.VersionString;

		public string DeviceName => Environment.MachineName;

		public string Version { get; } = Environment.OSVersion.Version.ToString();

		public Version VersionNumber => Environment.OSVersion.Version;

		public string AppVersion { get; }

		public string AppBuild { get; }

		public Platform Platform => Platform.Windows;

		public Idiom Idiom => Idiom.Desktop;

		public bool IsDevice => true;

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
	}
}
