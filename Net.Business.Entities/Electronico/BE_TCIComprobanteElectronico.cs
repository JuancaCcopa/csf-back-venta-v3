﻿using System;

namespace Net.Business.Entities
{
    public class BE_TCIComprobanteElectronico
    {
        public string generarnota { get; set; }
        public string mensaje { get; set; }


        public string codcomprobante { get; set; }
        public string codcomprobantee { get; set; }
        public string tipoplantilla { get; set; }
        public string tipocomp_sunat { get; set; }
        public string tipocomp_tci { get; set; }
        public string estado { get; set; }
        public string nombreestado { get; set; }
        public string codtipocliente { get; set; }
        public string tipomovimiento { get; set; }
        public string codventa { get; set; }
        public DateTime fechaemision { get; set; }
        public string anombrede { get; set; }
        public string direccion { get; set; }
        public string ruc { get; set; }
        public string tipodocidentidad_sunat { get; set; }
        public string ruc_sunat { get; set; }
        public string receptor_correo { get; set; }
        public string empresa_ruc { get; set; }
        public string empresa_tipodocidentidad { get; set; }
        public string empresa_nombre { get; set; }
        public string empresa_dicreccion { get; set; }
        public string empresa_telefono { get; set; }
        public string concepto { get; set; }
        public string codatencion { get; set; }
        public string codpaciente { get; set; }
        public string nombrepaciente { get; set; }
        public string nombretitular { get; set; }
        public string nombrecia { get; set; }
        public string codpoliza { get; set; }
        public string observaciones { get; set; }
        public string texto1 { get; set; }
        public string suc_direccion { get; set; }
        public string tipopagotxt { get; set; }


        public string ref_electronico { get; set; }
        public string ref_codcomprobantee { get; set; }
        public string ref_tipocomp_sunat { get; set; }
        public DateTime? ref_fechaemision { get; set; }
        public string c_codmotivo_sunat { get; set; }
        public string c_motivonota { get; set; }


        public string c_moneda { get; set; }
        public string c_nombremoneda { get; set; }
        public string c_simbolomoneda { get; set; }
        public decimal c_montodsctoplan { get; set; }
        public decimal c_montoafecto { get; set; }
        public decimal c_montoinafecto { get; set; }
        public decimal c_montoexonerado { get; set; }
        public decimal c_montogratuito { get; set; }
        public decimal c_montoigv { get; set; }
        public decimal c_montoneto { get; set; }
        public decimal porcentajeimpuesto { get; set; }
        public decimal porcentajecoaseguro { get; set; }
        public string flg_gratuito { get; set; }

        public string d_orden { get; set; }
        public string d_unidad { get; set; }
        public string d_codproducto { get; set; }
        public string nombreproducto { get; set; }
        public decimal d_cant_sunat { get; set; }
        public int d_stockfraccion { get; set; }
        public int d_cantidad { get; set; }
        public int d_cantidad_fraccion { get; set; }
        public decimal d_valorVVP { get; set; }
        public decimal d_precioventaPVP { get; set; }
        public decimal d_porcdctoproducto { get; set; }
        public decimal d_porcdctoplan { get; set; }
        public decimal d_preciounidadcondcto { get; set; }
        public decimal d_montototal { get; set; }
        public decimal d_montopaciente { get; set; }
        public decimal d_montoaseguradora { get; set; }
        public decimal d_ventaunitario_sinigv { get; set; }
        public decimal d_ventaunitario_conigv { get; set; }
        public decimal d_dscto_sinigv { get; set; }
        public decimal d_dscto_conigv { get; set; }
        public decimal d_total_sinigv { get; set; }
        public decimal d_total_conigv { get; set; }
        public decimal d_total_sinigv2 { get; set; }
        public decimal d_montoigv { get; set; }
        public string d_determinante { get; set; }
        public string d_codigotipoprecio { get; set; }
        public string d_afectacionigv { get; set; }
        public decimal d_montobase { get; set; }
        public string tipo_operacion { get; set; }
        public string hora_emision { get; set; }
        public decimal total_impuesto { get; set; }
        public decimal total_valorventa { get; set; }
        public decimal total_precioventa { get; set; }
        public string vs_ubl { get; set; }
        public decimal mt_total { get; set; }
        public decimal mtg_Base { get; set; }
        public decimal mtg_ValorImpuesto { get; set; }
        public decimal mtg_Porcentaje { get; set; }
        public decimal d_dscto_unitario { get; set; }
        public decimal d_total_condsctoigv { get; set; }
        public decimal d_dscto_montobase { get; set; }
        public string CodigoEstabSUNAT { get; set; }
        public string CodDistrito { get; set; }
        public string forma_pago { get; set; }
        public string cod_tributo { get; set; }
        public string cod_afectacionIGV { get; set; }
        public string des_tributo { get; set; }
        public string cod_UN { get; set; }
        public string CodProductoSUNAT { get; set; }
        public string cuadre { get; set; }
        //extra
        public string strXml { get; set; }
    }
}