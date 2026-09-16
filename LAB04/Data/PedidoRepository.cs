using System.Data;
using LAB04.Models;
using Microsoft.Data.SqlClient;

namespace LAB04.Data
{
    public class PedidoRepository
    {
        public List<Pedido> Listar()
        {
            var lista = new List<Pedido>();
            using var cn = ConexionBD.CrearConexion();
            using var cmd = new SqlCommand("dbo.sp_Pedidos_Listar", cn) { CommandType = CommandType.StoredProcedure };
            cn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read()) lista.Add(LeerPedido(reader));
            return lista;
        }

        private static Pedido LeerPedido(SqlDataReader r) => new Pedido
        {
            IdPedido = r.GetInt32(r.GetOrdinal("IdPedido")),
            IdCliente = r["IdCliente"] as int?,
            NombreCliente = r["NombreCliente"] as string,
            IdEmpleado = r["IdEmpleado"] as int?,
            NombreEmpleado = r["NombreEmpleado"] as string,
            FechaPedido = r.GetDateTime(r.GetOrdinal("FechaPedido")),
            FechaRequerida = r["FechaRequerida"] as DateTime?,
            FechaEnvio = r["FechaEnvio"] as DateTime?,
            IdTransportista = r["IdTransportista"] as int?,
            NombreTransportista = r["NombreTransportista"] as string,
            NombreDestinatario = r["NombreDestinatario"] as string,
            CiudadDestino = r["CiudadDestino"] as string,
            PaisDestino = r["PaisDestino"] as string
        };

        public void Insertar(Pedido p)
        {
            using var cn = ConexionBD.CrearConexion();
            using var cmd = new SqlCommand("dbo.sp_Pedidos_Insertar", cn) { CommandType = CommandType.StoredProcedure };
            AgregarParametros(cmd, p);
            var idParam = new SqlParameter("@IdPedido", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmd.Parameters.Add(idParam);
            cn.Open();
            cmd.ExecuteNonQuery();
            p.IdPedido = (int)idParam.Value!;
        }

        public void Actualizar(Pedido p)
        {
            using var cn = ConexionBD.CrearConexion();
            using var cmd = new SqlCommand("dbo.sp_Pedidos_Actualizar", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@IdPedido", p.IdPedido);
            AgregarParametros(cmd, p);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Eliminar(int idPedido)
        {
            using var cn = ConexionBD.CrearConexion();
            using var cmd = new SqlCommand("dbo.sp_Pedidos_Eliminar", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@IdPedido", idPedido);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        private static void AgregarParametros(SqlCommand cmd, Pedido p)
        {
            cmd.Parameters.AddWithValue("@IdCliente", (object?)p.IdCliente ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IdEmpleado", (object?)p.IdEmpleado ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FechaPedido", p.FechaPedido.Date);
            cmd.Parameters.AddWithValue("@FechaRequerida", (object?)p.FechaRequerida?.Date ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@FechaEnvio", (object?)p.FechaEnvio?.Date ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IdTransportista", (object?)p.IdTransportista ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@NombreDestinatario", (object?)p.NombreDestinatario ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CiudadDestino", (object?)p.CiudadDestino ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PaisDestino", (object?)p.PaisDestino ?? DBNull.Value);
        }

        public List<DetallePedido> ListarDetalles(int idPedido)
        {
            var lista = new List<DetallePedido>();
            using var cn = ConexionBD.CrearConexion();
            using var cmd = new SqlCommand("dbo.sp_DetallesPedidos_ListarPorPedido", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@IdPedido", idPedido);
            cn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new DetallePedido
                {
                    IdPedido = reader.GetInt32(reader.GetOrdinal("IdPedido")),
                    IdProducto = reader.GetInt32(reader.GetOrdinal("IdProducto")),
                    NombreProducto = reader["NombreProducto"] as string,
                    PrecioUnidad = reader.GetDecimal(reader.GetOrdinal("PrecioUnidad")),
                    Cantidad = reader.GetInt16(reader.GetOrdinal("Cantidad")),
                    Descuento = reader.GetDecimal(reader.GetOrdinal("Descuento")),
                    Subtotal = reader.GetDecimal(reader.GetOrdinal("Subtotal"))
                });
            }
            return lista;
        }

        public void InsertarDetalle(DetallePedido d)
        {
            using var cn = ConexionBD.CrearConexion();
            using var cmd = new SqlCommand("dbo.sp_DetallesPedidos_Insertar", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@IdPedido", d.IdPedido);
            cmd.Parameters.AddWithValue("@IdProducto", d.IdProducto);
            cmd.Parameters.AddWithValue("@PrecioUnidad", d.PrecioUnidad);
            cmd.Parameters.AddWithValue("@Cantidad", d.Cantidad);
            cmd.Parameters.AddWithValue("@Descuento", d.Descuento);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public void ActualizarDetalle(DetallePedido d)
        {
            using var cn = ConexionBD.CrearConexion();
            using var cmd = new SqlCommand("dbo.sp_DetallesPedidos_Actualizar", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@IdPedido", d.IdPedido);
            cmd.Parameters.AddWithValue("@IdProducto", d.IdProducto);
            cmd.Parameters.AddWithValue("@PrecioUnidad", d.PrecioUnidad);
            cmd.Parameters.AddWithValue("@Cantidad", d.Cantidad);
            cmd.Parameters.AddWithValue("@Descuento", d.Descuento);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public void EliminarDetalle(int idPedido, int idProducto)
        {
            using var cn = ConexionBD.CrearConexion();
            using var cmd = new SqlCommand("dbo.sp_DetallesPedidos_Eliminar", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@IdPedido", idPedido);
            cmd.Parameters.AddWithValue("@IdProducto", idProducto);
            cn.Open();
            cmd.ExecuteNonQuery();
        }

        public List<ItemCombo> ListarClientes() => ListarCombo("dbo.sp_Clientes_Listar");

        public List<ItemCombo> ListarEmpleados() => ListarCombo("dbo.sp_Empleados_Listar");

        public List<ItemCombo> ListarTransportistas() => ListarCombo("dbo.sp_Transportistas_Listar");

        private static List<ItemCombo> ListarCombo(string procedimiento)
        {
            var lista = new List<ItemCombo>();
            using var cn = ConexionBD.CrearConexion();
            using var cmd = new SqlCommand(procedimiento, cn) { CommandType = CommandType.StoredProcedure };
            cn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new ItemCombo
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Texto = reader["Texto"] as string ?? string.Empty
                });
            }
            return lista;
        }

        public List<ReporteDetallePedido> ReportePorFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            var lista = new List<ReporteDetallePedido>();
            using var cn = ConexionBD.CrearConexion();
            using var cmd = new SqlCommand("dbo.sp_DetallesPedidos_ReportePorFechas", cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio.Date);
            cmd.Parameters.AddWithValue("@FechaFin", fechaFin.Date);
            cn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lista.Add(new ReporteDetallePedido
                {
                    IdPedido = reader.GetInt32(reader.GetOrdinal("IdPedido")),
                    FechaPedido = reader.GetDateTime(reader.GetOrdinal("FechaPedido")),
                    NombreCliente = reader["NombreCliente"] as string,
                    NombreProducto = reader.GetString(reader.GetOrdinal("NombreProducto")),
                    PrecioUnidad = reader.GetDecimal(reader.GetOrdinal("PrecioUnidad")),
                    Cantidad = reader.GetInt16(reader.GetOrdinal("Cantidad")),
                    Descuento = reader.GetDecimal(reader.GetOrdinal("Descuento")),
                    Subtotal = reader.GetDecimal(reader.GetOrdinal("Subtotal"))
                });
            }
            return lista;
        }
    }
}
