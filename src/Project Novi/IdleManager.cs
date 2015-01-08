using Project_Novi.Api;
using System.Diagnostics;

namespace Project_Novi
{
    class IdleManager
    {
        private static bool _isIdle;

        public static Stopwatch idleTimer { get; set; }

        /// <summary>
        /// Check if current module is idle or not
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public static bool CheckIdle(IModule module)
        {
            if (idleTimer == null)
            {
                idleTimer = new Stopwatch();
                idleTimer.Start();
            }
            if (module.Name.Equals("Home"))
            {
                return CheckIdleTime(120);
            }
            else
            {
                return CheckIdleTime(60);
            }            
        }

        /// <summary>
        /// Check if idle time has been passed
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private static bool CheckIdleTime(int time)
        {
            if (idleTimer.ElapsedMilliseconds > time * 1000)
            {
                if (_isIdle)
                {
                    return false;
                }
                _isIdle = true;
                idleTimer.Restart();
            }
            else
            {
                _isIdle = false;
            }
            return _isIdle;
        }
            
    }
}
