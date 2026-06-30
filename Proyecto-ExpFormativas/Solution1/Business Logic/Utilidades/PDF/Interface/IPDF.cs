using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Logic.Utilidades.PDF.NewFolder
{
    public interface IPDF<Entity>
    {
        public (byte[] Bytes, string Nombre) ObtenerArchivo(Entity e);

    }
}
