using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace algs4.algs4
{
    public class StopwatchCPU
    {
        private readonly long _start;

        /// <summary>
        /// Initialize a stopwatch object.
        /// </summary>
        public StopwatchCPU()
        {
            _start = GetThreadTotalProcessorTime();
        }

        /// <summary>
        /// Returns the elapsed CPU time (in seconds) since the object was created.
        /// </summary>
        /// <returns></returns>
        public double ElapsedTime()
        {
            long now = GetThreadTotalProcessorTime();
            return (now - _start) / 1000.0;
        }

        private static long GetThreadTotalProcessorTime()
        {
            int currentWin32ThreadId = GetCurrentWin32ThreadId();
            foreach (ProcessThread thread in Process.GetCurrentProcess().Threads)
            {
                if (thread.Id == currentWin32ThreadId)
                {
                    return Convert.ToInt64(thread.TotalProcessorTime.TotalMilliseconds);
                }
            }
            return 0;
        }

        [DllImport("Kernel32", EntryPoint = "GetCurrentThreadId", ExactSpelling = true)]
        private static extern int GetCurrentWin32ThreadId();
    }
}