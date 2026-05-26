using Data.Infraestructure;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Logic.Service
{
    public class CategoriaService
    {
        private readonly ICategoria categoriaDB;

        public CategoriaService(ICategoria service)
        {
            categoriaDB = service;
        }

        public int GestionarCategoria(Categoria c)
        {
            if (c.IdCategoria == 0)
                return categoriaDB.Agregar(c);
            else
                return categoriaDB.Actualizar(c);
        }

        public List<Categoria> ListadoCategoria(string Buaqueda)
        {
            return categoriaDB.Listado(Buaqueda);
        }
    }
}
