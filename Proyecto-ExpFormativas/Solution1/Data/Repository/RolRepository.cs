using Data.Infraestructure;
using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Data.Repository
{
    public class RolRepository: IRol
    {
        private readonly string cadenaConexion;

        public RolRepository(IConfiguration config)
        {
            cadenaConexion = config["ConnectionStrings:database"] ?? string.Empty;
        }

        public int Agregar(Rol r)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_InsertarRol";
                    cmd.Parameters.AddWithValue("@NombreRol", r.NombreRol);
                    cmd.Parameters.AddWithValue("@Descripcion", r.Descripcion == null ? (object)DBNull.Value : r.Descripcion);
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

        public int Actualizar(Rol r)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_ActualizarRol";
                    cmd.Parameters.AddWithValue("@NombreRol", r.NombreRol);
                    cmd.Parameters.AddWithValue("@Descripcion", r.Descripcion == null ? (object)DBNull.Value : r.Descripcion);
                    cmd.Parameters.AddWithValue("@IdRol", r.IdRol);
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

        public Rol Detalle(int id)
        {
            var rol = new Rol();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                     SqlCommand cmd = new SqlCommand();
                     cmd.Connection = cn;
                     cmd.CommandType = CommandType.StoredProcedure;
                     cmd.CommandText = "sp_DetalleRol";
                     cn.Open();
                     SqlDataReader reader = cmd.ExecuteReader();
                     while (reader.Read())
                     {
                         rol = new Rol()
                         {
                             IdRol = Convert.ToInt32(reader["IdRol"]),
                             NombreRol = reader["NombreRol"].ToString(),
                             Descripcion = reader["Descripcion"] == DBNull.Value ? null : reader["Descripcion"].ToString()
                         };
                     }    
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return rol;
        }

        public List<Rol> Listado(string Busqueda)
        {
            var listado = new List<Rol>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_ListadoRol";
                    cmd.Parameters.AddWithValue("@Busqueda", Busqueda == null ? (object)DBNull.Value : Busqueda);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        listado.Add(new Rol
                        {
                            IdRol = Convert.ToInt32(reader["IdRol"]),
                            NombreRol = reader["NombreRol"].ToString(),
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
