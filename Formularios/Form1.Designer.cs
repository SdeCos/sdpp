namespace Formularios
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
            btCrearFormulario = new Button();
            numOperando2 = new NumericUpDown();
            numOperando1 = new NumericUpDown();
            txResultado = new TextBox();
            btDividir = new Button();
            ((System.ComponentModel.ISupportInitialize)numOperando2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numOperando1).BeginInit();
            SuspendLayout();
            // 
            // btCrearFormulario
            // 
            btCrearFormulario.Location = new Point(531, 355);
            btCrearFormulario.Name = "btCrearFormulario";
            btCrearFormulario.Size = new Size(198, 31);
            btCrearFormulario.TabIndex = 0;
            btCrearFormulario.Text = "Crear Otro Formulario";
            btCrearFormulario.UseVisualStyleBackColor = true;
            btCrearFormulario.Click += button1_Click;
            // 
            // numOperando2
            // 
            numOperando2.Location = new Point(138, 12);
            numOperando2.Name = "numOperando2";
            numOperando2.Size = new Size(120, 23);
            numOperando2.TabIndex = 2;
            numOperando2.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // numOperando1
            // 
            numOperando1.Location = new Point(12, 12);
            numOperando1.Name = "numOperando1";
            numOperando1.Size = new Size(120, 23);
            numOperando1.TabIndex = 3;
            numOperando1.Value = new decimal(new int[] { 18, 0, 0, 0 });
            // 
            // txResultado
            // 
            txResultado.Location = new Point(309, 12);
            txResultado.Name = "txResultado";
            txResultado.ReadOnly = true;
            txResultado.Size = new Size(100, 23);
            txResultado.TabIndex = 4;
            txResultado.TextChanged += txResultado_TextChanged;
            // 
            // btDividir
            // 
            btDividir.Location = new Point(104, 65);
            btDividir.Name = "btDividir";
            btDividir.Size = new Size(75, 23);
            btDividir.TabIndex = 5;
            btDividir.Text = "Dividir";
            btDividir.UseVisualStyleBackColor = true;
            btDividir.Click += btDividir_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btDividir);
            Controls.Add(txResultado);
            Controls.Add(numOperando1);
            Controls.Add(numOperando2);
            Controls.Add(btCrearFormulario);
            Name = "Form1";
            Text = "Form1";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)numOperando2).EndInit();
            ((System.ComponentModel.ISupportInitialize)numOperando1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btCrearFormulario;
        private NumericUpDown numOperando2;
        private NumericUpDown numOperando1;
        private TextBox txResultado;
        private Button btDividir;
    }
}