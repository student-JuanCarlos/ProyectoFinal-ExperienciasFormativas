using Data.Infraestructure;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Logic.Service
{
    public class PlatilloService
    {
        private readonly IPlatillo platilloDB;

        public PlatilloService(IPlatillo service)
        {
            platilloDB = service;
        }

        public int GestionarPlatillo(Platillo p)
        {
            if (p.IdPlatillo == 0)
                return platilloDB.Agregar(p);
            else
                return platilloDB.Actualizar(p);
        }

        public List<Platillo> ListadoPlatillo(string Busqueda)
        {
            return platilloDB.Listado(Busqueda);
        }

        public Platillo Detalle(int id)
        {
            return platilloDB.Detalle(id);
        }
    }
}
