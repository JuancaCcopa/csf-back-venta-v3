using Net.Business.Entities;
using System;
namespace Net.Business.DTO
{
    public class DtoPedidoPorAtencionResponse
    {
        public string codpedido { get; set; }
        public DateTime fechaatencion { get; set; }
        public string estado { get; set; }
        public string listado { get; set; }

        public DtoPedidoPorAtencionResponse RetornaDtoVentaCabeceraResponse(BE_Pedido value)
        {
            return new DtoPedidoPorAtencionResponse()
            {
                codpedido = value.codpedido,
                fechaatencion = value.fechaatencion,
                estado = value.estado,
                listado = value.listado
            };
        }
    }
}
