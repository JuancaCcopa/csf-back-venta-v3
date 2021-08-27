using Net.Business.Entities;

namespace Net.Business.DTO
{
    public class DtoPickingModificarEstadoReceta: EntityBase
    {
        public int id_receta { get; set; }
        public string codusuarioapu { get; set; }
        public int estado { get; set; }

        public BE_Picking RetornaPicking()
        {
            return new BE_Picking
            {
                id_receta = this.id_receta,
                codusuarioapu = this.codusuarioapu,
                estado = this.estado,
                RegIdUsuario = this.RegIdUsuario
            };
        }
    }
}
