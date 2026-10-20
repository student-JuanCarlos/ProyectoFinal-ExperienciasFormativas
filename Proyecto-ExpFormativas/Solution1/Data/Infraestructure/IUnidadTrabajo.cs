using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Infraestructure
{
    public interface IUnidadTrabajo
    {

        void Iniciar();

        void Confirmar();

        void Revertir();

    }
}
