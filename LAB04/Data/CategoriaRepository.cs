using LAB04.Models;
using Microsoft.Data.SqlClient;

namespace LAB04.Data
{
    public class CategoriaRepository
    {
        public List<Categoria> Listar()
        {
            var lista = new List<Categoria>();
            using var cn = ConexionBD.CrearConexion();
            using var cmd = new SqlCommand("dbo.sp_Categorias_Listar", cn) { CommandType = System.Data.CommandType.StoredProcedure };
            cn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Categoria
                {
                    IdCategoria = reader.GetInt32(reader.GetOrdinal("IdCategoria")),
                    NombreCategoria = reader.GetString(reader.GetOrdinal("NombreCategoria")),
                    Descripcion = reader["Descripcion"] as string
                });
            }
            return lista;
        }

        public void Insertar(Categoria c)
        {
            using var cn = ConexionBD.CrearConexion();
            using var cmd = new SqlCommand("dbo.sp_Categorias_Insertar", cn) { CommandType = System.Data.CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@NombreCategoria", c.NombreCategoria);
            cmd.Parameters.AddWithValue("@Descripcion", (object?)c.Descripcion ?? DBNull.Value);
            var idParam = new SqlParameter("@IdCategoria", System.Data.SqlDbType.Int) { Direction = System.Data.ParameterDirection.Output };
            cmd.Parameters.Add(idParam);
            cn.Open();
            cmd.ExecuteNonQuery();
            c.IdCategoria = (int)idParam.Value!;
        }

        public void Actualizar(Categoria c)
        {
            using var cn = ConexionBD.CrearConexion();
            using var cmd = new SqlCommand("dbo.sp_Categorias_Actualizar", cn) { CommandType = System.Data.CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@IdCategoria", c.IdCategoria);
            cmd.Parameters.AddWithValue("@NombreCategoria", c.NombreCategoria);
            cmd.Parameters.AddWithValue("@Descripcion", (object?)c.Descripcion ?? DBNull.Value);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Eliminar(int idCategoria)
        {
            using var cn = ConexionBD.CrearConexion();
            using var cmd = new SqlCommand("dbo.sp_Categorias_Eliminar", cn) { CommandType = System.Data.CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@IdCategoria", idCategoria);
            cn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
