using Net.Connection.Attributes;
using System.Data;

namespace Net.Business.Entities
{
    public class EF_VentaCabeceraConsulta
    {
        [DBParameter(SqlDbType.Char, 11, ActionType.Everything)]
        public string codcomprobante { get; set; }
        [DBParameter(SqlDbType.Char, 8, ActionType.Everything)]
        public string codventa { get; set; }

    }
}
