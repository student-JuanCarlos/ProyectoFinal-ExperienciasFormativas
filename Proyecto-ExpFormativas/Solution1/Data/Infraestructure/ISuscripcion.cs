using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Infraestructure
{
    public interface ISuscripcion: IGeneric<Suscripcion>
    {

        public Suscripcion ObtenerActivaPorRestaurante(int idRestaurante);

    }
}
