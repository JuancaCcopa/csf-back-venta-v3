using Net.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Net.Business.DTO
{
    public class DtoComprobanteListaTipoPagoResponse
    {
        public IEnumerable<DtoComprobanteTipoPagoResponse> lista { get; set; }

        public DtoComprobanteListaTipoPagoResponse RetornarListaCuadreCaja(IEnumerable<BE_CuadreCaja> listaCuadreCaja)
        {
            IEnumerable<DtoComprobanteTipoPagoResponse> listaModelo = (
                from value in listaCuadreCaja
                select new DtoComprobanteTipoPagoResponse
                {
                    codTipoPago = value.tipopago,
                    nombreTipoPago = value.nombretipopago,
                    codEntidad = value.nombreentidad,
                    nombreEntidad = value.descripcionentidad,
                    nroOperacion = value.numeroentidad,
                    montoDolar = value.montodolares,
                    montoSoles = value.monto,
                    montoMn = value.monto,
                    strFechaEmision = value.fechaemision.ToShortDateString(),
                    strFechaCancelacion = value.fechacancelacion.ToShortDateString(),
                    numeroPlanilla = value.numeroplanilla,
                    estado = value.estado,
                    codTerminal = value.codterminal,
                    terminal = value.numeroterminal,
                    codVenta = value.codventa,
                    moneda = value.moneda,
                    documento = value.documento,
                    codcomprobante = value.codcomprobante,
                }
                ); ;

            return new DtoComprobanteListaTipoPagoResponse() { lista = listaModelo };
        }


    }
}
