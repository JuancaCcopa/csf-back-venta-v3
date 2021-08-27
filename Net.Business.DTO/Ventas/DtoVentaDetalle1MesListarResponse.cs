using Net.Business.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Net.Business.DTO
{
    public class DtoVentaDetalle1MesListarResponse
    {
        public IEnumerable<DtoVentaDetalle1MesResponse> ListaVentaDetalle { get; set; }

        public DtoVentaDetalle1MesListarResponse RetornarVentaDetalle1MesListarResponse(IEnumerable<BE_VentasDetalle> listaVentaDetalle)
        {
            IEnumerable<DtoVentaDetalle1MesResponse> lista = (
                from value in listaVentaDetalle
                select new DtoVentaDetalle1MesResponse
                {
                    codventa = value.codventa,
                    tipomovimiento = value.tipomovimiento,
                    fechaemision = value.fechaemision,
                    codproducto = value.codproducto,
                    nombreproducto = value.nombreproducto,
                    cantidad = value.cantidad,
                    stockfraccion = value.stockfraccion
                }
            );

            return new DtoVentaDetalle1MesListarResponse() { ListaVentaDetalle = lista };
        }
    }
}
