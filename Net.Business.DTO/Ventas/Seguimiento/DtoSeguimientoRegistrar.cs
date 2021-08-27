using Net.Business.Entities;
using Net.CrossCotting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace Net.Business.DTO
{
    [DataContract]
    [Serializable]
    [XmlRoot("Seguimiento")]
    public class DtoSeguimientoRegistrar: EntityBase
    {
        [DataMember, XmlAttribute]
        public int Opcion { get; set; }
        [DataMember]
        [XmlElement(ElementName = "ListaSeguimientoVenta", Type = typeof(List<BE_SeguimientoVenta>))]
        public List<BE_SeguimientoVenta> listaSeguimientoVenta { get; set; }

        public BE_SeguimientoXml RetornaModelo()
        {

            var entiDom = new BE_SeguimientoXml();
            var ser = new Serializador();
            var ms = new MemoryStream();
            ser.SerializarXml(this, ms);
            entiDom.XmlData = Encoding.UTF8.GetString(ms.ToArray());
            ms.Dispose();

            return new BE_SeguimientoXml
            {
                Opcion = this.Opcion,
                XmlData = entiDom.XmlData,
                RegIdUsuario = this.RegIdUsuario
            };
        }
    }
}
