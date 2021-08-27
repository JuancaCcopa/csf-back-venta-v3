using Net.Business.Entities;

namespace Net.Business.DTO
{
    public class DtoConsolidadoPedidoPickingModificar: EntityBase
    {
        public int idconsolidadopicking { get; set; }
        public decimal cantidadpicking { get; set; }
        public string codusuarioapu { get; set; }

        public BE_ConsolidadoPedidoPicking RetornaConsolidadoPedidoPicking()
        {
            return new BE_ConsolidadoPedidoPicking
            {
                idconsolidadopicking = this.idconsolidadopicking,
                cantidadpicking = this.cantidadpicking,
                codusuarioapu = this.codusuarioapu,
                RegIdUsuario = this.RegIdUsuario
            };
        }
    }
}
