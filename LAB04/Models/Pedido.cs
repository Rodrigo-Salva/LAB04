namespace LAB04.Models
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public int? IdCliente { get; set; }
        public string? NombreCliente { get; set; }
        public int? IdEmpleado { get; set; }
        public string? NombreEmpleado { get; set; }
        public DateTime FechaPedido { get; set; } = DateTime.Now;
        public DateTime? FechaRequerida { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public int? IdTransportista { get; set; }
        public string? NombreTransportista { get; set; }
        public string? NombreDestinatario { get; set; }
        public string? CiudadDestino { get; set; }
        public string? PaisDestino { get; set; }
    }

    public class DetallePedido
    {
        public int IdPedido { get; set; }
        public int IdProducto { get; set; }
        public string? NombreProducto { get; set; }
        public decimal PrecioUnidad { get; set; }
        public short Cantidad { get; set; }
        public decimal Descuento { get; set; }
        public decimal Subtotal { get; set; }
    }

    public class ItemCombo
    {
        public int Id { get; set; }
        public string Texto { get; set; } = string.Empty;
        public override string ToString() => Texto;
    }

    public class ReporteDetallePedido
    {
        public int IdPedido { get; set; }
        public DateTime FechaPedido { get; set; }
        public string? NombreCliente { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public decimal PrecioUnidad { get; set; }
        public short Cantidad { get; set; }
        public decimal Descuento { get; set; }
        public decimal Subtotal { get; set; }
    }
}
