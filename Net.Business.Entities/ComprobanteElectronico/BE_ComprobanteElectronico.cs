using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Business.Entities
{
   public class BE_ComprobanteElectronico
    {

        public string codcomprobante { get; set; }
        public string tipo_otorgamiento { get; set; }
        public string estado_cdr { get; set; }
        public bool flg_confirma { get; set; }
        public Byte[] codigobarra { get; set; }
        public DateTime fecha_registro_sis { get; set; }
        public string tipo_comprobante { get; set; }
        public string codcomprobantee { get; set; }
        public StringBuilder xml_registro { get; set; }



    }

}
