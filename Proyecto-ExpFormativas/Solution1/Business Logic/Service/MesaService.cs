using Data.Infraestructure;
using Entities;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Business_Logic.Service
{
    public class MesaService
    {
        private readonly IMesa mesaDB;

        public MesaService(IMesa mesa)
        {
            mesaDB = mesa;
        }

        public int GestionarMesa(Mesa m)
        {
            if (m.IdMesa == 0)
                return mesaDB.Agregar(m);
            else
                return mesaDB.Actualizar(m);
        }

        public List<Mesa> ListadoMesa(string Busqueda)
        {
            return mesaDB.Listado(Busqueda);
        }

        public Mesa Detalle(int id)
        {
            return mesaDB.Detalle(id);
        }

        public void ActualizarEstadoMesasHoy()
        {

        }

    }
}
