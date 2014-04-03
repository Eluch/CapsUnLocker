using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CapsUnLocker
{
    class Worker
    {
        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        private bool shouldRun = false;

        public Worker()
        {
        }

        public void run()
        {
            shouldRun = true;
            while (shouldRun)
            {
                if (Console.CapsLock)
                {
                    Thread.Sleep(350); //I had to add this sleep, because without this the Caps Lock blinked..
                    const int KEYEVENTF_EXTENDEDKEY = 0x1;
                    const int KEYEVENTF_KEYUP = 0x2;
                    keybd_event(0x14, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
                    keybd_event(0x14, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP,
                    (UIntPtr)0);
                }
                Thread.Sleep(100); //This sleep needs for not to kill the processor.
            }
        }

        public void stopGracefully()
        {
            shouldRun = false;
        }
    }
}
