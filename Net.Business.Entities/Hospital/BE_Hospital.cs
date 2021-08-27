using Net.Connection.Attributes;
using System;
using System.Data;

namespace Net.Business.Entities
{
    public class BE_Hospital
    {
        //[DBParameter(SqlDbType.Char, 1, ActionType.Everything)]
        //public string quirofano { get; set; }
        //[DBParameter(SqlDbType.Char, 1, ActionType.Everything)]
        //public string recuperacion { get; set; }
        //[DBParameter(SqlDbType.DateTime, 0, ActionType.Everything)]
        //public DateTime fechaaltamedica { get; set; }
        [DBParameter(SqlDbType.Char, 8, ActionType.Everything)]
        public string cama { get; set; }
        //[DBParameter(SqlDbType.Char, 8, ActionType.Everything)]
        //public string cama_alias { get; set; }
        [DBParameter(SqlDbType.Char, 8, ActionType.Everything)]
        public string codatencion { get; set; }
        [DBParameter(SqlDbType.DateTime, 0, ActionType.Everything)]
        public DateTime fechainicio { get; set; }
        [DBParameter(SqlDbType.Char, 8, ActionType.Everything)]
        public string codpaciente { get; set; }
        //[DBParameter(SqlDbType.Char, 8, ActionType.Everything)]
        //public string codmedico { get; set; }
        [DBParameter(SqlDbType.Char, 20, ActionType.Everything)]
        public string codpoliza { get; set; }
        [DBParameter(SqlDbType.Char, 4, ActionType.Everything)]
        public string codaseguradora { get; set; }
        //[DBParameter(SqlDbType.Char, 1, ActionType.Everything)]
        //public string familiar { get; set; }
        [DBParameter(SqlDbType.Char, 10, ActionType.Everything)]
        public string planpoliza { get; set; }
        public string polizaplan { get => string.Format("{0}-{1}", codpoliza.Trim(), planpoliza.Trim()); }
        //[DBParameter(SqlDbType.Char, 1, ActionType.Everything)]
        //public string piso { get; set; }
        //[DBParameter(SqlDbType.Char, 1, ActionType.Everything)]
        //public string salaoperacion { get; set; }
        //[DBParameter(SqlDbType.Char, 1, ActionType.Everything)]
        //public string preoperatorio { get; set; }
        //[DBParameter(SqlDbType.Char, 1, ActionType.Everything)]
        //public string ucuidados { get; set; }
        //[DBParameter(SqlDbType.Char, 1, ActionType.Everything)]
        //public string cAmb { get; set; }
        //[DBParameter(SqlDbType.Int, 0, ActionType.Everything)]
        //public int hisRN { get; set; }
        [DBParameter(SqlDbType.VarChar, 300, ActionType.Everything)]
        public string nombres { get; set; }
        [DBParameter(SqlDbType.VarChar, 100, ActionType.Everything)]
        public string edad { get; set; }
        [DBParameter(SqlDbType.VarChar, 20, ActionType.Everything)]
        public string sexo { get; set; }
        [DBParameter(SqlDbType.VarChar, 100, ActionType.Everything)]
        public string nombremedico { get; set; }
        [DBParameter(SqlDbType.VarChar, 100, ActionType.Everything)]
        public string nombreaseguradora { get; set; }
        //[DBParameter(SqlDbType.VarChar, 100, ActionType.Everything)]
        //public string estadopaciente { get; set; }
        //[DBParameter(SqlDbType.VarChar, 100, ActionType.Everything)]
        //public string paquete { get; set; }
        //[DBParameter(SqlDbType.VarChar, 100, ActionType.Everything)]
        //public string diagnostico { get; set; }
    }
}
