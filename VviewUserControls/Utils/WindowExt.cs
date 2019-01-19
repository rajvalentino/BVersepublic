using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfScreenHelper;

namespace VviewUserControls.Utils
{
    static public class WindowExt
    {

        // NB : Best to call this function from the windows Loaded event or after showing the window
        // (otherwise window is just positioned to fill the secondary monitor rather than being maximised).
        public static void MaximizeToSecondaryMonitor(this Window window)
        {


            var secondaryScreen = Screen.AllScreens.Where(s => !s.Primary).FirstOrDefault();

            if (secondaryScreen != null)
            {
                if (!window.IsLoaded)
                    window.WindowStartupLocation = WindowStartupLocation.Manual;

                var workingArea = secondaryScreen.WorkingArea;
                window.Left = workingArea.Left;
                window.Top = workingArea.Top;
                window.Width = workingArea.Width;
                window.Height = workingArea.Height;
                // If window isn't loaded then maxmizing will result in the window displaying on the primary monitor
                if (window.IsLoaded)
                    window.WindowState = WindowState.Maximized;
            }
            else
            {
                secondaryScreen = Screen.AllScreens.Where(s => s.Primary).FirstOrDefault();
                if (secondaryScreen != null)
                {
                    if (!window.IsLoaded)
                        window.WindowStartupLocation = WindowStartupLocation.Manual;

                    var workingArea = secondaryScreen.WorkingArea;
                    window.Left = workingArea.Left;
                    window.Top = workingArea.Top;
                    window.Width = workingArea.Width;
                    window.Height = workingArea.Height;
                    // If window isn't loaded then maxmizing will result in the window displaying on the primary monitor
                    if (window.IsLoaded)
                        window.WindowState = WindowState.Maximized;
                }

            }
        }
        public static bool IsWindowOpen<T>(string name = "") where T : Window
        {
            return string.IsNullOrEmpty(name)
               ? Application.Current.Windows.OfType<T>().Any()
               : Application.Current.Windows.OfType<T>().Any(w => w.Name.Equals(name));
        }
    }
}
