using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Infraestructure
{
    public interface IVenta
    {
        public int RegistrarVenta(Venta v, List<DetalleVenta> d, List<DetalleDescuento> des);

        public Venta Detalle(int id);

        public List<Venta> Listado(string Busqueda);
    }
}
