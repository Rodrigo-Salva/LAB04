using System.Data;
using LAB04.Models;
using Microsoft.Data.SqlClient;

namespace LAB04.Data
{
    public class ProductoRepository
    {
        public List<Producto> Listar()
        {
            var lista = new List<Producto>();
            using var cn = ConexionBD.CrearConexion();
            using var cmd = new SqlCommand("dbo.sp_Productos_Listar", cn) { CommandType = CommandType.StoredProcedure };
            cn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new Producto
                {
                    IdProducto = reader.GetInt32(reader.GetOrdinal("IdProducto")),
                    NombreProducto = reader.GetString(reader.GetOrdinal("NombreProducto")),
                    IdProveedor = reader["IdProveedor"] as int?,
                    NombreProveedor = reader["NombreProveedor"] as string,
                    IdCategoria = reader["IdCategoria"] as int?,
                    NombreCategoria = reader["NombreCategoria"] as string,
                    CantidadPorUnidad = reader["CantidadPorUnidad"] as string,
                    PrecioUnidad = reader.GetDecimal(reader.GetOrdinal("PrecioUnidad")),
                    UnidadesEnExistencia = reader.GetInt16(reader.GetOrdinal("UnidadesEnExistencia")),
                    UnidadesEnPedido = reader.GetInt16(reader.GetOrdinal("UnidadesEnPedido")),
                    NivelReorden = reader.GetInt16(reader.GetOrdinal("NivelReorden")),
                    Descontinuado = reader.GetBoolean(reader.GetOrdinal("Descontinuado"))
                });
            }
            return lista;
        }

        public void Insertar(Producto p)
        {
            using var cn = ConexionBD.CrearConexion();
            using var cmd = new SqlCommand("dbo.sp_Productos_Insertar", cn) { CommandType = CommandType.StoredProcedure };
            AgregarParametros(cmd, p);
            var idParam = new SqlParameter("@IdProducto", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(idParam);
            cn.Open();
            cmd.ExecuteNonQuery();
            p.IdProducto = (int)idParam.Value!;
        }

        public void Actualizar(Producto p)
        {
            using var cn = ConexionBD.CrearConexion();
            using var cmd = new SqlCommand("dbo.sp_Productos_Actualizar", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@IdProducto", p.IdProducto);
            AgregarParametros(cmd, p);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Eliminar(int idProducto)
        {
            using var cn = ConexionBD.CrearConexion();
            using var cmd = new SqlCommand("dbo.sp_Productos_Eliminar", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@IdProducto", idProducto);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        private static void AgregarParametros(SqlCommand cmd, Producto p)
        {
            cmd.Parameters.AddWithValue("@NombreProducto", p.NombreProducto);
            cmd.Parameters.AddWithValue("@IdProveedor", (object?)p.IdProveedor ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IdCategoria", (object?)p.IdCategoria ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CantidadPorUnidad", (object?)p.CantidadPorUnidad ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PrecioUnidad", p.PrecioUnidad);
            cmd.Parameters.AddWithValue("@UnidadesEnExistencia", p.UnidadesEnExistencia);
            cmd.Parameters.AddWithValue("@UnidadesEnPedido", p.UnidadesEnPedido);
            cmd.Parameters.AddWithValue("@NivelReorden", p.NivelReorden);
            cmd.Parameters.AddWithValue("@Descontinuado", p.Descontinuado);
        }
    }
}
