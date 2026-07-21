using Data.Context;
using Data.Infraestructure;
using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Text;

namespace Data.Repository
{
    public class ClienteRepository : ICliente 
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public int Actualizar(Cliente c)
        {
            var cliente = _context.Clientes.Find(c.IdCliente);

            if(cliente == null)
            {
                return 0;
            }

            cliente.Nombres = c.Nombres;
            cliente.Apellidos = c.Apellidos;
            cliente.Fotografia = c.Fotografia;
            cliente.Documento = c.Documento;
            cliente.Telefono = c.Telefono;
            cliente.Email = c.Email;

            return _context.SaveChanges();
        }

        public int Agregar(Cliente c)
        {
            _context.Clientes.Add(c);
            return _context.SaveChanges();
        }

        public Cliente Detalle(int id)
        {
            return _context.Clientes.Find(id);
        }

        public Cliente Login(string Email, string Contraseña)
        {
            return _context.Clientes
                .AsNoTracking()
                .Where(c => c.Email == Email && c.Contraseña == Contraseña)
                .Select(c => new Cliente()
                {
                    IdCliente = c.IdCliente,
                    Nombres = c.Nombres,
                    Apellidos = c.Apellidos,
                    Telefono = c.Telefono,
                    Email = c.Email
                })
                .FirstOrDefault();
        }

        #region
        public List<Cliente> Listado(string Busqueda)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
