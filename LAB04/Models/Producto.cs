namespace LAB04.Models
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int? IdProveedor { get; set; }
        public string? NombreProveedor { get; set; }
        public int? IdCategoria { get; set; }
        public string? NombreCategoria { get; set; }
        public string? CantidadPorUnidad { get; set; }
        public decimal PrecioUnidad { get; set; }
        public short UnidadesEnExistencia { get; set; }
        public short UnidadesEnPedido { get; set; }
        public short NivelReorden { get; set; }
        public bool Descontinuado { get; set; }
    }
}
