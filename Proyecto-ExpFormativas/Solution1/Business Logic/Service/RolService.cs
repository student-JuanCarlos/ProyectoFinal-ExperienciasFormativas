using Data.Infraestructure;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Logic.Service
{
    public class RolService
    {
        private readonly IRol rolDB;

        public RolService(IRol service)
        {
            rolDB = service;
        }

        public int GestionarRol(Rol r)
        {
            if (r.IdRol == 0)
                return rolDB.Agregar(r);
            else
                return rolDB.Actualizar(r);
        }

        public List<Rol> Listado(string Busqueda)
        {
            return rolDB.Listado(Busqueda);
        }

        public Rol Detalle(int id)
        {
            return rolDB.Detalle(id);
        }
    }
}
