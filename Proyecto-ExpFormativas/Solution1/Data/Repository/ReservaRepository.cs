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
                    cmd.Parameters.AddWithValue("@TipoReserva", r.TipoReserva);
                    cmd.Parameters.AddWithValue("@FechaReserva", r.FechaReserva);
                    cmd.Parameters.AddWithValue("@HoraReserva", r.HoraReserva);
                    cmd.Parameters.AddWithValue("@CantidadPersonas", r.CantidadPersonas);
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
                    tvp.TypeName = "TVP_,Mesas";
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

                        Cliente cliente = new Cliente()
                        {
                            Nombres = reader["GeneradoPor"].ToString()
                        };

                        reserva = new Reserva()
                        {
                            TipoReserva = reader["TipoReserva"].ToString(),
                            FechaReserva = Convert.ToDateTime(reader["FechaReserva"]),
                            HoraReserva = (TimeSpan)reader["HoraReserva"],
                            CantidadPersonas = Convert.ToInt32(reader["CantidadPersonas"]),
                            CostoTotal = Convert.ToDecimal(reader["CostoTotal"]),
                            usuario = usuario,
                            cliente = cliente
                        };

                        reader.NextResult();
                        if (reader.Read())
                        {
                            reserva.DetalleMesa.Add(new Mesa
                            {
                                NumeroMesa = Convert.ToInt32(reader["NumeroMesa"])
                            });
                        }
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
                    cmd.Parameters.AddWithValue("@FechaReserva", r.FechaReserva);
                    cmd.Parameters.AddWithValue("@HoraReserva", r.HoraReserva);
                    cmd.Parameters.AddWithValue("@CantidadPersonas", r.CantidadPersonas);
                    cmd.Parameters.AddWithValue("@IdUsuario", r.IdUsuario == null ? (object)DBNull.Value : r.IdUsuario);
                    cn.Open();
                    f = cmd.ExecuteNonQuery();

                    SqlParameter tvp = cmd.Parameters.AddWithValue("@Mesas", detalle);
                    tvp.SqlDbType = SqlDbType.Structured;
                    tvp.TypeName = "TVP_Mesas";
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return f;
        }

        public List<Reserva> Listado(string Busqueda)
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
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Usuario usuario = new Usuario()
                        {
                            NombreUsuario = reader["GeneradoPor"].ToString()
                        };

                        Cliente cliente = new Cliente()
                        {
                            Nombres = reader["GeneradoPor"].ToString()
                        };

                        listado.Add(new Reserva
                        {
                            TipoReserva = reader["TipoReserva"].ToString(),
                            FechaReserva = Convert.ToDateTime(reader["FechaReserva"]),
                            CostoTotal = Convert.ToDecimal(reader["CostoTotal"]),
                            usuario = usuario,
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
    }
}
