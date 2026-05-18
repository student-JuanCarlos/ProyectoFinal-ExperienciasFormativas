using Data.Infraestructure;
using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public class ClienteRepository : ICliente 
    {
        private readonly string cadenaConexion;

        public ClienteRepository(IConfiguration config)
        {
            cadenaConexion = config["ConnectionStrings:database"] ?? string.Empty;
        }

        public int Actualizar(Cliente c)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_ActualizarCliente";
                    cmd.Parameters.AddWithValue("@Nombres", c.Nombres);
                    cmd.Parameters.AddWithValue("@Apellidos", c.Apellidos);
                    cmd.Parameters.AddWithValue("@Fotografia", c.Fotografia);
                    cmd.Parameters.AddWithValue("@Documento", c.Documento);
                    cmd.Parameters.AddWithValue("@Telefono", c.Telefono);
                    cmd.Parameters.AddWithValue("@Email", c.Email);
                    cmd.Parameters.AddWithValue("@Contraseña", c.Contraseña);
                    cmd.Parameters.AddWithValue("@IdCliente", c.IdCliente);
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

        public int Agregar(Cliente c)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_RegistrarCliente";
                    cmd.Parameters.AddWithValue("@Nombres", c.Nombres);
                    cmd.Parameters.AddWithValue("@Apellidos", c.Apellidos);
                    cmd.Parameters.AddWithValue("@Fotografia", c.Fotografia);
                    cmd.Parameters.AddWithValue("@Documento", c.Documento);
                    cmd.Parameters.AddWithValue("@Telefono", c.Telefono);
                    cmd.Parameters.AddWithValue("@Email", c.Email);
                    cmd.Parameters.AddWithValue("@Contraseña", c.Contraseña);
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

        public Cliente Detalle(int id)
        {
            var cliente = new Cliente();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_DetalleCliente";
                    cmd.Parameters.AddWithValue("@IdCliente", id);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        cliente = new Cliente()
                        {
                            IdCliente = Convert.ToInt32(reader["IdCliente"]),
                            NombreCompleto = reader["NombreCompleto"].ToString(),
                            Fotografia = reader["Fotografia"].ToString(),
                            Documento = reader["Documento"].ToString(),
                            Telefono = reader["Telefono"].ToString(),
                            Email = reader["Email"].ToString()
                        };
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return cliente;
        }

        public List<Cliente> Listado(string Busqueda) //Para el sistema interno, SOLO FILTRA CLIENTES CON RESERVAS DEL DIA DE HOY
        {
            var listado = new List<Cliente>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_FiltradoCliente";
                    cmd.Parameters.AddWithValue("@Busqueda", Busqueda);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        listado.Add(new Cliente()
                        {
                            IdCliente = Convert.ToInt32(reader["IdCliente"]),
                            NombreCompleto = reader["NombreCompleto"].ToString(),
                            Documento = reader["Documento"].ToString(),
                            Telefono = reader["Telefono"].ToString(),
                            Email = reader["Email"].ToString()
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

        public Cliente Login(string Email, string Contraseña)
        {
            var cliente = new Cliente();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_LoginCliente";
                    cmd.Parameters.AddWithValue("@Email", Email);
                    cmd.Parameters.AddWithValue("@Contraseña", Contraseña);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        cliente = new Cliente()
                        {
                            IdCliente = Convert.ToInt32(reader["IdCliente"]),
                            Nombres = reader["Nombres"].ToString(),
                            Apellidos = reader["Apellidos"].ToString(),
                            Documento = reader["Documento"].ToString(),
                            Telefono = reader["Telefono"].ToString(),
                            Email = reader["Email"].ToString()
                        };
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return cliente;
        }
    }
}
