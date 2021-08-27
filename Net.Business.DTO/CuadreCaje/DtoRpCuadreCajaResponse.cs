using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Business.DTO.CuadreCaje
{
    public class DtoRpCuadreCajaResponse
    {
        public string area { get; set; }
        public string usuario { get; set; }
        public string numeroplanilla { get; set; }
        public string documento { get; set; }
        public string paciente { get; set; }
        public decimal tc { get; set; }
        public decimal ingresos { get; set; }
        public decimal egresos { get; set; }
        public string docreferencia { get; set; }
        public string movimiento { get; set; }
        public DateTime fechaplanilla { get; set; }
        public string documentoe { get; set; }
    }
}
