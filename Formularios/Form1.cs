namespace Formularios
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Creando un nuevo formulario...", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Form nuevoForm = new Form();
            nuevoForm.Text = "Nuevo Formulario";
            nuevoForm.Width = 800;
            nuevoForm.Show();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {


        }

        private void btDividir_Click(object sender, EventArgs e)
        {
            try
            {
                decimal resultado = this.numOperando1.Value / this.numOperando2.Value;
                this.txResultado.Text = resultado.ToString();
            }
            catch (DivideByZeroException ex)
            {
                MessageBox.Show("No se puede dividir por 0: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txResultado_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageBox.Show("Se esta cerrando el formulario", "Aviso!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}