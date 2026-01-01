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
            SuspendLayout();
            // 
            // comboMonitors
            // 
            comboMonitors.FormattingEnabled = true;
            comboMonitors.Location = new Point(249, 84);
            comboMonitors.Name = "comboMonitors";
            comboMonitors.Size = new Size(302, 49);
            comboMonitors.TabIndex = 0;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(306, 174);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(188, 58);
            btnSave.TabIndex = 1;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnTest
            // 
            btnTest.Location = new Point(306, 276);
            btnTest.Name = "btnTest";
            btnTest.Size = new Size(188, 58);
            btnTest.TabIndex = 2;
            btnTest.Text = "Test";
            btnTest.UseVisualStyleBackColor = true;
            btnTest.Click += btnTest_Click;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new SizeF(17F, 41F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnTest);
            Controls.Add(btnSave);
            Controls.Add(comboMonitors);
            Name = "SettingsForm";
            Text = "SettingsForm";
            ResumeLayout(false);
        }

        #endregion

        private ComboBox comboMonitors;
        private Button btnSave;
        private Button btnTest;
    }
}