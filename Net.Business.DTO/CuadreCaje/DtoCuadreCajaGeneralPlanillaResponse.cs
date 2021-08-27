using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Business.DTO.CuadreCaje
{
    public class DtoCuadreCajaGeneralPlanillaResponse
    {
        public string documento { get; set; }
        public string nombres { get; set; }
        public decimal docmonto { get; set; }
        public string movimiento { get; set; }
        public string moneda { get; set; }
        public string usuario { get; set; }
        public string codcentro { get; set; }
        public string nombreusuario { get; set; }
        public string nombrecentro { get; set; }
        public string documentoe { get; set; }
        public decimal montoingreso { get; set; }
        public string procesar_planilla { get; set; }
        public string estado_cdr { get; set; }
    }
}
