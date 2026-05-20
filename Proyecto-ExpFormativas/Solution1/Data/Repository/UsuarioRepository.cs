using Data.Infraestructure;
using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.Pkcs;
using System.Text;

namespace Data.Repository
{
    public class UsuarioRepository: IUsuario
    {
        private readonly string cadenaConexion;

        public UsuarioRepository(IConfiguration config)
        {
            cadenaConexion = config["ConnectionStrings:database"] ?? string.Empty;
        }

        public int Actualizar(Usuario u)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_ActualizarUsuario";
                    cmd.Parameters.AddWithValue("@NombreUsuario", u.NombreUsuario);
                    cmd.Parameters.AddWithValue("@Documento", u.Documento);
                    cmd.Parameters.AddWithValue("@Telefono", u.Telefono);
                    cmd.Parameters.AddWithValue("@Email", u.Email);
                    cmd.Parameters.AddWithValue("@Contraseña", u.Contraseña);
                    cmd.Parameters.AddWithValue("@Sueldo", u.Sueldo);
                    cmd.Parameters.AddWithValue("@IdCargo", u.IdCargo);
                    cmd.Parameters.AddWithValue("@IdRol", u.IdRol);
                    cmd.Parameters.AddWithValue("@IdUsuario", u.IdUsuario);
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

        public int Agregar(Usuario u)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_RegistrarUsuario";
                    cmd.Parameters.AddWithValue("@NombreUsuario", u.NombreUsuario);
                    cmd.Parameters.AddWithValue("@Documento", u.Documento);
                    cmd.Parameters.AddWithValue("@Telefono", u.Telefono);
                    cmd.Parameters.AddWithValue("@Email", u.Email);
                    cmd.Parameters.AddWithValue("@Contraseña", u.Contraseña);
                    cmd.Parameters.AddWithValue("@Sueldo", u.Sueldo);
                    cmd.Parameters.AddWithValue("@IdCargo", u.IdCargo);
                    cmd.Parameters.AddWithValue("@IdRol", u.IdRol);
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

        public int CambiarEstado(int id)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_CambiarEstadoUsuario";
                    cmd.Parameters.AddWithValue("@IdUsuario", id);
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

        public Usuario Detalle(int id)
        {
            var usuario = new Usuario();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_DetalLeUsuario";
                    cmd.Parameters.AddWithValue("@IdUsuario", id);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Cargo cargo = new Cargo()
                        {
                            IdCargo = Convert.ToInt32(reader["IdCargo"]),
                            NombreCargo = reader["NombreCargo"].ToString()
                        };

                        Rol rol = new Rol()
                        {
                            IdRol = Convert.ToInt32(reader["IdRol"]),
                            NombreRol = reader["NombreRol"].ToString()
                        };

                        usuario = new Usuario()
                        {
                            IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                            IdCargo = Convert.ToInt32(reader["IdCargo"]),
                            IdRol = Convert.ToInt32(reader["IdRol"]),
                            NombreUsuario = reader["NombreUsuario"].ToString(),
                            Documento = reader["Documento"].ToString(),
                            Telefono = reader["Telefono"].ToString(),
                            FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"]),
                            cargo = cargo,
                            rol = rol,
                            Email = reader["Email"].ToString(),
                            Sueldo = Convert.ToDecimal(reader["Sueldo"]),
                            Estado = Convert.ToBoolean(reader["Estado"])
                        };
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return usuario;
        }

        public List<Usuario> Listado(string Busqueda, bool Estado)
        {
            var listado = new List<Usuario>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_FiltradoUsuario";
                    cmd.Parameters.AddWithValue("@Busqueda", Busqueda == null ? (object)DBNull.Value : Busqueda);
                    cmd.Parameters.AddWithValue("@Estado", Estado == null ? (object)DBNull.Value : Estado);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Cargo cargo = new Cargo()
                        {
                            NombreCargo = reader["NombreCargo"].ToString()
                        };

                        Rol rol = new Rol()
                        {
                            NombreRol = reader["NombreRol"].ToString()
                        };

                        listado.Add(new Usuario
                        {
                            IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                            NombreUsuario = reader["NombreUsuario"].ToString(),
                            Telefono = reader["Telefono"].ToString(),
                            Email = reader["Email"].ToString(),
                            Estado = Convert.ToBoolean(reader["Estado"]),
                            cargo = cargo,
                            rol = rol
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

        public Usuario Login(string Email, string Contraseña)
        {
            var usuario = new Usuario();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_LoginUsuario";
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@Contraseña", Contraseña);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        Cargo cargo = new Cargo()
                        {
                            IdCargo = Convert.ToInt32(reader["IdCargo"]),
                            NombreCargo = reader["NombreCargo"].ToString()
                        };

                        Rol rol = new Rol()
                        {
                            IdRol = Convert.ToInt32(reader["IdRol"]),
                            NombreRol = reader["NombreRol"].ToString()
                        };

                        usuario = new Usuario()
                        {
                            IdUsuario = Convert.ToInt32(reader["IdUsuario"]),
                            IdCargo = Convert.ToInt32(reader["IdCargo"]),
                            IdRol = Convert.ToInt32(reader["IdRol"]),
                            NombreUsuario = reader["NombreUsuario"].ToString(),
                            Telefono = reader["Telefono"].ToString(),
                            cargo = cargo,
                            rol = rol,
                            Email = reader["Email"].ToString(),
                            Estado = Convert.ToBoolean(reader["Estado"])
                        };
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return usuario;
        }

        #region
        public List<Usuario> Listado(string Busqueda)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
