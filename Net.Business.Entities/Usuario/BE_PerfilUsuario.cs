using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Business.Entities
{
    public class BE_PerfilUsuario
    {
        public int ide_opcion { get; set; }
        public int ide_modulo { get; set; }
        public string txt_opcion { get; set; }
        public string cod_opcion { get; set; }
        public int ide_opcion_sup { get; set; }
        public int ide_perfil { get; set; }
    }
}
