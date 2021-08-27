using Net.Business.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Net.Business.DTO
{
    public class DtoPedidoPorAtencionListarResponse
    {
        public IEnumerable<DtoPedidoPorAtencionResponse> ListaPedidoPorAtencion { get; set; }

        public DtoPedidoPorAtencionListarResponse RetornarListaPedidoPorAtencion(IEnumerable<BE_Pedido> listaPedidoPorAtencion)
        {
            IEnumerable<DtoPedidoPorAtencionResponse> lista = (
                from value in listaPedidoPorAtencion
                select new DtoPedidoPorAtencionResponse
                {
                    codpedido = value.codpedido,
                    fechaatencion = value.fechaatencion,
                    estado = value.estado,
                    listado = value.listado
                }
            );

            return new DtoPedidoPorAtencionListarResponse() { ListaPedidoPorAtencion = lista };
        }
    }
}
