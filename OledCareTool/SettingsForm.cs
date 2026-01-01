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
        public string SelectedDevice { get; private set; }
        public bool TestRequested { get; private set; } = false;

        public SettingsForm(string currentDevice)
        {
            InitializeComponent();
            // Populate dropdown with all available monitors
            var monitors = Screen.AllScreens.Select(s => s.DeviceName).ToArray();
            comboMonitors.Items.AddRange(monitors);

            // Select the current one
            if (monitors.Contains(currentDevice))
                comboMonitors.SelectedItem = currentDevice;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SelectedDevice = comboMonitors.SelectedItem?.ToString();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            // Capture the selected device so the test knows which screen to black out
            SelectedDevice = comboMonitors.SelectedItem?.ToString();
            TestRequested = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}