namespace Net.Business.Entities
{
    public class BE_VentasDetalleQuiebre
    {
        public bool narcotico { get; set; }
        public decimal igvproducto { get; set; }
        public string codtipoproducto { get; set; }
        public BE_VentasCabecera ventascabecera { get; set; }
    }
}
