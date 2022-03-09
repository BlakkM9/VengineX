using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Interop
{
    /// <summary>
    /// Class that provides functionality to show/hide the console window.
    /// </summary>
    public class ConsoleUtils
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AllocConsole();


        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();


        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);


        private const int SW_HIDE = 0;
        private const int SW_SHOW = 5;


        /// <summary>
        /// Shows the console.
        /// </summary>
        public static void ShowConsoleWindow()
        {
            var handle = GetConsoleWindow();

            if (handle == IntPtr.Zero)
            {
                AllocConsole();
            }
            else
            {
                ShowWindow(handle, SW_SHOW);
            }
        }


        /// <summary>
        /// Hides the console.
        /// </summary>
        public static void HideConsoleWindow()
        {
            var handle = GetConsoleWindow();
            ShowWindow(handle, SW_HIDE);
        }
    }
}
