using System.Data;
using LAB04.Models;
using Microsoft.Data.SqlClient;

namespace LAB04.Data
{
    public class ProveedorRepository
    {
        private static Proveedor Leer(SqlDataReader r) => new Proveedor
        {
            IdProveedor = r.GetInt32(r.GetOrdinal("IdProveedor")),
            NombreCompania = r.GetString(r.GetOrdinal("NombreCompania")),
            NombreContacto = r["NombreContacto"] as string,
            Cargo = r["Cargo"] as string,
            Direccion = r["Direccion"] as string,
            Ciudad = r["Ciudad"] as string,
            CodigoPostal = r["CodigoPostal"] as string,
            Pais = r["Pais"] as string,
            Telefono = r["Telefono"] as string,
            Fax = r["Fax"] as string
        };

        public List<Proveedor> Listar()
        {
            var lista = new List<Proveedor>();
            using var cn = ConexionBD.CrearConexion();
            using var cmd = new SqlCommand("dbo.sp_Proveedores_Listar", cn) { CommandType = CommandType.StoredProcedure };
            cn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) lista.Add(Leer(reader));
            return lista;
        }

        public List<Proveedor> BuscarPorContactoCiudad(string? nombreContacto, string? ciudad)
        {
            var lista = new List<Proveedor>();
            using var cn = ConexionBD.CrearConexion();
            using var cmd = new SqlCommand("dbo.sp_Proveedores_BuscarPorContactoCiudad", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@NombreContacto", (object?)nombreContacto ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Ciudad", (object?)ciudad ?? DBNull.Value);
            cn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) lista.Add(Leer(reader));
            return lista;
        }

        public void Insertar(Proveedor p)
        {
            using var cn = ConexionBD.CrearConexion();
            using var cmd = new SqlCommand("dbo.sp_Proveedores_Insertar", cn) { CommandType = CommandType.StoredProcedure };
            AgregarParametros(cmd, p);
            var idParam = new SqlParameter("@IdProveedor", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(idParam);
            cn.Open();
            cmd.ExecuteNonQuery();
            p.IdProveedor = (int)idParam.Value!;
        }

        public void Actualizar(Proveedor p)
        {
            using var cn = ConexionBD.CrearConexion();
            using var cmd = new SqlCommand("dbo.sp_Proveedores_Actualizar", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@IdProveedor", p.IdProveedor);
            AgregarParametros(cmd, p);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Eliminar(int idProveedor)
        {
            using var cn = ConexionBD.CrearConexion();
            using var cmd = new SqlCommand("dbo.sp_Proveedores_Eliminar", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@IdProveedor", idProveedor);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        private static void AgregarParametros(SqlCommand cmd, Proveedor p)
        {
            cmd.Parameters.AddWithValue("@NombreCompania", p.NombreCompania);
            cmd.Parameters.AddWithValue("@NombreContacto", (object?)p.NombreContacto ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Cargo", (object?)p.Cargo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Direccion", (object?)p.Direccion ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Ciudad", (object?)p.Ciudad ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CodigoPostal", (object?)p.CodigoPostal ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Pais", (object?)p.Pais ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Telefono", (object?)p.Telefono ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Fax", (object?)p.Fax ?? DBNull.Value);
        }
    }
}
