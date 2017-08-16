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

namespace CargarInfoServidores
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "csv files (*.csv)|*.csv|txt files (*.txt)|*.txt";

            if (d.ShowDialog()== DialogResult.OK)
            {
                tbxInputFile.Text = d.FileName;
            }
            else
            {
                tbxInputFile.Text = string.Empty;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog d = new SaveFileDialog();

            if (DialogResult.OK == d.ShowDialog())
            {
                tbxOutputFile.Text = d.FileName;
            }
            else
            {

                tbxOutputFile.Text = string.Empty;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbxInputFile.Text))
            {
                MessageBox.Show("DEBE SELECCIONAR UNA ARCHIVO DE ENTRADA");
                return;
            }

            if (string.IsNullOrEmpty(tbxOutputFile.Text))
            {
                tbxOutputFile.Text= string.Format(@"{0}\{1}_{2:yyyyMMddHHmmss}.sql", Path.GetDirectoryName(tbxInputFile.Text), Path.GetFileNameWithoutExtension(tbxInputFile.Text), DateTime.Now);
            }

            var ll = File.ReadLines(tbxInputFile.Text);
            foreach (string l in ll)
            {
                if (l.StartsWith("#"))
                {
                    continue;
                }

                string[] d = l.Split(new char[] {'|'});


                using (StreamWriter w = new StreamWriter(tbxOutputFile.Text,true))
                {
                    w.WriteLine("-- ###########################################################");
                    w.WriteLine("BEGIN;");

                    string q = string.Format("INSERT INTO servers_catalog(id,servidor,descripcion,estatus,criticidad,sitio,hardware,so) VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}');",
                        Guid.NewGuid().ToString().ToUpper() //id
                        , NormalizeText(d[0])               //servidor
                        , NormalizeText(d[5])               //descripcion
                        //, NormalizeText(d[2])               //ambiente
                        , NormalizeText(d[3])               //estatus
                        , NormalizeText(d[4])               //criticidad
                        //, NormalizeText(d[1])               //dirip
                        , string.Empty                      //sitio
                        , NormalizeText(d[6])               //hardware
                        , NormalizeText(d[7])               //so
                        //, NormalizeText(d[8])               //dominio
                        );

                    w.WriteLine(q);

                    //AMBIENTE
                    {
                        string[] li = ((string)NormalizeText(d[2])).Split(new char[] { '/' });

                        foreach (string s in li)
                        {
                            q = string.Format("SELECT sp_AddTag('{0}','{1}','{2}');", NormalizeText(d[0]), "AMBIENTE", s.Trim());
                            w.WriteLine(q);
                        }

                        
                    }
                    //DIRECION IP
                    {
                        string[] li = ((string)NormalizeText(d[1])).Split(new char[] { '/' });

                        foreach (string s in li)
                        {
                            q = string.Format("SELECT sp_AddTag('{0}','{1}','{2}');", NormalizeText(d[0]), "DIR. IP", s.Trim());
                            w.WriteLine(q);
                        }
                    }
                    //DOMINIO
                    {
                        string[] li = ((string)NormalizeText(d[8])).Split(new char[] { '/' });

                        foreach (string s in li)
                        {
                            q = string.Format("SELECT sp_AddTag('{0}','{1}','{2}');", NormalizeText(d[0]), "DOMINIO", s.Trim());
                            w.WriteLine(q);
                        }
                    }

                    w.WriteLine("COMMIT;");
                    w.WriteLine("END;");
                    w.WriteLine("-- ###########################################################");

                }
            }

            MessageBox.Show("TERMINADO");
        }

        private object NormalizeText(string v)
        {
            return v.ToUpper().Trim().Replace("\"",string.Empty).Replace("\\","\\\\");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
