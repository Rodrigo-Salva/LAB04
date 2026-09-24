using System.Data;
using Microsoft.Data.SqlClient;

namespace LAB04.Data
{
    internal static class AccesoDatos
    {
        public static SqlParameter Param(string nombre, object? valor) => new(nombre, valor ?? DBNull.Value);

        public static SqlParameter Salida(string nombre) => new(nombre, SqlDbType.Int) { Direction = ParameterDirection.Output };

        public static async Task EjecutarAsync(string procedimiento, params SqlParameter[] parametros)
        {
            using var cn = ConexionBD.CrearConexion();
            using var cmd = CrearComando(cn, procedimiento, parametros);
            await cn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public static Task<DataTable> ObtenerTablaAsync(string procedimiento, params SqlParameter[] parametros)
        {
            return Task.Run(() =>
            {
                using var cn = ConexionBD.CrearConexion();
                using var cmd = CrearComando(cn, procedimiento, parametros);
                using var adaptador = new SqlDataAdapter(cmd);
                var tabla = new DataTable();
                adaptador.Fill(tabla);
                return tabla;
            });
        }

        public static Task<DataSet> ObtenerDataSetAsync(params (string Tabla, string Procedimiento)[] origenes)
        {
            return Task.Run(() =>
            {
                var datos = new DataSet();
                using var cn = ConexionBD.CrearConexion();
                foreach (var (tabla, procedimiento) in origenes)
                {
                    using var cmd = CrearComando(cn, procedimiento);
                    using var adaptador = new SqlDataAdapter(cmd);
                    adaptador.Fill(datos, tabla);
                }
                return datos;
            });
        }

        private static SqlCommand CrearComando(SqlConnection cn, string procedimiento, params SqlParameter[] parametros)
        {
            var cmd = new SqlCommand(procedimiento, cn) { CommandType = CommandType.StoredProcedure };
            cmd.Parameters.AddRange(parametros);
            return cmd;
        }
    }
}
