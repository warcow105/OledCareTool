using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OledCareTool
{
    public partial class SettingsForm : Form
    {
        public string SelectedDevice { get; private set; } = string.Empty;
        public bool TestRequested { get; private set; } = false;
        public bool UseFullBlackout { get; private set; }
        public double DimOpacity { get; private set; }

        private class MonitorItem
        {
            public string FriendlyName { get; set; } = string.Empty;
            public string DeviceName { get; set; } = string.Empty;
            public override string ToString() => FriendlyName;
        }
        public SettingsForm(string currentDevice, bool currentBlackout, int currentDim)
        {
            InitializeComponent();
            chkFullBlackout.CheckedChanged += (s, e) => trackDimLevel.Enabled = !chkFullBlackout.Checked;
            trackDimLevel.Scroll += (s, e) => lblDimValue.Text = $"Dim: {trackDimLevel.Value}%";
            // Populate dropdown with all available monitors
            // var monitors = Screen.AllScreens.Select(s => s.DeviceName).ToArray();
            // comboMonitors.Items.AddRange(monitors);
            // 
            // Select the current one
            // if (monitors.Contains(currentDevice))
            //     comboMonitors.SelectedItem = currentDevice;

            // Populate dropdown with friendly names
            foreach (var screen in Screen.AllScreens)
            {
                var item = new MonitorItem
                {
                    // Note: Screen.DeviceName is the \\.\DISPLAY string
                    // We'll use a placeholder for now, or see below for the Win32 API 
                    // to get the actual hardware brand name.
                    FriendlyName = GetFriendlyName(screen),
                    DeviceName = screen.DeviceName
                };

                comboMonitors.Items.Add(item);

                // Select the current one if it matches
                if (screen.DeviceName == currentDevice)
                    comboMonitors.SelectedItem = item;
            }

            chkFullBlackout.Checked = currentBlackout;
            trackDimLevel.Value = currentDim;
            trackDimLevel.Enabled = !currentBlackout;
            lblDimValue.Text = $"Dim: {currentDim}%";
        }

        private string GetFriendlyName(Screen screen)
        {
            // Simple fallback: "Monitor 1 (Primary)", "Monitor 2", etc.
            int index = Array.IndexOf(Screen.AllScreens, screen) + 1;
            string label = $"Monitor {index}";
            if (screen.Primary) label += " (Primary)";

            // You can also append the resolution to help the user identify it
            label += $" [{screen.Bounds.Width}x{screen.Bounds.Height}]";

            return label;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            CaptureSettings();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            CaptureSettings();
            TestRequested = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void CaptureSettings()
        {
            if (comboMonitors.SelectedItem is MonitorItem selected)
            {
                SelectedDevice = selected.DeviceName;
            }
            UseFullBlackout = chkFullBlackout.Checked;
            DimOpacity = trackDimLevel.Value / 100.0;
        }
    }
}