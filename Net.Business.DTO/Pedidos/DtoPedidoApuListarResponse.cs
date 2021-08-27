using Net.Business.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Net.Business.DTO
{
    public class DtoPedidoApuListarResponse
    {
        public IEnumerable<DtoPedidoApuResponse> ListaPedido { get; set; }

        public DtoPedidoApuListarResponse RetornarListaPedido(IEnumerable<BE_Pedido> listaPedido)
        {
            IEnumerable<DtoPedidoApuResponse> lista = (
                from value in listaPedido
                select new DtoPedidoApuResponse
                {
                    
                    codpedido = value.codpedido,
                    codatencion = value.codatencion,
                    cama = value.cama,
                    fechagenera = value.fechagenera,
                    fechaatencion = value.fechaatencion,
                    codtipopedido = value.codtipopedido,
                    tipopedido = value.tipopedido,
                    key = value.key
                }
            );

            return new DtoPedidoApuListarResponse() { ListaPedido = lista };
        }
    }
}
