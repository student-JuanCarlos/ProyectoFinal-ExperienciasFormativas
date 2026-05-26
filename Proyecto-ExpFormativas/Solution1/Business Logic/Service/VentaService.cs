using Data.Infraestructure;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Logic.Service
{
    public class VentaService
    {
        private readonly IVenta ventaDB;

        public VentaService(IVenta service)
        {
            ventaDB = service;
        }

        public int RegistrarVenta(Venta v, List<DetalleVenta> d, List<DetalleDescuento> des)
        {
            return ventaDB.RegistrarVenta(v, d, des);
        }

        public Venta Detalle(int id)
        {
            return ventaDB.Detalle(id);
        }

        public List<Venta> Listado(string Busqueda)
        {
            return ventaDB.Listado(Busqueda);
        }
    }
}
