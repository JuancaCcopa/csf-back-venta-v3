using System;

namespace Net.Business.Entities
{
    public class BE_Picking: EntityBase
    {
        public int idpicking { get; set; }
        public string codpedido { get; set; }
        public int id_receta { get; set; }
        public string codproducto { get; set; }
        public decimal cantidad { get; set; }
        public decimal cantidadpicking { get; set; }
        public string lote { get; set; }
        public DateTime fechavencimiento { get; set; }
        public string codalmacen { get; set; }
        public int ubicacion { get; set; }
        public string codusuarioapu { get; set; }
        public int estado { get; set; }
    }
}
