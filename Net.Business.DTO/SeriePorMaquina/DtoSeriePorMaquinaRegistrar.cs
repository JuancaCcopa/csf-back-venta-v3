using Net.Business.Entities;

namespace Net.Business.DTO
{
    public class DtoSeriePorMaquinaRegistrar: EntityBase
    {
        public string nombremaquina { get; set; }
        public string seriefactura { get; set; }
        public string serieboleta { get; set; }
        public string serienotacredito { get; set; }
        public string serienotadebito { get; set; }
        public string serieguia { get; set; }
        public string codcentro { get; set; }
        public string codalmacen { get; set; }

        public BE_SeriePorMaquina RetornaSeriePorMaquina()
        {
            return new BE_SeriePorMaquina
            {
                nombremaquina = this.nombremaquina,
                seriefactura = this.seriefactura,
                serieboleta = this.serieboleta,
                serienotacredito = this.serienotacredito,
                serienotadebito = this.serienotadebito,
                serieguia = this.serieguia,
                codcentro = this.codcentro,
                codalmacen = this.codalmacen,
                RegIdUsuario = this.RegIdUsuario
            };
        }
    }
}
