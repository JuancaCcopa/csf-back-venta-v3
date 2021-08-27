using Net.Business.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Business.DTO
{
    public class DtoPlanillaTipoPagoRegistrar
    {
        public string codTipoPago { get; set; }
        public string nombreTipoPago { get; set; }
        public string codEntidad { get; set; }
        public string nombreEntidad { get; set; }
        public string nroOperacion { get; set; }
        public decimal montoSoles { get; set; }
        public decimal montoDolar { get; set; }
        public decimal montoMn { get; set; }
        public string codTerminal { get; set; }
        public string terminal { get; set; }
        public string documento { get; set; }
        public string strFechaCancelacion { get; set; }
        public decimal tipoCambio { get; set; }
    }

    public class DtoPlanillaTipoPagoRequest
    {
        public List<DtoPlanillaTipoPagoRegistrar> tipoPagos { get; set; }

        public List<BE_CuadreCaja> RetornaListaCuadreCaja()
        {
            List<BE_CuadreCaja> lista = new List<BE_CuadreCaja>();
            BE_CuadreCaja objeto;
            foreach (var item in this.tipoPagos)
            {
                objeto = new BE_CuadreCaja()
                {
                    codcomprobante=item.documento.Trim(),
                    tipopago = item.codTipoPago,
                    nombreentidad = item.codEntidad,//item.nombreEntidad,
                    descripcionentidad = item.nombreEntidad,
                    numeroentidad = item.nroOperacion,
                    montodolares = item.montoDolar,
                    codterminal = item.codTerminal,
                    moneda = item.montoDolar > 0 ? "D" : "S",
                    monto = item.montoDolar > 0 ? item.montoDolar:item.montoSoles,
                    documento = item.documento.Trim(),
                    fechacancelacion = Convert.ToDateTime(item.strFechaCancelacion),
                    tipodecambio = item.tipoCambio
                };

                lista.Add(objeto);
            }

            return lista;

        }
    }
}
