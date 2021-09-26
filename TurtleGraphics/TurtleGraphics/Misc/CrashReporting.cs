using System;
using System.Collections.Generic;
using System.Text;
using Plugin.FirebaseCrashlytics;

namespace TurtleGraphics.Misc
{
    public static class CrashReporting
    {
        private static string _userid = null;
        public static string UserId
        {
            get => _userid;
            set { _userid = value; CrossFirebaseCrashlytics.Current.SetUserId(_userid); }
        }
        public static bool IsSupported() => CrossFirebaseCrashlytics.IsSupported;

        public static void Enable() => CrossFirebaseCrashlytics.Current.HandleUncaughtException();
        //public static void SetUserId(string userid) => CrossFirebaseCrashlytics.Current.SetUserId(userid);
        public static void Log(string message) => CrossFirebaseCrashlytics.Current.Log(message);

        public static void SetKey(string key, string value) => CrossFirebaseCrashlytics.Current.SetCustomKey(key, value);

        
    }
}
