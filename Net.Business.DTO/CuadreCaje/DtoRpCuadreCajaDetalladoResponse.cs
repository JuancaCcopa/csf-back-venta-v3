using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Business.DTO.CuadreCaje
{
    public class DtoRpCuadreCajaDetalladoResponse
    {
        public string numeroplanilla { get; set; }
        public string numerogrupo { get; set; }
        public string codcomprobante { get; set; }
        public string fechacancelacion { get; set; }
        public string anombrede { get; set; }
        public string tipocliente { get; set; }
        public string nombretipocliente { get; set; }
        public decimal monto { get; set; }
        public decimal montoparcial { get; set; }
        public string tipopago { get; set; }
        public string nombreentidad { get; set; }
        public string moneda { get; set; }
        public DateTime fechaplanilla { get; set; }
        public string coduser { get; set; }
        public string nombreusuario { get; set; }
        public string estado { get; set; }
        public decimal m_sumagrupo { get; set; }
        public decimal montodolares { get; set; }
        public string nombrecentro { get; set; }
    }
}
