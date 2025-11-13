namespace DiskMaintenance
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox comboDrives;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private Button btnTemporary;
        private Button btnClean;
        private Button btnOptimize;

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            comboDrives = new ComboBox();
            btnTemporary = new Button();
            btnClean = new Button();
            btnOptimize = new Button();
            btnTemporary.Click += BtnTemporary_Click;
            btnClean.Click += BtnClean_Click;
            btnOptimize.Click += BtnOptimize_Click;

            SuspendLayout();

            // --- Form ---
            BackColor = Color.FromArgb(20, 20, 20);
            ForeColor = Color.White;
            Font = new Font("Segoe UI", 12.5F, FontStyle.Regular, GraphicsUnit.Point);
            ClientSize = new Size(550, 150);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Disk Maintenance";

            // --- ComboBox ---
            comboDrives.DropDownStyle = ComboBoxStyle.DropDownList;
            comboDrives.Size = new Size(500, 50);
            comboDrives.Top = 25;
            comboDrives.Left = (this.ClientSize.Width - comboDrives.Width) / 2;
            comboDrives.BackColor = Color.FromArgb(45, 45, 45);
            comboDrives.ForeColor = Color.White;
            comboDrives.FlatStyle = FlatStyle.Flat;
            Controls.Add(comboDrives);

            // --- Buttons ---
            int buttonWidth = 150;
            int buttonHeight = 50;
            int spacing = 25;
            int topPosition = comboDrives.Top + 50;
            int leftStart = (this.ClientSize.Width - (buttonWidth * 3 + spacing * 2)) / 2;

            btnTemporary.Text = "Temporary";
            btnTemporary.Size = new Size(buttonWidth, buttonHeight);
            btnTemporary.Top = topPosition;
            btnTemporary.Left = leftStart;
            btnTemporary.BackColor = Color.FromArgb(45, 45, 45);
            btnTemporary.ForeColor = Color.White;
            btnTemporary.FlatStyle = FlatStyle.Flat;

            btnClean.Text = "Clean";
            btnClean.Size = new Size(buttonWidth, buttonHeight);
            btnClean.Top = topPosition;
            btnClean.Left = leftStart + buttonWidth + spacing;
            btnClean.BackColor = Color.FromArgb(45, 45, 45);
            btnClean.ForeColor = Color.White;
            btnClean.FlatStyle = FlatStyle.Flat;

            btnOptimize.Text = "Optimize";
            btnOptimize.Size = new Size(buttonWidth, buttonHeight);
            btnOptimize.Top = topPosition;
            btnOptimize.Left = leftStart + (buttonWidth + spacing) * 2;
            btnOptimize.BackColor = Color.FromArgb(45, 45, 45);
            btnOptimize.ForeColor = Color.White;
            btnOptimize.FlatStyle = FlatStyle.Flat;

            Controls.Add(btnTemporary);
            Controls.Add(btnClean);
            Controls.Add(btnOptimize);
            Load += (s, e) => updateDriveList();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}