using OledCareTool;
using OledCareTool.Properties; // Replace OledCareTool with your project name
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.Versioning;

namespace OledCareTool
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        [SupportedOSPlatform("windows")]
        static void Main()
        {
            // Runtime guard: ensure we run only on Windows 11 (initial build 22000) or newer.
            // This both prevents running on unsupported OSes and helps the platform-compatibility
            // analyzer when combined with the SupportedOSPlatform attributes on Windows-specific types.
            if (!OperatingSystem.IsWindowsVersionAtLeast(10, 0, 22000))
            {
                MessageBox.Show(
                    "OLED Care Tool requires Windows 11 (build 22000 or newer). The application will now exit.",
                    "Unsupported OS",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            // Standard WinForms setup
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // This is the key: Application.Run(context) keeps the app 
            // running until context.ExitThread() or Application.Exit() is called.
            Application.Run(new OledAppContext());
        }
    }

    // Mark this class as Windows-only to inform the analyzer that it uses Windows APIs.
    [SupportedOSPlatform("windows")]
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
        private double maxOpacity = 1.0;
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

        private void ShowSettings(object? sender, EventArgs e)
        {
            // Pass current settings to the form (you'll want to add these to Settings.Default later)
            using (var settingsForm = new SettingsForm(oledDeviceName, maxOpacity >= 0.99, (int)(maxOpacity * 100)))
            {
                if (settingsForm.ShowDialog() == DialogResult.OK)
                {
                    oledDeviceName = settingsForm.SelectedDevice;

                    // Set the target based on user choice
                    maxOpacity = settingsForm.UseFullBlackout ? 1.0 : settingsForm.DimOpacity;

                    Settings.Default.OledDeviceName = oledDeviceName;
                    // Note: You should add 'MaxOpacity' to your Project Settings to persist this
                    Settings.Default.Save();

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

            if (currentScreen.DeviceName != oledDeviceName || isTesting)
            {
                // Use the dynamic maxOpacity instead of 1.0
                targetOpacity = maxOpacity;

                foreach (var s in Screen.AllScreens)
                {
                    if (s.DeviceName == oledDeviceName)
                    {
                        if (overlay.Bounds != s.Bounds) overlay.Bounds = s.Bounds;
                        if (!overlay.Visible) overlay.Show();
                    }
                }
            }
            else { targetOpacity = 0.0; }
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