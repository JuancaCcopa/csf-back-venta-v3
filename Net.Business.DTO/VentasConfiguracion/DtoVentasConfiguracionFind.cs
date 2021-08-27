using Net.Business.Entities;

namespace Net.Business.DTO
{
    public class DtoVentasConfiguracionFind
    {
        public int idconfiguracion { get; set; }
        public string nombre { get; set; }

        public BE_VentasConfiguracion RetornaVentasConfiguracion()
        {
            return new BE_VentasConfiguracion
            {
                idconfiguracion = this.idconfiguracion,
                nombre = this.nombre
            };
        }
    }
}
