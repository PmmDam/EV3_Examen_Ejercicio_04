using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLib.SQL
{
    public class SqlServer
    {

        private string _serverNameOrIp { get; set; }
        private string _login { get; set; }
        private string _password { get; set; }
        private string _dbName { get; set; }


        private string _connectionString { get; set; }
        public string ConnectionString
        {
            get
            {
                if (this._connectionString == null)
                {
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

                    builder.DataSource = this._serverNameOrIp;
                    builder.UserID = this._login;
                    builder.Password = this._password;
                    builder.InitialCatalog = this._dbName;
                    builder.TrustServerCertificate = true;

                    this._connectionString = builder.ToString();

                }

                return this._connectionString;
            }
        }


        public SqlServer(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public SqlServer(string serverName, string login, string password, string dbname)
        {
            this._serverNameOrIp = serverName;
            this._login = login;
            this._password = password;
            this._dbName = dbname;
        }



        // el problema de no pasarle un connectionString al método hace que no podamos hacer llamadas
        // a distintas bases de datos desde la misma instancia de SqlServer
        //Quizá lo suyo seria pasarle de manera independiente el connectionString para abri la conexión
        public async Task<int> ExecuteNonQueryAsync(string query, CommandType queryType, params SqlParameter[] parameters)
        {
            int affectedRows = -1;




            SqlConnection connection = new SqlConnection(this.ConnectionString);

            try
            {
                await connection.OpenAsync();
                SqlCommand cmd = connection.CreateCommand();
                try
                {
                    cmd.CommandText = query;
                    cmd.CommandType = queryType;
                    cmd.Parameters.AddRange(parameters);
                    affectedRows = await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                }
                finally
                {
                    if (cmd != null)
                    {
                        cmd.Parameters.Clear();
                        await cmd.DisposeAsync().ConfigureAwait(false);
                    }
                }

            }
            finally
            {
                if (connection != null)
                {
                    await connection.CloseAsync().ConfigureAwait(false);
                    await connection.DisposeAsync().ConfigureAwait(false);
                }

            }
            return affectedRows;
        }

        public async Task<object> ExecuteScalarAsync(string query, CommandType queryType, params SqlParameter[] parameters)
        {
            object objectResult = null;

            SqlConnection connection = new SqlConnection(this.ConnectionString);

            try
            {
                await connection.OpenAsync();
                SqlCommand cmd = connection.CreateCommand();
                try
                {
                    cmd.CommandText = query;
                    cmd.CommandType = queryType;
                    cmd.Parameters.AddRange(parameters);
                    objectResult = await cmd.ExecuteScalarAsync().ConfigureAwait(false);
                }
                finally
                {
                    if (cmd != null)
                    {
                        cmd.Parameters.Clear();
                        await cmd.DisposeAsync().ConfigureAwait(false);
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    await connection.CloseAsync().ConfigureAwait(false);
                    await connection.DisposeAsync().ConfigureAwait(false);
                }

            }
            return objectResult;
        }



        public async Task ExecuteReaderAsync(string query, CommandType queryType, Func<SqlDataReader, Task> dataHandlerAsync, params SqlParameter[] parameters)
        {


            SqlConnection connection = new SqlConnection(this.ConnectionString);

            try
            {
                await connection.OpenAsync();
                SqlCommand cmd = connection.CreateCommand();
                try
                {
                    cmd.CommandText = query;
                    cmd.CommandType = queryType;
                    cmd.Parameters.AddRange(parameters);
                    SqlDataReader reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
                    try
                    {
                        await dataHandlerAsync(reader).ConfigureAwait(false);
                    }

                    finally
                    {
                        if (reader != null)
                        {
                            await reader.DisposeAsync().ConfigureAwait(false);
                        }
                    }



                }
                finally
                {
                    if (cmd != null)
                    {
                        cmd.Parameters.Clear();
                        await cmd.DisposeAsync().ConfigureAwait(false);
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    await connection.CloseAsync().ConfigureAwait(false);
                    await connection.DisposeAsync().ConfigureAwait(false);
                }

            }


        }

        public bool CheckConnection()
        {
            bool result = false;

            SqlConnection conn = new SqlConnection(this.ConnectionString);

            try
            {
                conn.Open();
                result = true;
            }

            finally
            {
                if (conn != null)
                {
                    conn.Dispose();
                }
            }

            return result;
        }
    }
}
