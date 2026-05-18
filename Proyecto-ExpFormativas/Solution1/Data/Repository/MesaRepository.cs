using Data.Infraestructure;
using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Repository
{
    public class MesaRepository : IMesa
    {
        private readonly string cadenaConexion;

        public MesaRepository(IConfiguration config)
        {
            cadenaConexion = config["ConnectionStrings:database"] ?? string.Empty;
        }

        public int Actualizar(Mesa m)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_ActualizarMesa";
                    cmd.Parameters.AddWithValue("@NumeroMesa", m.NumeroMesa);
                    cmd.Parameters.AddWithValue("@EspacioOcupable", m.EspacioOcupable);
                    cmd.Parameters.AddWithValue("@IdMesa", m.IdMesa);
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

        public int Agregar(Mesa m)
        {
            int f = 0;
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_InsertarMesa";
                    cmd.Parameters.AddWithValue("@NumeroMesa", m.NumeroMesa);
                    cmd.Parameters.AddWithValue("@EspacioOcupable", m.EspacioOcupable);
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

        public Mesa Detalle(int id)
        {
            var mesa = new Mesa();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_DetalleMesa";
                    cmd.Parameters.AddWithValue("@IdMesa", id);
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        mesa = new Mesa()
                        {
                            IdMesa = Convert.ToInt32(reader["IdMesa"]),
                            NumeroMesa = Convert.ToInt32(reader["NumeroMesa"]),
                            EspacioOcupable = Convert.ToInt32(reader["EspacioOcupable"]),
                            Estado = Convert.ToBoolean(reader["Estado"])
                        };
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return mesa;
        }

        public List<Mesa> Listado()
        {
            var listado = new List<Mesa>();
            using (SqlConnection cn = new SqlConnection(cadenaConexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = cn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "sp_ListadoMesa";
                    cn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        listado.Add(new Mesa()
                        {
                            IdMesa = Convert.ToInt32(reader["IdMesa"]),
                            NumeroMesa = Convert.ToInt32(reader["NumeroMesa"]),
                            EspacioOcupable = Convert.ToInt32(reader["EspacioOcupable"]),
                            Estado = Convert.ToBoolean(reader["Estado"])
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

        #region
        public List<Mesa> Listado(string Busqueda)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
