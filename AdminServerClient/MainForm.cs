using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utiles.Forms;

namespace AdminServerClient
{
    public partial class MainForm : MainAppFormBase
    {
        static MainForm()
        {
            ExecPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            AppName = Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location);

            Utiles.Logger.Tracer.Generate(ExecPath);

            UserName = String.Format(@"{0}\{1}", Environment.UserDomainName, Environment.UserName).ToUpper();
            MachineName = Environment.MachineName;


            string _conf = string.Format(@"{0}\Config", ExecPath);

            if (!Directory.Exists(_conf))
            {
                Directory.CreateDirectory(_conf);
            }

            try
            {
                Config = (ExpandoObject)Utiles.Config.Json.ConvertoToDynamic(string.Format(@"{0}\{1}.json", _conf, AppName));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (Config == null)
                {
                    throw new Exception(string.Format(Utiles.Messages.Exceptions._OBJECT_IS_NULL_, "Config"));
                }
            }

            CheckPasswords();
            BuildDatabaseConnection();

        }
        
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            tssl01.Text = UserName;
            tssl02.Text = string.Empty;
            tssl03.Text = string.Empty;
            tssl04.Text = string.Format("{0}",Database.Connection.State);

            Database.ShowDatabaseInfo();
            GenerateColumns(dgvServidores);

            FillData();
            
        }

        private void dgvServidores_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            tssl02.Text = string.Format("Total: {0}",dgvServidores.Rows.Count);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Database.Connection != null)
            {
                Trace.WriteLine("Cerrando Base de datos");
                Database.Close();
                Trace.WriteLine("\tOK");

                Utiles.Logger.Tracer.PrintDateTime();
            }
        }
    }
}
