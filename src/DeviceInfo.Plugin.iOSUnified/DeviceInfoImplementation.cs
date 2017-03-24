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
using Foundation;
#elif __WATCHOS__
using WatchKit;
#else
using UIKit;
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
#endif
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

#if __MACOS__
        public string Id => string.Empty;
#elif __WATCHOS__
        public string Id => string.Empty;
#else
        public string Id => UIDevice.CurrentDevice.IdentifierForVendor.AsString();
#endif

#if __MACOS__
        public string Model => string.Empty;
#elif __WATCHOS__
        public string Model => WKInterfaceDevice.CurrentDevice.Model;
#else
        public string Model => UIDevice.CurrentDevice.Model;
#endif

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


#if __IOS__
        public Platform Platform => Platform.iOS;
#elif __MACOS__
        public Platform Platform => Platform.macOS;
#elif __WATCHOS__
        public Platform Platform => Platform.watchOS;
#elif __TVOS__
        public Platform Platform => Platform.tvOS;
#endif



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
    }
}
