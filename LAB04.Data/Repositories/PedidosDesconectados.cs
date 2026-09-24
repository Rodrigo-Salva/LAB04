using System.Data;
using LAB04.Models;

namespace LAB04.Data
{
    public class PedidosDesconectados
    {
        internal const string TablaPedidos = "Pedidos";
        internal const string TablaDetalle = "DetallePedidos";

        private readonly DataSet _datos;
        private readonly DataRelation _relacion;

        internal PedidosDesconectados(DataSet datos)
        {
            _datos = datos;
            var pedidos = _datos.Tables[TablaPedidos]!;
            var detalle = _datos.Tables[TablaDetalle]!;
            pedidos.PrimaryKey = [pedidos.Columns["IdPedido"]!];
            _relacion = _datos.Relations.Add("Pedido_Detalle", pedidos.Columns["IdPedido"]!, detalle.Columns["IdPedido"]!, false);
        }

        public List<Pedido> Pedidos()
        {
            return _datos.Tables[TablaPedidos]!.AsEnumerable().Select(r => new Pedido
            {
                IdPedido = r.Field<int>("IdPedido"),
                IdCliente = r.Field<int?>("IdCliente"),
                NombreCliente = r.Field<string>("NombreCliente"),
                IdEmpleado = r.Field<int?>("IdEmpleado"),
                NombreEmpleado = r.Field<string>("NombreEmpleado"),
                FechaPedido = r.Field<DateTime>("FechaPedido"),
                FechaRequerida = r.Field<DateTime?>("FechaRequerida"),
                FechaEnvio = r.Field<DateTime?>("FechaEnvio"),
                IdTransportista = r.Field<int?>("IdTransportista"),
                NombreTransportista = r.Field<string>("NombreTransportista"),
                NombreDestinatario = r.Field<string>("NombreDestinatario"),
                CiudadDestino = r.Field<string>("CiudadDestino"),
                PaisDestino = r.Field<string>("PaisDestino")
            }).ToList();
        }

        public List<DetallePedido> DetalleDe(int idPedido)
        {
            var pedido = _datos.Tables[TablaPedidos]!.Rows.Find(idPedido);
            if (pedido == null) return [];

            return pedido.GetChildRows(_relacion).Select(r => new DetallePedido
            {
                IdPedido = r.Field<int>("IdPedido"),
                IdProducto = r.Field<int>("IdProducto"),
                NombreProducto = r.Field<string>("NombreProducto"),
                PrecioUnidad = r.Field<decimal>("PrecioUnidad"),
                Cantidad = r.Field<short>("Cantidad"),
                Descuento = r.Field<decimal>("Descuento"),
                Subtotal = r.Field<decimal>("Subtotal")
            }).ToList();
        }
    }
}
