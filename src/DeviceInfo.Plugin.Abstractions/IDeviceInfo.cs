﻿using System;

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

namespace Plugin.DeviceInfo.Abstractions
{
    /// <summary>
    /// Interface for DeviceInfo
    /// </summary>
    public interface IDeviceInfo
    {
        /// <summary>
        /// Generates a an AppId optionally using the PhoneId a prefix and a suffix and a Guid to ensure uniqueness
        ///
        /// The AppId format is as follows {prefix}guid{phoneid}{suffix}, where parts in {} are optional.
        /// </summary>
        /// <param name="usingPhoneId">Setting this to true adds the device specific id to the AppId (remember to give the app the correct permissions)</param>
        /// <param name="prefix">Sets the prefix of the AppId</param>
        /// <param name="suffix">Sets the suffix of the AppId</param>
        /// <returns></returns>
        string GenerateAppId(bool usingPhoneId = false, string prefix = null, string suffix = null);

        /// <summary>
        /// This is the device specific Id (remember the correct permissions in your app to use this)
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Get the model of the device
        /// </summary>
        string Model { get; }

		/// <summary>
		/// Get the name of the device
		/// </summary>
		string Manufacturer { get; }

		/// <summary>
		/// Get the name of the device
		/// </summary>
		string DeviceName { get; }

		/// <summary>
		/// Gets the version of the operating system as a string
		/// </summary>
		string Version { get; }

        /// <summary>
        /// Gets the version number of the operating system as a <see cref="Version"/>
        /// </summary>
        Version VersionNumber { get; }

		/// <summary>
		/// Returns the current version of the app, as defined in the PList, e.g. "4.3".
		/// </summary>
		/// <value>The current version.</value>
		string AppVersion { get; }


		/// <summary>
		/// Returns the current build of the app, as defined in the PList, e.g. "4300".
		/// </summary>
		/// <value>The current build.</value>
		string AppBuild { get; }

		/// <summary>
		/// Get the platform of the device
		/// </summary>
		Platform Platform { get; }

        /// <summary>
        /// Get the idom of the device
        /// </summary>
        Idiom Idiom { get; }

        /// <summary>
        /// Checks whether this is a real device or an emulator/simulator
        /// </summary>
        bool IsDevice { get; }

        /// <summary>
        /// Returns the current device language, as a two character string, e.g. "en", "fr", "nl"...
        /// </summary>
        string Language { get; }
    }
}
