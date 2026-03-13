using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Rol
    {
        public static ML.Result Add(string nombre)
        {

            ML.Result result = new ML.Result();

            try
            {
                using (SqlConnection context = new SqlConnection(DL.Connection.GetConnection()))
                {

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = context;
                    cmd.CommandText = "INSERT INTO Rol VALUES (@kdjbfksdf)";

                    cmd.Parameters.AddWithValue("@Imagen", null);

                    context.Open();

                    int filasAfectadas = cmd.ExecuteNonQuery();

                    if(filasAfectadas > 0)
                    {
                        result.Correct = true;
                    } else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se inserto";
                    }



                } //context.Close();

            } catch(Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }

            return result;

            



        }

        public static List<object> GetAllSqlDataAdapter()
        {
            SqlConnection context = new SqlConnection(DL.Connection.GetConnection());

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = context;
            cmd.CommandText = "SELECT IdRol, Nombre FROM Rol";

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);

            List<object> roles = new List<object>();

            if (dataTable.Rows.Count > 0)
            {
                // Estructuras de datos
                //1 arreglos
                //2 pilas
                //3 colas
                //4 LISTAS


                //for es el peor ciclo 
                foreach (DataRow row in dataTable.Rows)
                {
                    ML.Rol rolEncontrado = new ML.Rol();
                    rolEncontrado.IdRol = Convert.ToInt32(row[0]);
                    rolEncontrado.Nombre = row[1].ToString();

                    roles.Add(rolEncontrado);
                }

                return roles;
            }
            else
            {
                //no hay informacion
                return roles;
            }
        }

        public static ML.Result GetAll()
        {
          

            ML.Result result = new ML.Result(); //instanciando

            try
            {
                //intenta
                //1 no se conecte a la BD
                //2 la tabla no exista
                //3 no trae datos
                //4 que obtenga mal la informacion

                using (SqlConnection context = new SqlConnection(DL.Connection.GetConnection()))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = context;
                    cmd.CommandText = "RolGetAll";
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    if (dataTable.Rows.Count > 0)
                    {
                        //hay registros
                        result.Objects = new List<object>(); //null vs vacio

                        foreach (DataRow row in dataTable.Rows)
                        {
                            ML.Rol rol = new ML.Rol();
                            rol.IdRol = Convert.ToInt32(row[0]);
                            rol.Nombre = row[1].ToString();

                            result.Objects.Add(rol);
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

        public static object GetByIdSELECTMal(int id)
        {
            SqlConnection context = new SqlConnection(DL.Connection.GetConnection());

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = context;
            cmd.CommandText = "SELECT * FROM Rol WHERE IdRol = @IdRol";

            cmd.Parameters.AddWithValue("@IdRol", id);

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();

            adapter.Fill(dataTable);

            if (dataTable.Rows.Count > 0)
            {
                //si trajo algo
                DataRow row = dataTable.Rows[0];

                ML.Rol rolEncontrado = new ML.Rol();
                rolEncontrado.IdRol = Convert.ToInt32(row[0]);
                rolEncontrado.Nombre = row[1].ToString();

                object bolsa = rolEncontrado; //boxing

                return bolsa;
            }
            else
            {
                return null;
            }
        }

    }
}
