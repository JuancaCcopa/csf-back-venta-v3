using Net.Connection.Attributes;
using System.Data;

namespace Net.Business.Entities
{
    public class BE_VentasConfiguracion: EntityBase
    {
        public int idconfiguracion { get; set; }
        [DBParameter(SqlDbType.VarChar, 50, ActionType.Everything)]
        public string nombre { get; set; }
        public bool flgautomatico { get; set; }
        public bool flgreceta { get; set; }
        public bool flgpedido { get; set; }
        public bool flgmanual { get; set; }
        public bool flgimpresionautomatico { get; set; }
        public string codalmacen { get; set; }
        public string desalmacen { get; set; }
    }
}
