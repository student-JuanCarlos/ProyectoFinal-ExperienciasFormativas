using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Infraestructure
{
    public interface IDescuento: IGeneric<Descuento>
    {
        public int CambiarEstado(int id);

        public List<Descuento> Listado(string Busqueda, bool? Estado);

        public int ActualizarEstadoDescuentosHoy();
    }
}
