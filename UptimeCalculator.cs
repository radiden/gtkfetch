using System;

namespace gtkfetch {
    public class UptimeCalculator {
        /// <summary> Gets uptime of system and formats it </summary>
        public static string GetUptimeStr() {
            // doesn't include time where machine was asleep
            TimeSpan uptime = new TimeSpan(Environment.TickCount64*TimeSpan.TicksPerMillisecond);
            
            if (uptime.Days == 0) {
                if (uptime.Hours == 0) {
                    return $"{uptime.Minutes} m";
                }
                else {
                    return $"{uptime.Hours} h, {uptime.Minutes} m";
                }
            }
            else {
                return $"{uptime.Days} d, {uptime.Hours} h, {uptime.Minutes} m";
            }
        }
    }
}