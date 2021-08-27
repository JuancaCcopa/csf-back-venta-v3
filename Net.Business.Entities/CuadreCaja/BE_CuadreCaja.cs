using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Business.Entities
{
    public class BE_CuadreCaja
    {

        public string correlativo { get; set; }
        public DateTime fecha { get; set; }
        public string coduser { get; set; }
        public string documento { get; set; }
        public string tipopago { get; set; }
        public string nombreentidad { get; set; }
        public string descripcionentidad { get; set; }
        public string numeroentidad { get; set; }
        public string movimiento { get; set; }
        public decimal monto { get; set; }
        public string estado { get; set; }
        public string numeroplanilla { get; set; }
        public string moneda { get; set; }
        public decimal montodolares { get; set; }
        public decimal tipodecambio { get; set; }
        public string tipodocumento { get; set; }
        public string codcentro { get; set; }
        public string codterminal { get; set; }
        public bool flg_enviosap { get; set; }
        public DateTime fec_enviosap { get; set; }
        public int ide_trans { get; set; }
        public int doc_entry { get; set; }
        public bool flg_envioanularsap { get; set; }
        public DateTime fec_envioanularsap { get; set; }
        public int regcreateidusuario { get; set; }
        public DateTime regcreate { get; set; }
		
		 public int regupdateidusuario { get; set; }
        public DateTime regupdate { get; set; }
        public int flgeliminado { get; set; }
		
		 public string nombres { get; set; }
        public decimal docmonto { get; set; }
        public string usuario { get; set; }
        public string nombreusuario { get; set; }
        public string nombrecentro { get; set; }
        public string documentoe { get; set; }
        public decimal montoingreso { get; set; }
        public string procesar_planilla { get; set; }
        public string estado_cdr { get; set; }
		
		public string area { get; set; }
        public decimal tc { get; set; }
        public string paciente { get; set; }
        public decimal ingresos { get; set; }
        public decimal egresos { get; set; }
        public string docreferencia { get; set; }
        public DateTime fechaplanilla { get; set; }

        public string numerogrupo { get; set; }
        public string codcomprobante { get; set; }
        public string anombrede { get; set; }
        public string tipocliente { get; set; }
        public string nombretipocliente { get; set; }
        public decimal montoparcial { get; set; }
        public decimal m_sumagrupo { get; set; }

        public string nombrenombreentidad { get; set; }

        //extra
        public string nombretipopago { get; set; }
        public string numeroterminal { get; set; }
        public DateTime fechaemision { get; set; }
        public DateTime fechacancelacion { get; set; }
        public string codventa { get; set; }
        //public string strFechaCancelacion { get; set; }


    }
}
