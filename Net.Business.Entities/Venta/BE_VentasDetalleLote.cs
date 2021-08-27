using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Net.Business.Entities
{
    [DataContract]
    [Serializable]
    [XmlRoot("VentasDetalleLote")]
    public class BE_VentasDetalleLote
    {
        [DataMember, XmlIgnore]
        public int idlote { get; set; }
        [DataMember, XmlAttribute]
        public string codproducto { get; set; }
        [DataMember, XmlIgnore]
        public string dsc_producto { get; set; }
        [DataMember, XmlIgnore]
        public string coddetalle { get; set; }
        [DataMember, XmlAttribute]
        public string lote { get; set; }
        [DataMember, XmlAttribute]
        public DateTime fechavencimiento { get; set; }
        [DataMember, XmlAttribute]
        public decimal cantidad { get; set; }
        [DataMember, XmlIgnore]
        public int? ubicacion { get; set; }
        [DataMember, XmlIgnore]
        public string ubicaciondescripcion { get; set; }
        [DataMember, XmlIgnore]
        public decimal cantidaddev { get; set; }
        [DataMember, XmlAttribute]
        public decimal cantidadxdev { get; set; }
    }
}
