using Net.Business.Entities;
using System;

namespace Net.Business.DTO
{
    public class DtoVentasNotaFiltroResponse
    {
        public string codnota { get; set; }
        public string codtipocliente { get; set; }
        public string codcliente { get; set; }
        public string codatencion { get; set; }
        public string anombredequien { get; set; }
        public string concepto { get; set; }
        public string direccion { get; set; }
        public string ruc { get; set; }
        public string nombrepaciente { get; set; }
        public string codpaciente { get; set; }
        public string codcomprobante { get; set; }
        public DateTime fechagenera { get; set; }
        public DateTime fechaemision { get; set; }
        public DateTime fechacancelacion { get; set; }
        public DateTime fechaanulacion { get; set; }
        public decimal porcentajeimpuesto { get; set; }
        public decimal monto { get; set; }
        public decimal montoimpuesto { get; set; }
        public string tipo { get; set; }
        public string estado { get; set; }
        public string coduser { get; set; }
        public string codcentro { get; set; }
        public string numeroplanilla { get; set; }
        public string codventa { get; set; }
        public string codmotivo { get; set; }
        public string codnotae { get; set; }
        public int flg_electronico { get; set; }
        public int flg_gratuito { get; set; }
        public string moneda { get; set; }
        public decimal tipodecambio { get; set; }
        public decimal montodolares { get; set; }
        public decimal montoimpuestodolares { get; set; }
        public string cardcode { get; set; }
        public int regcreateidusuario { get; set; }
        public DateTime regcreate { get; set; }
        public int regupdateidusuario { get; set; }
        public DateTime regupdate { get; set; }
        public int flgeliminado { get; set; }

        public DtoVentasNotaFiltroResponse RetornaDtoVentasNotaFiltroResponse(BE_VentasNota value)
        {
            return new DtoVentasNotaFiltroResponse()
            {
                codnota = value.codnota,
                codtipocliente = value.codtipocliente,
                codcliente = value.codcliente,
                codatencion = value.codatencion,
                anombredequien = value.anombredequien,
                concepto = value.concepto,
                direccion = value.direccion,
                ruc = value.ruc,
                nombrepaciente = value.nombrepaciente,
                codpaciente = value.codpaciente,
                codcomprobante = value.codcomprobante,
                fechagenera = value.fechagenera,
                fechaemision = value.fechaemision,
                fechacancelacion = value.fechacancelacion,
                fechaanulacion = value.fechaanulacion,
                porcentajeimpuesto = value.porcentajeimpuesto,
                monto = value.monto,
                montoimpuesto = value.montoimpuesto,
                tipo = value.tipo,
                estado = value.estado,
                coduser = value.coduser,
                codcentro = value.codcentro,
                numeroplanilla = value.numeroplanilla,
                codventa = value.codventa,
                codmotivo = value.codmotivo,
                codnotae = value.codnotae,
                flg_electronico = value.flg_electronico,
                flg_gratuito = value.flg_gratuito,
                moneda = value.moneda,
                tipodecambio = value.tipodecambio,
                montodolares = value.montodolares,
                montoimpuestodolares = value.montoimpuestodolares,
                cardcode = value.cardcode,
                regcreateidusuario = value.regcreateidusuario,
                regcreate = value.regcreate,
                regupdateidusuario = value.regupdateidusuario,
                regupdate = value.regupdate,
                flgeliminado = value.flgeliminado
            };
        }
    }
}
