using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FunkyFridayAutoPlay
{
    internal class Draw
    {
        public static void DrawRect(IntPtr handle, Rectangle rect)
        {
            IntPtr pointer = GetDC(IntPtr.Zero);
            Graphics g = Graphics.FromHdc(pointer);

            g.FillRectangle(Brushes.Red, rect);
            g.DrawRectangle(Pens.Red, rect);

            g.Dispose();
            //ReleaseDC(handle, pointer);
        }

        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("User32.dll")]
        public static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);
    }
}
