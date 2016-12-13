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
using System;
using Android.OS;
using Plugin.CurrentActivity;
using static Android.Provider.Settings;

namespace Plugin.DeviceInfo
{
    /// <summary>
    /// Implementation for Feature
    /// </summary>
    public class DeviceInfoImplementation : IDeviceInfo
    {
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

        string id = string.Empty;
        /// <inheritdoc/>
        public string Id
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(id))
                    return id;

                id = Build.Serial;
                if(id == Build.Unknown)
                {
                    try
                    {
                        var context = CrossCurrentActivity.Current.Activity ?? Android.App.Application.Context;
                        id = Secure.GetString(context.ContentResolver, Secure.AndroidId);
                    }
                    catch(Exception ex)
                    {
                        Android.Util.Log.Warn("DeviceInfo", "Unable to get id: " + ex.ToString());
                    }
                }

                return id;
            }
        }
        /// <inheritdoc/>
        public string Model => Build.Model;

        /// <inheritdoc/>
        public string Version => Build.VERSION.Release; 

        /// <inheritdoc/>
        public Platform Platform => Platform.Android; 

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

        const int TabletCrossover = 600;

        public Idiom Idiom
        {
            get
            {
                var context = CrossCurrentActivity.Current.Activity ?? Android.App.Application.Context;
                if (context == null)
                    return Idiom.Unknown;

                int minWidthDp = CrossCurrentActivity.Current.Activity.Resources.Configuration.SmallestScreenWidthDp;

                return  minWidthDp >= TabletCrossover ? Idiom.Tablet : Idiom.Phone;
            }
        }
    }
}
