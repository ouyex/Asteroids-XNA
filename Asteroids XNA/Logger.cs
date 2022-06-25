using System;
using System.Collections.Generic;
using System.Text;

namespace Asteroids_XNA
{
    public static class Logger
    {
        enum LogType
        {
            INFO,
            ERROR
        };

        public static uint LogLevel;

        private static void LogTime(string message, uint logLevel, LogType type)
        {
            DateTime time = DateTime.Now;
            string timeString = $"[{time.DayOfWeek.ToString().Substring(0, 3)} {time.Hour}:{time.Minute}:{time.Second}] [{type}/{logLevel}]";
            Console.WriteLine(timeString + $": {message}");
        }

        public static void LogInfo(string message, uint logLevel = 2)
        {
            LogTime(message, logLevel, LogType.INFO);
        }

        public static void LogError(string message, string errorMessage = "")
        {
            if (errorMessage != "")
            {
                LogTime(message + $" {{errorMessage}}", 0, LogType.ERROR);
            }
            else
            {
                LogTime(message, 0, LogType.ERROR);
            }
        }
    }
}
