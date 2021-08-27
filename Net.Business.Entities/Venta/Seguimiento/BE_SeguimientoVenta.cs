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
    [XmlRoot("Seguimiento")]
    public class BE_SeguimientoVenta
    {
        [DataMember, XmlAttribute]
        [DBParameter(SqlDbType.Char, 8, ActionType.Everything)]
        public string codventa { get; set; }
    }
}
