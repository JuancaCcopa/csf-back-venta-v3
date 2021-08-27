using Net.Connection.Attributes;
using System.Data;

namespace Net.Business.Entities
{
    public class BE_HospitalDatos
    {
        [DBParameter(SqlDbType.Char, 8, ActionType.Everything)]
        public string codmedicoemergencia { get; set; }
    }
}
