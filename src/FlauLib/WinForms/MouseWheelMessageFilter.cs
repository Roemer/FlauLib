using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FlauLib.WinForms
{
    /// <summary>
    /// Helper class to get correct mousewheel behavior (so that it works
    /// when the mouse is over and not just when the control has focus)
    /// Example usage:
    /// Application.AddMessageFilter(new MouseWheelMessageFilter());
    /// </summary>
    public class MouseWheelMessageFilter : IMessageFilter
    {
        const int WM_MOUSEWHEEL = 0x20a;

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_MOUSEWHEEL)
            {
                // LParam contains the location of the mouse pointer
                var pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);
                var hWnd = WindowFromPoint(pos);
                if (hWnd != IntPtr.Zero && hWnd != m.HWnd && Control.FromHandle(hWnd) != null)
                {
                    // Redirect the message to the correct control
                    SendMessage(hWnd, m.Msg, m.WParam, m.LParam);
                    return true;
                }
            }
            return false;
        }

        // P/Invoke declarations
        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(Point pt);
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
    }
}
