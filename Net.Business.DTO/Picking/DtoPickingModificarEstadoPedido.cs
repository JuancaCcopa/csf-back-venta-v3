using Net.Business.Entities;

namespace Net.Business.DTO
{
    public class DtoPickingModificarEstadoPedido : EntityBase
    {
        public string codpedido { get; set; }
        public string codusuarioapu { get; set; }
        public int estado { get; set; }

        public BE_Picking RetornaPicking()
        {
            return new BE_Picking
            {
                codpedido = this.codpedido,
                codusuarioapu = this.codusuarioapu,
                estado = this.estado,
                RegIdUsuario = this.RegIdUsuario
            };
        }
    }
}
