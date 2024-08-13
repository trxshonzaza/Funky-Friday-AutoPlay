using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FunkyFridayAutoPlay
{
    internal class Pixels
    {
        public static Color GetColorAt(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);

            uint pixel = GetPixel(hdc, x, y);

            ReleaseDC(IntPtr.Zero, hdc);

            Color color = Color.FromArgb(255, (int)(pixel & 0x000000FF),             // R
                                              (int)(pixel & 0x0000FF00) >> 8,        // G
                                              (int)(pixel & 0x00FF0000) >> 16);      // B
            return color;
        }

        public static void PollGameCheck(Point location, Color color)
        {
            while (true)
            {
                var c = GetColorAt(location.X, location.Y);

                if (c.R == color.R && c.G == color.G && c.B == color.B)
                {
                    Program.GameStarted();
                    return;
                }

                Thread.Sleep(Program.pollingMS + 10);
            }
        }

        public static void PollPixelChangeToCallInput(int bind, Point location, Color color, Color color1)
        {
            bool isPressing = false;
            int poll = Program.pollingMS; // optimize memory load

            //int assumeEnded = 0;

            while (true)
            {
                var c = GetColorAt(location.X, location.Y);

                if (!c.Equals(color) && c.Equals(color1) && !isPressing)
                {
                    Input.Press(bind);
                    isPressing = true;
                }

                Thread.Sleep(7);

                if (isPressing && c.Equals(color1))
                {
                    Input.Press(bind, true);
                    isPressing = false;
                }

                Thread.Sleep(poll);
            }
        }

        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern uint GetPixel(IntPtr dc, int x, int y);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern int ReleaseDC(IntPtr window, IntPtr dc);
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);
        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr hWnd);
    }
}
