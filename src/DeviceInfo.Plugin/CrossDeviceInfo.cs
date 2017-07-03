using Plugin.DeviceInfo.Abstractions;
using System;

namespace Plugin.DeviceInfo
{
	/// <summary>
	/// Cross platform DeviceInfo implemenations
	/// </summary>
	public class CrossDeviceInfo
	{
		static Lazy<IDeviceInfo> implementation = new Lazy<IDeviceInfo>(() => CreateDeviceInfo(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

		/// <summary>
		/// Gets if the plugin is supported on the current platform.
		/// </summary>
		public static bool IsSupported => implementation.Value == null ? false : true;

		/// <summary>
		/// Current plugin implementation to use
		/// </summary>
		public static IDeviceInfo Current
		{
			get
			{
				var ret = implementation.Value;
				if (ret == null)
				{
					throw NotImplementedInReferenceAssembly();
				}
				return ret;
			}
		}

		static IDeviceInfo CreateDeviceInfo()
		{
#if NETSTANDARD1_0
			return null;
#else
			return new DeviceInfoImplementation();
#endif
		}

		internal static Exception NotImplementedInReferenceAssembly() =>
			new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
		
	}
}
