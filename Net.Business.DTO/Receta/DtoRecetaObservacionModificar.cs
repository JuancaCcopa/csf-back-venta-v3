using Net.Business.Entities;

namespace Net.Business.DTO
{
    public class DtoRecetaObservacionModificar: EntityBase
    {
        public int idobs { get; set; }
        public int idusuarioanulacion { get; set; }

        public BE_RecetaObservacion RetornaModelo()
        {
            return new BE_RecetaObservacion
            {
                idobs = idobs,
                RegIdUsuario = this.RegIdUsuario
            };
        }
    }
}
