using Data.Infraestructure;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Logic.Service
{
    public class CargoService
    {
        private readonly ICargo cargoDB;

        public CargoService(ICargo service)
        {
            cargoDB = service;
        }

        public int GestionarCargo(Cargo c)
        {
            if (c.IdCargo == 0)
                return cargoDB.Agregar(c);
            else
                return cargoDB.Actualizar(c);
        }

        public List<Cargo> ListadoCargo(string Busqueda)
        {
            return cargoDB.Listado(Busqueda);
        }

        public Cargo Detalle(int id)
        {
            return cargoDB.Detalle(id);
        }

    }
}
