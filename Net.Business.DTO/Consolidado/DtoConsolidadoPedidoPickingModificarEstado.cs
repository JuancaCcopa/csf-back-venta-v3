using Net.Business.Entities;

namespace Net.Business.DTO
{
    public class DtoConsolidadoPedidoPickingModificarEstado: EntityBase
    {
        public int idconsolidado { get; set; }
        public string codusuarioapu { get; set; }
        public int estado { get; set; }

        public BE_ConsolidadoPedidoPicking RetornaConsolidadoPedidoPicking()
        {
            return new BE_ConsolidadoPedidoPicking
            {
                idconsolidado = this.idconsolidado,
                codusuarioapu = this.codusuarioapu,
                estado = this.estado,
                RegIdUsuario = this.RegIdUsuario
            };
        }
    }
}
