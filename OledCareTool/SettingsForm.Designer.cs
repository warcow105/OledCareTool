namespace OledCareTool
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            comboMonitors = new ComboBox();
            btnSave = new Button();
            btnTest = new Button();
            chkFullBlackout = new CheckBox();
            trackDimLevel = new TrackBar();
            lblDimValue = new Label();
            ((System.ComponentModel.ISupportInitialize)trackDimLevel).BeginInit();
            SuspendLayout();
            // 
            // comboMonitors
            // 
            comboMonitors.FormattingEnabled = true;
            comboMonitors.Location = new Point(48, 84);
            comboMonitors.Name = "comboMonitors";
            comboMonitors.Size = new Size(693, 49);
            comboMonitors.TabIndex = 0;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(553, 371);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(188, 58);
            btnSave.TabIndex = 1;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnTest
            // 
            btnTest.Location = new Point(48, 371);
            btnTest.Name = "btnTest";
            btnTest.Size = new Size(188, 58);
            btnTest.TabIndex = 2;
            btnTest.Text = "Test";
            btnTest.UseVisualStyleBackColor = true;
            btnTest.Click += btnTest_Click;
            // 
            // chkFullBlackout
            // 
            chkFullBlackout.AutoSize = true;
            chkFullBlackout.Location = new Point(48, 212);
            chkFullBlackout.Name = "chkFullBlackout";
            chkFullBlackout.Size = new Size(223, 45);
            chkFullBlackout.TabIndex = 3;
            chkFullBlackout.Text = "Full Blackout";
            chkFullBlackout.UseVisualStyleBackColor = true;
            // 
            // trackDimLevel
            // 
            trackDimLevel.Location = new Point(481, 212);
            trackDimLevel.Maximum = 100;
            trackDimLevel.Minimum = 10;
            trackDimLevel.Name = "trackDimLevel";
            trackDimLevel.Size = new Size(260, 114);
            trackDimLevel.TabIndex = 4;
            trackDimLevel.Value = 50;
            // 
            // lblDimValue
            // 
            lblDimValue.AutoSize = true;
            lblDimValue.Location = new Point(563, 155);
            lblDimValue.Name = "lblDimValue";
            lblDimValue.Size = new Size(144, 41);
            lblDimValue.TabIndex = 5;
            lblDimValue.Text = "Dim: 50%";
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(17F, 41F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblDimValue);
            Controls.Add(trackDimLevel);
            Controls.Add(chkFullBlackout);
            Controls.Add(btnTest);
            Controls.Add(btnSave);
            Controls.Add(comboMonitors);
            Name = "SettingsForm";
            Text = "SettingsForm";
            ((System.ComponentModel.ISupportInitialize)trackDimLevel).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox comboMonitors;
        private Button btnSave;
        private Button btnTest;
        private CheckBox chkFullBlackout;
        private TrackBar trackDimLevel;
        private Label lblDimValue;
    }
}