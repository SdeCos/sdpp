using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using FileManagementClient.Services;

namespace FileManagementClient
{

    /// Formulario principla de la aplicación.
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
            listBoxMenu.SelectedIndex = 0;
        }

        private async void listBoxMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            _currentParentId = null; // Reset navigation when switching tabs
            _folderHistory.Clear();
            await LoadFilesAsync();
        }

        /// Carga y muestra la lista de archivos según el filtro seleccionado (Todos, Destacados, Compartidos).
        private async Task LoadFilesAsync()
        {
            try
            {
                int filterIndex = listBoxMenu.SelectedIndex;
                if (filterIndex == 2)  // Compartido conmigo
                {
                    if (_currentParentId == null)
                    {
                         var files = await _apiClient.GetSharedFilesAsync();
                         dataGridViewFiles.DataSource = files;
                    }
                    else
                    {
                         // Navigate into shared folder
                         var files = await _apiClient.GetFilesAsync(_currentParentId, false);
                         dataGridViewFiles.DataSource = files;
                    }
                }
                else
                {
                    bool starredOnly = filterIndex == 1;
                    int? parentId = starredOnly ? null : _currentParentId;
                    var files = await _apiClient.GetFilesAsync(parentId, starredOnly);
                    dataGridViewFiles.DataSource = files;
                }
                UpdateRowColors();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando archivos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnExpected_Placeholder() { } // Dummy to help anchor if needed? No, just appending logic.

        /// Sube un archivo a la carpeta actual.
        private async void btnUpload_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        await _apiClient.UploadFileAsync(openFileDialog.FileName, _currentParentId);
                        await LoadFilesAsync();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error subiendo archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// Comparte el archivo seleccionado con otro usuario.
        private async void btnShare_Click(object sender, EventArgs e)
        {
            if (dataGridViewFiles.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un archivo para compartir.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = dataGridViewFiles.SelectedRows[0];
            var fileId = (int)selectedRow.Cells["Id"].Value;

            string targetUser = Microsoft.VisualBasic.Interaction.InputBox("Introduce el nombre de usuario con quien compartir:", "Compartir Archivo", "");
            if (!string.IsNullOrWhiteSpace(targetUser))
            {
                try
                {
                    await _apiClient.ShareFileAsync(fileId, targetUser);
                    MessageBox.Show("Archivo compartido exitosamente.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error compartiendo archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// Descarga el archivo o carpeta seleccionada.
        private async void btnDownload_Click(object sender, EventArgs e)
        {
            if (dataGridViewFiles.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un archivo para descargar.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error downloading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// Refresca la lista de archivos actual.
        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await LoadFilesAsync();
        }

        /// Elimina el archivo o carpeta seleccionada tras confirmación.
        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewFiles.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un archivo para borrar.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = dataGridViewFiles.SelectedRows[0];
            var fileId = (int)selectedRow.Cells["Id"].Value;
            var fileName = (string)selectedRow.Cells["FileName"].Value;

            var confirmResult = MessageBox.Show($"Estás seguro que deseas eliminar '{fileName}'?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                try
                {
                    await _apiClient.DeleteFileAsync(fileId);
                    await LoadFilesAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error borrando el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// Crea una nueva carpeta vacía en la ubicación actual.
        private async void btnNewFolder_Click(object sender, EventArgs e)
        {
            string folderName = Microsoft.VisualBasic.Interaction.InputBox("Introduce el nombre de la carpeta:", "New Folder", "New Folder");
            if (!string.IsNullOrWhiteSpace(folderName))
            {
                try
                {
                    await _apiClient.CreateFolderAsync(folderName, _currentParentId);
                    await LoadFilesAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error creando la carpeta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// Carga una carpeta recursivamente desde el sistema local.
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
                        MessageBox.Show($"Error subiendo la carpeta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        /// Navega un nivel hacia arriba en la jerarquía de carpetas.
        private async void btnUp_Click(object sender, EventArgs e)
        {
            if (_folderHistory.Count > 0)
            {
                _currentParentId = _folderHistory.Pop();
                await LoadFilesAsync();
            }
        }

        /// Maneja el doble clic en un archivo o carpeta.
        /// Si es carpeta, entra en ella.
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

        /// Marca o desmarca un archivo como destacado (favorito).
        private async void btnStar_Click(object sender, EventArgs e)
        {
            if (dataGridViewFiles.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona un archivo para destacar/quitar destacado.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = dataGridViewFiles.SelectedRows[0];
            var fileId = (int)selectedRow.Cells["Id"].Value;

            try
            {
                await _apiClient.ToggleFileStarAsync(fileId);
                await LoadFilesAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error destacando archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// Actualiza los colores de las filas segun estado (ej. destacados en oro).
        private void UpdateRowColors()
        {
            foreach (DataGridViewRow row in dataGridViewFiles.Rows)
            {
                var isStarred = (bool)row.Cells["IsStarred"].Value;
                if (isStarred)
                {
                    row.DefaultCellStyle.BackColor = Color.Gold;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        /// Cierra la sesión y regresa a la pantalla de login.
        private void btnLogOff_Click(object sender, EventArgs e)
        {
            IsLogout = true;
            this.Close();
        }
    }
}
