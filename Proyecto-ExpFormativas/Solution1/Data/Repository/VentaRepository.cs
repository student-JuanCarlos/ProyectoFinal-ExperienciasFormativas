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

                    foreach(var d in descuentos)
                    {
                        ddesc.Rows.Add(d.IdDescuento, d.DescuentoUnitario);
                    }

                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "sp_RegistrarVenta";
                    cmd.Parameters.AddWithValue("@IdReserva", v.IdReserva);
                    cmd.Parameters.AddWithValue("@IdUsuario", v.IdUsuario);
                    cmd.Parameters.AddWithValue("@MetodoPago", v.MetodoPago);

                    SqlParameter tvpd = cmd.Parameters.AddWithValue("@Detalle", dventa);
                    tvpd.SqlDbType = SqlDbType.Structured;
                    tvpd.TypeName = "TVP_DetalleVenta";

                    SqlParameter tvpdes = cmd.Parameters.AddWithValue("@Descuento", ddesc);
                    tvpdes.SqlDbType = SqlDbType.Structured;
                    tvpdes.TypeName = "TVP_DetalleDescuento";

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
                            NombreCompleto = reader["NombreCompleto"].ToString()
                        };

                        Reserva reserva = new Reserva()
                        {
                            NombreCliente = reader["NombreCompleto"].ToString()
                        };

                        listado.Add(new Venta
                        {
                            IdVenta = Convert.ToInt32(reader["IdVenta"]),
                            reserva = reserva,
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
                            NombreCompleto = reader["NombreCompleto"] == DBNull.Value ? null : reader["NombreCompleto"].ToString(),
                            Email = reader["Contacto"].ToString(),
                        };

                        Reserva reserva = new Reserva()
                        {
                            NombreCliente = reader["NombreCompleto"] == DBNull.Value ? null : reader["NombreCompleto"].ToString(),
                            TelefonoCliente = reader["Contacto"] == DBNull.Value ? null : reader["Contacto"].ToString(),
                            TipoReserva = reader["TipoReserva"].ToString(),
                            CantidadPersonas = Convert.ToInt32(reader["CantidadPersonas"]),
                            CostoTotal = Convert.ToDecimal(reader["CostoTotal"]),
                            cliente = cliente
                        };

                        Usuario usuario = new Usuario()
                        {
                            NombreUsuario = reader["NombreUsuario"].ToString()
                        };

                        venta = new Venta()
                        {
                            reserva = reserva,
                            usuario = usuario,
                            FechaVenta = Convert.ToDateTime(reader["FechaVenta"]),
                            MetodoPago = reader["MetodoPago"].ToString(),
                            Total = Convert.ToDecimal(reader["Total"]),
                            detalles = new List<DetalleVenta>(),
                            descuentos = new List<DetalleDescuento>()
                        };

                        reader.NextResult();
                        while (reader.Read())
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
                        while (reader.Read())
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
