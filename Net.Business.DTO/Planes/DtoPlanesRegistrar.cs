using Net.Business.Entities;
using System;

namespace Net.Business.DTO
{
    public class DtoPlanesRegistrar: EntityBase
    {
        //public int IdPlan { get; set; }
        public string Nombre { get; set; }
        public decimal PorcentajeDescuento { get; set; }
        public Boolean FlgEstado { get; set; }

        public BE_Planes RetornaPlanes()
        {
            return new BE_Planes
            {
                //IdPlan = this.IdPlan,
                Nombre = this.Nombre,
                FlgEstado = this.FlgEstado,
                PorcentajeDescuento = this.PorcentajeDescuento,
                RegIdUsuario = this.RegIdUsuario
            };
        }
    }
}
