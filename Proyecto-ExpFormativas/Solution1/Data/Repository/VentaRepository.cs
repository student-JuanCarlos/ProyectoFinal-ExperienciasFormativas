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
    public class VentaRepository: IVenta
    {
        private readonly string cadenaConexion;

        public VentaRepository(IConfiguration config)
        {
            cadenaConexion = config["ConnectionStrings:database"] ?? string.Empty;
        }

        public int RegistrarVenta(Venta v, List<DetalleVenta> detalle, List<DetalleDescuento> descuentos)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    DataTable dventa = new DataTable();
                    dventa.Columns.Add("IdPlatillo", typeof(int));
                    dventa.Columns.Add("Cantidad", typeof(int));

                    foreach (var d in detalle)
                    {
                        dventa.Rows.Add(d.IdPlatillo, d.Cantidad);
                    }

                    DataTable ddesc = new DataTable();
                    ddesc.Columns.Add("IdDescuento", typeof(int));
                    ddesc.Columns.Add("PorcentajeAplicado", typeof(decimal));


                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_RegistrarVenta";
                    cmd.Parameters.AddWithValue("@IdCliente", v.IdCliente == null ? (object)DBNull.Value : v.IdCliente);
                    cmd.Parameters.AddWithValue("@IdReserva", v.IdReserva);
                    cmd.Parameters.AddWithValue("@IdUsuario", v.IdUsuario);
                    cmd.Parameters.AddWithValue("@NombreCliente", v.NombreCliente == null ? (object)DBNull.Value : v.NombreCliente);
                    cmd.Parameters.AddWithValue("@MetodoPago", v.MetodoPago);
                    cn.Open();
                    f = cmd.ExecuteNonQuery();


                    SqlParameter tvpd = cmd.Parameters.AddWithValue("@Detalle", dventa);
                    tvpd.SqlDbType = SqlDbType.Structured;
                    tvpd.TypeName = "TVP_DetalleVenta";

                    SqlParameter tvpdes = cmd.Parameters.AddWithValue("@Descuento", ddesc);
                    tvpdes.SqlDbType = SqlDbType.Structured;
                    tvpdes.TypeName = "TVP_DetalleDescuento";
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return f;
        }

        public List<Venta> Listado(string Busqueda)
        {
            var listado = new List<Venta>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_FiltradoVentas";
                    cmd.Parameters.AddWithValue("@Busqueda", Busqueda == null ? (object)DBNull.Value : Busqueda);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        Cliente cliente = new Cliente()
                        {
                            IdCliente = Convert.ToInt32(reader["IdCliente"]),
                            Nombres = reader["Nombres"].ToString(),
                            Apellidos = reader["Apellidos"].ToString()
                        };

                        listado.Add(new Venta
                        {
                            IdVenta = Convert.ToInt32(reader["IdVenta"]),
                            NombreCliente = reader["NombreCompleto"] == DBNull.Value ? null : reader["NombreCompleto"].ToString(),
                            cliente = cliente,
                            FechaVenta = Convert.ToDateTime(reader["FechaVenta"]),
                            MetodoPago = reader["MetodoPago"].ToString(),
                            Total = Convert.ToDecimal(reader["Total"])
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

        public Venta Detalle(int id)
        {
            var venta = new Venta();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_DetalleVenta";
                    cmd.Parameters.AddWithValue("@IdVenta", id);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        Cliente cliente = new Cliente()
                        {
                            IdCliente = Convert.ToInt32(reader["IdCliente"]),
                            Nombres = reader["Nombres"].ToString(),
                            Apellidos = reader["Apellidos"].ToString(),
                            Email = reader["Email"].ToString(),
                        };

                        Reserva reserva = new Reserva()
                        {
                            TipoReserva = reader["TipoReserva"].ToString(),
                            CantidadPersonas = Convert.ToInt32(reader["CantidadPersonas"]),
                            CostoTotal = Convert.ToDecimal(reader["CostoReserva"])
                        };

                        Usuario usuario = new Usuario()
                        {
                            NombreUsuario = reader["NombreUsuario"].ToString()
                        };

                        venta = new Venta()
                        {
                            NombreCliente = reader["NombreCompleto"] == DBNull.Value ? null : reader["NombreCompleto"].ToString(),
                            cliente = cliente,
                            reserva = reserva,
                            usuario = usuario,
                            FechaVenta = Convert.ToDateTime(reader["FechaVenta"]),
                            MetodoPago = reader["MetodoPago"].ToString(),
                            Total = Convert.ToDecimal(reader["Total"]),
                            
                        };

                        reader.NextResult();
                        if (reader.Read())
                        {
                            Platillo platillo = new Platillo()
                            {
                                NombrePlatillo = reader["NombrePlatillo"].ToString()
                            };

                            venta.detalles.Add(new DetalleVenta
                            {
                                Cantidad = Convert.ToInt32(reader["Cantidad"]),
                                PrecioUnitario = Convert.ToDecimal(reader["PrecioUnitario"]),
                                platillo = platillo
                            });
                        }

                        reader.NextResult();
                        if (reader.Read())
                        {
                            Descuento descuento = new Descuento()
                            {
                                NombreDescuento = reader["NombreDescuento"].ToString(),
                                ColorCard = reader["ColorCard"].ToString()
                            };

                            venta.descuentos.Add(new DetalleDescuento
                            {
                                DescuentoUnitario = Convert.ToDecimal(reader["DescuentoUnitario"]),
                                descuento = descuento
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return venta;
        }

    }
}
