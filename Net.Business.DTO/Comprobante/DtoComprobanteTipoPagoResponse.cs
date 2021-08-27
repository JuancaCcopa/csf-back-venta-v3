using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Business.DTO
{
    public class DtoComprobanteTipoPagoResponse
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
        public string strFechaEmision { get; set; }
        public string strFechaCancelacion{ get; set; }
        public string estado { get; set; }
        public string numeroPlanilla { get; set; }
        public string codVenta { get; set; }
        public string moneda { get; set; }
        public string documento { get; set; }
        public string codcomprobante { get; set; }

    }
}
