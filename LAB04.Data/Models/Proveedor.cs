namespace LAB04.Models
{
    public class Proveedor
    {
        public int IdProveedor { get; set; }
        public string NombreCompania { get; set; } = string.Empty;
        public string? NombreContacto { get; set; }
        public string? Cargo { get; set; }
        public string? Direccion { get; set; }
        public string? Ciudad { get; set; }
        public string? CodigoPostal { get; set; }
        public string? Pais { get; set; }
        public string? Telefono { get; set; }
        public string? Fax { get; set; }
    }
}
