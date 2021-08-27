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
    [XmlRoot("Cabecera")]
    public class DtoVentaDevolucionRegistrar : EntityBase
    {
        [DataMember, XmlAttribute]
        public string tipodevolucion { get; set; }
        [DataMember, XmlAttribute]
        public string codalmacen { get; set; }
        [DataMember, XmlAttribute]
        public string tipomovimiento { get; set; }
        [DataMember, XmlAttribute]
        public string codtipocliente { get; set; }
        [DataMember, XmlAttribute]
        public string codcliente { get; set; }
        [DataMember, XmlAttribute]
        public string codpaciente { get; set; }
        [DataMember, XmlAttribute]
        public string nombre { get; set; }
        [DataMember, XmlAttribute]
        public string cama { get; set; }
        [DataMember, XmlAttribute]
        public string codmedico { get; set; }
        [DataMember, XmlAttribute]
        public string nombremedico { get; set; }
        [DataMember, XmlAttribute]
        public string codatencion { get; set; }
        [DataMember, XmlAttribute]
        public string codpoliza { get; set; }
        [DataMember, XmlAttribute]
        public string planpoliza { get; set; }
        [DataMember, XmlAttribute]
        public double deducible { get; set; }
        [DataMember, XmlAttribute]
        public string codaseguradora { get; set; }
        [DataMember, XmlAttribute]
        public string codcia { get; set; }
        [DataMember, XmlAttribute]
        public decimal porcentajecoaseguro { get; set; }
        [DataMember, XmlAttribute]
        public decimal porcentajeimpuesto { get; set; }
        [DataMember, XmlAttribute]
        public decimal montodctoplan { get; set; }
        [DataMember, XmlAttribute]
        public decimal porcentajedctoplan { get; set; }
        [DataMember, XmlAttribute]
        public string moneda { get; set; }
        [DataMember, XmlAttribute]
        public decimal montototal { get; set; }
        [DataMember, XmlAttribute]
        public decimal montoigv { get; set; }
        [DataMember, XmlAttribute]
        public decimal montoneto { get; set; }
        [DataMember, XmlAttribute]
        public string codplan { get; set; }
        [DataMember, XmlAttribute]
        public decimal montopaciente { get; set; }
        [DataMember, XmlAttribute]
        public decimal montoaseguradora { get; set; }
        [DataMember, XmlAttribute]
        public string observacion { get; set; }
        [DataMember, XmlAttribute]
        public string codcentro { get; set; }
        [DataMember, XmlAttribute]
        public decimal tipocambio { get; set; }
        [DataMember, XmlAttribute]
        public string codpedido { get; set; }
        [DataMember, XmlAttribute]
        public string nombrediagnostico { get; set; }
        [DataMember, XmlAttribute]
        public string flagpaquete { get; set; }
        [DataMember, XmlAttribute]
        public string nombreaseguradora { get; set; }
        [DataMember, XmlAttribute]
        public string nombrecia { get; set; }
        [DataMember, XmlAttribute]
        public string codventadevolucion { get; set; }
        [DataMember, XmlAttribute]
        public string nombremaquina { get; set; }
        [DataMember, XmlAttribute]
        public string usuario { get; set; }
        [DataMember]
        [XmlElement(ElementName = "ListVentasDetalle", Type = typeof(List<BE_VentasDetalle>))]
        public List<BE_VentasDetalle> listaVentaDetalle { get; set; }
        [DataMember]
        [XmlElement(ElementName = "ListVentasDetalleUbicacion", Type = typeof(List<BE_VentasDetalleUbicacion>))]
        public List<BE_VentasDetalleUbicacion> listVentasDetalleUbicacion { get; set; }
        /// <summary>
        /// codcomprobante
        /// </summary>
        /// <returns></returns>
        [DataMember, XmlAttribute]
        public string codcomprobante { get; set; }
        [DataMember, XmlAttribute]
        public string tipo { get; set; }
        [DataMember, XmlAttribute]
        public string anombredequien { get; set; }
        [DataMember, XmlAttribute]
        public string concepto { get; set; }
        [DataMember, XmlAttribute]
        public string direccion { get; set; }
        [DataMember, XmlAttribute]
        public string ruc { get; set; }
        [DataMember, XmlAttribute]
        public string codmotivo { get; set; }
        [DataMember, XmlAttribute]
        public string cardcode { get; set; }
        [DataMember, XmlAttribute]
        public bool  flg_gratuito { get; set; }
        /// <summary>
        /// Devuelve el XML
        /// </summary>
        /// <returns></returns>
        public BE_VentaXml RetornaModelo()
        {

            var entiDom = new BE_VentaXml();
            var ser = new Serializador();
            var ms = new MemoryStream();
            ser.SerializarXml(this, ms);
            entiDom.XmlData = Encoding.UTF8.GetString(ms.ToArray());
            ms.Dispose();

            return new BE_VentaXml
            {
                XmlData = entiDom.XmlData,
                RegIdUsuario = this.RegIdUsuario
            };
        }
    }
}
