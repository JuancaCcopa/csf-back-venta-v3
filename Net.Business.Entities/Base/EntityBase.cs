using Net.Connection.Attributes;
using System.Data;

namespace Net.Business.Entities
{
    public class EntityBase
    {
        [DBParameter(SqlDbType.Int, 0, ActionType.Everything)]
        public int? RegIdUsuario { get; set; }
    }
}
