using Net.Business.Entities;

namespace Net.Business.DTO
{

    public class DtoPlanesEliminar: EntityBase
    {
        public string CodPlan { get; set; }

        public BE_Planes RetornaPlanes()
        {
            return new BE_Planes
            {
                CodPlan = this.CodPlan,
                RegIdUsuario = this.RegIdUsuario
            };
        }
    }
}
