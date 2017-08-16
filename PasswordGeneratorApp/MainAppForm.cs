using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasswordGeneratorApp
{
    public partial class MainAppForm : Form
    {
        static string ExecPath { get; set; }
        static string AppName { get; set; }
        

        static MainAppForm()
        {
            ExecPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            AppName = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location);

            Utiles.Logger.Tracer.Generate(ExecPath);
        }

        public MainAppForm()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbxCadena.Text))
            {
                MessageBox.Show(string.Format("DEBE INGRESAR UNA CADENA A <[{0}]>", tscbxOpciones.Text));
            }

            switch (tscbxOpciones.Text) {
                case "ENCRIPTAR":
                    tbxOutput.Text = Utiles.Crypt.Offusque.Encrypt(tbxCadena.Text);
                break;
                case "DESENCRIPTAR":
                    tbxOutput.Text = Utiles.Crypt.Offusque.Decrypt(tbxCadena.Text);
                break;
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tscbxOpciones.Text = "ENCRIPTAR";
            tssl01.Text = string.Empty;
        }
    }
}
