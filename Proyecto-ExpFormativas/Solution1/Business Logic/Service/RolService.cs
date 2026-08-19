using Data.Infraestructure;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Logic.Service
{
    public class RolService
    {
        private readonly IRol rolDB;

        public RolService(IRol service)
        {
            rolDB = service;
        }

        public List<Rol> Listado()
        {
            return rolDB.Listado();
        }
    }
}
