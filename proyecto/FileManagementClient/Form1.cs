using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileManagementClient.Services;

namespace FileManagementClient
{
    public partial class Form1 : Form
    {
        private readonly ApiClient _apiClient;
        private int? _currentParentId = null;
        private Stack<int?> _folderHistory = new Stack<int?>();
        public bool IsLogout { get; private set; } = false;

        public Form1(ApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
            dataGridViewFiles.CellDoubleClick += DataGridViewFiles_CellDoubleClick;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadFilesAsync();
        }

        private async Task LoadFilesAsync()
        {
            try
            {
                var files = await _apiClient.GetFilesAsync(_currentParentId);
                dataGridViewFiles.DataSource = files;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading files: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnUpload_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        await _apiClient.UploadFileAsync(openFileDialog.FileName, _currentParentId);
                        // MessageBox.Show("File uploaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        await LoadFilesAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error uploading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            if (dataGridViewFiles.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a file to download.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = dataGridViewFiles.SelectedRows[0];
            var fileId = (int)selectedRow.Cells["Id"].Value;
            var fileName = (string)selectedRow.Cells["FileName"].Value;
            var isFolder = (bool)selectedRow.Cells["IsFolder"].Value;

            if (isFolder)
            {
                using (var saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.FileName = fileName + ".zip";
                    saveFileDialog.Filter = "Zip Files (*.zip)|*.zip";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            var stream = await _apiClient.DownloadFolderAsZipAsync(fileId);
                            using (var fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                            {
                                await stream.CopyToAsync(fileStream);
                            }
                            // MessageBox.Show("Folder downloaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error downloading folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                return;
            }

            using (var saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.FileName = fileName;
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var stream = await _apiClient.DownloadFileAsync(fileId);
                        using (var fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                        {
                            await stream.CopyToAsync(fileStream);
                        }
                        // MessageBox.Show("File downloaded successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error downloading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadFilesAsync();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewFiles.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a file to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = dataGridViewFiles.SelectedRows[0];
            var fileId = (int)selectedRow.Cells["Id"].Value;
            var fileName = (string)selectedRow.Cells["FileName"].Value;

            var confirmResult = MessageBox.Show($"Are you sure you want to delete '{fileName}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    await _apiClient.DeleteFileAsync(fileId);
                    // MessageBox.Show("File deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    await LoadFilesAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnNewFolder_Click(object sender, EventArgs e)
        {
            string folderName = Microsoft.VisualBasic.Interaction.InputBox("Enter folder name:", "New Folder", "New Folder");
            if (!string.IsNullOrWhiteSpace(folderName))
            {
                try
                {
                    await _apiClient.CreateFolderAsync(folderName, _currentParentId);
                    await LoadFilesAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creating folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnUploadFolder_Click(object sender, EventArgs e)
        {
            using (var folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        await UploadFolderRecursive(folderBrowserDialog.SelectedPath, _currentParentId);
                        await LoadFilesAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error uploading folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async Task UploadFolderRecursive(string folderPath, int? parentId)
        {
            var folderName = new DirectoryInfo(folderPath).Name;
            var folderMetadata = await _apiClient.CreateFolderAsync(folderName, parentId);

            foreach (var filePath in Directory.GetFiles(folderPath))
            {
                await _apiClient.UploadFileAsync(filePath, folderMetadata.Id);
            }

            foreach (var subFolderPath in Directory.GetDirectories(folderPath))
            {
                await UploadFolderRecursive(subFolderPath, folderMetadata.Id);
            }
        }

        private async void btnUp_Click(object sender, EventArgs e)
        {
            if (_folderHistory.Count > 0)
            {
                _currentParentId = _folderHistory.Pop();
                await LoadFilesAsync();
            }
        }

        private async void DataGridViewFiles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dataGridViewFiles.Rows[e.RowIndex];
                var isFolder = (bool)row.Cells["IsFolder"].Value;
                if (isFolder)
                {
                    var folderId = (int)row.Cells["Id"].Value;
                    _folderHistory.Push(_currentParentId);
                    _currentParentId = folderId;
                    await LoadFilesAsync();
                }
            }
        }
        private void btnLogOff_Click(object sender, EventArgs e)
        {
            IsLogout = true;
            this.Close();
        }
    }
}
