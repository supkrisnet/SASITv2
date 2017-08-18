using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utiles.Database;

namespace AdminServerClient.Forms
{
    public partial class ServidorForm : Form
    {
        public string Oid { get; set;}
        public PgSQLDatabase Database { get; set; } 

        public ServidorForm()
        {
            InitializeComponent();
        }

        private void ServidorForm_Load(object sender, EventArgs e)
        {
            //this.Text = string.Format("{0}",Oid);

            PreInitializeData();


            if (string.IsNullOrEmpty(Oid))
            {

            }
            else
            {
                RunEditarServidor();
            }
        }

        private void PreInitializeData()
        {
            FillComboBox("estatus");
            FillComboBox("criticidad");
            FillComboBox("sitio");
            FillComboBox("hardware");
            FillComboBox("so");
        }

        private void FillComboBox(string tag)
        {
            string qs = string.Format("SELECT DISTINCT {0} FROM servers_catalog ORDER BY {0}",tag);

            DataTable dt = Database.QueryToDataTable(qs);

            if (dt.Rows.Count==0)
            {
                return;
            }

            ComboBox cbx = (ComboBox) Utiles.Forms.Helper.FindByTag(this,tag);

            foreach(DataRow d in dt.Rows)
            {
                //cbx.Items.Add(d[tag]);
            }
            
        }

        private void RunEditarServidor()
        {
            tbxServidor.ReadOnly = true;
            FillInfo();
        }

        private void FillInfo()
        {
            string qs = string.Format("SELECT id, servidor, estatus,descripcion,criticidad,sitio,hardware,so FROM servers_catalog WHERE id='{0}' LIMIT 1", Oid);

            DataTable dt = Database.QueryToDataTable(qs);

            if (dt.Rows.Count < 1 && dt.Rows.Count > 1)
            {
                MessageBox.Show("ERROR: MANY ROWS RETURNED");
                return;
            }

            tbxServidor.Text = string.Format("{0}",dt.Rows[0]["servidor"]);
            cbxEstatus.Text= string.Format("{0}", dt.Rows[0]["estatus"]);
            tbxDescripcion.Text = string.Format("{0}", dt.Rows[0]["descripcion"]);
            cbxCriticidad.Text = string.Format("{0}", dt.Rows[0]["criticidad"]);
            cbxSitio.Text = string.Format("{0}", dt.Rows[0]["sitio"]);
            cbxTipoHardware.Text = string.Format("{0}", dt.Rows[0]["hardware"]);
            //cbxsog.Text = string.Format("{0}", dt.Rows[0]["so"]);
            
            this.Text= string.Format("{0}|{1}", dt.Rows[0]["servidor"], dt.Rows[0]["id"]);
        }

        private void groupBox11_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox12_Enter(object sender, EventArgs e)
        {

        }
    }
}
