using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Infraestructure
{
    public interface IMesa:IGeneric<Mesa>
    {
        public List<Mesa> Listado(string Busqueda);
    }
}
