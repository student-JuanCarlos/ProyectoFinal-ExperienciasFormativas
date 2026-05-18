using Data.Infraestructure;
using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public class CategoriaRepository : ICategoria
    {
        private readonly string cadenaConexion;

        public CategoriaRepository(IConfiguration config)
        {
            cadenaConexion = config["ConnectionStrings:database"] ?? string.Empty;
        }

        public int Actualizar(Categoria c)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_ActualizarCategoria";
                    cmd.Parameters.AddWithValue("@NombreCategoria", c.NombreCategoria);
                    cmd.Parameters.AddWithValue("@Descripcion", c.Descripcion == null ? (object)DBNull.Value : c.Descripcion);
                    cmd.Parameters.AddWithValue("@IdCategoria", c.IdCategoria);
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

        public int Agregar(Categoria c)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_InsertarCategoria";
                    cmd.Parameters.AddWithValue("@NombreCategoria", c.NombreCategoria);
                    cmd.Parameters.AddWithValue("@Descripcion", c.Descripcion == null ? (object)DBNull.Value : c.Descripcion);
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

        public Categoria Detalle(int id)
        {
            var categoria = new Categoria();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_DetalleCategoria";
                    cmd.Parameters.AddWithValue("@IdCategoria", id);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        categoria = new Categoria()
                        {
                            IdCategoria = Convert.ToInt32(reader["IdCategoria"]),
                            NombreCategoria = reader["NombreCategoria"].ToString(),
                            Descripcion = reader["Descripcion"] == DBNull.Value ? null : reader["Descripcion"].ToString()
                        };
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return categoria;
        }

        public List<Categoria> Listado(string Busqueda)
        {
            var listado = new List<Categoria>();
            using (SqlConnection cn = new SqlConnection())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_DetalleCategoria";
                    cmd.Parameters.AddWithValue("@Busqueda", Busqueda == null ? (object)DBNull.Value : Busqueda);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        listado.Add(new Categoria()
                        {
                            IdCategoria = Convert.ToInt32(reader["IdCategoria"]),
                            NombreCategoria = reader["NombreCategoria"].ToString(),
                            Descripcion = reader["Descripcion"] == DBNull.Value ? null : reader["Descripcion"].ToString()
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
    }
}
