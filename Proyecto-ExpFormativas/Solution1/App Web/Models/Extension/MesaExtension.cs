using App_Web.Models.VM;
using Data.DTOs.MesaDTO;
using Data.Infraestructure;
using Entities;

namespace App_Web.Models.Extension
{
    public static class MesaExtension
    {

        public static MesaVM DetalleDTOtoVM(this MesaDetalleDTO dto)
        {
            return new MesaVM()
            {
                IdMesa = dto.IdMesa,
                NumeroMesa = dto.NumeroMesa,
                EspacioOcupable = dto.EspacioOcupable,
                Estado = dto.Estado,
                reserva = dto.HoraReserva != null ? new ReservaVM()
                {
                    HoraReserva = dto.HoraReserva,
                    NombreCliente = dto.OcupadoPor
                } : null,
                cliente = dto.OcupadoPor != null ? new ClienteVM()
                {
                    NombreCompleto = dto.OcupadoPor
                } : null
            };
        }

        public static MesaVM ListadoDTOtoVM (this MesaListadoDTO dto)
        {
            return new MesaVM()
            {
                IdMesa = dto.IdMesa,
                NumeroMesa = dto.NumeroMesa,
                EspacioOcupable = dto.EspacioOcupable,
                Estado = dto.Estado,
            };
        }

        public static Mesa ToEntity(this MesaVM model)
        {
            return new Mesa()
            {
                IdMesa = model.IdMesa,
                NumeroMesa = model.NumeroMesa,
                EspacioOcupable = model.EspacioOcupable,
                Estado = model.Estado,
            };
        }

    }
}
