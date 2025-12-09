namespace FileManagementClient
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridViewFiles = new DataGridView();
            listBoxMenu = new ListBox();
            btnStar = new Button();
            btnShare = new Button();
            btnUpload = new Button();
            btnDownload = new Button();
            btnRefresh = new Button();
            btnDelete = new Button();
            btnNewFolder = new Button();
            btnUp = new Button();
            btnUploadFolder = new Button();
            btnLogOff = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridViewFiles).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewFiles
            // 
            dataGridViewFiles.AllowUserToAddRows = false;
            dataGridViewFiles.AllowUserToDeleteRows = false;
            dataGridViewFiles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewFiles.Location = new Point(213, 20);
            dataGridViewFiles.Margin = new Padding(4, 5, 4, 5);
            dataGridViewFiles.Name = "dataGridViewFiles";
            dataGridViewFiles.ReadOnly = true;
            dataGridViewFiles.RowHeadersWidth = 62;
            dataGridViewFiles.RowTemplate.Height = 25;
            dataGridViewFiles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewFiles.Size = new Size(907, 583);
            dataGridViewFiles.TabIndex = 0;
            // 
            // listBoxMenu
            // 
            listBoxMenu.FormattingEnabled = true;
            listBoxMenu.Items.AddRange(new object[] { "Todos los archivos", "Archivos destacados", "Compartido conmigo" });
            listBoxMenu.Location = new Point(12, 20);
            listBoxMenu.Name = "listBoxMenu";
            listBoxMenu.Size = new Size(194, 579);
            listBoxMenu.TabIndex = 9;
            listBoxMenu.SelectedIndexChanged += listBoxMenu_SelectedIndexChanged;
            // 
            // btnStar
            // 
            btnStar.Location = new Point(186, 700);
            btnStar.Margin = new Padding(4, 5, 4, 5);
            btnStar.Name = "btnStar";
            btnStar.Size = new Size(143, 50);
            btnStar.TabIndex = 8;
            btnStar.Text = "Destacar";
            btnStar.UseVisualStyleBackColor = true;
            btnStar.Click += btnStar_Click;
            // 
            // btnShare
            // 
            btnShare.Location = new Point(357, 700);
            btnShare.Name = "btnShare";
            btnShare.Size = new Size(143, 50);
            btnShare.TabIndex = 10;
            btnShare.Text = "Compartir";
            btnShare.UseVisualStyleBackColor = true;
            btnShare.Click += btnShare_Click;
            // 
            // btnUpload
            // 
            btnUpload.Location = new Point(17, 633);
            btnUpload.Margin = new Padding(4, 5, 4, 5);
            btnUpload.Name = "btnUpload";
            btnUpload.Size = new Size(143, 50);
            btnUpload.TabIndex = 1;
            btnUpload.Text = "Subir Archivo";
            btnUpload.UseVisualStyleBackColor = true;
            btnUpload.Click += btnUpload_Click;
            // 
            // btnDownload
            // 
            btnDownload.Location = new Point(186, 633);
            btnDownload.Margin = new Padding(4, 5, 4, 5);
            btnDownload.Name = "btnDownload";
            btnDownload.Size = new Size(143, 50);
            btnDownload.TabIndex = 2;
            btnDownload.Text = "Descargar";
            btnDownload.UseVisualStyleBackColor = true;
            btnDownload.Click += btnDownload_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(357, 633);
            btnRefresh.Margin = new Padding(4, 5, 4, 5);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(143, 50);
            btnRefresh.TabIndex = 3;
            btnRefresh.Text = "Actualizar";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(529, 633);
            btnDelete.Margin = new Padding(4, 5, 4, 5);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(143, 50);
            btnDelete.TabIndex = 4;
            btnDelete.Text = "Borrar";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnNewFolder
            // 
            btnNewFolder.Location = new Point(700, 633);
            btnNewFolder.Margin = new Padding(4, 5, 4, 5);
            btnNewFolder.Name = "btnNewFolder";
            btnNewFolder.Size = new Size(143, 50);
            btnNewFolder.TabIndex = 5;
            btnNewFolder.Text = "Crear Carpeta";
            btnNewFolder.UseVisualStyleBackColor = true;
            btnNewFolder.Click += btnNewFolder_Click;
            // 
            // btnUp
            // 
            btnUp.Location = new Point(871, 633);
            btnUp.Margin = new Padding(4, 5, 4, 5);
            btnUp.Name = "btnUp";
            btnUp.Size = new Size(143, 50);
            btnUp.TabIndex = 6;
            btnUp.Text = "Atrás";
            btnUp.UseVisualStyleBackColor = true;
            btnUp.Click += btnUp_Click;
            // 
            // btnUploadFolder
            // 
            btnUploadFolder.Location = new Point(17, 700);
            btnUploadFolder.Margin = new Padding(4, 5, 4, 5);
            btnUploadFolder.Name = "btnUploadFolder";
            btnUploadFolder.Size = new Size(143, 50);
            btnUploadFolder.TabIndex = 7;
            btnUploadFolder.Text = "Subir Carpeta";
            btnUploadFolder.UseVisualStyleBackColor = true;
            btnUploadFolder.Click += btnUploadFolder_Click;
            // 
            // btnLogOff
            // 
            btnLogOff.Location = new Point(871, 707);
            btnLogOff.Margin = new Padding(4, 5, 4, 5);
            btnLogOff.Name = "btnLogOff";
            btnLogOff.Size = new Size(143, 50);
            btnLogOff.TabIndex = 6;
            btnLogOff.Text = "Log Off";
            btnLogOff.UseVisualStyleBackColor = true;
            btnLogOff.Click += btnLogOff_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1142, 771);
            Controls.Add(listBoxMenu);
            Controls.Add(btnStar);
            Controls.Add(btnShare);
            Controls.Add(btnLogOff);
            Controls.Add(btnUp);
            Controls.Add(btnUploadFolder);
            Controls.Add(btnNewFolder);
            Controls.Add(btnDelete);
            Controls.Add(btnRefresh);
            Controls.Add(btnDownload);
            Controls.Add(btnUpload);
            Controls.Add(dataGridViewFiles);
            Margin = new Padding(4, 5, 4, 5);
            Name = "Form1";
            Text = "Gestor de Archivos";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridViewFiles).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewFiles;
        private System.Windows.Forms.Button btnStar;
        private System.Windows.Forms.Button btnShare;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnNewFolder;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnUploadFolder;
        private System.Windows.Forms.Button btnLogOff;
        private System.Windows.Forms.ListBox listBoxMenu;
    }
}
