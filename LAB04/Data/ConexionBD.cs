using System.Configuration;
using Microsoft.Data.SqlClient;

namespace LAB04.Data
{
    public static class ConexionBD
    {
        public static string ObtenerCadenaConexion()
        {
            var cs = ConfigurationManager.ConnectionStrings["NeptunoDB"];
            if (cs == null)
                throw new InvalidOperationException(
                    "No se encontró la cadena de conexión 'NeptunoDB' en App.config.");
            return cs.ConnectionString;
        }

        public static SqlConnection CrearConexion()
        {
            return new SqlConnection(ObtenerCadenaConexion());
        }
    }
}
