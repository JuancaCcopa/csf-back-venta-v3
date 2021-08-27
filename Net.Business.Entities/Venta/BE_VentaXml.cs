using Net.Connection.Attributes;
using System.Data;

namespace Net.Business.Entities
{
    public class BE_VentaXml: EntityBase
    {
        [DBParameter(SqlDbType.VarChar, 10, ActionType.Everything)]
        public string codventa { get; set; }
        [DBParameter(SqlDbType.Xml, 0, ActionType.Everything)]
        public string XmlData { get; set; }
    }
}

