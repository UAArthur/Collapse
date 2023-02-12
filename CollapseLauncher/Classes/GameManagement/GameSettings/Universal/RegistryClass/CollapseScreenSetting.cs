﻿using CollapseLauncher.Interfaces;
using Hi3Helper;
using Microsoft.Win32;
using System;
using System.Text;
using System.Text.Json;
using static CollapseLauncher.GameSettings.Statics;
using static Hi3Helper.Logger;

namespace CollapseLauncher.GameSettings.Universal
{
    internal class CollapseScreenSetting : IGameSettingsValue<CollapseScreenSetting>
    {
        #region Fields
        private const string _ValueName = "CollapseLauncher_ScreenSetting";
        #endregion

        #region Properties
        /// <summary>
        /// This defines if the game will be running in a custom resolution.<br/><br/>
        /// Default: false
        /// </summary>
        public bool UseCustomResolution { get; set; } = false;

        /// <summary>
        /// This defines if the game will be running in Exclusive Fullscreen mode.<br/><br/>
        /// Default: false
        /// </summary>
        public bool UseExclusiveFullscreen { get; set; } = false;

        /// <summary>
        /// This defines the Graphics API will be used for the game to run.<br/><br/>
        /// Values:<br/>
        ///     - 0 = DirectX 11 (Feature Level: 10.1)<br/>
        ///     - 1 = DirectX 11 (Feature Level: 11.0) No Single-thread<br/>
        ///     - 2 = DirectX 11 (Feature Level: 11.1)<br/>
        ///     - 3 = DirectX 11 (Feature Level: 11.1) No Single-thread<br/>
        ///     - 4 = DirectX 12 (Feature Level: 12.0) [Experimental]<br/><br/>
        /// Default: 3
        /// </summary>
        public byte GameGraphicsAPI { get; set; } = 3;
        #endregion

        #region Methods
#nullable enable
        public static CollapseScreenSetting Load()
        {
            try
            {
                if (RegistryRoot == null) throw new NullReferenceException($"Cannot load {_ValueName} RegistryKey is unexpectedly not initialized!");

                object? value = RegistryRoot.GetValue(_ValueName, null);

                if (value != null)
                {
                    ReadOnlySpan<byte> byteStr = (byte[])value;
                    return (CollapseScreenSetting?)JsonSerializer.Deserialize(byteStr.Slice(0, byteStr.Length - 1), typeof(CollapseScreenSetting), CollapseScreenSettingContext.Default) ?? new CollapseScreenSetting();
                }
            }
            catch (Exception ex)
            {
                LogWriteLine($"Failed while reading {_ValueName}\r\n{ex}", LogType.Error, true);
            }

            return new CollapseScreenSetting();
        }

        public void Save()
        {
            try
            {
                if (RegistryRoot == null) throw new NullReferenceException($"Cannot save {_ValueName} since RegistryKey is unexpectedly not initialized!");

                string data = JsonSerializer.Serialize(this, typeof(CollapseScreenSetting), CollapseScreenSettingContext.Default) + '\0';
                byte[] dataByte = Encoding.UTF8.GetBytes(data);

                RegistryRoot.SetValue(_ValueName, dataByte, RegistryValueKind.Binary);
            }
            catch (Exception ex)
            {
                LogWriteLine($"Failed to save {_ValueName}!\r\n{ex}", LogType.Error, true);
            }
        }
#nullable disable
        #endregion
    }
}