namespace Net.Business.Entities
{
    public class BE_ConsolidadoPedido: EntityBase
    {
        public int idconsolidadopedido { get; set; }
        public int idconsolidado { get; set; }
        public string codpedido { get; set; }
        public string codproducto { get; set; }
        public decimal cantidad { get; set; }
        public bool flgpicking { get; set; }
        public decimal cantidadpicking { get; set; }
    }
}
