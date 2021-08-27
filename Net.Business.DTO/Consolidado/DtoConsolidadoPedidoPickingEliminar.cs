using Net.Business.Entities;

namespace Net.Business.DTO
{
    public class DtoConsolidadoPedidoPickingEliminar: EntityBase
    {
        public int idconsolidadopicking { get; set; }
        public string codusuarioapu { get; set; }

        public BE_ConsolidadoPedidoPicking RetornaConsolidadoPedidoPicking()
        {
            return new BE_ConsolidadoPedidoPicking
            {
                idconsolidadopicking = this.idconsolidadopicking,
                codusuarioapu = this.codusuarioapu,
                RegIdUsuario = this.RegIdUsuario
            };
        }
    }
}
