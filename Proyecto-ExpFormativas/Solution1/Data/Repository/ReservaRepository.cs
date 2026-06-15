using Data.Infraestructure;
using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Data.Repository
{
    public class ReservaRepository: IReserva
    {
        private readonly string cadenaConexion;

        public ReservaRepository(IConfiguration config)
        {
            cadenaConexion = config["ConnectionStrings:database"] ?? string.Empty;
        }

        public int ActualizarMesas(int IdReserva, DataTable mesas)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_ActualizarMesas";
                    cmd.Parameters.AddWithValue("@IdReserva", IdReserva);

                    SqlParameter tvp = cmd.Parameters.Add("@Mesas", SqlDbType.Structured);
                    tvp.TypeName = "TVP_Mesas";
                    tvp.Value = mesas;

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

        public int ActualizarReserva(Reserva r)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_ActualizarReserva";
                    cmd.Parameters.AddWithValue("@IdReserva", r.IdReserva);
                    cmd.Parameters.AddWithValue("@NombreCliente", r.NombreCliente);
                    cmd.Parameters.AddWithValue("@TelefonoCliente", r.TelefonoCliente);
                    cmd.Parameters.AddWithValue("@CantidadPersonas", r.CantidadPersonas);
                    cn.Open();
                    f = cmd.ExecuteNonQuery();
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
            return f;
        }

        public int ActualizarReserva_Cliente(Reserva r)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_ActualizarReserva_Cliente";
                    cmd.Parameters.AddWithValue("@IdReserva", r.IdReserva);
                    cmd.Parameters.AddWithValue("@FechaReserva", r.FechaReserva);
                    cmd.Parameters.AddWithValue("@HoraReserva", r.HoraReserva);
                    cmd.Parameters.AddWithValue("@CantidadPersonas", r.CantidadPersonas);
                    cn.Open();
                    f = cmd.ExecuteNonQuery();
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
            }
            return f;
        }

        public int CancelarReserva(int IdReserva, DataTable mesas)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_CancelarReserva";
                    cmd.Parameters.AddWithValue("@IdReserva", IdReserva);

                    SqlParameter tvp = cmd.Parameters.Add("@Mesas", SqlDbType.Structured);
                    tvp.TypeName = "TVP_Mesas";
                    tvp.Value = mesas;

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

        public Reserva Detalle(int id)
        {
            var reserva = new Reserva();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_DetalleReserva";
                    cmd.Parameters.AddWithValue("@IdReserva", id);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Usuario usuario = new Usuario()
                        {
                            NombreUsuario = reader["GeneradoPor"].ToString()
                        };

                        reserva = new Reserva()
                        {
                            TipoReserva = reader["TipoReserva"].ToString(),
                            FechaReserva = Convert.ToDateTime(reader["FechaReserva"]),
                            HoraReserva = (TimeSpan)reader["HoraReserva"],
                            CantidadPersonas = Convert.ToInt32(reader["CantidadPersonas"]),
                            CostoTotal = Convert.ToDecimal(reader["CostoTotal"]),
                            DetalleMesa = new List<DetalleReserva>(),
                            usuario = usuario
                        };
                    }

                    reader.NextResult();
                    while (reader.Read())
                    {
                        reserva.DetalleMesa.Add(new DetalleReserva()
                        {
                            mesa = new Mesa { NumeroMesa = Convert.ToInt32(reader["NumeroMesa"]) }
                        });
                    }

                    reader.NextResult();
                    if (reader.Read())
                    {
                        reserva.cliente = new Cliente()
                        {
                            IdCliente = Convert.ToInt32(reader["IdCliente"]),
                            Nombres = reader["NombreCompleto"].ToString(),
                            Fotografia = reader["Fotografia"] == DBNull.Value ? null : reader["Fotografia"].ToString(),
                            Telefono = reader["Telefono"].ToString(),
                            Email = reader["Email"].ToString(),
                            Documento = reader["Documento"].ToString()
                        };
                    }
                }
                catch (Exception ex) 
                { 
                    throw new Exception(ex.Message); 
                }
            }
            return reserva;
        }

        public Reserva DetalleReserva_Cliente(int id)
        {
            var reserva = new Reserva();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_DetalleReserva_Cliente";
                    cmd.Parameters.AddWithValue("@IdReserva", id);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        reserva = new Reserva()
                        {
                            IdReserva = Convert.ToInt32(reader["IdReserva"]),
                            FechaReserva = Convert.ToDateTime(reader["FechaReserva"]),
                            HoraReserva = (TimeSpan)reader["HoraReserva"],
                            CantidadPersonas = Convert.ToInt32(reader["CantidadPersonas"]),
                            CostoTotal = Convert.ToDecimal(reader["CostoTotal"]),
                            Estado = Convert.ToInt32(reader["Estado"]),
                            DetalleMesa = new List<DetalleReserva>(),
                        };
                    }

                    reader.NextResult();
                    while (reader.Read())
                    {
                        var mesa = new Mesa()
                        {
                            IdMesa = Convert.ToInt32(reader["IdMesa"]),
                            NumeroMesa = Convert.ToInt32(reader["NumeroMesa"])
                        };

                        reserva.DetalleMesa.Add(new DetalleReserva()
                        {
                            mesa = mesa
                        });
                    }
                }
                catch (Exception ex) 
                { 
                    throw new Exception(ex.Message); 
                }
            }
            return reserva;
        }

        public int InsertarReserva(Reserva r, List<DetalleReserva> detalle)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("IdMesa", typeof(int));

                    foreach(var d in detalle)
                    {
                        dt.Rows.Add(d.IdMesa);
                    }

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_InsertarReserva";
                    cmd.Parameters.AddWithValue("@IdCliente", r.IdCliente == null ? (object)DBNull.Value : r.IdCliente);
                    cmd.Parameters.AddWithValue("@TipoReserva", r.TipoReserva);
                    cmd.Parameters.AddWithValue("@NombreCliente", r.NombreCliente == null ? (object)DBNull.Value : r.NombreCliente);
                    cmd.Parameters.AddWithValue("@TelefonoCliente", r.TelefonoCliente == null ? (object)DBNull.Value : r.TelefonoCliente);
                    cmd.Parameters.AddWithValue("@FechaReserva", r.FechaReserva == null ? (object)DBNull.Value : r.FechaReserva);
                    cmd.Parameters.AddWithValue("@HoraReserva", r.HoraReserva == null ? (object)DBNull.Value : r.HoraReserva);
                    cmd.Parameters.AddWithValue("@CantidadPersonas", r.CantidadPersonas);
                    cmd.Parameters.AddWithValue("@IdUsuario", r.IdUsuario == null ? (object)DBNull.Value : r.IdUsuario);

                    SqlParameter tvp = cmd.Parameters.AddWithValue("@Mesas", dt);
                    tvp.SqlDbType = SqlDbType.Structured;
                    tvp.TypeName = "TVP_Mesas";

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

        public List<Reserva> Listado(string Busqueda, int? Estado)
        {
            var listado = new List<Reserva>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_FiltradoReservas";
                    cmd.Parameters.AddWithValue("@Busqueda", Busqueda == null ? (object)DBNull.Value : Busqueda);
                    cmd.Parameters.AddWithValue("@Estado", Estado == null ? (object)DBNull.Value : Estado);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        Cliente cliente = new Cliente()
                        {
                            NombreCompleto = reader["Cliente"].ToString()
                        };

                        listado.Add(new Reserva
                        {
                            IdReserva = Convert.ToInt32(reader["IdReserva"]),
                            TipoReserva = reader["TipoReserva"].ToString(),
                            NombreCliente = reader["Cliente"] == DBNull.Value ? null : reader["Cliente"].ToString(),
                            FechaReserva = Convert.ToDateTime(reader["FechaReserva"]),
                            HoraReserva = (TimeSpan)reader["HoraReserva"],
                            CantidadPersonas = Convert.ToInt32(reader["CantidadPersonas"]),
                            CostoTotal = Convert.ToDecimal(reader["CostoTotal"]),
                            Estado = Convert.ToInt32(reader["Estado"]),
                            cliente = cliente
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

        public List<Reserva> ListadoReserva_Cliente(int IdCliente)
        {
            var listado = new List<Reserva>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_ListadoReserva_Cliente";
                    cmd.Parameters.AddWithValue("@IdCliente", IdCliente);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        listado.Add(new Reserva()
                        {
                            IdReserva = Convert.ToInt32(reader["IdReserva"]),
                            FechaReserva = Convert.ToDateTime(reader["FechaReserva"]),
                            HoraReserva = (TimeSpan)reader["HoraReserva"],
                            CantidadPersonas = Convert.ToInt32(reader["CantidadPersonas"]),
                            CostoTotal = Convert.ToDecimal(reader["CostoTotal"]),
                            Estado = Convert.ToInt32(reader["Estado"]),
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
