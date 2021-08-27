using System;

namespace Net.Business.Entities
{
    public class BE_Receta
    {
        public int ide_receta { get; set; }
        public int ide_historia { get; set; }
        public DateTime fec_registra { get; set; }
        public string cod_atencion { get; set; }
        public string paciente { get; set; }
        public string medico { get; set; }
        public string est_estado { get; set; }
        public string telefono { get; set; }
        public string telefono2 { get; set; }
        public string est_consulta_medica { get; set; }
        public bool flg_atendido_online { get; set; }
        public string key { get => ide_receta.ToString() + ide_historia.ToString() + cod_atencion; }
    }
}
