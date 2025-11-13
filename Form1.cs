using System.Diagnostics;

namespace DiskMaintenance
{
    public partial class Form1 : Form
    {
        private const int WM_DEVICECHANGE = 0x0219;
        private const int DBT_DEVICEARRIVAL = 0x8000;
        private const int DBT_DEVICEREMOVECOMPLETE = 0x8004;

        public Form1() => InitializeComponent();

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_DEVICECHANGE)
            {
                int wParam = (int)m.WParam;
                if (wParam == DBT_DEVICEARRIVAL || wParam == DBT_DEVICEREMOVECOMPLETE)
                    BeginInvoke((Action)(() => updateDriveList()));
            }
        }

        private void updateDriveList()
        {
            var drives = DriveInfo.GetDrives().Where(d => d.IsReady).OrderBy(d => d.Name).ToList();
            comboDrives.Items.Clear();

            foreach (var d in drives)
            {
                string line = $"{d.Name.TrimEnd('\\')} - {d.DriveFormat} - {formatBytes(d.TotalSize)} - {formatBytes(d.TotalFreeSpace)} free";
                comboDrives.Items.Add(line);
            }

            if (comboDrives.Items.Count > 0)
                comboDrives.SelectedIndex = 0;
        }

        private string formatBytes(long bytes) => $"{bytes / (1024.0 * 1024.0 * 1024.0):N2} GB";

        private static void cleanTempFiles()
        {
            string windowsTemp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Temp");
            string userTemp = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp");

            int deletedFiles = 0;
            int skippedFiles = 0;

            deletedFiles += deleteFilesInDirectory(userTemp, ref skippedFiles);
            deletedFiles += deleteFilesInDirectory(windowsTemp, ref skippedFiles);

            MessageBox.Show($"Temporary files cleaned!\nFiles deleted: {deletedFiles}\nFiles skipped (in use or locked): {skippedFiles}",
                            "Cleanup Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private static int deleteFilesInDirectory(string path, ref int skippedFiles)
        {
            int deletedCount = 0;

            if (!Directory.Exists(path)) return 0;

            DirectoryInfo dirInfo = new DirectoryInfo(path);

            foreach (var file in dirInfo.GetFiles())
            {
                try
                {
                    file.Attributes = FileAttributes.Normal;
                    file.Delete();
                    deletedCount++;
                }
                catch
                {
                    skippedFiles++;
                }
            }

            foreach (var dir in dirInfo.GetDirectories())
            {
                try
                {
                    deletedCount += deleteFilesInDirectory(dir.FullName, ref skippedFiles);
                    dir.Delete();
                    deletedCount++;
                }
                catch
                {
                    skippedFiles++;
                }
            }

            return deletedCount;
        }

        private void BtnTemporary_Click(object sender, EventArgs e)
        {
            cleanTempFiles();
        }

        private void BtnClean_Click(object sender, EventArgs e)
        {
            if (comboDrives.SelectedItem == null)
            {
                MessageBox.Show("Please select a drive first.", "No Drive Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string driveLetter = comboDrives.SelectedItem.ToString().Split(' ')[0];

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cleanmgr.exe",
                    Arguments = $"/d {driveLetter}",
                    Verb = "runas",
                    UseShellExecute = true
                };

                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to launch Disk Cleanup: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnOptimize_Click(object sender, EventArgs e)
        {
            string selected = comboDrives.SelectedItem as string;
            if (string.IsNullOrEmpty(selected))
            {
                MessageBox.Show("Please select a drive first.", "No Drive Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string driveLetter = selected.Split(' ')[0];

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "defrag.exe",
                    Arguments = $"{driveLetter} /O",
                    Verb = "runas",
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Normal
                };

                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to optimize the drive: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}