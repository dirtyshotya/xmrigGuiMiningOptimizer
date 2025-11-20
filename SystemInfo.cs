using System;
using System.Diagnostics;

namespace XmrigLauncher
{
    public static class SystemInfo
    {
        private static readonly PerformanceCounter cpuCounter;
        private static readonly PerformanceCounter ramCounter;

        static SystemInfo()
        {
            try
            {
                // CPU usage counter for total processor time
                cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                // RAM counter for available memory in MB
                ramCounter = new PerformanceCounter("Memory", "Available MBytes");

                // First call returns 0, so call once to initialize
                cpuCounter.NextValue();
            }
            catch
            {
                // If PerformanceCounter fails, set to null
                cpuCounter = null;
                ramCounter = null;
            }
        }

        // Returns CPU usage as a percentage
        public static float GetCpuUsage()
        {
            if (cpuCounter == null) return 0;
            try
            {
                return cpuCounter.NextValue();
            }
            catch
            {
                return 0;
            }
        }

        // Returns available RAM in MB
        public static float GetRamUsage()
        {
            if (ramCounter == null) return 0;
            try
            {
                return ramCounter.NextValue();
            }
            catch
            {
                return 0;
            }
        }
    }
}
