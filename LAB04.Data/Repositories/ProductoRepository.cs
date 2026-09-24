using System.Data;
using Microsoft.Data.SqlClient;
using LAB04.Models;

namespace LAB04.Data
{
    public class ProductoRepository
    {
        public async Task<List<Producto>> ListarAsync()
        {
            var tabla = await AccesoDatos.ObtenerTablaAsync("dbo.sp_Productos_Listar");
            return tabla.AsEnumerable().Select(r => new Producto
            {
                IdProducto = r.Field<int>("IdProducto"),
                NombreProducto = r.Field<string>("NombreProducto") ?? string.Empty,
                IdProveedor = r.Field<int?>("IdProveedor"),
                NombreProveedor = r.Field<string>("NombreProveedor"),
                IdCategoria = r.Field<int?>("IdCategoria"),
                NombreCategoria = r.Field<string>("NombreCategoria"),
                CantidadPorUnidad = r.Field<string>("CantidadPorUnidad"),
                PrecioUnidad = r.Field<decimal>("PrecioUnidad"),
                UnidadesEnExistencia = r.Field<short>("UnidadesEnExistencia"),
                UnidadesEnPedido = r.Field<short>("UnidadesEnPedido"),
                NivelReorden = r.Field<short>("NivelReorden"),
                Descontinuado = r.Field<bool>("Descontinuado")
            }).ToList();
        }

        public async Task InsertarAsync(Producto p)
        {
            var id = AccesoDatos.Salida("@IdProducto");
            await AccesoDatos.EjecutarAsync("dbo.sp_Productos_Insertar", [.. Parametros(p), id]);
            p.IdProducto = (int)id.Value;
        }

        public Task ActualizarAsync(Producto p)
        {
            return AccesoDatos.EjecutarAsync("dbo.sp_Productos_Actualizar",
                [AccesoDatos.Param("@IdProducto", p.IdProducto), .. Parametros(p)]);
        }

        public Task EliminarAsync(int idProducto)
        {
            return AccesoDatos.EjecutarAsync("dbo.sp_Productos_Eliminar",
                AccesoDatos.Param("@IdProducto", idProducto));
        }

        private static SqlParameter[] Parametros(Producto p) =>
        [
            AccesoDatos.Param("@NombreProducto", p.NombreProducto),
            AccesoDatos.Param("@IdProveedor", p.IdProveedor),
            AccesoDatos.Param("@IdCategoria", p.IdCategoria),
            AccesoDatos.Param("@CantidadPorUnidad", p.CantidadPorUnidad),
            AccesoDatos.Param("@PrecioUnidad", p.PrecioUnidad),
            AccesoDatos.Param("@UnidadesEnExistencia", p.UnidadesEnExistencia),
            AccesoDatos.Param("@UnidadesEnPedido", p.UnidadesEnPedido),
            AccesoDatos.Param("@NivelReorden", p.NivelReorden),
            AccesoDatos.Param("@Descontinuado", p.Descontinuado)
        ];
    }
}
