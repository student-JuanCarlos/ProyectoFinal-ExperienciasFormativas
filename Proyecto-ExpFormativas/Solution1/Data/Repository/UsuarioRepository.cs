using Data.Context;
using Data.Infraestructure;
using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.Pkcs;
using System.Text;

namespace Data.Repository
{
    public class UsuarioRepository: IUsuario
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public int Actualizar(Usuario u)
        {
            var usuario = _context.Usuarios.Find(u.IdUsuario);

            if(usuario == null)
            {
                return 0;
            }

            usuario.IdUsuario = u.IdUsuario;
            usuario.NombreUsuario = u.NombreUsuario;
            usuario.Documento = u.Documento;
            usuario.Telefono = u.Telefono;
            usuario.Email = u.Email;
            usuario.Sueldo = u.Sueldo;
            usuario.IdCargo = u.IdCargo;
            usuario.IdRol = u.IdRol;

            return _context.SaveChanges();
        }

        public int Agregar(Usuario u)
        {
            _context.Usuarios.Add(u);
            return _context.SaveChanges();
        }

        public int CambiarEstado(int id)
        {
            var usuario = _context.Usuarios.Find(id);

            if(usuario == null)
            {
                return 0;
            }

            usuario.Estado = !usuario.Estado;

            return _context.SaveChanges();
        }

        public Usuario Detalle(int id)
        {
            return _context.Usuarios.Include(u => u.rol)
                                    .Include(u => u.cargo)
                                    .AsNoTracking()
                                    .FirstOrDefault(u => u.IdUsuario == id);
        }

        public List<Usuario> Listado(string Busqueda, bool? Estado)
        {
            var query = _context.Usuarios.Include(u => u.rol).Include(u => u.cargo).AsQueryable();

            if(Busqueda != null)
            {
                query = query.Where(u => u.NombreUsuario.Contains(Busqueda) || u.Email.Contains(Busqueda));
            }

            if (Estado.HasValue)
            {
                query = query.Where(u => u.Estado == Estado);
            }

            return query.AsNoTracking().ToList();
        }

        public Usuario Login(string Email, string Contraseña)
        {
            return _context.Usuarios
                           .AsNoTracking()
                           .Include(u => u.rol)
                           .Include(u => u.cargo)
                           .Where(u => u.Email == Email && u.Contraseña == Contraseña)
                           .FirstOrDefault();
                           
        }

        #region
        public List<Usuario> Listado(string Busqueda)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
