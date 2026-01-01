using OledCareTool;
using OledCareTool.Properties; // Replace OledCareTool with your project name
using System;
using System.Drawing;
using System.Windows.Forms;

// [STAThread]
// static void Main()
// {
//     Application.EnableVisualStyles();
//     Application.SetCompatibleTextRenderingDefault(false);
//     Application.Run(new OledAppContext());
// }

namespace OledCareTool
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Standard WinForms setup
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // This is the key: Application.Run(context) keeps the app 
            // running until context.ExitThread() or Application.Exit() is called.
            Application.Run(new OledAppContext());
        }
    }
    public class OledAppContext : ApplicationContext
    {
        private NotifyIcon trayIcon;
        private Form overlay;
        // Use the fully qualified name to resolve ambiguity
        private System.Windows.Forms.Timer monitorTimer;
        private System.Windows.Forms.Timer fadeTimer;
        // private Timer monitorTimer;
        // private Timer fadeTimer;
        private string oledDeviceName;
        private double targetOpacity = 0;
        private bool isTesting = false;
        private System.Windows.Forms.Timer testTimeoutTimer;

        public OledAppContext()
        {
            // 1. Load the saved monitor name from settings
            oledDeviceName = Settings.Default.OledDeviceName;

            overlay = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                BackColor = Color.Black,
                TopMost = true,
                ShowInTaskbar = false,
                Opacity = 0,
                StartPosition = FormStartPosition.Manual
            };

            trayIcon = new NotifyIcon()
            {
                Icon = SystemIcons.Shield,
                ContextMenuStrip = new ContextMenuStrip(),
                Visible = true,
                Text = "OLED Care Tool"
            };
            trayIcon.ContextMenuStrip.Items.Add("Settings", null, ShowSettings);
            trayIcon.ContextMenuStrip.Items.Add("-");
            trayIcon.ContextMenuStrip.Items.Add("Exit", null, (s, e) => Exit());

            // Poll mouse every 150ms
            monitorTimer = new System.Windows.Forms.Timer { Interval = 150 };
            monitorTimer.Tick += (s, e) => CheckMousePosition();
            monitorTimer.Start();

            // Smooth fade at 50fps
            fadeTimer = new System.Windows.Forms.Timer { Interval = 20 };
            fadeTimer.Tick += (s, e) => UpdateOpacity();
            fadeTimer.Start();

            // Initialize the 10-second test timer
            testTimeoutTimer = new System.Windows.Forms.Timer { Interval = 10000 };
            testTimeoutTimer.Tick += (s, e) => {
                isTesting = false;
                testTimeoutTimer.Stop();
            };

            // Ensure the overlay doesn't block clicks during tests
            overlay.Enabled = false;
        }

        private void ShowSettings(object sender, EventArgs e)
        {
            using (var settingsForm = new SettingsForm(oledDeviceName))
            {
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    oledDeviceName = settingsForm.SelectedDevice;
                    Settings.Default.OledDeviceName = oledDeviceName;
                    Settings.Default.Save();

                    // If they clicked the Test button
                    if (settingsForm.TestRequested)
                    {
                        isTesting = true;
                        testTimeoutTimer.Start();
                    }
                }
            }
        }

        private void CheckMousePosition()
        {
            if (string.IsNullOrEmpty(oledDeviceName)) return;

            Point mousePos = Cursor.Position;
            Screen currentScreen = Screen.FromPoint(mousePos);

            // Logic: Trigger blackout if (Mouse is away) OR (Test mode is active)
            if (currentScreen.DeviceName != oledDeviceName || isTesting)
            {
                targetOpacity = 1.0;

                // --- ADD THIS LOGIC BELOW ---
                // Find the OLED screen and move the overlay to it
                foreach (var s in Screen.AllScreens)
                {
                    if (s.DeviceName == oledDeviceName)
                    {
                        // Move the overlay only if it's not already there
                        if (overlay.Bounds != s.Bounds)
                        {
                            overlay.Bounds = s.Bounds;
                        }

                        // Show the window if it's currently hidden
                        if (!overlay.Visible)
                        {
                            overlay.Show();
                        }
                    }
                }
            }
            else
            {
                targetOpacity = 0.0;
            }
        }

        private void UpdateOpacity()
        {
            if (overlay.Opacity < targetOpacity) overlay.Opacity += 0.05;
            else if (overlay.Opacity > targetOpacity) overlay.Opacity -= 0.05;

            if (overlay.Opacity <= 0 && targetOpacity == 0) overlay.Hide();
        }

        void Exit() { trayIcon.Visible = false; Application.Exit(); }
    }
}