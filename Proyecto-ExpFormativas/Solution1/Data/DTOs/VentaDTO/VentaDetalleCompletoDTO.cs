using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTOs.VentaDTO
{
    public class VentaDetalleCompletoDTO
    {
       public VentaEncabezadoDTO Encabezado { get; set; }
       public List<VentaPlatilloDTO> Platillos { get; set; }
      public List<VentaDescuentoDTO> Descuentos { get; set; }
    }
}
