using Data.Infraestructure;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Data.Repository
{
    public class CargoRepository : ICargo
    {
        private readonly string cadenaConexion;

        public CargoRepository(IConfiguration config)
        {
            cadenaConexion = config["ConnectionStrings:database"] ?? string.Empty;
        }

        public int Actualizar(Cargo c)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_ActualizarCargo";
                    cmd.Parameters.AddWithValue("@NombreCargo", c.NombreCargo);
                    cmd.Parameters.AddWithValue("@Descripcion", c.Descripcion == null ? (object)DBNull.Value : c.Descripcion);
                    cmd.Parameters.AddWithValue("@IdCargo", c.IdCargo);
                    cn.Open();
                    f = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return f;
        }

        public int Agregar(Cargo c)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_InsertarCargo";
                    cmd.Parameters.AddWithValue("@NombreCargo", c.NombreCargo);
                    cmd.Parameters.AddWithValue("@Descripcion", c.Descripcion == null ? (object)DBNull.Value : c.Descripcion );
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

        public Cargo Detalle(int id)
        {
            var cargo = new Cargo();
            using (SqlConnection cn = new SqlConnection())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_DetalleCargo";
                    cmd.Parameters.AddWithValue("@IdCargo", id);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        cargo = new Cargo()
                        {
                            IdCargo = Convert.ToInt32(reader["IdCargo"]),
                            NombreCargo = reader["NombreCargo"].ToString(),
                            Descripcion = reader["Descripcion"] == DBNull.Value ? null : reader["Descripcion"].ToString()
                        };
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return cargo;
        }

        public List<Cargo> Listado(string Busqueda)
        {
            var listado = new List<Cargo>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_FiltradoCargo";
                    cmd.Parameters.AddWithValue("@Busquda", Busqueda == null ? (object)DBNull.Value : Busqueda);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        listado.Add(new Cargo()
                        {
                            IdCargo = Convert.ToInt32(reader["IdCargo"]),
                            NombreCargo = reader["NombreCargo"].ToString(),
                            Descripcion = reader["Descripcion"] == DBNull.Value ? null : reader["Descripcion"].ToString()
                        });
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return listado;
        }
    }
}
