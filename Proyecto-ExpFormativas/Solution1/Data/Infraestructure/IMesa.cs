using Data.DTOs.MesaDTO;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Infraestructure
{
    public interface IMesa
    {
        public List<MesaListadoDTO> Listado();

        public MesaDetalleDTO Detalle(int id);

        public void ActualizarEstadoMesasHoy();

        public List<MesaListadoDTO> FiltradoMesas_Cliente(DateTime FechaReserva, TimeSpan HoraReserva);

        public int Actualizar(Mesa m);

        public int Agregar(Mesa m);

    }
}
