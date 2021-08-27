using Net.Connection.Attributes;
using System;
using System.Data;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Net.Business.Entities
{
    [DataContract]
    [Serializable]
    [XmlRoot("Ubicacion")]
    public class BE_VentasDetalleUbicacion
    {
        [DataMember, XmlIgnore]
        [DBParameter(SqlDbType.Int, 0, ActionType.Everything)]
        public int idubicacion { get; set; }
        [DataMember, XmlIgnore]
        [DBParameter(SqlDbType.VarChar, 10, ActionType.Everything)]
        public string coddetalle { get; set; }
        [DataMember, XmlAttribute]
        [DBParameter(SqlDbType.Char, 8, ActionType.Everything)]
        public string codproducto { get; set; }
        [DataMember, XmlAttribute]
        [DBParameter(SqlDbType.Int, 0, ActionType.Everything)]
        public int ubicacion { get; set; }
        [DataMember, XmlAttribute]
        [DBParameter(SqlDbType.VarChar, 220, ActionType.Everything)]
        public string ubicaciondescripcion { get; set; }
        [DataMember, XmlAttribute]
        [DBParameter(SqlDbType.Decimal, 0, ActionType.Everything)]
        public decimal cantidad { get; set; }
        [DataMember, XmlIgnore]
        [DBParameter(SqlDbType.Decimal, 0, ActionType.Everything)]
        public decimal cantidaddev { get; set; }
    }
}
