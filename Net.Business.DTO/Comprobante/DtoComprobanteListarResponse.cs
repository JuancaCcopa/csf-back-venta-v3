using Net.Business.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Net.Business.DTO
{
    public class DtoComprobanteListarResponse
    {
        public IEnumerable<DtoComprobanteResponse> ListaVentaCabecera { get; set; }

        public DtoComprobanteListarResponse RetornarListaComprobante(IEnumerable<BE_Comprobante> listaArticulos)
        {
            IEnumerable<DtoComprobanteResponse> lista = (
            from value in listaArticulos
            select new DtoComprobanteResponse
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
            });

            return new DtoComprobanteListarResponse() { ListaVentaCabecera = lista };
        }
    }
}
