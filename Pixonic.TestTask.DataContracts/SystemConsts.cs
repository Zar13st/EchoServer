using System;

namespace Pixonic.TestTask.DataContracts
{
    public static class SystemConsts
    {
        #region Public Fields

        public static string ConfigName = "config.txt";
        public static TimeSpan HeartBitTimeSpan = TimeSpan.FromMilliseconds(100);
        public static TimeSpan PseudoInfinity = TimeSpan.FromDays(40);
        public static string ServerName = "Test1";

        #endregion Public Fields
    }
}