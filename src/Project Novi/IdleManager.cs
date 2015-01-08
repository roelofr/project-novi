using Project_Novi.Api;
using System.Diagnostics;

namespace Project_Novi
{
    class IdleManager
    {
        private static bool _isIdle;

        public static Stopwatch idleTimer { get; set; }

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
