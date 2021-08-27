using Net.Business.Entities;

namespace Net.Business.DTO
{
    public class DtoSeriePorMaquinaEliminar: EntityBase
    {
        public int id { get; set; }

        public BE_SeriePorMaquina RetornaSeriePorMaquina()
        {
            return new BE_SeriePorMaquina
            {
                id = this.id,
                RegIdUsuario = this.RegIdUsuario
            };
        }
    }
}
