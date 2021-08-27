using Net.Business.Entities;
using System;

namespace Net.Business.DTO.Planilla
{
    public class DtoPlanillaModificar
    {
        public string campo { get; set; }
        public string numeroplanilla { get; set; }
        public DateTime fecha { get; set; }
        public string strfecha { get; set; }

        public BE_Planilla RetornaPlanilla()
        {
            return new BE_Planilla
            {
                campo = this.campo,
                numeroplanilla = this.numeroplanilla,
                fecha = Convert.ToDateTime(strfecha)
            };
        }
    }
}
