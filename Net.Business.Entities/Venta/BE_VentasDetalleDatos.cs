using Net.Connection.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Net.Business.Entities
{
    [DataContract]
    [Serializable]
    [XmlRoot("DetalleDatos")]
    public class BE_VentasDetalleDatos
    {
        [DataMember, XmlIgnore]
        [DBParameter(SqlDbType.Char, 10, ActionType.Everything)]
        public string coddetalle { get; set; }
        [DataMember, XmlIgnore]
        [DBParameter(SqlDbType.Char, 8, ActionType.Everything)]
        public string codventa { get; set; }
        [DataMember, XmlAttribute]
        [DBParameter(SqlDbType.Char, 8, ActionType.Everything)]
        public string codproducto { get; set; }
        [DataMember, XmlAttribute]
        [DBParameter(SqlDbType.Char, 2, ActionType.Everything)]
        public string tipodocumentoautorizacion { get; set; }
        [DataMember, XmlAttribute]
        [DBParameter(SqlDbType.Char, 8, ActionType.Everything)]
        public string numerodocumentoautorizacion { get; set; }
        [DataMember, XmlIgnore]
        [DBParameter(SqlDbType.Decimal, 0, ActionType.Everything)]
        public decimal valor_dscto { get; set; }
        [DataMember, XmlIgnore]
        [DBParameter(SqlDbType.Decimal, 0, ActionType.Everything)]
        public decimal porc_dscto { get; set; }
        [DataMember, XmlIgnore]
        [DBParameter(SqlDbType.VarChar, 50, ActionType.Everything)]
        public string obs_dscto { get; set; }
        [DataMember, XmlIgnore]
        [DBParameter(SqlDbType.VarChar, 4, ActionType.Everything)]
        public string tipo_dscto { get; set; }
        [DataMember, XmlIgnore]
        [DBParameter(SqlDbType.Int, 0, ActionType.Everything)]
        public int num_sec { get; set; }
        [DataMember, XmlIgnore]
        [DBParameter(SqlDbType.Char, 1, ActionType.Everything)]
        public string estado { get; set; }
        [DataMember, XmlIgnore]
        [DBParameter(SqlDbType.Int, 0, ActionType.Everything)]
        public int flg_asi { get; set; }
        [DataMember, XmlIgnore]
        [DBParameter(SqlDbType.Decimal, 0, ActionType.Everything)]
        public decimal val_venta { get; set; }
        [DataMember, XmlIgnore]
        [DBParameter(SqlDbType.Char, 12, ActionType.Everything)]
        public string codpresotor { get; set; }
    }
}
