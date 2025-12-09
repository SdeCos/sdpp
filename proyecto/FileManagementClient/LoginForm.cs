using System;
using System.Windows.Forms;
using FileManagementClient.Services;

namespace FileManagementClient
{
    /// Formulario de inicio de sesión y registro de usuarios.
    public partial class LoginForm : Form
    {
        private readonly ApiClient _apiClient;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnLogin;
        private Button btnRegister;
        private Label lblUsername;
        private Label lblPassword;

        public LoginForm()
        {
            InitializeComponent();
            _apiClient = new ApiClient();
        }

        private void InitializeComponent()
        {
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            btnLogin = new Button();
            btnRegister = new Button();
            lblUsername = new Label();
            lblPassword = new Label();
            SuspendLayout();
            // 
            // txtUsername
            // 
            txtUsername.Location = new Point(143, 50);
            txtUsername.Margin = new Padding(4, 5, 4, 5);
            txtUsername.Name = "txtUsername";
            txtUsername.Size = new Size(284, 31);
            txtUsername.TabIndex = 0;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(143, 117);
            txtPassword.Margin = new Padding(4, 5, 4, 5);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';
            txtPassword.Size = new Size(284, 31);
            txtPassword.TabIndex = 1;
            // 
            // btnLogin
            // 
            btnLogin.Location = new Point(143, 183);
            btnLogin.Margin = new Padding(4, 5, 4, 5);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(129, 50);
            btnLogin.TabIndex = 2;
            btnLogin.Text = "Iniciar Sesión";
            btnLogin.UseVisualStyleBackColor = true;
            btnLogin.Click += btnLogin_Click;
            // 
            // btnRegister
            // 
            btnRegister.Location = new Point(300, 183);
            btnRegister.Margin = new Padding(4, 5, 4, 5);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(129, 50);
            btnRegister.TabIndex = 3;
            btnRegister.Text = "Registrar";
            btnRegister.UseVisualStyleBackColor = true;
            btnRegister.Click += btnRegister_Click;
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.Location = new Point(29, 55);
            lblUsername.Margin = new Padding(4, 0, 4, 0);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(76, 25);
            lblUsername.TabIndex = 4;
            lblUsername.Text = "Usuario:";
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(29, 122);
            lblPassword.Margin = new Padding(4, 0, 4, 0);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(105, 25);
            lblPassword.TabIndex = 5;
            lblPassword.Text = "Contraseña:";
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(500, 300);
            Controls.Add(lblPassword);
            Controls.Add(lblUsername);
            Controls.Add(btnRegister);
            Controls.Add(btnLogin);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 5, 4, 5);
            MaximizeBox = false;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Inicio de sesión";
            ResumeLayout(false);
            PerformLayout();
        }

        /// Intenta autenticar al usuario con las credenciales proporcionadas.
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text;
            var password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter username and password.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnLogin.Enabled = false;
                var success = await _apiClient.LoginAsync(username, password);
                if (success)
                {
                    this.Hide();
                    var mainForm = new Form1(_apiClient);
                    mainForm.FormClosed += (s, args) => 
                    {
                        if (mainForm.IsLogout)
                        {
                            this.Show();
                            txtPassword.Clear();
                        }
                        else
                        {
                            this.Close();
                        }
                    };
                    mainForm.Show();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Login error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLogin.Enabled = true;
            }
        }

        /// Crea un nuevo usuario en el sistema.
        private async void btnRegister_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text;
            var password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter username and password.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                btnRegister.Enabled = false;
                var success = await _apiClient.RegisterAsync(username, password);
                if (success)
                {
                    MessageBox.Show("Registration successful! You can now login.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Registration failed. Username might be taken.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Registration error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnRegister.Enabled = true;
            }
        }
    }
}
