using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Miracle.Helpers
{
    public class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        private const string IdIsUserLoggedIn = "IsUserLoggedIn";
        private static readonly bool IdIsUserLoggedInDefault = false;

        public static bool IsUserLoggedIn
        {
            get => AppSettings.GetValueOrDefault(IdIsUserLoggedIn, IdIsUserLoggedInDefault);
            set => AppSettings.AddOrUpdateValue(IdIsUserLoggedIn, value);
        }

    }
}
