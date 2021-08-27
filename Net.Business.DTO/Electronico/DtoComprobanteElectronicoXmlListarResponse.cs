using Net.Business.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Net.Business.DTO
{
    public class DtoComprobanteElectronicoXmlListarResponse
    {
        public IEnumerable<DtoComprobanteElectronicoXmlResponse> ListaComprobanteElectronicoXml { get; set; }

        public DtoComprobanteElectronicoXmlListarResponse RetornarListaComprobanteElectronicoXml(IEnumerable<BE_TCIComprobanteElectronico> listaComprobanteElectronicoXml)
        {
            IEnumerable<DtoComprobanteElectronicoXmlResponse> lista = (
                from value in listaComprobanteElectronicoXml
                select new DtoComprobanteElectronicoXmlResponse
                {
                    codcomprobante = value.codcomprobante,
                    codcomprobantee = value.codcomprobantee,
                    tipoplantilla = value.tipoplantilla,
                    tipocomp_sunat = value.tipocomp_sunat,
                    tipocomp_tci = value.tipocomp_tci,
                    estado = value.estado,
                    nombreestado = value.nombreestado,
                    codtipocliente = value.codtipocliente,
                    tipomovimiento = value.tipomovimiento,
                    codventa = value.codventa,
                    fechaemision = value.fechaemision,
                    anombrede = value.anombrede,
                    direccion = value.direccion,
                    ruc = value.ruc,
                    tipodocidentidad_sunat = value.tipodocidentidad_sunat,
                    ruc_sunat = value.ruc_sunat,
                    receptor_correo = value.receptor_correo,
                    empresa_ruc = value.empresa_ruc,
                    empresa_tipodocidentidad = value.empresa_tipodocidentidad,
                    empresa_nombre = value.empresa_nombre,
                    empresa_dicreccion = value.empresa_dicreccion,
                    empresa_telefono = value.empresa_telefono,
                    concepto = value.concepto,
                    codatencion = value.codatencion,
                    codpaciente = value.codpaciente,
                    nombrepaciente = value.nombrepaciente,
                    nombretitular = value.nombretitular,
                    nombrecia = value.nombrecia,
                    codpoliza = value.codpoliza,
                    observaciones = value.observaciones,
                    texto1 = value.texto1,
                    suc_direccion = value.suc_direccion,
                    tipopagotxt = value.tipopagotxt,
                    c_moneda = value.c_moneda,
                    c_nombremoneda = value.c_nombremoneda,
                    c_simbolomoneda = value.c_simbolomoneda,
                    c_montodsctoplan = value.c_montodsctoplan,
                    c_montoafecto = value.c_montoafecto,
                    c_montoinafecto = value.c_montoinafecto,
                    c_montoexonerado = value.c_montoexonerado,
                    c_montogratuito = value.c_montogratuito,
                    c_montoigv = value.c_montoigv,
                    c_montoneto = value.c_montoneto,
                    porcentajeimpuesto = value.porcentajeimpuesto,
                    porcentajecoaseguro = value.porcentajecoaseguro,
                    flg_gratuito = value.flg_gratuito,
                    d_orden = value.d_orden,
                    d_unidad = value.d_unidad,
                    d_codproducto = value.d_codproducto,
                    nombreproducto = value.nombreproducto,
                    d_cant_sunat = value.d_cant_sunat,
                    d_stockfraccion = value.d_stockfraccion,
                    d_cantidad = value.d_cantidad,
                    d_cantidad_fraccion = value.d_cantidad_fraccion,
                    d_valorVVP = value.d_valorVVP,
                    d_precioventaPVP = value.d_precioventaPVP,
                    d_porcdctoproducto = value.d_porcdctoproducto,
                    d_porcdctoplan = value.d_porcdctoplan,
                    d_preciounidadcondcto = value.d_preciounidadcondcto,
                    d_montototal = value.d_montototal,
                    d_montopaciente = value.d_montopaciente,
                    d_montoaseguradora = value.d_montoaseguradora,
                    d_ventaunitario_sinigv = value.d_ventaunitario_sinigv,
                    d_ventaunitario_conigv = value.d_ventaunitario_conigv,
                    d_dscto_sinigv = value.d_dscto_sinigv,
                    d_dscto_conigv = value.d_dscto_conigv,
                    d_total_sinigv = value.d_total_sinigv,
                    d_total_conigv = value.d_total_conigv,
                    d_total_sinigv2 = value.d_total_sinigv2,
                    d_montoigv = value.d_montoigv,
                    d_determinante = value.d_determinante,
                    d_codigotipoprecio = value.d_codigotipoprecio,
                    d_afectacionigv = value.d_afectacionigv,
                    d_montobase = value.d_montobase,
                    tipo_operacion = value.tipo_operacion,
                    hora_emision = value.hora_emision,
                    total_impuesto = value.total_impuesto,
                    total_valorventa = value.total_valorventa,
                    total_precioventa = value.total_precioventa,
                    vs_ubl = value.vs_ubl,
                    mt_total = value.mt_total,
                    mtg_Base = value.mtg_Base,
                    mtg_ValorImpuesto = value.mtg_ValorImpuesto,
                    mtg_Porcentaje = value.mtg_Porcentaje,
                    d_dscto_unitario = value.d_dscto_unitario,
                    d_total_condsctoigv = value.d_total_condsctoigv,
                    d_dscto_montobase = value.d_dscto_montobase,
                    CodigoEstabSUNAT = value.CodigoEstabSUNAT,
                    CodDistrito = value.CodDistrito,
                    forma_pago = value.forma_pago,
                    cod_tributo = value.cod_tributo,
                    cod_afectacionIGV = value.cod_afectacionIGV,
                    des_tributo = value.des_tributo,
                    cod_UN = value.cod_UN,
                    CodProductoSUNAT = value.CodProductoSUNAT,
                    cuadre = value.cuadre
                }
            );

            return new DtoComprobanteElectronicoXmlListarResponse() { ListaComprobanteElectronicoXml = lista };
        }
    }
}
