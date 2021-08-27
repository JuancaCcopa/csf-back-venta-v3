using Net.Connection.Attributes;
using System;
using System.Data;

namespace Net.Business.Entities
{
    public class BE_Planes: EntityBase
    {
        //[DBParameter(SqlDbType.Int, ActionType.Everything, true)]
        //public int? IdPlan { get; set; }
        [DBParameter(SqlDbType.Char, 8, ActionType.Everything)]
        public string CodPlan { get; set; }
        [DBParameter(SqlDbType.VarChar, 60, ActionType.Everything)]
        public string Nombre { get; set; }
        [DBParameter(SqlDbType.Decimal, 0, ActionType.Everything)]
        public decimal? PorcentajeDescuento { get; set; }
        [DBParameter(SqlDbType.Bit, 0, ActionType.Everything)]
        public Boolean? FlgEstado { get; set; }
    }
}
