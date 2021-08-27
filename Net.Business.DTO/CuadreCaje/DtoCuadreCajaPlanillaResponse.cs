using System;

namespace Net.Business.DTO.CuadreCaje
{
    public class DtoCuadreCajaPlanillaResponse
    {
        public string correlativo { get; set; }
        public DateTime fecha { get; set; }
        public string coduser { get; set; }
        public string documento { get; set; }
        public string tipopago { get; set; }
        public string nombreentidad { get; set; }
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
        public string nombretipopago { get; set; }
        public string nombrenombreentidad { get; set; }
    }
}
