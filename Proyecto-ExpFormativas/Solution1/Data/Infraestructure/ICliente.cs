using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Infraestructure
{
    public interface ICliente: IGeneric<Cliente>
    {
        public Cliente Login(string Email, string Contraseña);

    }
}
