using System.Data;
using LAB04.Models;

namespace LAB04.Data
{
    public class CategoriaRepository
    {
        public async Task<List<Categoria>> ListarAsync()
        {
            var tabla = await AccesoDatos.ObtenerTablaAsync("dbo.sp_Categorias_Listar");
            return tabla.AsEnumerable().Select(r => new Categoria
            {
                IdCategoria = r.Field<int>("IdCategoria"),
                NombreCategoria = r.Field<string>("NombreCategoria") ?? string.Empty,
                Descripcion = r.Field<string>("Descripcion")
            }).ToList();
        }

        public async Task InsertarAsync(Categoria c)
        {
            var id = AccesoDatos.Salida("@IdCategoria");
            await AccesoDatos.EjecutarAsync("dbo.sp_Categorias_Insertar",
                AccesoDatos.Param("@NombreCategoria", c.NombreCategoria),
                AccesoDatos.Param("@Descripcion", c.Descripcion),
                id);
            c.IdCategoria = (int)id.Value;
        }

        public Task ActualizarAsync(Categoria c)
        {
            return AccesoDatos.EjecutarAsync("dbo.sp_Categorias_Actualizar",
                AccesoDatos.Param("@IdCategoria", c.IdCategoria),
                AccesoDatos.Param("@NombreCategoria", c.NombreCategoria),
                AccesoDatos.Param("@Descripcion", c.Descripcion));
        }

        public Task EliminarAsync(int idCategoria)
        {
            return AccesoDatos.EjecutarAsync("dbo.sp_Categorias_Eliminar",
                AccesoDatos.Param("@IdCategoria", idCategoria));
        }
    }
}
