using System;

namespace Net.Business.Entities
{
    public class BE_AseguradoraxProducto
    {
        public string codaseguradora { get; set; }
        public string codproducto { get; set; }
        public DateTime fec_registro { get; set; }
        public int cod_tipoatencion_mae { get; set; }
    }
}
