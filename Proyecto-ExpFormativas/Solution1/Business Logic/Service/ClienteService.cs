using Data.Infraestructure;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business_Logic.Service
{
    public class ClienteService
    {
        private readonly ICliente clienteDB;

        public ClienteService(ICliente service)
        {
            clienteDB = service;
        }

        public int GestionarCliente(Cliente c)
        {
            if (c.IdCliente == 0)
                return clienteDB.Agregar(c);
            else
                return clienteDB.Actualizar(c);
        }

        public List<Cliente> ListadoClienteHoy(string Busqueda)
        {
            return clienteDB.Listado(Busqueda);
        }

        public Cliente Detalle(int id)
        {
            return clienteDB.Detalle(id);
        }

        public Cliente LoginCliente(string Email, string Contraseña)
        {
            return clienteDB.Login(Email, Contraseña);
        }
    }
}
