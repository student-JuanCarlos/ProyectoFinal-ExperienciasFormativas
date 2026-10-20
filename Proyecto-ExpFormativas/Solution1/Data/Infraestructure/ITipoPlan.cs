using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Infraestructure
{
    public interface ITipoPlan
    {

        public List<TipoPlan> ListarPlanesActivos();

        public TipoPlan Detalle(int id);

    }
}
