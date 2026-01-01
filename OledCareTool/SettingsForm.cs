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

        public SettingsForm(string currentDevice, bool currentBlackout, int currentDim)
        {
            InitializeComponent();
            // Populate dropdown with all available monitors
            var monitors = Screen.AllScreens.Select(s => s.DeviceName).ToArray();
            comboMonitors.Items.AddRange(monitors);

            // Select the current one
            if (monitors.Contains(currentDevice))
                comboMonitors.SelectedItem = currentDevice;

            chkFullBlackout.Checked = currentBlackout;
            trackDimLevel.Value = currentDim;
            trackDimLevel.Enabled = !currentBlackout;
            lblDimValue.Text = $"Dim: {currentDim}%";
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
            SelectedDevice = comboMonitors.SelectedItem?.ToString() ?? string.Empty;
            UseFullBlackout = chkFullBlackout.Checked;
            // Convert 0-100 scale to 0.0-1.0 double for the Opacity property
            DimOpacity = trackDimLevel.Value / 100.0;
        }
    }
}