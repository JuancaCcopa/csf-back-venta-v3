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
    [XmlRoot("RecetaObservacion")]
    public class DtoRecetaObservacionRegistrar: EntityBase
    {
        [DataMember, XmlAttribute]
        public int id_receta { get; set; }
        [DataMember, XmlAttribute]
        public string codatencion { get; set; }
        [DataMember, XmlAttribute]
        public int idusuarioregistro { get; set; }
        [DataMember, XmlAttribute]
        public string comentario { get; set; }
        public BE_RecetaObservacionXml RetornaModelo()
        {

            var entiDom = new BE_RecetaObservacionXml();
            var ser = new Serializador();
            var ms = new MemoryStream();
            ser.SerializarXml(this, ms);
            entiDom.XmlData = Encoding.UTF8.GetString(ms.ToArray());
            ms.Dispose();

            return new BE_RecetaObservacionXml
            {
                XmlData = entiDom.XmlData,
                RegIdUsuario = this.RegIdUsuario
            };
        }
    }
}
