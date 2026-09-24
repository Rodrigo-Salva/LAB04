using System.Data;
using Microsoft.Data.SqlClient;
using LAB04.Models;

namespace LAB04.Data
{
    public class ProveedorRepository
    {
        public async Task<List<Proveedor>> ListarAsync()
        {
            var tabla = await AccesoDatos.ObtenerTablaAsync("dbo.sp_Proveedores_Listar");
            return Mapear(tabla);
        }

        public async Task<List<Proveedor>> BuscarPorContactoCiudadAsync(string? nombreContacto, string? ciudad)
        {
            var tabla = await AccesoDatos.ObtenerTablaAsync("dbo.sp_Proveedores_BuscarPorContactoCiudad",
                AccesoDatos.Param("@NombreContacto", nombreContacto),
                AccesoDatos.Param("@Ciudad", ciudad));
            return Mapear(tabla);
        }

        public async Task InsertarAsync(Proveedor p)
        {
            var id = AccesoDatos.Salida("@IdProveedor");
            await AccesoDatos.EjecutarAsync("dbo.sp_Proveedores_Insertar", [.. Parametros(p), id]);
            p.IdProveedor = (int)id.Value;
        }

        public Task ActualizarAsync(Proveedor p)
        {
            return AccesoDatos.EjecutarAsync("dbo.sp_Proveedores_Actualizar",
                [AccesoDatos.Param("@IdProveedor", p.IdProveedor), .. Parametros(p)]);
        }

        public Task EliminarAsync(int idProveedor)
        {
            return AccesoDatos.EjecutarAsync("dbo.sp_Proveedores_Eliminar",
                AccesoDatos.Param("@IdProveedor", idProveedor));
        }

        private static List<Proveedor> Mapear(DataTable tabla) =>
            tabla.AsEnumerable().Select(r => new Proveedor
            {
                IdProveedor = r.Field<int>("IdProveedor"),
                NombreCompania = r.Field<string>("NombreCompania") ?? string.Empty,
                NombreContacto = r.Field<string>("NombreContacto"),
                Cargo = r.Field<string>("Cargo"),
                Direccion = r.Field<string>("Direccion"),
                Ciudad = r.Field<string>("Ciudad"),
                CodigoPostal = r.Field<string>("CodigoPostal"),
                Pais = r.Field<string>("Pais"),
                Telefono = r.Field<string>("Telefono"),
                Fax = r.Field<string>("Fax")
            }).ToList();

        private static SqlParameter[] Parametros(Proveedor p) =>
        [
            AccesoDatos.Param("@NombreCompania", p.NombreCompania),
            AccesoDatos.Param("@NombreContacto", p.NombreContacto),
            AccesoDatos.Param("@Cargo", p.Cargo),
            AccesoDatos.Param("@Direccion", p.Direccion),
            AccesoDatos.Param("@Ciudad", p.Ciudad),
            AccesoDatos.Param("@CodigoPostal", p.CodigoPostal),
            AccesoDatos.Param("@Pais", p.Pais),
            AccesoDatos.Param("@Telefono", p.Telefono),
            AccesoDatos.Param("@Fax", p.Fax)
        ];
    }
}
