using Npgsql;
using System;
using System.Data;
using System.Diagnostics;

namespace Utiles.Database
{
    public class PgSQLDatabase : IDatabase
    {
        public PgSQLDatabase()
        {
            ConnectionString = null;
        }

        public PgSQLDatabase(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public override bool Open()
        {
            Connection = new NpgsqlConnection(ConnectionString);

            try
            {
                Connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public override void Close()
        {
            if (Connection != null)
            {
                try
                {

                    Connection.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
        }

        /* -------------------------------------------------------- */
        public override int ExecuteNonQuery(IDbCommand command)
        {
            if (Connection == null)
            {
                throw new Exception(Messages.Exceptions._NO_CONNECTION_);
            }

            command.Connection = Connection;
            command.CommandTimeout = 0;

            return command.ExecuteNonQuery();
        }

        public override int ExecuteNonQuery(string queryString)
        {
            if (Connection == null)
            {
                throw new Exception(Messages.Exceptions._NO_CONNECTION_);
            }

            NpgsqlCommand _cmd = new NpgsqlCommand(queryString, (NpgsqlConnection)Connection);
            _cmd.CommandTimeout = 0;

            return _cmd.ExecuteNonQuery();
        }

        public override IDbDataAdapter Query(IDbCommand command)
        {
            if (Connection == null)
            {
                throw new Exception(Messages.Exceptions._NO_CONNECTION_);
            }

            command.Connection = Connection;
            command.CommandTimeout = 0;

            NpgsqlDataAdapter _sda = new NpgsqlDataAdapter((NpgsqlCommand)command);


            return _sda;
        }

        public override IDbDataAdapter Query(string queryString)
        {
            if (Connection == null)
            {
                throw new Exception(Messages.Exceptions._NO_CONNECTION_);
            }

            NpgsqlCommand _cmd = new NpgsqlCommand(queryString, (NpgsqlConnection)Connection);
            _cmd.CommandTimeout = 0;

            NpgsqlDataAdapter _sda = new NpgsqlDataAdapter(_cmd);

            return _sda;
        }

        public override DataTable QueryToDataTable(IDbCommand command)
        {
            if (Connection == null)
            {
                throw new Exception(Messages.Exceptions._NO_CONNECTION_);
            }

            DataTable _dt = new DataTable();

            command.Connection = Connection;
            command.CommandTimeout = 0;

            NpgsqlDataAdapter _sda = new NpgsqlDataAdapter((NpgsqlCommand)command);

            _sda.Fill(_dt);

            return _dt;
        }

        public override DataTable QueryToDataTable(string queryString)
        {
            if (Connection == null)
            {
                throw new Exception(Messages.Exceptions._NO_CONNECTION_);
            }

            DataTable _dt = new DataTable();

            NpgsqlCommand _cmd = new NpgsqlCommand(queryString, (NpgsqlConnection)Connection);
            _cmd.CommandTimeout = 0;

            NpgsqlDataAdapter _sda = new NpgsqlDataAdapter(_cmd);

            _sda.Fill(_dt);

            return _dt;
        }

        public override void ShowDatabaseInfo()
        {
            NpgsqlConnection conn = (NpgsqlConnection)Connection;

            Trace.WriteLine("====================================================");
            Trace.WriteLine(string.Format("Database Version:{0}",conn.PostgreSqlVersion));
            Trace.WriteLine(string.Format("Database State:{0}",conn.FullState));

            Trace.WriteLine("====================================================");
        }
    }
}
