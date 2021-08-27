using System;

namespace Net.Business.DTO
{
    public class DtoRecetaApuResponse
    {
        public int ide_receta { get; set; }
        public int ide_historia { get; set; }
        public DateTime fec_registra { get; set; }
        public string cod_atencion { get; set; }
        public string paciente { get; set; }
        public string key { get; set; }
    }
}
