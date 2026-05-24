using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Data.Repository
{
    public class PlatilloRepository
    {
        private readonly string cadenaConexion;

        public PlatilloRepository(IConfiguration config)
        {
            cadenaConexion = config["ConnectionStrings:database"] ?? string.Empty;
        }

        public int Agregar(Platillo p)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_InsertarPlatillo";
                    cmd.Parameters.AddWithValue("@NombrePlatillo", p.NombrePlatillo);
                    cmd.Parameters.AddWithValue("@Fotografia", p.Fotografia);
                    cmd.Parameters.AddWithValue("@Precio", p.Precio);
                    cmd.Parameters.AddWithValue("@IdCategoria", p.IdCategoria);
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

        public int Actualizar(Platillo p)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_ActualizarPlatillo";
                    cmd.Parameters.AddWithValue("@NombrePlatillo", p.NombrePlatillo);
                    cmd.Parameters.AddWithValue("@Fotografia", p.Fotografia);
                    cmd.Parameters.AddWithValue("@Precio", p.Precio);
                    cmd.Parameters.AddWithValue("@IdCategoria", p.IdCategoria);
                    cmd.Parameters.AddWithValue("@IdPlatillo", p.IdPlatillo);
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

        public List<Platillo> Listado(string Busqueda)
        {
            var listado = new List<Platillo>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_FiltradoPlatillo";
                    cmd.Parameters.AddWithValue("@Busqueda", Busqueda == null ? (object)DBNull.Value : Busqueda);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Categoria categoria = new Categoria()
                        {
                            NombreCategoria = reader["NombreCategoria"].ToString()
                        };

                        listado.Add(new Platillo()
                        {
                            IdPlatillo = Convert.ToInt32(reader["IdPlatillo"]),
                            NombrePlatillo = reader["NombrePlatillo"].ToString(),
                            Fotografia = reader["Fotografia"].ToString(),
                            Precio = Convert.ToDecimal(reader["Precio"]),
                            categoria = categoria
                        });
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return listado;
        }

        public Platillo Detalle(int id)
        {
            var platillo = new Platillo();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_DetallePlatillo";
                    cmd.Parameters.AddWithValue("@IdPlatillo", id);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        Categoria categoria = new Categoria()
                        {
                            IdCategoria = Convert.ToInt32(reader["IdCategoria"]),
                            NombreCategoria = reader["NombreCategoria"].ToString()
                        };

                        platillo = new Platillo()
                        {
                            IdPlatillo = Convert.ToInt32(reader["IdPlatillo"]),
                            NombrePlatillo = reader["NombrePlatillo"].ToString(),
                            IdCategoria = Convert.ToInt32(reader["IdCategoria"]),
                            Fotografia = reader["Fotografia"].ToString(),
                            Precio = Convert.ToDecimal(reader["Precio"]),
                            categoria = categoria
                        };
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return platillo;
        }

    }
}
