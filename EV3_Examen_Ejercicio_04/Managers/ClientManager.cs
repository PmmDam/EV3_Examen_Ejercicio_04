using Microsoft.Data.SqlClient;
using MyLib.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using EV3_Examen_Ejercicio_04.Models;

namespace EV3_Examen_Ejercicio_04.Managers
{
    public class ClientManager
    {
        private SqlServer _myDb { get; set; }
        public ClientManager(SqlServer db)
        {
            this._myDb = db;
        }



        // Esto es una chapuza pero es para que el ExecuteReaderAsync no me tenga que devolver un tipo
        public UserModel currentUser { get; set; }


        // El procedimiento almacenado "GetUserhash" realiza la consulta:
        // SELECT * FROM Clients WHERE Id_Client = @Id;

        public async Task GetUserHashFromDbAsync(int idUser)
        {

            SqlParameter idParameter = GetSqlParameter(idUser);
            await this._myDb.ExecuteReaderAsync("GetUserhash", CommandType.StoredProcedure, ReadUser, idParameter);

        }

        private async Task ReadUser(SqlDataReader reader)
        {

           
            if (reader.HasRows)
            {

                while (await reader.ReadAsync())
                {

                    UserModel user = new UserModel();

                    if (!reader.IsDBNull(0))
                    {
                        user.Id = reader.GetInt32(0);
                    }
                    if (!reader.IsDBNull(1))
                    {
                        user.Name = reader.GetString(1);
                    }

                    if (!reader.IsDBNull(2))
                    {
                        user.PasswordHash= reader.GetString(2);
                    }

                    this.currentUser = user;
                }


            }

        }


        private SqlParameter GetSqlParameter(int idUser)
        {

            SqlParameter idParameter = new SqlParameter();
            idParameter.ParameterName = "@Id";
            idParameter.Value = idUser;
            idParameter.DbType = DbType.Int32;
            

            return idParameter;

        }

        
    }
}
