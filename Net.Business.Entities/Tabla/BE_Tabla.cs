using Net.Connection.Attributes;
using System.Data;

namespace Net.Business.Entities
{
    public class BE_Tabla: EntityBase
    {
        [DBParameter(SqlDbType.Char, 30, ActionType.Everything)]
        public string codtabla { get; set; }
        [DBParameter(SqlDbType.Char, 4, ActionType.Everything)]
        public string codigo { get; set; }
        [DBParameter(SqlDbType.VarChar, 250, ActionType.Everything)]
        public string nombre { get; set; }
        [DBParameter(SqlDbType.Float, 0, ActionType.Everything)]
        public double valor { get; set; }
        [DBParameter(SqlDbType.Char, 1, ActionType.Everything)]
        public string estado { get; set; }
    }
}
