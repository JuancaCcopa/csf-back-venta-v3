using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.Business.DTO;
using Net.Data;

namespace Net.Business.Services.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ApiExplorerSettings(GroupName = "ApiVenta")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class VentaCajaController : ControllerBase
    {
        private readonly IRepositoryWrapper _repository;
        public VentaCajaController(IRepositoryWrapper repository)
        {
            this._repository = repository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codventa"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVentaCabeceraPorCodVenta([FromQuery] string codventa)
        {

            var objectGetAll = await _repository.VentaCaja.GetVentaCabeceraPorCodVenta(codventa);
            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            if(objectGetAll.data.codventa==null) return BadRequest($"El codigo de la venta no existe: {codventa}");

            decimal montoNeto = 0;
            if (objectGetAll.data.codtipocliente.Equals("01"))
            {
                montoNeto = objectGetAll.data.montopaciente;
            }
            if (objectGetAll.data.codtipocliente.Equals("02") || objectGetAll.data.codtipocliente.Equals("03") || objectGetAll.data.codtipocliente.Equals("04"))
            {
                montoNeto = objectGetAll.data.montoaseguradora;
            }

            objectGetAll.data.montoigv = decimal.Round((montoNeto * (objectGetAll.data.porcentajeimpuesto / 100)), 2);
            objectGetAll.data.montoneto = decimal.Round(montoNeto, 2);
            objectGetAll.data.montototal = decimal.Round((montoNeto + objectGetAll.data.montoigv), 2);
            if (objectGetAll.data.flg_gratuito) {
                objectGetAll.data.montototal = 0;
            }

                
            return Ok(objectGetAll.data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoCliente"></param>
        /// <param name="codCliente"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDatoCardCodeConsulta([FromQuery] string tipoCliente,string codCliente)
        {
            var objectGetAll = await _repository.VentaCaja.GetDatoCardCodeConsulta(tipoCliente, codCliente);
            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll);
        }

        //GetDatoCardCodeConsulta

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codventa"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVentaDetallePorCodVenta([FromQuery] string codventa)
        {
            var objectGetAll = await _repository.VentaCaja.GetVentaDetallePorCodVenta(codventa);
            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }
            return Ok(objectGetAll.dataList);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codPaciente"></param>
        /// <param name="codAseguradora"></param>
        /// <param name="codCia"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRucConsultav2PorFiltro([FromQuery] string codPaciente, string codAseguradora, string codCia)
        {
            var objectGetAll = await _repository.VentaCaja.GetRucConsultav2PorFiltro(codPaciente.Trim(), codAseguradora.Trim(), codCia.Trim());
            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }
            return Ok(objectGetAll.data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ide_pagos_bot"></param>
        /// <param name="ide_mdsyn_reserva"></param>
        /// <param name="ide_correl_reserva"></param>
        /// <param name="cod_liquidacion"></param>
        /// <param name="cod_venta"></param>
        /// <param name="orden"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMdsynPagosConsulta([FromQuery] long ide_pagos_bot, int ide_mdsyn_reserva, int ide_correl_reserva, string cod_liquidacion, string cod_venta, int orden)
        {

            cod_liquidacion = cod_liquidacion is null ? string.Empty : cod_liquidacion;

            var objectGetAll = await _repository.VentaCaja.GetMdsynPagosConsulta(ide_pagos_bot, ide_mdsyn_reserva, ide_correl_reserva, cod_liquidacion, cod_venta, orden);
            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.data);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="codPersonal"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetLimiteConsumoPersonalPorCodPersonal([FromQuery] string codPersonal)
        {
            var objectGetAll = await _repository.VentaCaja.GetLimiteConsumoPersonalPorCodPersonal(codPersonal);
            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.data);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GenerarPagar([FromBody]  DtoComprobanteCabeceraRegistrar value)
        {
            try
            {
                if (value == null)
                {
                    return BadRequest(ModelState);
                }

                string wStrURL = string.Empty;

                if (value.wFlg_electronico)
                {
                    var objWebServices = await _repository.VentaCaja.GetWebServicesPorCodTabla("EFACT_TCI_WS");
                    if (objWebServices.IdRegistro.Equals(0))
                    {
                        wStrURL = objWebServices.data;
                    }
                    else {
                        return Ok("No se tiene configurado en url del servicio");
                    }
                }

                //code 18 -> Verifica TipoMovimiento de Venta y code 27 Verifica Datos de Venta
                string strCodCliente = string.Empty;
                var objVentasCabecera = await _repository.VentaCaja.GetVentaCabeceraPorCodVenta(value.codVenta);
                if (objVentasCabecera.IdRegistro == 0)
                {
                    if (objVentasCabecera.data.codcomprobante.Trim() != "")
                    {
                        var resul = new { exito = false, mensaje = $"NO puede Facturar, Venta tiene Comprobante : {objVentasCabecera.data.codcomprobante.Trim()}" };
                        return Ok(resul);
                    }
                    if (objVentasCabecera.data.tipomovimiento.Equals("DV") && objVentasCabecera.data.codcomprobante.Trim() == "")
                    {
                        //Beep graba
                    }
                    else {
                        var resul = new { exito = false, mensaje = $"Verifique el TipoMovimiento de la Venta" };
                        return Ok(resul);
                    }

                    if (objVentasCabecera.data.strTienedevolucion.Equals("S"))
                    {
                        var resul = new { exito = false, mensaje = "No puede facturar, la venta tiene una devolución asociada o esta anulada" };
                        return Ok(resul);
                    }

                    strCodCliente = objVentasCabecera.data.codcliente;
                }
                else {
                    var resul = new { exito = false, mensaje = "NO existe Nro de Venta" };
                    return Ok(resul);
                }

                //code 21
                // limite de consumo de personal
                string vender = "S";
                if (value.codTipoCliente.Equals("03")) { 
                
                    var objLConsumo = await _repository.VentaCaja.GetCsLimiteConsumoPersonalPorCodPersonal(strCodCliente);
                    if (objLConsumo.ResultadoCodigo == -1)
                    {
                        return BadRequest(objLConsumo);
                    }

                     vender = objLConsumo.data.vender;
                     var montoconsumo = objLConsumo.data.montoconsumo;
                     var montolimite = objLConsumo.data.montolimite;
                     var fecha1 = objLConsumo.data.fecha1;
                     var fecha2 = objLConsumo.data.fecha2;

                    if (vender == "N")
                    {
                        var resul = new { exito = false, mensaje = $"El Consumo al CREDITO es mayor al límite_de_consumo <br><br>en el período del ${fecha1} al ${fecha2} <br>Monto Consumo (no incluye esta venta {montoconsumo} <br> Monto Limite de Consumo : ${montolimite}):" };
                        return Ok(resul);
                    }
                    else {

                       var wMontoConsumo = objLConsumo.data.montoconsumo + value.montoTotal;

                        if (wMontoConsumo <= montolimite)
                        {
                            //beep graba
                        }
                        else {
                            var resul = new { exito = false, mensaje = $"El Consumo al CREDITO es mayor al límite_de_consumo <br> en el período del {fecha1} al ${fecha2} <br> Monto Consumo (incluye esta venta): ${montoconsumo} <br> Monto Limite de Consumo : ${montolimite}" };
                            return Ok(resul);
                        }

                    }
                    
                    foreach (var item in value.tipoPagos)
                    {

                    }

                }

                //code 24
                //Flag si puede generar FACTURA-Electronica pero no se envian a sunat. Por default si genera BOLETA-ELECTRONIA pero no se envía a Sunat.
                string wFlagGeneraFACTURA = string.Empty;
                var objFlgGENERAFACTURA = await _repository.Tabla.GetTablasClinicaPorFiltros("EFACT_FLAG_GENERAFACTURA", string.Empty, 0, 0, 5); //Electronico Enviar o no a SUNAT
                wFlagGeneraFACTURA = objFlgGENERAFACTURA.data.codigo;
                if (value.tipoComprobante.Substring(0, 1) == "F" && wFlagGeneraFACTURA == "N")
                {
                    var resul = new { exito = false, mensaje = "En estos momentos no se puede generar FACTURAS." };
                    return Ok(resul);
                }

                // code 28

                var objectGetAllTC = await _repository.TipoCambio.GetObtieneTipoCambio();

                if (objectGetAllTC.ResultadoCodigo == -1)
                {
                    return BadRequest(objectGetAllTC);
                }

                decimal tipoDeCambio =0;

                foreach (var item in objectGetAllTC.dataList)
                {
                    value.tipoCambioVenta = item.Rate;
                    tipoDeCambio = item.Rate;
                }

                if(tipoDeCambio<=0) return Ok(new { exito = false, mensaje = "NO TIENE CONFIGURADO EL TIPO DE CAMBIO" });

                var setDatos = value.RetornaComprobanteCabecera();

                var objComprobante = await _repository.VentaCaja.ComprobantesRegistrar(setDatos, value.tipoComprobante.Substring(0,1),value.correo,value.codTipoAfectacionIgv,value.wFlg_electronico,value.maquina);

                if (objComprobante.IdRegistro == 0)
                {
                    return Ok(new { exito = true,comprobante= objComprobante.data, mensaje = $"SE REGISTRO CORRECTAMENTE EL PAGO: {objComprobante.data}" });
                }
                else {
                    return Ok(new { exito = false, mensaje = "SE GENERO UN ERROR AL MOMENTO DE GUARDAR EL PAGO" });
                }

                

            }
            catch (Exception ex)
            {
                return BadRequest($"Hubo un error en la solicitud : { ex.Message.ToString() }");
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetObtener()
        {
            var objectGetAll = await _repository.VentaCaja.GetCorrelativoConsulta();
            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }

            return Ok(objectGetAll.data);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCorrelativoConsulta()
        {
            var objectGetAll = await _repository.VentaCaja.GetCorrelativoConsulta();
            if (objectGetAll.ResultadoCodigo == -1)
            {
                return BadRequest(objectGetAll);
            }
            return Ok(objectGetAll.data);
        }

        //[HttpGet("{codventa}", Name = "GenerarValeVentaPrint")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public async Task<FileContentResult> GenerarValeVentaPrint(string codventa,string maquina,int idusuario,int orden)
        {
            //string codcomprobante,string maquina,int idusuario, int orden
            var objectGetById = await _repository.VentaCaja.GenerarValeVentaPrint(codventa, maquina, idusuario, orden);
            
            var pdf = File(objectGetById.data.GetBuffer(), "applicacion/pdf", codventa + ".pdf");

            return pdf;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVistaPrevia(string codComprobante)
        {//clinica.dbo.Sp_Tablas_Consulta 'EFACT_PARAMETROSWSTCI','',0,0,7
            //FileContentResult
            try
            {

                var objExiste = await _repository.VentaCaja.GetComprobanteElectroncioCodVenta(codComprobante, 0, 0, "", "", "", "", "", "", 9);
                if (objExiste.data.codcomprobante == null) BadRequest("NO ES UN COMPROBANTE ELECTRONICO");


                var objListConf = await _repository.Tabla.GetListTablaClinicaPorFiltros("EFACT_PARAMETROSWSTCI","",0,0,7);
                string xRucEmisor, xRutaArchivoPDF, xUrlWebService;

                foreach (var item in objListConf.dataList)
                {
                    if (item.codigo.Equals("RUC")) xRucEmisor = item.nombre;
                    if (item.codigo.Equals("TEMP")) xRutaArchivoPDF = item.nombre;
                    if (item.codigo.Equals("WS")) xUrlWebService = item.nombre;
                }
                
                string xRutaArchivo = @"C:\temp\";
                var resp = await _repository.VentaCaja.ObtenerCodigoBarraJPG(codComprobante, xRutaArchivo);

                //var objCPE = await _repository.VentaCaja.GetComprobanteElectroncioCodVenta(codComprobantePK, 0, 0, "", "", "", "", "", "", 1);
                if (resp.ResultadoCodigo == 0)
                {
                    if (resp.data == string.Empty)
                    {

                        if (codComprobante.Substring(0, 1).Equals("B") || codComprobante.Substring(0, 1).Equals("F")) { 
                        
                        
                        }

                        //If Mid(pCodcomprobante, 1, 1) = "B" Or Mid(pCodcomprobante, 1, 1) = "F" Then
                        //    zImprime.ImpresionElectronicaLOG Trim(pCodcomprobante), pPictureBarrasHash, pPreview, pPictureViewComprobante
                        //Else
                        //    zImprime.ImpresionElectronicaLOG_Nota Trim(pCodcomprobante), pPictureBarrasHash, pPreview, pPictureViewComprobante
                        //End If


                        //'Eliminamos el archivo JPG obtenido
                        //If xMensaje = "" Then
                        //    Kill xRutaArchivo + pCodcomprobante + ".jpg"
                        //End If

                    }
                    else {
                        string mensaje = resp.data;
                    }

                    //xObtenerByte = objCPE.data.codigobarra;
                    //if (xObtenerByte.Length == 0)
                    //{

                    //}
                    //else
                    //{
                    //    //Crea el directorio por si no existe
                    //    if (Directory.Exists(pRutaArchivoJPG) == false) Directory.CreateDirectory(pRutaArchivoJPG);
                    //    xRuta = pRutaArchivoJPG + codComprobantePK + ".jpg";
                    //    File.WriteAllBytes(xRuta, xObtenerByte); //Creamos el archivo 
                    //}
                }
                else
                {
                    resp.ResultadoDescripcion = "El comprobante que esta intentado obtener no es electrónico./ No existe el comprobante.";
                }
            }
            catch (Exception ex)
            {
                return null;// BadRequest(ex);
            }

            return null;
        }

    }
}
