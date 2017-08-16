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

namespace Utiles.Forms
{
    public partial class MainAppFormBase : Form
    {
        public static string ExecPath { get; set; }
        public static string AppName { get; set; }

        public static String UserName { get; set; }
        public static String MachineName { get; set; }

        public static IDatabase Database { get; set; }

        public static dynamic Config { get; set; }

        public MainAppFormBase()
        {
            InitializeComponent();
        }
    }
}
