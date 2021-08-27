using Net.Business.Entities;

namespace Net.Business.DTO
{
    public class DtoVentaUpdateSinStock: EntityBase
    {
        public string codventa { get; set; }

        public BE_VentasCabecera RetornaVentaUpdateSinStock()
        {
            return new BE_VentasCabecera
            {
                codventa = this.codventa,
                RegIdUsuario = this.RegIdUsuario
            };
        }
    }
}
