using Net.Business.Entities;

namespace Net.Business.DTO
{
    public class DtoVentasConfiguracionEliminar: EntityBase
    {
        public int idconfiguracion { get; set; }

        public BE_VentasConfiguracion RetornaVentasConfiguracion()
        {
            return new BE_VentasConfiguracion
            {
                idconfiguracion = this.idconfiguracion,
                RegIdUsuario = this.RegIdUsuario
            };
        }
    }
}
