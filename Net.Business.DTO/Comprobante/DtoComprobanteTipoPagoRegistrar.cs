using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Business.DTO
{
    public class DtoComprobanteTipoPagoRegistrar
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

    }
}
