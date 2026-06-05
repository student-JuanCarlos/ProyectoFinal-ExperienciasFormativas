using Data.Infraestructure;
using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public class PrecioReservaRepository: IConfigurationReserva
    {
        private readonly string cadenaConexion;

        public PrecioReservaRepository(IConfiguration config)
        {
            cadenaConexion = config["ConnectionStrings:database"] ?? string.Empty;
        }

        public int ActualizarPrecio(decimal precio)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_PrecioReserva";
                    cmd.Parameters.AddWithValue("@Precio", precio);
                    cn.Open();
                    f = cmd.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return f;
        }

        public ConfiguracionReserva DetallePrecioReserva()
        {
            var config = new ConfiguracionReserva();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandText = "SELECT PrecioReserva FROM ConfiguracionReserva";
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        config = new ConfiguracionReserva()
                        {
                            PrecioReserva = Convert.ToDecimal(reader["PrecioReserva"])
                        };
                    };
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return config;

        }
    }
}
