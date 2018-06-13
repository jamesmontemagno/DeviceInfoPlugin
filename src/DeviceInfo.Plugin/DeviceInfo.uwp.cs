using Plugin.DeviceInfo.Abstractions;
using System;
using Windows.System.Profile;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Foundation.Metadata;
using Windows.ApplicationModel.Resources.Core;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel;

namespace Plugin.DeviceInfo
{
    /// <summary>
    /// Implementation for DeviceInfo
    /// </summary>
    public class DeviceInfoImplementation : IDeviceInfo
    {
		/// <summary>
		/// Get the name of the device
		/// </summary>
		public string Manufacturer => deviceInfo.SystemManufacturer;

		/// <summary>
		/// Get the name of the device
		/// </summary>
		public string DeviceName => deviceInfo.FriendlyName;

		EasClientDeviceInformation deviceInfo;
        public DeviceInfoImplementation()
        {
            deviceInfo = new EasClientDeviceInformation();
        }
        /// <inheritdoc/>
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
        string id = null;
        /// <inheritdoc/>
        public string Id
        {
            get
            {

                if (id != null)
                    return id;

                try
                {
                    if (ApiInformation.IsTypePresent("Windows.System.Profile.SystemIdentification"))
                    {
                        var systemId = SystemIdentification.GetSystemIdForPublisher();

                        // Make sure this device can generate the IDs
                        if (systemId.Source != SystemIdentificationSource.None)
                        {
                            // The Id property has a buffer with the unique ID
                            var hardwareId = systemId.Id;
                            var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(hardwareId);

                            var bytes = new byte[hardwareId.Length];
                            dataReader.ReadBytes(bytes);

                            id = Convert.ToBase64String(bytes);
                        }
                    }
                    else if (ApiInformation.IsTypePresent("Windows.System.Profile.HardwareIdentification"))
                    {
                        var token = HardwareIdentification.GetPackageSpecificToken(null);
                        var hardwareId = token.Id;
                        var dataReader = Windows.Storage.Streams.DataReader.FromBuffer(hardwareId);

                        var bytes = new byte[hardwareId.Length];
                        dataReader.ReadBytes(bytes);

                        id = Convert.ToBase64String(bytes);
                    }
                    else
                    {
                        id = "unsupported";
                    }

                }
                catch (Exception)
                {

                }

                return id;
            }
        }
		/// <inheritdoc/>
		public string Model => deviceInfo.SystemProductName;

		/// <inheritdoc/>
		public string Version
        {
            get
            {
                var sv = AnalyticsInfo.VersionInfo.DeviceFamilyVersion;
                try
                {

                    var v = ulong.Parse(sv);
                    var v1 = (v & 0xFFFF000000000000L) >> 48;
                    var v2 = (v & 0x0000FFFF00000000L) >> 32;
                    var v3 = (v & 0x00000000FFFF0000L) >> 16;
                    var v4 = v & 0x000000000000FFFFL;
                    return $"{v1}.{v2}.{v3}.{v4}";
                }
                catch { }

                return sv;
            }
        }
        /// <inheritdoc/>
        public Abstractions.Platform Platform
        {
            get
            {


                switch (AnalyticsInfo.VersionInfo.DeviceFamily)
                {
                    case "Windows.Mobile":
                        return Abstractions.Platform.WindowsPhone;
                    case "Windows.Desktop":
	                    try
	                    {
		                    return UIViewSettings.GetForCurrentView().UserInteractionMode == UserInteractionMode.Mouse
			                    ? Abstractions.Platform.Windows
			                    : Abstractions.Platform.WindowsTablet;
						}
	                    catch
	                    {
		                    return Abstractions.Platform.Windows;
				        }
                    case "Windows.IoT":
                        return Abstractions.Platform.IoT;
                    case "Windows.Xbox":
                        return Abstractions.Platform.Xbox;
                    case "Windows.Team":
                        return Abstractions.Platform.SurfaceHub;
                    default:
                        return Abstractions.Platform.Windows;
                }
            }
        }
        /// <inheritdoc/>
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
                    return new Version(10, 0);
                }
            }
        }

		/// <summary>
		/// Returns the current version of the app, as defined in the PList, e.g. "4.3".
		/// </summary>
		/// <value>The current version.</value>
		public string AppVersion
		{
			get
			{
				var version = Package.Current.Id.Version;

				return string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
			}
		}


		/// <summary>
		/// Returns the current build of the app, as defined in the PList, e.g. "4300".
		/// </summary>
		/// <value>The current build.</value>
		public string AppBuild => Package.Current.Id.Version.Build.ToString();

		public Idiom Idiom
        {
            get
            {
                switch (Platform)
                {
                    case Abstractions.Platform.Windows:
                        return Idiom.Desktop;
                    case Abstractions.Platform.WindowsPhone:
                        return Idiom.Phone;
                    case Abstractions.Platform.WindowsTablet:
                        return Idiom.Tablet;
                    default:
                        return Idiom.Unknown;

                }
            }
        }

        /// <summary>
        /// Checks whether this is a real device or an emulator/simulator
		/// 
		/// Source: http://igrali.com/2014/07/17/get-device-information-windows-phone-8-1-winrt/
        /// </summary>
		public bool IsDevice => deviceInfo.SystemProductName != "Virtual";
    }
}
