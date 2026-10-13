using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Infraestructure
{
    public interface IOrdenPago: IGeneric<OrdenPago>
    {

        public OrdenPago ObtenerPorReferenciaExterna(string referencia);

        public OrdenPago ObtenerPendienteReciente(int idRestaurante);

    }
}
