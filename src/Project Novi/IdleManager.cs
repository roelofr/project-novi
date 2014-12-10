using System;
using System.Runtime.InteropServices;

namespace Project_Novi
{
    class IdleManager
    {
        private static bool _isIdle;
        [DllImport("user32.dll")]
        public static extern Boolean GetLastInputInfo(ref LastInputInfo plii);
        internal struct LastInputInfo
        {
            public uint CbSize;

            public uint DwTime;
        }

        public static uint GetIdleTime()
        {   
            // The number of ticks that passed since last input    
            long idleTicks = 0;
            // Set the struct    
            LastInputInfo lastInputInfo = new LastInputInfo();
            lastInputInfo.CbSize = (uint)Marshal.SizeOf(lastInputInfo);
            lastInputInfo.DwTime = 0;

            // If we have a value from the function    
            if (GetLastInputInfo(ref lastInputInfo))
            {
                // Get the number of ticks at the point when the last activity was seen    
                var lastInputTicks = lastInputInfo.DwTime;
                // Number of idle ticks = system uptime ticks - number of ticks at last input    
                idleTicks = (Environment.TickCount - lastInputTicks) / 1000;
            }

            return (uint)idleTicks;
        }

        public static bool CheckIdle()
        {
            if (GetIdleTime() > 10)
            {
                if (_isIdle)
                {
                    return false;
                }
                _isIdle = true;
            }
            else
            {
                _isIdle = false;
            }
            return _isIdle;
        }
    }
}
