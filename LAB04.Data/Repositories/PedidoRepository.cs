using System.Data;
using Microsoft.Data.SqlClient;
using LAB04.Models;

namespace LAB04.Data
{
    public class PedidoRepository
    {
        public async Task<PedidosDesconectados> CargarDesconectadoAsync()
        {
            var datos = await AccesoDatos.ObtenerDataSetAsync(
                (PedidosDesconectados.TablaPedidos, "dbo.sp_Pedidos_Listar"),
                (PedidosDesconectados.TablaDetalle, "dbo.sp_DetallesPedidos_ListarActivos"));
            return new PedidosDesconectados(datos);
        }

        public async Task InsertarAsync(Pedido p)
        {
            var id = AccesoDatos.Salida("@IdPedido");
            await AccesoDatos.EjecutarAsync("dbo.sp_Pedidos_Insertar", [.. Parametros(p), id]);
            p.IdPedido = (int)id.Value;
        }

        public Task ActualizarAsync(Pedido p)
        {
            return AccesoDatos.EjecutarAsync("dbo.sp_Pedidos_Actualizar",
                [AccesoDatos.Param("@IdPedido", p.IdPedido), .. Parametros(p)]);
        }

        public Task EliminarAsync(int idPedido)
        {
            return AccesoDatos.EjecutarAsync("dbo.sp_Pedidos_Eliminar",
                AccesoDatos.Param("@IdPedido", idPedido));
        }

        public Task InsertarDetalleAsync(DetallePedido d)
        {
            return AccesoDatos.EjecutarAsync("dbo.sp_DetallesPedidos_Insertar", ParametrosDetalle(d));
        }

        public Task ActualizarDetalleAsync(DetallePedido d)
        {
            return AccesoDatos.EjecutarAsync("dbo.sp_DetallesPedidos_Actualizar", ParametrosDetalle(d));
        }

        public Task EliminarDetalleAsync(int idPedido, int idProducto)
        {
            return AccesoDatos.EjecutarAsync("dbo.sp_DetallesPedidos_Eliminar",
                AccesoDatos.Param("@IdPedido", idPedido),
                AccesoDatos.Param("@IdProducto", idProducto));
        }

        public Task<List<ItemCombo>> ListarClientesAsync() => ListarComboAsync("dbo.sp_Clientes_Listar");

        public Task<List<ItemCombo>> ListarEmpleadosAsync() => ListarComboAsync("dbo.sp_Empleados_Listar");

        public Task<List<ItemCombo>> ListarTransportistasAsync() => ListarComboAsync("dbo.sp_Transportistas_Listar");

        public async Task<List<ReporteDetallePedido>> ReportePorFechasAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            var tabla = await AccesoDatos.ObtenerTablaAsync("dbo.sp_DetallesPedidos_ReportePorFechas",
                AccesoDatos.Param("@FechaInicio", fechaInicio.Date),
                AccesoDatos.Param("@FechaFin", fechaFin.Date));
            return tabla.AsEnumerable().Select(r => new ReporteDetallePedido
            {
                IdPedido = r.Field<int>("IdPedido"),
                FechaPedido = r.Field<DateTime>("FechaPedido"),
                NombreCliente = r.Field<string>("NombreCliente"),
                NombreProducto = r.Field<string>("NombreProducto") ?? string.Empty,
                PrecioUnidad = r.Field<decimal>("PrecioUnidad"),
                Cantidad = r.Field<short>("Cantidad"),
                Descuento = r.Field<decimal>("Descuento"),
                Subtotal = r.Field<decimal>("Subtotal")
            }).ToList();
        }

        private static async Task<List<ItemCombo>> ListarComboAsync(string procedimiento)
        {
            var tabla = await AccesoDatos.ObtenerTablaAsync(procedimiento);
            return tabla.AsEnumerable().Select(r => new ItemCombo
            {
                Id = r.Field<int>("Id"),
                Texto = r.Field<string>("Texto") ?? string.Empty
            }).ToList();
        }

        private static SqlParameter[] Parametros(Pedido p) =>
        [
            AccesoDatos.Param("@IdCliente", p.IdCliente),
            AccesoDatos.Param("@IdEmpleado", p.IdEmpleado),
            AccesoDatos.Param("@FechaPedido", p.FechaPedido.Date),
            AccesoDatos.Param("@FechaRequerida", p.FechaRequerida?.Date),
            AccesoDatos.Param("@FechaEnvio", p.FechaEnvio?.Date),
            AccesoDatos.Param("@IdTransportista", p.IdTransportista),
            AccesoDatos.Param("@NombreDestinatario", p.NombreDestinatario),
            AccesoDatos.Param("@CiudadDestino", p.CiudadDestino),
            AccesoDatos.Param("@PaisDestino", p.PaisDestino)
        ];

        private static SqlParameter[] ParametrosDetalle(DetallePedido d) =>
        [
            AccesoDatos.Param("@IdPedido", d.IdPedido),
            AccesoDatos.Param("@IdProducto", d.IdProducto),
            AccesoDatos.Param("@PrecioUnidad", d.PrecioUnidad),
            AccesoDatos.Param("@Cantidad", d.Cantidad),
            AccesoDatos.Param("@Descuento", d.Descuento)
        ];
    }
}
