using Net.Connection.Attributes;
using System.Data;

namespace Net.Business.Entities
{
    public class BE_PlanillaDetalle : EntityBase
    {
        [DBParameter(SqlDbType.Char, 8, ActionType.Everything)]
        public string numeroplanilla { get; set; }
        [DBParameter(SqlDbType.Char, 8, ActionType.Everything)]
        public string correlativo { get; set; }
        [DBParameter(SqlDbType.Char, 11, ActionType.Everything)]
        public string codcomprobante { get; set; }
        [DBParameter(SqlDbType.Decimal, 0, ActionType.Everything)]
        public decimal monto { get; set; }
        [DBParameter(SqlDbType.Char, 2, ActionType.Everything)]
        public string tipodocumento { get; set; }
        [DBParameter(SqlDbType.Char, 1, ActionType.Everything)]
        public string ingresoegreso { get; set; }
    }
}