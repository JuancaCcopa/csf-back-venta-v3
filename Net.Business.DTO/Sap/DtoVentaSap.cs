using Net.Business.Entities;

namespace Net.Business.DTO
{
    public class DtoVentaSap: EntityBase
    {
        public string codventa { get; set; }

        public BE_VentasCabecera RetornaVentaSap()
        {
            return new BE_VentasCabecera
            {
                codventa = this.codventa,
                RegIdUsuario = this.RegIdUsuario
            };
        }
    }
}
