using Net.Business.Entities;
using Net.CrossCotting;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Net.Business.DTO
{
    [DataContract]
    [Serializable]
    [XmlRoot("ValeDelivery")]
    public class DtoValeDeliveryRegistrar: EntityBase
    {
        [DataMember, XmlAttribute]
        public int idvaledelivery { get; set; }
        [DataMember, XmlAttribute]
        public string codatencion { get; set; }
        [DataMember, XmlAttribute]
        public string nombrepaciente { get; set; }
        [DataMember, XmlAttribute]
        public string telefono { get; set; }
        [DataMember, XmlAttribute]
        public string celular { get; set; }
        [DataMember, XmlAttribute]
        public string direccion { get; set; }
        [DataMember, XmlAttribute]
        public string distrito { get; set; }
        [DataMember, XmlAttribute]
        public string referencia { get; set; }
        [DataMember, XmlAttribute]
        public DateTime fechaentrega { get; set; }
        [DataMember, XmlAttribute]
        public string lugarentrega { get; set; }
        [DataMember, XmlAttribute]
        public string prioridad_1 { get; set; }
        [DataMember, XmlAttribute]
        public string prioridad_2 { get; set; }
        [DataMember, XmlAttribute]
        public bool estado { get; set; }
        public BE_ValeDeliveryXml RetornaModelo()
        {
            var entiDom = new BE_ValeDeliveryXml();
            var ser = new Serializador();
            var ms = new MemoryStream();
            ser.SerializarXml(this, ms);
            entiDom.XmlData = Encoding.UTF8.GetString(ms.ToArray());
            ms.Dispose();

            return new BE_ValeDeliveryXml
            {
                XmlData = entiDom.XmlData,
                RegIdUsuario = this.RegIdUsuario
            };
        }
    }
}
