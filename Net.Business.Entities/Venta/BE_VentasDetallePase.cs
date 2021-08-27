using Net.Connection.Attributes;
using System.Data;

namespace Net.Business.Entities
{
    public class BE_VentasDetallePase
    {
        [DBParameter(SqlDbType.Char, 12, ActionType.Everything)]
        public string codpresotor { get; set; }
        [DBParameter(SqlDbType.Char, 8, ActionType.Everything)]
        public string codventa { get; set; }
        [DBParameter(SqlDbType.Char, 8, ActionType.Everything)]
        public string codproducto { get; set; }
        [DBParameter(SqlDbType.Char, 1, ActionType.Everything)]
        public string gnc { get; set; }
    }
}
