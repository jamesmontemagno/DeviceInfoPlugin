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
using Java.Interop;
using Android.Runtime;
using Android.Content.Res;
using Android.App;
using Android.Content;

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
		
		static JniPeerMembers buildMembers = new XAPeerMembers("android/os/Build", typeof(Build));

		static string GetSerialField()
		{
			try
			{
				const string id = "SERIAL.Ljava/lang/String;";
				var value = buildMembers.StaticFields.GetObjectValue(id);
				return JNIEnv.GetString(value.Handle, JniHandleOwnership.TransferLocalRef);
			}
			catch
			{
				return string.Empty;

			}
		}

		string id = string.Empty;
        /// <inheritdoc/>
        public string Id
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(id))
                    return id;


                id = GetSerialField();
                if(string.IsNullOrWhiteSpace(id) || id == Build.Unknown || id == "0")
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
                try
                {
                    var context = CrossCurrentActivity.Current.Activity ?? Application.Context;
                    if (context == null)
                        return Idiom.Unknown;


                    //easiest way to get ui mode
                    var uiModeManager = context.GetSystemService(Context.UiModeService) as UiModeManager;

                    try
                    {
                        switch (uiModeManager?.CurrentModeType ?? UiMode.TypeUndefined)
                        {
                            case UiMode.TypeTelevision: return Idiom.TV;
                            case UiMode.TypeCar: return Idiom.Car;
                        }
                    }
                    finally
                    {
                        uiModeManager?.Dispose();
                    }


                    var config = context.Resources.Configuration;

                    if (config == null)
                        return Idiom.Unknown;


                    var mode = config.UiMode;
                    if ((int)Build.VERSION.SdkInt >= 20)
                    {
                        if (mode.HasFlag(UiMode.TypeWatch))
                            return Idiom.Watch;
                    }

                    if (mode.HasFlag(UiMode.TypeTelevision))
                        return Idiom.TV;
                    if (mode.HasFlag(UiMode.TypeCar))
                        return Idiom.Car;
                    if (mode.HasFlag(UiMode.TypeDesk))
                        return Idiom.Desktop;

                    int minWidthDp = config.SmallestScreenWidthDp;

                    return minWidthDp >= TabletCrossover ? Idiom.Tablet : Idiom.Phone;
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Unable to get idiom: {ex}");
                }

                return Idiom.Unknown;
            }
        }

        /// <summary>
        /// Checks whether this is a real device or an emulator/simulator
        /// 
        /// Shamelessly taken from https://stackoverflow.com/a/13635166
        /// </summary>
        public bool IsDevice => !(
            Build.Fingerprint.StartsWith("generic", StringComparison.InvariantCulture)
            || Build.Fingerprint.StartsWith("unknown", StringComparison.InvariantCulture)
            || Build.Model.Contains("google_sdk")
            || Build.Model.Contains("Emulator")
            || Build.Model.Contains("Android SDK built for x86")
            || Build.Manufacturer.Contains("Genymotion")
            || (Build.Brand.StartsWith("generic", StringComparison.InvariantCulture) && Build.Device.StartsWith("generic", StringComparison.InvariantCulture))
            || Build.Product.Equals("google_sdk", StringComparison.InvariantCulture)
        );
    }
}
