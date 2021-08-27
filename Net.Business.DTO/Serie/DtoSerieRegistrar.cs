using Net.Business.Entities;

namespace Net.Business.DTO
{
    public class DtoSerieRegistrar: EntityBase
    {
        public string tiposerie { get; set; }
        public string serie { get; set; }
        public string correlativo { get; set; }

        public BE_Serie RetornaSerie()
        {
            return new BE_Serie
            {
                tiposerie = this.tiposerie,
                serie = this.serie,
                correlativo = this.correlativo,
                RegIdUsuario = this.RegIdUsuario
            };
        }
    }
}
