using Data.Infraestructure;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Logic.Service
{
    public class OrdenPagoService : IOrdenPago
    {
        private readonly IOrdenPago ordenpagoDB;

        public OrdenPagoService(IOrdenPago service)
        {
            ordenpagoDB = service;
        }

        public int Actualizar(OrdenPago o)
        {
            return ordenpagoDB.Actualizar(o);
        }

        public int Agregar(OrdenPago o)
        {
            return ordenpagoDB.Agregar(o);
        }

        public OrdenPago Detalle(int id)
        {
            return ordenpagoDB.Detalle(id);
        }

        public OrdenPago ObtenerPendienteReciente(int idRestaurante, int idTipoPlan)
        {
            return ordenpagoDB.ObtenerPendienteReciente(idRestaurante, idTipoPlan);
        }

        public OrdenPago ObtenerPorReferenciaExterna(string referencia)
        {
            return ordenpagoDB.ObtenerPorReferenciaExterna(referencia);
        }

        #region
        public List<OrdenPago> Listado(string Busqueda)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
