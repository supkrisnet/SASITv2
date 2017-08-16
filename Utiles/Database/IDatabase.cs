using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utiles.Database
{
    public abstract class IDatabase
    {
        public IDbConnection Connection { get; set; }
        public string ConnectionString { get; set; }

        public abstract bool Open();
        public abstract void Close();
        public abstract DataTable QueryToDataTable(string qs);
        public abstract int ExecuteNonQuery(string qs);

        public abstract DataTable QueryToDataTable(IDbCommand cmd);
        public abstract IDbDataAdapter Query(string qs);
        public abstract IDbDataAdapter Query(IDbCommand cmd);
        public abstract int ExecuteNonQuery(IDbCommand cmd);

        public abstract void ShowDatabaseInfo();
    }
}
