using Net.Business.Entities;
using System;

namespace Net.Business.DTO
{
    public class DtoVentaDetalle1MesResponse
    {
        public string codventa { get; set; }
        public string tipomovimiento { get; set; }
        public DateTime fechaemision { get; set; }
        public string codproducto { get; set; }
        public string nombreproducto { get; set; }
        public decimal cantidad { get; set; }
        public int stockfraccion { get; set; }

        public DtoVentaDetalle1MesResponse RetornaVentaDetalle1MesResponse(BE_VentasDetalle value)
        {
            return new DtoVentaDetalle1MesResponse()
            {
                codventa = value.codventa,
                tipomovimiento = value.tipomovimiento,
                fechaemision = value.fechaemision,
                codproducto = value.codproducto,
                nombreproducto = value.nombreproducto,
                cantidad = value.cantidad,
                stockfraccion = value.stockfraccion
            };
        }
    }
}
