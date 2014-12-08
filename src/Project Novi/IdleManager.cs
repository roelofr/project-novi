using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Timers;

namespace Project_Novi
{
    class IdleManager
    {
        [DllImport("user32.dll")]
        public static extern Boolean GetLastInputInfo(ref LASTINPUTINFO plii);
        internal struct LASTINPUTINFO
        {
            public uint cbSize;

            public uint dwTime;
        }

        public static uint GetIdleTime()
        {
            //int Uptime = System.Environment.TickCount;
            //int LastInputTicks = 0;
            //int IdleTicks = 0;
            //LASTINPUTINFO LastInput = new LASTINPUTINFO();
            //LastInput.cbSize = (uint)System.Runtime.InteropServices.Marshal.SizeOf(LastInput);
            //LastInput.dwTime = 0;

            //if (GetLastInput(ref LastInput))    
            //{


            //return ((uint)Environment.TickCount/1000) - LastInput.dwTime;

            // Get the system uptime    
            int systemUptime = Environment.TickCount;
            // The tick at which the last input was recorded    
            int LastInputTicks = 0;
            // The number of ticks that passed since last input    
            int IdleTicks = 0;
            // Set the struct    
            LASTINPUTINFO LastInputInfo = new LASTINPUTINFO();
            LastInputInfo.cbSize = (uint)Marshal.SizeOf(LastInputInfo);
            LastInputInfo.dwTime = 0;

            // If we have a value from the function    
            if (GetLastInputInfo(ref LastInputInfo))
            {
                // Get the number of ticks at the point when the last activity was seen    
                LastInputTicks = (int)LastInputInfo.dwTime;
                // Number of idle ticks = system uptime ticks - number of ticks at last input    
                IdleTicks = systemUptime - LastInputTicks;
            }

            return (uint)IdleTicks;
        }

        public static Boolean CheckIdle()
        {
            if (GetIdleTime() > 100)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
