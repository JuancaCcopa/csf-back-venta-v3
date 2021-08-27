using Net.Business.Entities;

namespace Net.Business.DTO
{
    public class DtoVentaCabeceraEnvioPisoModificar: EntityBase
    {
        public string codventa { get; set; }

        public BE_VentasCabecera RetornaVentasCabeceraResponse()
        {
            return new BE_VentasCabecera()
            {
                codventa = this.codventa,
                RegIdUsuario = this.RegIdUsuario
            };
        }
    }
}
