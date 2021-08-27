using Microsoft.VisualBasic;
using MSXML2;
using Net.Business.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Net.TCI
{
    public class FacturacionElectronica
    {
        public string GetComprobateElectronicoXml(List<BE_TCIComprobanteElectronico> response)
        {
            //
            string XMLT;
            string XML_1;
            string XML_2;
            string XML_3;
            string XML_4;
            string XML_5;
            string XML_6;
            string XML_7;
            string XML_8;
            string XML_9;

            string XML_11;
            string XML_12; //'TMACASSI 22/02/2019 Montos Totales - Gravado
            string XML_Detalle;
            string secuencia = "\r";

            //string wAdoEfac As New ADODB.Recordset
            //string wDatosEfac As New ADODB.Recordset
            string wSql;
            string wCuadre;
            //'--<I-Variables XML> --
            string wCodcomprobante;
            string wTipoPlantilla;
            string wTipoComp_Sunat;
            string wSerie;
            string wNumero;
            string wFechaEmision;
            string wMoneda;
            string wObservaciones;
            string wReceptor_Correo;
            string wReceptor_TipoDocIdentidad;
            string wReceptor_Ruc;
            string wReceptor_Anombrede;
            string wReceptor_Direccion;
            string wTexto1;
            string wSucursal_Direccion;
            string wTextoGratuito; //  'AGARCIA.25/06/2016
            string wFlgGratuito; //    'AGARCIA.12/07/2016
            //'i-EnEmpresa
            string wEmpresa_Ruc;
            string wEmpresa_TipoDocIdentidad;
            string wEmpresa_Nombre;
            string wEmpresa_Direccion;
            string wEmpresa_Telefono;
            //'f-EnEmpresa
            //'i-Salud
            string wConcepto;
            string wCodventa;   //'.ENSalud.Liquidacion
            string wCodAtencion;
            string wCodPaciente;
            string wNombrePaciente;
            string wNombreTitular;
            string wNombreContratante;
            string wPoliza;
            string wCoaseguro;
            //'f-Salud
            string wPorcentajeIGV;
            string wMontoNeto;   //'con Igv
            string wMontoTotal;  //'sin igv
            string wMontoGravado;
            string wMontoInafecto;
            string wMontoExonerado;
            string wMontoGratuito;
            //'Impuestos Cabecera:
            string wMontoIgv;
            string wImporteExplicito;
            string wImp_CodigoTributo;
            string wImp_Destributo;
            string wImp_CodigoUN;
            //'Referencias:
            string wRef_CodComprobanteE;
            string wRef_Serie;
            string wRef_Numero;
            string wRef_TipoComp_Sunat;
            string wRef_FechaEmision;
            string wCodMotivo_Sunat;
            string wMotivoNota;
            //'--<F-Variables XML> --
            string wTipo_operacion;
            string wHora_emision;
            string wTotal_impuesto;
            string wTotal_valorventa;
            string wTotal_precioventa;
            string wVs_ubl;
            string wMt_total;
            string wMtg_Base;
            string wMtg_ValorImpuesto;
            string wMtg_Porcentaje;

            XMLT = "";
            XML_1 = "";
            XML_2 = "";
            XML_3 = "";
            XML_4 = "";
            XML_5 = "";
            XML_6 = "";
            XML_7 = "";
            XML_8 = "";
            XML_9 = "";
            XML_11 = "";
            XML_12 = "";
            XML_Detalle = "";

            wCuadre = "";                    //'ref
            wTipoPlantilla = "EST";          //'ENComprobante.TipoPlantilla
            wTipoComp_Sunat = "";            //'ENComprobante.TipoComprobante
            wSerie = "";                     //'ENComprobante.Serie
            wNumero = "";                    //'ENComprobante.Numero
            wFechaEmision = "";              //'ENComprobante.FechaEmision
            //'wMoneda = "PEN"               //'ENComprobante.Moneda; Catalogo02: "PEN" = Nuevo Sol, "USD" = Dolares; En Farmacia todas las ventas son en SOLES
            wMoneda = "";                    //'AGARCIA.25/11/2016


            wReceptor_Anombrede = "";        //'ENComprobante.RazonSocial; Tipo Dato : an(100)
            wReceptor_Direccion = "";        //'ENComprobante.ENReceptor.Calle;   Tipo Dato : an(500)
            wReceptor_TipoDocIdentidad = ""; //'ENComprobante.TipoDocumentoIdentidad; Catalogo no. 06: Códigos de tipos de documentos de identidad
            wReceptor_Ruc = "";              //'ENComprobante.Ruc; Tipo Dato : an(11)
            wReceptor_Correo = "";           //'ENComprobante.CorreoElectronico;                         FALTA DEFINIR
            //'I-EnEmpresa
            wEmpresa_Ruc = "";               //'ENComprobante.ENEmpresa.Ruc
            wEmpresa_TipoDocIdentidad = "6"; //'ENComprobante.ENEmpresa.TipoDocumentoIdentidad
            wEmpresa_Nombre = "";            //'ENComprobante.ENEmpresa.RazonSocial
            wEmpresa_Direccion = "";         //'ENComprobante.ENEmpresa.Calle
            wEmpresa_Telefono = "";          //'ENComprobante.ENEmpresa.Telefono
            //'F-EnEmpresa
            //'I-salud
            wConcepto = "";                  //'ENComprobante.ENSalud.Concepto
            wCodventa = "";                  //'ENComprobante.ENSalud.Liquidacion
            wCodAtencion = "";               //'ENComprobante.ENSalud.OA
            wCodPaciente = "";               //'ENComprobante.ENSalud.HistoriaClinica
            wNombrePaciente = "";            //'ENComprobante.ENSalud.Paciente
            wNombreTitular = "";             //'ENComprobante.ENSalud.Titular
            wNombreContratante = "";         //'ENComprobante.ENSalud.Empresa
            wPoliza = "";                    //'ENComprobante.ENSalud.Poliza
            wCoaseguro = "0";                //'ENComprobante.ENSalud.Coaseguro
            //'F-salud
            wObservaciones = "";             //'ENComprobante.Glosa; Tipo Dato: an(1000)
            wTexto1 = "";                    //'ENComprobante.ENTexto.texto1; Tipo Dato : an(1000);      FALTA???
            wSucursal_Direccion = "";        //'ENComprobante.ENSucursal.Direccion; Tipo Dato : an(300); FALTA??
            //'ENComprobante.ENComprobantePropiedadesAdicionales.Codigo;                                FALTA?
            //'ENComprobante.ENComprobantePropiedadesAdicionales.Valor;                                 FALTA?
            wTextoGratuito = "";             //'ENComprobante.ENComprobantePropiedadesAdicionales.Valor;  an(100)

            wPorcentajeIGV = "0";            //'
            wMontoNeto = "0";                //'ENComprobante.ImporteTotal; Formato n(12,2) Ej:12000.00
            //'wMontoTotal = "0;"            // 'Subtotal(sinIgv); catalogo_Nro_14
            wMontoGravado = "0";             //'ENComprobante.ENComprobanteMontosAdicionalesObligatorios.Codigo =10001; para Total Valor Venta - Operaciones Gravadas  = Subtotal(sinIgv); catalogo_Nro_14
            wMontoInafecto = "0";            //'ENComprobante.ENComprobanteMontosAdicionalesObligatorios.Codigo =10002; para Total Valor Venta - Operaciones Inafectas
            wMontoExonerado = "0";           //'ENComprobante.ENComprobanteMontosAdicionalesObligatorios.Codigo =10003; para Total Valor Venta - Operaciones Exoneradas
            wMontoGratuito = "0";            //'ENComprobante.ENMontosAdicionalesOtros.Codigo = 1004; para Total Valor Venta - Operaciones Gratuitas
            //'Impuestos Cabecera:
            wMontoIgv = "0";                  //'ENComprobante.ENComprobanteImpuestos.ImporteTributo; Monto del Impuesto ;  Tipo Dato: n(12,2) Ejemplo: 12000.00
            wImporteExplicito = "0";       //'ENComprobante.ENComprobanteImpuestos.ImporteExplicito; Monto del Impuesto (es el mismo que ImporteTributo); Tipo Dato: an(12,2) Ejemplo: 12000.00
            wImp_CodigoTributo = "1000";     //'ENComprobante.ENComprobanteImpuestos.CodigoTributo; De acuerdo al catálogo 5: para IGV es 1000, ISC es 2000
            wImp_Destributo = "IGV";         //'ENComprobante.ENComprobanteImpuestos.Destributo; Nombre del tributo; De acuerdo al catálogo 5.
            wImp_CodigoUN = "VAT";           //'ENComprobante.ENComprobanteImpuestos.CodigoUN; De acuerdo al catálogo 5: para igv se coloca "VAT", para isc se coloca "EXC"

            wRef_CodComprobanteE = "";       //'ENComprobante.ENComprobanteNotaDocRef.Serie, ENComprobante.ENComprobanteNotaDocRef.Numero;  ENComprobante.ENComprobanteMotivoDocumento.SerieDocRef, ENComprobante.ENComprobanteMotivoDocumento.NroDocRef
            wRef_Serie = "";                 //'ENComprobante.ENComprobanteNotaDocRef.Serie  ; ENComprobante.ENComprobanteMotivoDocumento.SerieDocRef
            wRef_Numero = "";                //'ENComprobante.ENComprobanteNotaDocRef.Numero ; ENComprobante.ENComprobanteMotivoDocumento.NroDocRef
            wRef_TipoComp_Sunat = "";        //'ENComprobante.ENComprobanteNotaDocRef.TipoComprobante
            wRef_FechaEmision = "";          //'ENComprobante.ENComprobanteNotaDocRef.FechaDocRef
            wCodMotivo_Sunat = "";           //'ENComprobante.ENComprobanteMotivoDocumento.CodigoMotivoEmision
            wMotivoNota = "";                //'ENComprobante.ENComprobanteMotivoDocumento.ENComprobanteMotivoDocumentoSustento.Sustento


            wFlgGratuito = "";               //'ref AGARCIA.11/07/2016


            wTipo_operacion = "";            //'ENComprobante.TipoOperacion
            wHora_emision = "";              //'ENComprobante.HoraEmision
            wTotal_impuesto = "";            //'ENComprobante.TotalImpuesto
            wTotal_valorventa = "";          //'ENComprobante.TotalValorVenta
            wTotal_precioventa = "";         //'ENComprobante.TotalPrecioVenta
            wVs_ubl = "";                    //'ENComprobante.VersionUbl
            wMt_total = "";                  //'ENComprobante.MontosTotales.Total
            wMtg_Base = "";                  //'ENComprobante.MontosTotales.Gravado.GravadoIGV.Base
            wMtg_ValorImpuesto = "";         //'ENComprobante.MontosTotales.Gravado.GravadoIGV.ValorImpuesto
            wMtg_Porcentaje = "";            //'ENComprobante.MontosTotales.Gravado.GravadoIGV.Porcentaje

            wCodcomprobante = response[0].codcomprobante;

            if (response.Count > 0)
            {
                wTipoPlantilla = response[0].tipoplantilla;
                wTipoComp_Sunat = response[0].tipocomp_sunat.Substring(0, 2);
                if (wCodcomprobante.Substring(0, 1) == "F" | wCodcomprobante.Substring(0, 1) == "B")                //'catalogo: '01'=factura, '03'=boleta
                {
                    wSerie = response[0].codcomprobantee.Substring(0, 4);
                    wNumero = response[0].codcomprobantee.Substring(4);
                }
                else if (wCodcomprobante.Substring(0, 1) == "C" | wCodcomprobante.Substring(0, 1) == "D")
                {
                    wSerie = response[0].codcomprobantee.Substring(0, 4);
                    wNumero = response[0].codcomprobantee.Substring(4);
                }
                wFechaEmision = response[0].fechaemision.ToString("yyyy-mm-dd");
                wMoneda = response[0].c_moneda.Trim();                                                              //'ENComprobante.Moneda
                if (wMoneda.Equals("D")) { wMoneda = "USD"; }
                if (wMoneda.Equals("S")) { wMoneda = "PEN"; }

                wReceptor_Anombrede = response[0].anombrede;                                                        //'RazonSocial
                wReceptor_Direccion = response[0].direccion;                                                        //'ENReceptor.Calle
                wReceptor_TipoDocIdentidad = response[0].tipodocidentidad_sunat;
                wReceptor_Ruc = response[0].ruc_sunat;
                wReceptor_Correo = response[0].receptor_correo;
                //'I-EnEmpresa
                wEmpresa_Ruc = response[0].empresa_ruc;                                                             //'ENComprobante.ENEmpresa.Ruc
                wEmpresa_TipoDocIdentidad = response[0].empresa_tipodocidentidad;                                   //'ENComprobante.ENEmpresa.TipoDocumentoIdentidad
                wEmpresa_Nombre = response[0].empresa_nombre;                                                       //'ENComprobante.ENEmpresa.RazonSocial
                wEmpresa_Direccion = response[0].empresa_dicreccion;                                                //'ENComprobante.ENEmpresa.Calle
                wEmpresa_Telefono = response[0].empresa_telefono;                                                   //'ENComprobante.ENEmpresa.Telefono
                //'F-EnEmpresa
                //'I-EnSalud
                wConcepto = response[0].concepto;                                                                   //'ENComprobante.ENSalud.Concepto
                wCodventa = response[0].codventa;                                                                   //'ENComprobante.ENSalud.Liquidacion
                wCodAtencion = response[0].codatencion;                                                             //'ENComprobante.ENSalud.OA
                wCodPaciente = response[0].codpaciente;                                                             //'ENComprobante.ENSalud.HistoriaClinica
                wNombrePaciente = response[0].nombrepaciente;                                                       //'ENComprobante.ENSalud.Paciente
                wNombreTitular = "";                                                                                //'ENComprobante.ENSalud.Titular
                wNombreContratante = "";                                                                            //'ENComprobante.ENSalud.Empresa
                wPoliza = "";                                                                                       //'ENComprobante.ENSalud.Poliza
                wCoaseguro = response[0].porcentajecoaseguro.ToString();                                            //'ENComprobante.ENSalud.Coaseguro
                //'F-EnSalud

                wObservaciones = response[0].observaciones;
                wTexto1 = response[0].texto1;                                                                       //'ENTexto.texto1
                wSucursal_Direccion = response[0].suc_direccion;                                                    //'ENSucursal.Direccion
                //'ENComprobante.ENComprobantePropiedadesAdicionales.Codigo
                //'ENComprobante.ENComprobantePropiedadesAdicionales.Valor
                wTextoGratuito = "TRANSFERENCIA GRATUITA DE UN BIEN Y/O SERVICIO PRESTADO GRATUITAMENTE";           //'ENComprobante.ENComprobantePropiedadesAdicionales.Valor
                wPorcentajeIGV = response[0].porcentajeimpuesto.ToString();
                wMontoNeto = response[0].c_montoneto.ToString("N2");                                                //'ENComprobante.ImporteTotal
                wMontoGravado = response[0].c_montoafecto.ToString("N2");
                wMontoInafecto = response[0].c_montoinafecto.ToString("N2");
                wMontoExonerado = "0";
                wMontoGratuito = response[0].c_montogratuito.ToString("N2");

                //'Impuestos Cabecera:
                wTipo_operacion = response[0].tipo_operacion;                                                       //'ENComprobante.TipoOperacion
                wHora_emision = response[0].hora_emision;                                                           //'ENComprobante.HoraEmision
                wTotal_impuesto = response[0].total_impuesto.ToString("N2");                                        //'ENComprobante.TotalImpuesto
                wTotal_valorventa = response[0].total_valorventa.ToString("N2");                                    //'ENComprobante.TotalValorVenta
                wTotal_precioventa = response[0].total_precioventa.ToString("N2");                                  //'ENComprobante.TotalPrecioVenta
                wVs_ubl = response[0].vs_ubl;                                                                       //'ENComprobante.VersionUbl
                wMt_total = response[0].mt_total.ToString("N2");                                                    //'ENComprobante.MontosTotales.Total
                wMtg_Base = response[0].mtg_Base.ToString("N2");                                                    //'ENComprobante.MontosTotales.Gravado.GravadoIGV.Base
                wMtg_ValorImpuesto = response[0].mtg_ValorImpuesto.ToString("N2");                                  //'ENComprobante.MontosTotales.Gravado.GravadoIGV.ValorImpuesto
                wMtg_Porcentaje = response[0].mtg_Porcentaje.ToString("N2");                                        //'ENComprobante.MontosTotales.Gravado.GravadoIGV.Porcentaje

                wMontoIgv = response[0].c_montoigv.ToString("N2");                                                  //'ENComprobante.ENComprobanteImpuestos.ImporteTributo; es el monto del impuesto
                wImporteExplicito = wMontoIgv;

                //'wImp_CodigoTributo
                //'wImp_Destributo
                //'wImp_CodigoUN

                //'Notas_Referencia:
                if (wCodcomprobante.Substring(0, 1) == "C" | wCodcomprobante.Substring(0, 1) == "D")
                {
                    wRef_CodComprobanteE = response[0].ref_codcomprobantee;                                        //'ENComprobante.ENComprobanteNotaDocRef.Serie, ENComprobante.ENComprobanteNotaDocRef.Numero;  ENComprobante.ENComprobanteMotivoDocumento.SerieDocRef, ENComprobante.ENComprobanteMotivoDocumento.NroDocRef
                    wRef_TipoComp_Sunat = response[0].ref_tipocomp_sunat;                                          //'ENComprobante.ENComprobanteNotaDocRef.TipoComprobante
                    wRef_FechaEmision = Convert.ToDateTime(response[0].ref_fechaemision).ToString("yyyy-mm-dd");
                    wCodMotivo_Sunat = response[0].c_codmotivo_sunat;                                              //'ENComprobante.ENComprobanteMotivoDocumento.CodigoMotivoEmision
                    wMotivoNota = response[0].c_motivonota;
                    if (wRef_CodComprobanteE.Length == 12)
                    {
                        wRef_Serie = response[0].codcomprobantee.Substring(0, 4);
                        wRef_Numero = response[0].codcomprobantee.Substring(4);
                    }
                    else if (wRef_CodComprobanteE.Length == 11)
                    {
                        wRef_Serie = response[0].codcomprobantee.Substring(0, 3);
                        wRef_Numero = response[0].codcomprobantee.Substring(3);
                    }
                }
                wFlgGratuito = response[0].flg_gratuito;

                if (wFlgGratuito == "1")
                {
                    wTotal_valorventa = "0.00";
                    wTotal_precioventa = "0.00";
                    wTotal_impuesto = "0.00";
                }

                //'--- Montos AdicionalesObligatorios: Del Catalogo No.14 (Otros conceptos tributarios): Se considera los códigos 1001(Total ventas gravadas), 1002(inafectas) y 1003(exoneradas).


                XML_1 = "<ENComprobanteMontosAdicionalesObligatorios>" + secuencia +
                            "<Codigo>1001</Codigo>" + secuencia +
                            "<Monto>" + wMontoGravado + "</Monto>" + secuencia +
                            "</ENComprobanteMontosAdicionalesObligatorios>" + secuencia +
                            "<ENComprobanteMontosAdicionalesObligatorios>" + secuencia +
                            "<Codigo>1002</Codigo>" + secuencia +
                            "<Monto>" + wMontoInafecto + "</Monto>" + secuencia +
                            "</ENComprobanteMontosAdicionalesObligatorios>" + secuencia +
                            "<ENComprobanteMontosAdicionalesObligatorios>" + secuencia +
                            "<Codigo>1003</Codigo>" + secuencia +
                            "<Monto>" + wMontoExonerado + "</Monto>" + secuencia +
                        "</ENComprobanteMontosAdicionalesObligatorios>" + secuencia;

                //Se agrega - TCI: APLICA solo para gratuitos: a)si a FT/BV, b)no a NC-Gratuita
                if (wFlgGratuito == "1" && (wCodcomprobante.Substring(0, 1) == "F" | wCodcomprobante.Substring(0, 1) == "B"))
                {
                    XML_11 = "<ENComprobanteMontosAdicionalesOtros>" + secuencia +
                                 "<Codigo>1004</Codigo>" + secuencia +
                                 "<Monto>" + wMontoGratuito + "</Monto>" + secuencia +
                             "</ENComprobanteMontosAdicionalesOtros>" + secuencia;
                }

                //'--- Impuestos Globales
                XML_2 = "<ENComprobanteImpuestos>" + secuencia +
                            "<ImporteTributo>" + wMontoIgv + "</ImporteTributo>" + secuencia +
                            "<ImporteExplicito>" + wMontoIgv + "</ImporteExplicito>" + secuencia +
                            "<CodigoTributo>" + wImp_CodigoTributo + "</CodigoTributo>" + secuencia +
                            "<DesTributo>" + wImp_Destributo + "</DesTributo>" + secuencia +
                            "<CodigoUN>" + wImp_CodigoUN + "</CodigoUN>" + secuencia +
                        "</ENComprobanteImpuestos>" + secuencia;


                XML_3 = "<Salud>" + secuencia +
                            "<Concepto>" + wConcepto + "</Concepto>" + secuencia +
                            "<Codigo>" + wCodventa + "</Codigo>" + secuencia +
                            "<OAM>" + wCodAtencion + "</OAM>" + secuencia +
                            "<Historia>" + wCodPaciente + "</Historia>" + secuencia +
                            "<Paciente>" + wNombrePaciente + "</Paciente>" + secuencia +
                            "<Titular>" + wNombreTitular + "</Titular>" + secuencia +
                            "<Empresa>" + wNombreContratante + "</Empresa>" + secuencia +
                            "<Poliza>" + wPoliza + "</Poliza>" + secuencia +
                            "<Coaseguro>" + wCoaseguro + "</Coaseguro>" + secuencia +
                        "</Salud>" + secuencia;


                XML_4 = "<ENTexto>" + secuencia +
                        "<Texto1>" + wTexto1 + "</Texto1>" + secuencia +
                        "</ENTexto>" + secuencia;


                XML_5 = "<Sucursal>" + secuencia +
                        "<ENSucursal>" + secuencia +
                        "<Direccion>" + wSucursal_Direccion + "</Direccion>" + secuencia +
                        "</ENSucursal>" + secuencia +
                        "</Sucursal>" + secuencia;

                if (wFlgGratuito == "1" && (wCodcomprobante.Substring(0, 1) == "F" || wCodcomprobante.Substring(0, 1) == "B"))
                {
                    XML_6 = "<ComprobantePropiedadesAdicionales>" + secuencia +
                                "<ENComprobantePropiedadesAdicionales>" + secuencia +
                                    "<Codigo>1002</Codigo>" + secuencia +
                                    "<Valor>" + wTextoGratuito + "</Valor>" + secuencia +
                                "</ENComprobantePropiedadesAdicionales>" +
                            "</ComprobantePropiedadesAdicionales>" + secuencia;

                    //'XML_6 = "<ENComprobantePropiedadesAdicionales>"+ secuencia + 
                    //'"<Codigo>1002</Codigo>"+ secuencia + 
                    //'"<Valor>" + wTextoGratuito + "</Valor>"+ secuencia + 
                    //'"</ENComprobantePropiedadesAdicionales>" + Chr(10)
                }

                //'string wDepartamento;
                //'string wProvincia;
                //'string wDistrito;
                //'string wUrbanizacion;
                //'string wWeb;
                //'string wCorreo;
                string wCodigoEstabSUNAT;
                string wCodDistrito;
                string wForma_pago;

                wForma_pago = response[0].forma_pago;
                wCodigoEstabSUNAT = response[0].CodigoEstabSUNAT;
                wCodDistrito = response[0].CodDistrito;

                XML_7 = "<oENEmpresa>" + secuencia +
                            "<CodigoTipoDocumento>" + wEmpresa_TipoDocIdentidad + "</CodigoTipoDocumento>" + secuencia +
                            "<Ruc>" + wEmpresa_Ruc + "</Ruc>" + secuencia +
                            "<RazonSocial>" + wEmpresa_Nombre + "</RazonSocial>" + secuencia +
                            "<CodDistrito>" + wCodDistrito + "</CodDistrito>" + secuencia +
                            "<Calle>" + wEmpresa_Direccion + "</Calle>" + secuencia +
                            "<Telefono>" + wEmpresa_Telefono + "</Telefono>" + secuencia +
                            "<CodPais>PE</CodPais>" + secuencia +
                            "<CodigoEstablecimientoSUNAT>" + wCodigoEstabSUNAT + "</CodigoEstablecimientoSUNAT>" + secuencia +
                        "</oENEmpresa>" + secuencia;

                //'"<TipoDocumentoIdentidad>" + wEmpresa_TipoDocIdentidad + "</TipoDocumentoIdentidad>" + secuencia + 

                XML_8 = "<ComprobanteNotaCreditoDocRef>" + secuencia +
                            "<ENComprobanteNotaDocRef>" + secuencia +
                                "<Serie>" + wRef_Serie + "</Serie>" + secuencia +
                                "<Numero>" + wRef_Numero + "</Numero>" + secuencia +
                                "<TipoComprobante>" + wRef_TipoComp_Sunat + "</TipoComprobante>" + secuencia +
                                "<FechaDocRef>" + wRef_FechaEmision + "</FechaDocRef>" + secuencia +
                            "</ENComprobanteNotaDocRef>" + secuencia +
                        "</ComprobanteNotaCreditoDocRef>" + secuencia;

                XML_9 = "<ComprobanteMotivosDocumentos>" + secuencia +
                            "<ENComprobanteMotivoDocumento>" + secuencia +
                                "<SerieDocRef>" + wRef_Serie + "</SerieDocRef>" + secuencia +
                                "<NumeroDocRef>" + wRef_Numero + "</NumeroDocRef>" + secuencia +
                                "<CodigoMotivoEmision>" + wCodMotivo_Sunat + "</CodigoMotivoEmision>" + secuencia +
                                "<Sustentos>" + secuencia +
                                "<ENComprobanteMotivoDocumentoSustento>" + secuencia +
                                "<Sustento>" + wMotivoNota.Trim() + "</Sustento>" + secuencia +
                                "</ENComprobanteMotivoDocumentoSustento>" + secuencia +
                                "</Sustentos>" + secuencia +
                            "</ENComprobanteMotivoDocumento>" + secuencia +
                        "</ComprobanteMotivosDocumentos>" + secuencia;

                //'Montos Totales - Gravado
                if ((wCodcomprobante.Substring(0, 1) == "F" | wCodcomprobante.Substring(0, 1) == "B" | wCodcomprobante.Substring(0, 1) == "C" | wCodcomprobante.Substring(0, 1) == "D") && wFlgGratuito == "1")
                {
                    XML_12 = "<Gratuito>" + secuencia +
                             "<Total>" + wMontoGratuito + "</Total>" + secuencia +
                             "<GratuitoImpuesto>" + secuencia +
                             "<Base>" + wMontoGratuito + "</Base>" + secuencia +
                             "<ValorImpuesto>" + wMtg_ValorImpuesto + "</ValorImpuesto>" + secuencia +
                             "</GratuitoImpuesto>" + secuencia +
                             "</Gratuito>" + secuencia;
                }
                else if ((wCodcomprobante.Substring(0, 1) == "F" | wCodcomprobante.Substring(0, 1) == "B" | wCodcomprobante.Substring(0, 1) == "C" | wCodcomprobante.Substring(0, 1) == "D") && decimal.Parse(wMontoInafecto) > 0)
                {
                    XML_12 = "<Inafecto>" + secuencia +
                             "<Total>" + wMontoInafecto + "</Total>" + secuencia +
                             "</Inafecto>" + secuencia;
                }
                else
                {
                    XML_12 = "<Gravado>" + secuencia +
                             "<Total>" + wMt_total + "</Total>" + secuencia +
                             "<GravadoIGV>" + secuencia +
                             "<Base>" + wMtg_Base + "</Base>" + secuencia +
                             "<ValorImpuesto>" + wMtg_ValorImpuesto + "</ValorImpuesto>" + secuencia +
                             "<Porcentaje>" + wMtg_Porcentaje + "</Porcentaje>" + secuencia +
                             "</GravadoIGV>" + secuencia +
                             "</Gravado>" + secuencia;
                }

                XML_Detalle = GetComprobateElectronicoDetalleXml(response);

                if (XML_Detalle != "")
                {
                    XMLT = XMLT + "<oENComprobante>" + secuencia;
                    XMLT = XMLT + "<TipoPlantilla>" + wTipoPlantilla + "</TipoPlantilla>" + secuencia;
                    XMLT = XMLT + "<TipoComprobante>" + wTipoComp_Sunat + "</TipoComprobante>" + secuencia;
                    XMLT = XMLT + "<Serie>" + wSerie + "</Serie>" + secuencia;
                    XMLT = XMLT + "<Numero>" + wNumero + "</Numero>" + secuencia;
                    XMLT = XMLT + "<FechaEmision>" + wFechaEmision + "</FechaEmision>" + secuencia;
                    XMLT = XMLT + "<Moneda>" + wMoneda + "</Moneda>" + secuencia;
                    XMLT = XMLT + "<RazonSocial>" + wReceptor_Anombrede + "</RazonSocial>" + secuencia;
                    XMLT = XMLT + "<Receptor>" + secuencia;
                    XMLT = XMLT + "<ENReceptor>" + secuencia;
                    XMLT = XMLT + "<Calle>" + wReceptor_Direccion + "</Calle>" + secuencia;
                    XMLT = XMLT + "</ENReceptor>" + secuencia;
                    XMLT = XMLT + "</Receptor>" + secuencia;
                    XMLT = XMLT + "<TipoDocumentoIdentidad>" + wReceptor_TipoDocIdentidad + "</TipoDocumentoIdentidad>" + secuencia;
                    XMLT = XMLT + "<Ruc>" + wReceptor_Ruc + "</Ruc>" + secuencia;
                    XMLT = XMLT + "<CorreoElectronico>" + wReceptor_Correo + "</CorreoElectronico>" + secuencia;
                    XMLT = XMLT + XML_3;  //'ENSalud
                    XMLT = XMLT + "<Glosa>" + wObservaciones + "</Glosa>" + secuencia;
                    if (wCodcomprobante.Substring(0, 1) == "F" | wCodcomprobante.Substring(0, 1) == "B")
                    {
                        XMLT = XMLT + "<Texto>" + secuencia + XML_4 + "</Texto>" + secuencia;
                    }
                    XMLT = XMLT + XML_5;   //'sucursal

                    if ((wCodcomprobante.Substring(0, 1) == "F" | wCodcomprobante.Substring(0, 1) == "B") && wFlgGratuito == "1")
                    {
                        XMLT = XMLT + XML_6;   //'ENComprobantePropiedadesAdicionales = Gratuidad
                    }

                    XMLT = XMLT + "<ImporteTotal>" + wMontoNeto + "</ImporteTotal>" + secuencia;
                    XMLT = XMLT + "<ComprobanteMontosAdicionalesObligatorios>" + secuencia + XML_1 + "</ComprobanteMontosAdicionalesObligatorios>" + secuencia;


                    //'25/06/2016 se agrega para gratuitos
                    if ((wCodcomprobante.Substring(0, 1) == "F" | wCodcomprobante.Substring(0, 1) == "B") && wFlgGratuito == "1")
                    {
                        XMLT = XMLT + "<ComprobanteMontosAdicionalesOtros>" + secuencia + XML_11 + "</ComprobanteMontosAdicionalesOtros>" + secuencia;
                    }

                    XMLT = XMLT + "<ComprobanteImpuestos>" + secuencia + XML_2 + "</ComprobanteImpuestos>" + secuencia;
                    //'Detraccion : Aplica solo a Facturas. Pero en logistica no hay detracciones
                    if (wCodcomprobante.Substring(0, 1) == "C" | wCodcomprobante.Substring(0, 1) == "D")
                    {
                        XMLT = XMLT + XML_8;
                        XMLT = XMLT + XML_9;
                    }

                    XMLT = XMLT + "<ComprobanteDetalle>" + secuencia + XML_Detalle + "</ComprobanteDetalle>" + secuencia;

                    //'22/02/2019
                    XMLT = XMLT + "<TipoOperacion>" + wTipo_operacion + "</TipoOperacion>" + secuencia;
                    XMLT = XMLT + "<HoraEmision>" + wHora_emision + "</HoraEmision>" + secuencia;
                    XMLT = XMLT + "<TotalImpuesto>" + wTotal_impuesto + "</TotalImpuesto>" + secuencia;
                    XMLT = XMLT + "<TotalValorVenta>" + wTotal_valorventa + "</TotalValorVenta>" + secuencia;
                    XMLT = XMLT + "<TotalPrecioVenta>" + wTotal_precioventa + "</TotalPrecioVenta>" + secuencia;
                    XMLT = XMLT + "<VersionUbl>" + wVs_ubl + "</VersionUbl>" + secuencia;

                    XMLT = XMLT + "<MontosTotales>" + secuencia + XML_12 + "</MontosTotales>" + secuencia;
                    XMLT = XMLT + "</oENComprobante>" + secuencia;
                    XMLT = XMLT + XML_7;
                }
                else
                {
                    XMLT = "";
                }
            }
            else
            {
                XMLT = "";
            }

            return XMLT;
        }

        public string GetComprobateElectronicoDetalleXml(List<BE_TCIComprobanteElectronico> response)
        {
            string XMLT;
            string XML2;
            string XML3;
            string XML4;
            string secuencia = "\r";
            //Dime wAdoEfac As New ADODB.Recordset
            string wSql;
            //'<I-variables del XML>
            int wContador;
            string WCodcomprobante;
            string wUnidadComercial;
            string wNombreProducto;
            string wCantidad;
            string wValorVentaUnitario;
            string wValorVentaUnitarioIncIgv;
            string wDescuento_Indicador;
            string wMontoDescuento;
            string wMontoDescuentoIncIgv;
            string wMontoTotal;
            string wMontoTotalIncIgv;
            string wDeterminante;
            //'--i-variables Impuestos en Detalle
            string wCodigoTipoPrecio;
            string wImporteTributo;
            string wImporteExplicito;
            string wAfectacionIGV;
            string wMontoInafecto;
            string wCodigoTributo;
            string wDesTributo;
            string wCodigoUN;
            string wImpGratuito;
            string wD_montobase;
            //'--f-variables Impuestos en Detalle
            //'<F-variables del XML>
            string wPorcentajeIGV;


            string wImpTasaAplicada;
            string wImpMontoBase;
            string wImpuestoTotal;
            string wdTotalCondsctoigv;
            string wDporcdctoplan;
            string wDvalorVVP;
            string wDsctoUnitario;
            string wCodigoProductoSunat;

            XMLT = "";
            XML2 = "";
            XML3 = "";
            XML4 = "";
            wContador = 0;                      //'ENComprobanteDetalle.Item; Nro de Item del detalle empieza en 1
            wUnidadComercial = "NIU";           //'ENComprobanteDetalle.UnidadComercial; Unidad de Medida, Para unidades es "NIU" y pasa servicio es "ZZ"
            wNombreProducto = "";               //'ENComprobanteDetalle.Descripcion;  Tipo Dato : an(250)
            wCantidad = "0";                    //'ENComprobanteDetalle.Cantidad; para servicios=1
            wValorVentaUnitario = "0";          //'ENComprobanteDetalle.ValorVentaUnitario;       Valores de venta unitarios por item (VU) no incluye impuestos.
            wValorVentaUnitarioIncIgv = "0";    //'ENComprobanteDetalle.ValorVentaUnitarioIncIgv; Precio de Venta Unitario (Incluye impuestos)
            wDescuento_Indicador = "0";         //'ENComprobanteDetalle.ENDescuentoCargoDetalle.Indicador; 0:Descuento, 1:Cargo
            wMontoDescuento = "0";              //'ENComprobanteDetalle.ENDescuentoCargoDetalle.Monto; Monto del dscto
            wMontoInafecto = "0";               //'ENComprobante.ENComprobanteMontosAdicionalesObligatorios.Codigo =10002; para Total Valor Venta - Operaciones Inafectas
            wMontoDescuentoIncIgv = "0";        //'ENComprobanteDetalle.DescuentoIncIgv; Monto del dscto con IGV
            wMontoTotal = "0";                  //'ENComprobanteDetalle.Total; subtot_sinigv; Valor Venta del Item (Sin impuestos);  TipoDato: n(22,2) Ejemplo: 12000.00
            wMontoTotalIncIgv = "0";            //'ENComprobanteDetalle.PrecioVentaItem;      Total del item con impuesto
            wDeterminante = "1";                //'ENComprobanteDetalle.Determinante;      1:Onerosa  2:No Onerosa
            wCodigoTipoPrecio = "01";           //'ENComprobanteDetalle.CodigoTipoPrecio; 01:Onerosa | 02:No Onerosa
            wImporteTributo = "0";              //'ENComprobanteDetalle.ENComprobanteDetalleImpuestos.ImporteTributo;   Importe total de un tributo para este item;
            wImporteExplicito = "0";            //'ENComprobanteDetalle.ENComprobanteDetalleImpuestos.ImporteExplicito; Importe explícito a tributar (= Tasa Porcentaje * Base Imponible) ; Tipo Dato : n(12,2) Ejemplo: 12000.00


            wAfectacionIGV = "";                //'ENComprobanteDetalle.ENComprobanteDetalleImpuestos.AfectacionIGV; Catalogo No. 07: Códigos de Tipo de Afectación del IGV;  10=Gravado-Operacion Onerosa, 30=Inafecto--Operacion Onerosa
            wCodigoTributo = "1000";            //'ENComprobanteDetalle.ENComprobanteDetalleImpuestos.CodigoTributo; Identificación del tributo según catálogo SUNAT; Catalogo No. 05: Códigos de Tipos de Tributos; 1000=IGV
            wDesTributo = "IGV";                //'ENComprobanteDetalle.ENComprobanteDetalleImpuestos.DesTributo
            wCodigoUN = "VAT";                  //'ENComprobanteDetalle.ENComprobanteDetalleImpuestos.CodigoUN
            wImpGratuito = "0";                 //'ENComprobanteDetalle.ENComprobanteDetalleImpuestos.ImpGratuito; Identifica que los impuestos son Gratuitos; 0:No Gratuita | 1:Gratuita
            wPorcentajeIGV = "0";


            wImpMontoBase = "0";                //'ENComprobanteDetalle.ENComprobanteDetalleImpuestos.MontoBase         TMACASSI 2/02/2019
            wImpTasaAplicada = "0";             //'ENComprobanteDetalle.ENComprobanteDetalleImpuestos.TasaAplicada      TMACASSI 22/02/2019
            wdTotalCondsctoigv = "0";           //'ENComprobanteDetalle.ENComprobanteDetalle.ValorVentaUnitarioIncIgv   TMACASSI 22/02/2019  total de item con dscto con impuesto
            wImpuestoTotal = "0";               //'ENComprobanteDetalle.ImpuestoTotal         TMACASSI 22/02/2019
            wD_montobase = "0";                 //'ENComprobante.ComprobanteDetalle.DescuentoCargoDetalle.MontoBase     TMACASSI 05/06/2019
            wCodigoProductoSunat = "0";         //'ENComprobante.ComprobanteDetalle.CodigoProductoSunat        TMACASSI 05/06/2019
            wDporcdctoplan = "0";
            wDvalorVVP = "0";
            wDsctoUnitario = "0";

            WCodcomprobante = response[0].codcomprobantee;

            if (response.Count > 0)
            {
                foreach (var item in response)
                {
                    wContador += 1;
                    wUnidadComercial = item.d_unidad;
                    wNombreProducto = item.nombreproducto;
                    wCantidad = item.d_cant_sunat.ToString();
                    wValorVentaUnitario = item.d_ventaunitario_sinigv.ToString("N2");
                    wValorVentaUnitarioIncIgv = item.d_ventaunitario_conigv.ToString("N2");
                    wDescuento_Indicador = "0";
                    wMontoDescuento = item.d_dscto_unitario.ToString("N2");
                    wMontoDescuentoIncIgv = item.d_dscto_conigv.ToString("N2");
                    wMontoTotal = item.d_total_sinigv.ToString("N2");
                    wMontoTotalIncIgv = item.d_total_conigv.ToString("N2");
                    wDeterminante = item.d_determinante;
                    wCodigoTipoPrecio = item.d_codigotipoprecio;
                    wImporteTributo = item.d_montoigv.ToString("N2");
                    wImporteExplicito = wImporteTributo;
                    wPorcentajeIGV = item.porcentajeimpuesto.ToString();
                    wAfectacionIGV = item.d_afectacionigv;
                    wMontoInafecto = item.c_montoinafecto.ToString("N2");
                    wImpMontoBase = wMontoTotal;
                    wImpTasaAplicada = wPorcentajeIGV;

                    wCodigoTributo = item.cod_tributo;
                    wAfectacionIGV = item.cod_afectacionIGV;
                    wDesTributo = item.des_tributo;
                    wCodigoUN = item.cod_UN;
                    wD_montobase = item.d_montobase.ToString();
                    wImpuestoTotal = item.d_montoigv.ToString();
                    wDporcdctoplan = item.d_porcdctoplan.ToString();
                    wdTotalCondsctoigv = item.d_total_condsctoigv.ToString();
                    wDvalorVVP = item.d_dscto_montobase.ToString("N2");
                    wDsctoUnitario = item.d_dscto_unitario.ToString();
                    wCodigoProductoSunat = item.CodProductoSUNAT;
                    if (item.flg_gratuito == "1")
                    {
                        wImpGratuito = "1";
                        wImpMontoBase = wMontoTotalIncIgv;
                        //'wImpTasaAplicada = Format("0.00", "##########.00")
                        wMontoTotal = wMontoTotalIncIgv;
                        wMontoTotalIncIgv = "0";
                        wImpuestoTotal = "0";
                    }
                    else if (decimal.Parse(wMontoInafecto) > 0)
                    {
                        wImpGratuito = "0";
                        wImpMontoBase = wMontoInafecto;
                        wImpTasaAplicada = "0.00";
                        wImpGratuito = "0";
                        //'wMontoTotalIncIgv = 0
                    }

                    XML2 = "";
                    XML3 = "";
                    XML4 = "";

                    //'AGARCIA.25/06/2016 TCI: Se envía Dcto solo a F,B,NC normales; no aplica a gratuitos.
                    string wCodigoDescuento;
                    string wNota;
                    wCodigoDescuento = "00";
                    string wCoaseguro;
                    string wNota2;
                    wCoaseguro = item.porcentajecoaseguro.ToString();

                    if (wImpGratuito == "0")           //'Se agrega - Se envía Dcto solo a F,B,NC normales; no aplica a gratuitos.
                    {
                        if (decimal.Parse(wMontoDescuento) > 0)
                        {
                            wNota = item.d_dscto_sinigv.ToString("N2");
                            wNota2 = item.d_dscto_conigv.ToString("N");
                            wD_montobase = item.d_dscto_montobase.ToString();
                            if (int.Parse(wCoaseguro) > 0 && (WCodcomprobante.Substring(0, 1) == "C" | WCodcomprobante.Substring(0, 1) == "D"))
                            {
                                wValorVentaUnitarioIncIgv = wdTotalCondsctoigv;
                            }

                            XML2 = "<DescuentoCargoDetalle>" + secuencia +
                                    "<ENDescuentoCargoDetalle>" + secuencia +
                                       "<Monto>" + wNota + "</Monto>" + secuencia +
                                       "<Descripcion>DESCUENTO A</Descripcion>" + secuencia +
                                       "<Indicador>" + wDescuento_Indicador + "</Indicador>" + secuencia +
                                       "<Porcentaje>" + wDporcdctoplan + "</Porcentaje>" + secuencia +
                                       "<CodigoAplicado>" + wCodigoDescuento + "</CodigoAplicado>" + secuencia +
                                       "<MontoBase>" + wD_montobase + "</MontoBase>" + secuencia +
                                   "</ENDescuentoCargoDetalle>" + secuencia +
                                "</DescuentoCargoDetalle>" + secuencia;
                        }
                        else
                        {
                            wMontoDescuentoIncIgv = "0.00";
                            wNota2 = "0";
                        }

                        //'PREGUNTAR a karla sobre descuento catalogo 53 wCodigoDescuento


                        //'11/07/2016 - Se envía Dcto solo a F,B,NC normales; no aplica a gratuitos.
                        if (wImpGratuito == "0")
                        {
                            XML4 = "<DescuentoIncIgv>" + wMontoDescuentoIncIgv + "</DescuentoIncIgv>" + secuencia;
                        }

                        XML3 = "<ENComprobanteDetalleImpuestos>" + secuencia +
                                    "<ImporteTributo>" + wImporteTributo + "</ImporteTributo>" + secuencia +
                                    "<ImporteExplicito>" + wImporteExplicito + " </ImporteExplicito>" + secuencia +
                                    "<AfectacionIGV>" + wAfectacionIGV + "</AfectacionIGV>" + secuencia +
                                    "<CodigoTributo>" + wCodigoTributo + "</CodigoTributo>" + secuencia +
                                    "<DesTributo>" + wDesTributo + "</DesTributo>" + secuencia +
                                    "<CodigoUN>" + wCodigoUN + "</CodigoUN>" + secuencia +
                                    "<ImpGratuito>" + wImpGratuito + "</ImpGratuito>" + secuencia +
                                    "<MontoBase>" + wImpMontoBase + "</MontoBase>" + secuencia +
                                    "<TasaAplicada>" + wImpTasaAplicada + "</TasaAplicada>" + secuencia +
                               "</ENComprobanteDetalleImpuestos>" + secuencia;

                        wCodigoProductoSunat = "";

                        XMLT = XMLT + "<ENComprobanteDetalle>" + secuencia +
                                            "<Item>" + wContador.ToString() + "</Item>" + secuencia +
                                            "<UnidadComercial>" + wUnidadComercial + "</UnidadComercial>" + secuencia +
                                            "<Descripcion>" + wNombreProducto + "</Descripcion>" + secuencia +
                                            "<Cantidad>" + wCantidad + "</Cantidad>" + secuencia +
                                            "<ValorVentaUnitario>" + wValorVentaUnitario + "</ValorVentaUnitario>" + secuencia +
                                            "<ValorVentaUnitarioIncIgv>" + wValorVentaUnitarioIncIgv + "</ValorVentaUnitarioIncIgv>" + secuencia +
                                            "<Nota>" + wNota2 + "</Nota>" + secuencia +
                                            "<CodigoProductoSunat>" + wCodigoProductoSunat + "</CodigoProductoSunat>" + secuencia +
                                            "<ImpuestoTotal>" + wImpuestoTotal + "</ImpuestoTotal>" + secuencia +
                                            XML2;

                        XMLT = XMLT + XML4;
                        XMLT = XMLT + "<Total>" + wMontoTotal + "</Total>" + secuencia +
                                            "<PrecioVentaItem>" + wMontoTotalIncIgv + "</PrecioVentaItem>" + secuencia +
                                            "<Determinante>" + wDeterminante + "</Determinante>" + secuencia +
                                            "<CodigoTipoPrecio>" + wCodigoTipoPrecio + "</CodigoTipoPrecio>" + secuencia +
                                            "<ComprobanteDetalleImpuestos>" + secuencia + XML3 + "</ComprobanteDetalleImpuestos>" + secuencia +
                                      "</ENComprobanteDetalle>" + secuencia;
                    }
                }
            }
            else
            {
                XMLT = "";
            }

            return XMLT;
        }
        public bool InvokeWebService_TCI(string strSoap, string strSOAPAction, string strURL, DOMDocument30 xmlResponse)
        {
            bool blnSuccess;

            goto MsgError;

            XMLHTTP30 xmlhttp = new XMLHTTP30();
            xmlhttp.open("POST", strURL, false);
            xmlhttp.setRequestHeader("Man", "POST " + strURL + " HTTP/1.1");
            xmlhttp.setRequestHeader("Content-Type", "text/xml; charset=utf-8");
            xmlhttp.setRequestHeader("SOAPAction", strSOAPAction);
            xmlhttp.send(strSoap);

            if (xmlhttp.status == 200) //'Ali: xmlhttp.StatusText = "OK"
            {
                blnSuccess = true;
            }
            else
            {
                blnSuccess = false;
            }

            xmlResponse = xmlhttp.responseXML;
            xmlhttp = null;
            return blnSuccess;

        MsgError:
            blnSuccess = false;
            return blnSuccess;

        }
        public string Leer_ResponseXML(string strResponseXML, string CampoInicio, string CampoFin)
        {
            string xCadena;
            int xPos1;
            int xPos2;
            string xResultado;

            xResultado = "";

            xCadena = strResponseXML;

            xPos1 = Strings.InStr(1, xCadena, CampoInicio, CompareMethod.Text) + CampoInicio.Length;
            xPos2 = Strings.InStr(1, xCadena, CampoFin, CompareMethod.Text);

            if (xPos2 > 0)
            {
                xResultado = xCadena.Substring(xPos1, xPos2 - xPos1);
            }

            return xResultado;
        }


        //public string ConvertirCodigoBarraJPG(string pCodComprobantePK,string pCadenaImagen,Object ConexionClinica)
        //{
        //    if (Iniciar(pConexionClinica))
        //    {
        //        ConvertirCodigoBarraJPG = zCsfDLL.ConvertirCodigoBarraJPG(pCodComprobantePK, pCadenaImagen);
        //    else
        //    {
        //            ConvertirCodigoBarraJPG = "Error al invocar el metodo para convertir el Codigo de Barra a JPG.";
        //    }
        //}
    }
}
