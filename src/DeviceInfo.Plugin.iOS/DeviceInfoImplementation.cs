/*
 * Ported with permission from: Thomasz Cielecki @Cheesebaron
 * AppId: https://github.com/Cheesebaron/Cheesebaron.MvxPlugins
 */
//---------------------------------------------------------------------------------
// Copyright 2013 Tomasz Cielecki (tomasz@ostebaronen.dk)
// Licensed under the Apache License, Version 2.0 (the "License"); 
// You may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0 

// THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED, 
// INCLUDING WITHOUT LIMITATION ANY IMPLIED WARRANTIES OR 
// CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, 
// MERCHANTABLITY OR NON-INFRINGEMENT. 

// See the Apache 2 License for the specific language governing 
// permissions and limitations under the License.
//---------------------------------------------------------------------------------
using Plugin.DeviceInfo.Abstractions;



#if __MACOS__
using AppKit;
using System.Runtime.InteropServices;
using Foundation;
using ObjCRuntime;
using System.Diagnostics;
#elif __WATCHOS__
using WatchKit;
using ObjCRuntime;
using Platform = Plugin.DeviceInfo.Abstractions.Platform;
#else
using UIKit;
using ObjCRuntime;
using Platform = Plugin.DeviceInfo.Abstractions.Platform;
#endif
using System;


namespace Plugin.DeviceInfo
{
    /// <summary>
    /// Implementation for DeviceInfo
    /// </summary>
    public class DeviceInfoImplementation : IDeviceInfo
    {
#if __MACOS__
        NSProcessInfo info;
        string id, model = null;
#endif
        /// <summary>
        /// Default Constructor
        /// </summary>
        public DeviceInfoImplementation()
        {

#if __MACOS__
            info = new NSProcessInfo();
#endif
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

        /// <summary>
        /// Returns the unique identifier of the device if supported
        /// </summary>
#if __MACOS__
        public string Id => id ?? (id = GetSerialNumber());


        [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
        static extern uint IOServiceGetMatchingService(uint masterPort, IntPtr matching);

        [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
        static extern IntPtr IOServiceMatching(string s);

        [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
        static extern IntPtr IORegistryEntryCreateCFProperty(uint entry, IntPtr key, IntPtr allocator, uint options);

        [DllImport("/System/Library/Frameworks/IOKit.framework/IOKit")]
        static extern int IOObjectRelease(uint o);

        string GetSerialNumber()
        {
            var serial = string.Empty;

            try
            {
                
                var platformExpert = IOServiceGetMatchingService(0, IOServiceMatching("IOPlatformExpertDevice"));
                if (platformExpert != 0)
                {
                    var key = (NSString)"IOPlatformSerialNumber";
                    var serialNumber = IORegistryEntryCreateCFProperty(platformExpert, key.Handle, IntPtr.Zero, 0);
                    if (serialNumber != IntPtr.Zero)
                    {
                        serial = Runtime.GetNSObject<NSString>(serialNumber);
                    }
                    IOObjectRelease(platformExpert);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get serial number: " + ex.Message);
            }

            return serial;
        }
#elif __WATCHOS__
        public string Id => string.Empty;
#else
        public string Id => UIDevice.CurrentDevice.IdentifierForVendor.AsString();
#endif

        /// <summary>
        /// Returns the model of the device
        /// </summary>
#if __MACOS__
        public string Model => model ?? (model = GetModel());

        string GetModel()
        {
            var modelString = string.Empty;

            try
            {
                var platformExpert = IOServiceGetMatchingService(0, IOServiceMatching("IOPlatformExpertDevice"));
                if (platformExpert != 0)
                {
                    var modelKey = (NSString)"model";
                    var model = IORegistryEntryCreateCFProperty(platformExpert, modelKey.Handle, IntPtr.Zero, 0);
                    if (model != IntPtr.Zero)
                    {
                        modelString = Runtime.GetNSObject<NSString>(model);          
                    }
                    IOObjectRelease(platformExpert);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get model: " + ex.Message);
            }

            return modelString;
        }
#elif __WATCHOS__
        public string Model => WKInterfaceDevice.CurrentDevice.Model;
#else
        public string Model => UIDevice.CurrentDevice.Model;
#endif

        /// <summary>
        /// Returns the version number as a string
        /// </summary>
#if __MACOS__
        public string Version => info.OperatingSystemVersionString;
#elif __WATCHOS__
        public string Version => WKInterfaceDevice.CurrentDevice.SystemVersion;
#else
        public string Version => UIDevice.CurrentDevice.SystemVersion;
#endif


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
                    return new Version();
                }
            }
        }

        /// <summary>
        /// Returns platform of device
        /// </summary>
#if __IOS__
        public Platform Platform => Platform.iOS;
#elif __MACOS__
        public Abstractions.Platform Platform => Abstractions.Platform.macOS;
#elif __WATCHOS__
        public Platform Platform => Platform.watchOS;
#elif __TVOS__
        public Platform Platform => Platform.tvOS;
#endif


        /// <summary>
        /// Returns the idiom type of the device
        /// </summary>
        public Idiom Idiom
        {
            get
            {
#if __MACOS__
                return Idiom.Desktop;
#elif __WATCHOS__
                return Idiom.Watch;
#elif __TVOS__
                return Idiom.TV;
#else 

                switch (UIDevice.CurrentDevice.UserInterfaceIdiom)
                {
                    case UIUserInterfaceIdiom.Pad:
                        return Idiom.Tablet;
                    case UIUserInterfaceIdiom.Phone:
                        return Idiom.Phone;
                    case UIUserInterfaceIdiom.TV:
                        return Idiom.TV;
                    case UIUserInterfaceIdiom.CarPlay:
                        return Idiom.Car;
                    default:
                        return Idiom.Unknown;
                }
        #endif
            }

        }

        /// <summary>
        /// Checks whether this is a real device or an emulator/simulator
        /// </summary>
#if __MACOS__
        public bool IsDevice => true; // There is no simulator for mac OS
#else
        public bool IsDevice => Runtime.Arch == Arch.DEVICE;
#endif
    }
}
