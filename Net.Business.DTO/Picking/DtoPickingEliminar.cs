using Net.Business.Entities;

namespace Net.Business.DTO
{
    public class DtoPickingEliminar: EntityBase
    {
        public int idpicking { get; set; }
        public string codusuarioapu { get; set; }

        public BE_Picking RetornaPicking()
        {
            return new BE_Picking
            {
                idpicking = this.idpicking,
                codusuarioapu = this.codusuarioapu,
                RegIdUsuario = this.RegIdUsuario
            };
        }
    }
}
