namespace Net.Business.Entities
{
    public class BE_RecetaDetalle
    {
        public int ide_receta { get; set; }
        public string codproducto { get; set; }
        public decimal cantidad { get; set; }
        public string num_frecuencia { get; set; }
        public string num_dosis { get; set; }
        public string nombreproducto { get; set; }
        public decimal cantidadpicking { get; set; }
    }
}
