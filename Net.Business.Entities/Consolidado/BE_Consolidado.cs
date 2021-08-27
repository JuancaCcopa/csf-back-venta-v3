using System;

namespace Net.Business.Entities
{
    public class BE_Consolidado: EntityBase
    {
        public int idconsolidado { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fechahora { get; set; }
    }
}
