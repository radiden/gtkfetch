using System;

namespace gtkfetch {
    public class UptimeCalculator {
        public static string GetUptimeStr() {
            // gets the uptime, excluding the time the machine slept
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