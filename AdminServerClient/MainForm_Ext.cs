using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using Utiles.Database;
using Utiles.Forms;

namespace AdminServerClient
{
    public partial class MainForm : MainAppFormBase
    {
        private void GenerateColumns(DataGridView dgv)
        {
            {
                DataGridViewColumn c = new DataGridViewTextBoxColumn();
                c.Name = "id";
                c.HeaderText = "ID";
                c.ReadOnly = true;
                c.Tag = "id";

                dgv.Columns.Add(c);
            }

            {
                DataGridViewColumn c = new DataGridViewTextBoxColumn();
                c.Name = "servidor";
                c.HeaderText = "SERVIDOR";
                c.ReadOnly = false;
                c.Tag = "servidor";

                dgv.Columns.Add(c);
            }

            {
                DataGridViewColumn c = new DataGridViewTextBoxColumn();
                c.Name = "dirip";
                c.HeaderText = "DIR. IP";
                c.ReadOnly = false;
                c.Tag = "dirip";

                dgv.Columns.Add(c);
            }

            {
                DataGridViewColumn c = new DataGridViewTextBoxColumn();
                c.Name = "descripcion";
                c.HeaderText = "DESCRIPCION";
                c.ReadOnly = false;
                c.Tag = "descripcion";

                dgv.Columns.Add(c);
            }

            {
                DataGridViewColumn c = new DataGridViewTextBoxColumn();
                c.Name = "ambiente";
                c.HeaderText = "AMBIENTE";
                c.ReadOnly = false;
                c.Tag = "ambiente";

                dgv.Columns.Add(c);
            }

            {
                DataGridViewColumn c = new DataGridViewTextBoxColumn();
                c.Name = "estatus";
                c.HeaderText = "ESTATUS";
                c.ReadOnly = false;
                c.Tag = "estatus";

                dgv.Columns.Add(c);
            }

            {
                DataGridViewColumn c = new DataGridViewTextBoxColumn();
                c.Name = "criticidad";
                c.HeaderText = "CRITICIDAD";
                c.ReadOnly = false;
                c.Tag = "criticidad";

                dgv.Columns.Add(c);
            }

            {
                DataGridViewColumn c = new DataGridViewTextBoxColumn();
                c.Name = "dominio";
                c.HeaderText = "DOMINIO";
                c.ReadOnly = false;
                c.Tag = "dominio";

                dgv.Columns.Add(c);
            }
            {
                DataGridViewColumn c = new DataGridViewTextBoxColumn();
                c.Name = "hardware";
                c.HeaderText = "TIPO";
                c.ReadOnly = false;
                c.Tag = "hardware";

                dgv.Columns.Add(c);
            }
            {
                DataGridViewColumn c = new DataGridViewTextBoxColumn();
                c.Name = "proveedor";
                c.HeaderText = "PROVEEDOR";
                c.ReadOnly = false;
                c.Tag = "hardware";

                dgv.Columns.Add(c);
            }
            //-----------------------------------------------------
            dgv.AutoGenerateColumns = false;
        }

        private static void BuildDatabaseConnection()
        {
            string cs = string.Format("Server={0}; Port={1}; User Id={2}; Password={3}; Database={4}", Config.Database.Host, Config.Database.Port, Config.Database.User, Config.Database.Password, Config.Database.Schema);

            Trace.WriteLine(cs);
            
            Database = new PgSQLDatabase(cs);
            Database.Open();
        }

        private static void CheckPasswords()
        {
            try
            {
                Config.Database.Password = Utiles.Crypt.Offusque.Decrypt(Config.Database.Password);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }

        private void FillData()
        {
            dgvServidores.ClearSelection();
            dgvServidores.Rows.Clear();

            String qs = string.Format(@"SELECT sc.id,sc.servidor,sc.descripcion,sc.estatus,sc.criticidad,sc.sitio,sc.hardware,sc.so, (SELECT string_agg(valor,'/') FROM Tags WHERE sid=sc.id AND etiqueta='DIR. IP') AS dirip, (SELECT string_agg(valor,'/') FROM Tags WHERE sid=sc.id AND etiqueta='AMBIENTE') AS ambiente, (SELECT string_agg(valor,'/') FROM Tags WHERE sid=sc.id AND etiqueta='DOMINIO') AS dominio
                                        FROM servers_catalog AS sc
                                        ORDER BY servidor ASC");

            DataTable dt = Database.QueryToDataTable(qs);
            
            foreach(DataRow dr in dt.Rows)
            {
                dgvServidores.Rows.Add(FillCells(dr));
            }

            dgvServidores.ClearSelection();
        }

        private object[] FillCells(DataRow r)
        {
            List<object> l = new List<object>();

            foreach(DataGridViewColumn c in dgvServidores.Columns)
            {
                string n = string.Format("{0}", c.Name);

                if (r.Table.Columns.Contains(n))
                {
                    l.Add(r[string.Format("{0}", c.Name)]);
                }
                else
                {
                    l.Add(string.Empty);
                }
            }

            return l.ToArray();
        }


    }
}
