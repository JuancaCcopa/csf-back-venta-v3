namespace Net.Business.Entities
{
    public class BE_SeriePorMaquina: EntityBase
    {
        public int id { get; set; }
        public string nombremaquina { get; set; }
        public string seriefactura { get; set; }
        public string serieboleta { get; set; }
        public string serienotacredito { get; set; }
        public string serienotadebito { get; set; }
        public string serieguia { get; set; }
        public string codcentro { get; set; }
        public string descentro { get; set; }
        public string codalmacen { get; set; }
        public string desalmacen { get; set; }
        public string serienotacreditofactura { get; set; }
        public string serienotadebitofactura { get; set; }
    }
}
