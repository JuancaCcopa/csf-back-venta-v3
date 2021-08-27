using Net.Business.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Net.Business.DTO
{
    public class DtoPedidoListarResponse
    {
        public IEnumerable<DtoPedidoResponse> ListaPedido { get; set; }

        public DtoPedidoListarResponse RetornarListaPedido(IEnumerable<BE_Pedido> listaPedido)
        {
            IEnumerable<DtoPedidoResponse> lista = (
                from value in listaPedido
                select new DtoPedidoResponse
                {
                    estimpresion = value.estimpresion,
                    tiplistado = value.tiplistado,
                    estimpresiondsc = value.estimpresiondsc,
                    tiplistadodsc = value.tiplistadodsc,
                    codventa = value.codventa,
                    codpedido = value.codpedido,
                    codcentro = value.codcentro,
                    codalmacen = value.codalmacen,
                    codatencion = value.codatencion,
                    cama = value.cama,
                    fechagenera = value.fechagenera,
                    fechaatencion = value.fechaatencion,
                    nompaciente = value.nompaciente,
                    nomobservacion = value.nomobservacion,
                    nomusuario = value.nomusuario,
                    nomcentro = value.nomcentro,
                    nomalmacen = value.nomalmacen,
                    orden = value.orden,
                    codtipopedido = value.codtipopedido,
                    tipopedido = value.tipopedido,
                    tipomovimiento = value.tipomovimiento,
                    key = value.key
                }
            );

            return new DtoPedidoListarResponse() { ListaPedido = lista };
        }
    }
}
