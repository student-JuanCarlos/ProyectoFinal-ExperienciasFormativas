using Data.Infraestructure;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Logic.Service
{
    public class SuscripcionService : ISuscripcion
    {
        private readonly ISuscripcion suscripcionDB;

        public SuscripcionService(ISuscripcion service)
        {
            suscripcionDB = service;
        }

        public int Actualizar(Suscripcion s)
        {
            return suscripcionDB.Actualizar(s);
        }

        public int Agregar(Suscripcion s)
        {
            return suscripcionDB.Agregar(s);
        }

        public Suscripcion Detalle(int id)
        {
            return suscripcionDB.Detalle(id);
        }

        public Suscripcion ObtenerActivaPorRestaurante(int idRestaurante)
        {
            return suscripcionDB.ObtenerActivaPorRestaurante(idRestaurante);
        }
        #region
        public List<Suscripcion> Listado(string Busqueda)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
