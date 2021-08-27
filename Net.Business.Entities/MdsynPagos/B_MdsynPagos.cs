using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Business.Entities
{
   public class B_MdsynPagos
    {
        public int ide_pagos_bot { get; set; }
        public string cod_tipo { get; set; }
        public int ide_mdsyn_reserva { get; set; }
        public int ide_correl_reserva { get; set; }
        public string cod_liquidacion { get; set; }
        public string cod_venta { get; set; }
        public decimal cnt_monto_pago { get; set; }
        public string ide_unique_identifier { get; set; }
        public string cod_rpta_synapsis { get; set; }
        public int usr_reg_orden_synapsis { get; set; }
        public DateTime fec_reg_orden_synapsis { get; set; }
        public string txt_json_orden { get; set; }
        public string est_pagado { get; set; }
        public string nro_operacion { get; set; }
        public string tip_tarjeta { get; set; }
        public string num_tarjeta { get; set; }
        public string txt_json_rpta { get; set; }
        public DateTime fec_recepcion_pago { get; set; }
        public string flg_pago_usado { get; set; }
        public string cod_comprobante { get; set; }
        public int usr_pago_usado { get; set; }
        public DateTime fec_pago_usado { get; set; }
        //Extra
        public string terminal { get; set; }
        

    }
}
