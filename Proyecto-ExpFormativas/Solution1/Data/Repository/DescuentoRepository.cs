using Data.Infraestructure;
using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public class DescuentoRepository: IDescuento
    {
        private readonly string cadenaConexion;

        public DescuentoRepository(IConfiguration config)
        {
            cadenaConexion = config["ConnectionStrings:database"] ?? string.Empty;
        }

        public int Actualizar(Descuento d)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_ActualizarDescuento";
                    cmd.Parameters.AddWithValue("@NombreDescuento", d.NombreDescuento);
                    cmd.Parameters.AddWithValue("@TipoDescuento", d.TipoDescuento);
                    cmd.Parameters.AddWithValue("@PorcentajeDescuento", d.PorcentajeDescuento);
                    cmd.Parameters.AddWithValue("@FechaInicio", d.FechaInicio == null ? (object)DBNull.Value : d.FechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", d.FechaFin == null ? (object)DBNull.Value : d.FechaFin);
                    cmd.Parameters.AddWithValue("@ColorCard", d.ColorCard);
                    cmd.Parameters.AddWithValue("@IdDescuento", d.IdDescuento);
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

        public int ActualizarEstadoDescuentosHoy()
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_ActualizarEstadoDescuentosHoy";
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

        public int Agregar(Descuento d)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_InsertarDescuento";
                    cmd.Parameters.AddWithValue("@NombreDescuento", d.NombreDescuento);
                    cmd.Parameters.AddWithValue("@TipoDescuento", d.TipoDescuento);
                    cmd.Parameters.AddWithValue("@PorcentajeDescuento", d.PorcentajeDescuento);
                    cmd.Parameters.AddWithValue("@FechaInicio", d.FechaInicio == null ? (object)DBNull.Value : d.FechaInicio);
                    cmd.Parameters.AddWithValue("@FechaFin", d.FechaFin == null ? (object)DBNull.Value : d.FechaFin);
                    cmd.Parameters.AddWithValue("@ColorCard", d.ColorCard);
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
                    cmd.CommandText = "sp_CambiarEstadoDescuento";
                    cmd.Parameters.AddWithValue("@IdDescuento", id);
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

        public Descuento Detalle(int id)
        {
            var descuento = new Descuento();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_DetalleDescuento";
                    cmd.Parameters.AddWithValue("@IdDescuento", id);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        descuento = new Descuento()
                        {
                            NombreDescuento = reader["NombreDescuento"].ToString(),
                            TipoDescuento = reader["TipoDescuento"].ToString(),
                            PorcentajeDescuento = Convert.ToDecimal(reader["PorcentajeDescuento"]),
                            FechaInicio = reader["FechaInicio"] == DBNull.Value ? null : Convert.ToDateTime(reader["FechaInicio"]),
                            FechaFin = reader["FechaFin"] == DBNull.Value ? null : Convert.ToDateTime(reader["FechaFin"]),
                            ColorCard = reader["ColorCard"].ToString(),
                            Estado = Convert.ToBoolean(reader["Estado"])
                        };
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return descuento;
        }

        public List<Descuento> Listado(string Busqueda, bool? Estado)
        {
            var listado = new List<Descuento>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_FiltradoDescuento";
                    cmd.Parameters.AddWithValue("@Busqueda", Busqueda == null ? (object)DBNull.Value : Busqueda);
                    cmd.Parameters.AddWithValue("@Estado", Estado == null ? (object)DBNull.Value : Estado);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        listado.Add(new Descuento()
                        {
                            IdDescuento = Convert.ToInt32(reader["IdDescuento"]),
                            NombreDescuento = reader["NombreDescuento"].ToString(),
                            TipoDescuento = reader["TipoDescuento"].ToString(),
                            PorcentajeDescuento = Convert.ToDecimal(reader["PorcentajeDescuento"]),
                            ColorCard = reader["ColorCard"].ToString(),
                            Estado = Convert.ToBoolean(reader["Estado"])
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

        #region
        public List<Descuento> Listado(string Busqueda)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
