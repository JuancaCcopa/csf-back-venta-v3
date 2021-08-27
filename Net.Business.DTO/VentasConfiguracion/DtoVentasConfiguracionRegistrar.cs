using Net.Business.Entities;

namespace Net.Business.DTO
{
    public class DtoVentasConfiguracionRegistrar : EntityBase
    {
        public int idconfiguracion { get; set; }
        public string nombre { get; set; }
        public bool flgautomatico { get; set; }
        public bool flgmanual { get; set; }
        public bool flgpedido { get; set; }
        public bool flgreceta { get; set; }
        public bool flgimpresionautomatico { get; set; }
        public string codalmacen { get; set; }
        public string desalmacen { get; set; }

        public BE_VentasConfiguracion RetornaVentasConfiguracion()
        {
            return new BE_VentasConfiguracion
            {
                idconfiguracion = this.idconfiguracion,
                nombre = this.nombre,
                flgautomatico = this.flgautomatico,
                flgmanual = this.flgmanual,
                flgpedido = this.flgpedido,
                flgreceta = this.flgreceta,
                codalmacen = this.codalmacen,
                desalmacen = this.desalmacen,
                flgimpresionautomatico = this.flgimpresionautomatico,
                RegIdUsuario = this.RegIdUsuario
            };
        }
    }
}
