using Net.Business.Entities;
using System;

namespace Net.Business.DTO
{
    public class DtoVentaCabeceraResponse
    {
        public string codventa { get; set; }
        public string codalmacen { get; set; }
        public string tipomovimiento { get; set; }
        public string codempresa { get; set; }
        public string codtipocliente { get; set; }
        public string nombretipocliente { get; set; }
        public string codatencion { get; set; }
        public string codcomprobante { get; set; }
        public string nombreestado { get; set; }
        public string nombre { get; set; }
        public decimal montopaciente { get; set; }
        public decimal montoaseguradora { get; set; }
        public Boolean flg_gratuito { get; set; }
        public DateTime fechagenera { get; set; }
        public DateTime fechaemision { get; set; }
        public string codpaciente { get; set; }
        public string codcliente { get; set; }
        public string codpedido { get; set; }
        public string estado { get; set; }
        public string usuarioanulacion { get; set; }

        public DtoVentaCabeceraResponse RetornaDtoVentaCabeceraResponse(BE_VentasCabecera value)
        {
            return new DtoVentaCabeceraResponse()
            {
                codventa = value.codventa,
                codalmacen = value.codalmacen,
                tipomovimiento = value.tipomovimiento,
                codempresa = value.codempresa,
                codtipocliente = value.codtipocliente,
                nombretipocliente = value.nombretipocliente,
                codatencion = value.codatencion,
                codcomprobante = value.codcomprobante,
                nombreestado = value.nombreestado,
                nombre = value.nombre,
                montopaciente = value.montopaciente,
                montoaseguradora = value.montoaseguradora,
                flg_gratuito = value.flg_gratuito,
                fechagenera = value.fechagenera,
                fechaemision = value.fechaemision,
                codpaciente = value.codpaciente,
                codcliente = value.codcliente,
                codpedido = value.codpedido,
                estado = value.estado,
                usuarioanulacion = value.usuarioanulacion
            };
        }
    }
}
