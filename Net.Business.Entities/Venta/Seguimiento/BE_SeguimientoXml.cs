using Net.Connection.Attributes;
using System.Data;

namespace Net.Business.Entities
{
    public class BE_SeguimientoXml : EntityBase
    {
        [DBParameter(SqlDbType.Int, 0, ActionType.Everything)]
        public int Opcion { get; set; }
        [DBParameter(SqlDbType.Xml, 0, ActionType.Everything)]
        public string XmlData { get; set; }
    }
}
