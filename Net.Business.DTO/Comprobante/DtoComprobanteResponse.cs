using Net.Business.Entities;
using System;
using System.Collections.Generic;

namespace Net.Business.DTO
{
    public class DtoComprobanteResponse
    {
        public DtoComprobanteResponse()
        {
            this.cuadredecaja = new HashSet<DtoComprobanteTipoPagoRegistrar>();
        }
        public string codcomprobante { get; set; }
        public string codcomprobantee { get; set; }
        public string estado { get; set; }
        public string codventa { get; set; }
        public string codtipocliente { get; set; }
        public string anombrede { get; set; }
        public decimal montototal { get; set; }
        public double montototaldolares { get; set; }
        public string moneda { get; set; }
        public DateTime fechagenera { get; set; }
        public DateTime fechaemision { get; set; }
        public DateTime? fechacancelacion { get; set; }
        public DateTime? fechaanulacion { get; set; }
        public string codcliente { get; set; }
        public string numeroplanilla { get; set; }
        public string nombreestado { get; set; }
        public string nombretipocliente { get; set; }
        public string codatencion { get; set; }
        public bool flg_gratuito { get; set; }
        public string direccion { get; set; }
        public string ruc { get; set; }
        public decimal porcentajeimpuesto { get; set; }
        public string cardcode { get; set; }
        public decimal tipodecambio { get; set; }
        public string correo { get; set; }
        public string cliente { get; set; }
        public string tipdocidentidad { get; set; }
        public string docidentidad { get; set; }
        public string nombretipdocidentidad { get; set; }
        public string codplan { get; set; }
        public string nombreplan { get; set; }
        public decimal porcentajecoaseguro { get; set; }
        public decimal porcentajedctoplan { get; set; }
        public decimal montoigv { get; set; }
        public bool flgElectronico { get; set; }
        public ICollection<DtoComprobanteTipoPagoRegistrar> cuadredecaja { get; set; }
        public DtoComprobanteResponse RetornaDtoVentaCabeceraResponse(BE_Comprobante value)
        {
            return new DtoComprobanteResponse()
            {
                codcomprobante = value.codcomprobante,
                codcomprobantee = value.codcomprobantee,
                estado = value.estado,
                codventa = value.codventa,
                codtipocliente = value.codtipocliente,
                anombrede = value.anombrede,
                montototal = value.montototal,
                montototaldolares = value.montototaldolares,
                moneda = value.moneda,
                fechagenera = value.fechagenera,
                fechaemision = value.fechaemision,
                fechacancelacion = value.fechacancelacion,
                fechaanulacion = value.fechaanulacion,
                codcliente = value.codcliente,
                numeroplanilla = value.numeroplanilla,
                nombreestado = value.nombreestado,
                nombretipocliente = value.nombretipocliente,
                codatencion = value.codatencion,
                flg_gratuito = value.flg_gratuito,
                direccion = value.direccion,
                ruc = value.ruc,
                porcentajeimpuesto = value.porcentajeimpuesto,
                cardcode = value.cardcode,
                tipodecambio = value.tipodecambio
            };
        }

        public DtoComprobanteResponse RetornaDtoComprobanteResponse(BE_Comprobante value)
        {
            IList<DtoComprobanteTipoPagoRegistrar> lista = new List<DtoComprobanteTipoPagoRegistrar>();
            DtoComprobanteTipoPagoRegistrar itemReg;
            foreach (var item in value.cuadreCaja)
            {
                itemReg = new DtoComprobanteTipoPagoRegistrar()
                {
                    codTipoPago = item.tipopago,
                    nombreTipoPago = item.nombretipopago,
                    codEntidad = item.nombreentidad,
                    nombreEntidad = item.descripcionentidad,
                    nroOperacion = item.numeroentidad,
                    montoDolar = (item.moneda == "D") ? item.montodolares : 0,
                    montoSoles = (item.moneda == "S") ? item.monto : 0,
                    montoMn = item.monto + item.tipodecambio,
                    codTerminal = item.codterminal,
                    terminal = item.numeroterminal
                };

                lista.Add(itemReg);
            }

            return new DtoComprobanteResponse()
            {
                codventa = value.codventa,
                codcomprobante = value.codcomprobante,
                //codcomprobantee = value.codcomprobantee,
                codtipocliente = value.codtipocliente,
                codcliente = value.codcliente,
                cliente = value.anombrede,
                direccion = value.direccion,
                ruc = value.ruc,
                correo = value.correo,
                tipdocidentidad = value.tipdocidentidad,
                docidentidad = value.docidentidad,
                nombretipdocidentidad = value.nombretipdocidentidad,
                fechaemision = value.fechaemision,
                nombretipocliente = value.nombretipocliente,
                codplan = value.codplan,
                nombreplan = value.nombreplan,
                porcentajedctoplan = value.porcentajedctoplan,
                porcentajecoaseguro = value.porcentajecoaseguro,
                estado = value.estado,
                montototal = value.montototal,
                montoigv = value.montoigv,
                cardcode = value.cardcode,
                flg_gratuito = value.flg_gratuito,
                porcentajeimpuesto = value.porcentajeimpuesto,
                flgElectronico = value.flg_electronico,
                moneda = value.moneda,
                nombreestado = value.nombreestado,
                cuadredecaja = lista
                //montototaldolares = value.montototaldolares,
                //moneda = value.moneda,
                //fechagenera = value.fechagenera,
                //fechaemision = value.fechaemision,
                //fechacancelacion = value.fechacancelacion,
                //fechaanulacion = value.fechaanulacion,
                //numeroplanilla = value.numeroplanilla,
                //nombreestado = value.nombreestado,
                //codatencion = value.codatencion,
            };

        }
    }
}
