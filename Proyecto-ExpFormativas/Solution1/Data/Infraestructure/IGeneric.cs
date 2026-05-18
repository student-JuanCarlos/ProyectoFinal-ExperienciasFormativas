using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Infraestructure
{
    public interface IGeneric<Entity> where Entity : class
    {

        public int Agregar(Entity entity);

        public int Actualizar(Entity entity);

        public List<Entity> Listado(string Busqueda);

        public Entity Detalle(int id);



    }
}
