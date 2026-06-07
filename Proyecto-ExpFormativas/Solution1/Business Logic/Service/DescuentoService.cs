using Data.Infraestructure;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Logic.Service
{
    public class DescuentoService
    {
        private readonly IDescuento descuentoDB;

        public DescuentoService(IDescuento service)
        {
            descuentoDB = service;
        }

        public int GestionarDescuento(Descuento d)
        {
            if (d.IdDescuento == 0)
                return descuentoDB.Agregar(d);
            else
                return descuentoDB.Actualizar(d);
        }

        public int CambiarEstadoDescuento(int id)
        {
            return descuentoDB.CambiarEstado(id);
        }

        public Descuento Detalle(int id)
        {
            return descuentoDB.Detalle(id);
        }

        public List<Descuento> ListadoDescuento(string Busqueda, bool? Estado)
        {
            return descuentoDB.Listado(Busqueda, Estado);
        }
    }
}
