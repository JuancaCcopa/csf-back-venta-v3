using Net.Business.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Net.Business.DTO
{
    public class DtoVentaCabeceraListarResponse
    {
        public IEnumerable<DtoVentaCabeceraResponse> ListaVentaCabecera { get; set; }

        public DtoVentaCabeceraListarResponse RetornarListaVentaCabecera(IEnumerable<BE_VentasCabecera> listaArticulos)
        {
            IEnumerable<DtoVentaCabeceraResponse> lista = (
                from value in listaArticulos
                select new DtoVentaCabeceraResponse
                {
                    codventa = value.codventa,
                    codalmacen = value.codalmacen,
                    tipomovimiento = value.tipomovimiento,
                    codempresa = value.codempresa,
                    codtipocliente = value.codtipocliente,
                    nombretipocliente = value.nombretipocliente,
                    codatencion = value.codatencion,
                    codcomprobante = value.codcomprobante,
                    nombreestado = value.nombreestado,
                    nombre = value.nombre,
                    montopaciente = value.montopaciente,
                    montoaseguradora = value.montoaseguradora,
                    flg_gratuito = value.flg_gratuito,
                    fechagenera = value.fechagenera,
                    fechaemision = value.fechaemision,
                    codpaciente = value.codpaciente,
                    codcliente = value.codcliente,
                    codpedido = value.codpedido,
                    estado = value.estado,
                    usuarioanulacion = value.usuarioanulacion
                }
            );

            return new DtoVentaCabeceraListarResponse() { ListaVentaCabecera = lista };
        }
    }
}
