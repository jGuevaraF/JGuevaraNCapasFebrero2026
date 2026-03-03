using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Usuario
    {
        //public static bool Add(ML.Usuario usuario)
        //{
        //    using (SqlConnection context = new SqlConnection(DL.Connection.GetConnection()))
        //    {
        //        SqlCommand cmd = new SqlCommand();
        //        cmd.Connection = context;
        //        cmd.CommandText = "UsuarioAdd"; //INSERT, UPDATE, DELETE, SELECT
        //        cmd.CommandType = System.Data.CommandType.StoredProcedure;

        //        cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
        //        cmd.Parameters.AddWithValue("@ApellidoPaterno", usuario.ApellidoPaterno);
        //        cmd.Parameters.AddWithValue("@ApellidoMaterno", usuario.ApellidoMaterno);
        //        cmd.Parameters.AddWithValue("@IdRol", usuario.IdRol);


        //    }
        //}

        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (SqlConnection context = new SqlConnection(DL.Connection.GetConnection()))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = context;
                    cmd.CommandText = "UsuarioGetAll";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();

                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        //obtener la informacion

                        result.Objects = new List<object>();

                        foreach (DataRow row in dataTable.Rows)
                        {
                            ML.Usuario usuario = new ML.Usuario();
                            usuario.IdUsuario = Convert.ToInt32(row[0]);
                            usuario.Nombre = row[1].ToString();
                            usuario.ApellidoPaterno = row[2].ToString();
                            usuario.ApellidoMaterno = row[3].ToString();



                            usuario.Rol = new ML.Rol();

                            if (row[4] == DBNull.Value)
                            {
                                usuario.Rol.IdRol = 0;
                            }
                            else
                            {

                                usuario.Rol.IdRol = Convert.ToInt32(row[4]);
                            }

                            usuario.Rol.Nombre = row[5].ToString();

                            result.Objects.Add(usuario);

                        }

                        result.Correct = true;

                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No hay registros";
                    }
                }

            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;
        }
    }
}
