using Net.Business.Entities;
using Net.Connection;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using Net.CrossCotting;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
//using System.Collections.ObjectModel;
//using Microsoft.Web.Services3.Messaging;
//using CSFDLLEFACT.WsTci;

namespace Net.Data
{
    public class VentaCajaRepository : RepositoryBase<BE_VentasCabecera>, IVentaCajaRepository
    {
        private readonly string _urlLogo;
        private readonly string _cnx;
        private readonly string _cnxClinica;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private string _aplicacionName;
        private string _metodoName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        const string DB_ESQUEMA = "";
        const string SP_GET_VENTA_CABECERA_BY_CODVENTA = DB_ESQUEMA + "VEN_VentasCabecera_Consulta";
        const string SP_GET_VENTA_DETALLE_BY_CODVENTA = DB_ESQUEMA + "VEN_VentasDetallePorCodventaGet";
        const string SP_GET_RUC_CONSULTAV2_BY_FILTRO = DB_ESQUEMA + "Fa_Ruc_Consultav2";
        const string SP_GET_MDSYN_PAGOS_CONSULTA_BY_FILTRO = DB_ESQUEMA + "Sp_MdsynPagos_Consulta";
        //const string SP_GET_LIMITE_CONSUMO_PERSONAL_BY_CODPERSONAL = DB_ESQUEMA + "Sp_LimiteConsumoPersonal";
        const string SP_GET_LIMITE_CONSUMO_PERSONAL_BY_CODPERSONAL = DB_ESQUEMA + "VEN_LimiteConsumoPersonal";
        const string SP_GET_WEBSERVICES_CONSULTA_BY_CODTABLA = DB_ESQUEMA + "Sp_TCI_WebService_Consulta";
        const string SP_GET_CORRELATIVO_CONSULTA = DB_ESQUEMA + "Sp_Correlativo_Consulta";
        const string SP_GET_COMPROBANTE_INSERT = DB_ESQUEMA + "VEN_Comprobantes_Ins";
        //const string SP_GET_DATOCARDCODE_CONSULTAR = DB_ESQUEMA + "Sp_DatoCardCode_Consulta";
        const string SP_GET_DATOCARDCODE_CONSULTAR = DB_ESQUEMA + "VEN_DatoCardCode_Consulta";
        //const string SP_GET_COMPROBANTE_UPDATE_PERSO = DB_ESQUEMA + "Sp_Comprobantes_Update";
        const string SP_POST_COMPROBANTE_VAL_UPDATE = DB_ESQUEMA + "VEN_ComprobantesValUpd";
        const string SP_POST_CLINICA_COMPROBANTE_UPDATE = DB_ESQUEMA + "VEN_ClinicaComprobantesElectronicos_Upd";
        const string SP_POST_COMPROBANTE_ELECTRONICO_POR_CODCOMPROBANTE_TK = DB_ESQUEMA + "VEN_ComprobantesElectronicos_Consulta";
        const string SP_POST_COMPROBANTE_ELECTRONICO_POR_CODCOMPROBANTE_VB = DB_ESQUEMA + "VEN_ComprobantesElectronicos_ConsultaVB";
        const string SP_POST_COMPROBANTE_ELECTRONICO_LOG_XML_CAB_PRINT = DB_ESQUEMA + "VEN_ComprobantesElectronicosLOG_XML_Cab_Print";

        
        public VentaCajaRepository(IHttpClientFactory clientFactory, IConnectionSQL context, IConfiguration configuration)
            : base(context)
        {
            _urlLogo = configuration["archivoImg:urlLogo"];
            _cnx = configuration.GetConnectionString("cnnSqlLogistica");
            _cnxClinica = configuration.GetConnectionString("cnnSqlClinica");
            _configuration = configuration;
            _clientFactory = clientFactory;
            _aplicacionName = this.GetType().Name;
        }

        public async Task<ResultadoTransaccion<BE_VentasCabecera>> GetVentaCabeceraPorCodVenta(string codVenta)
        {
            ResultadoTransaccion<BE_VentasCabecera> vResultadoTransaccion = new ResultadoTransaccion<BE_VentasCabecera>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_GET_VENTA_CABECERA_BY_CODVENTA, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codigo", codVenta)); //codventa

                        BE_VentasCabecera response = new BE_VentasCabecera();

                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.codatencion = ((reader["codatencion"]) is DBNull) ? string.Empty : reader["codatencion"].ToString().Trim();
                                response.fechagenera = ((reader["fechagenera"]) is DBNull) ? DateTime.MinValue : (DateTime)reader["fechagenera"];
                                response.fechaemision = ((reader["fechaemision"]) is DBNull) ? DateTime.MinValue : (DateTime)reader["fechaemision"];
                                response.codcomprobante = ((reader["codcomprobante"]) is DBNull) ? string.Empty : reader["codcomprobante"].ToString().Trim();
                                response.planpoliza = ((reader["planpoliza"]) is DBNull) ? string.Empty : reader["planpoliza"].ToString().Trim();
                                response.codaseguradora = ((reader["codaseguradora"]) is DBNull) ? string.Empty : reader["codaseguradora"].ToString().Trim();
                                response.codcia = ((reader["codcia"]) is DBNull) ? string.Empty : reader["codcia"].ToString().Trim();
                                response.codventa = ((reader["codventa"]) is DBNull) ? string.Empty : reader["codventa"].ToString().Trim();
                                response.estado = ((reader["estado"]) is DBNull) ? string.Empty : reader["estado"].ToString().Trim();
                                response.cardcode = ((reader["cardcode"]) is DBNull) ? string.Empty : reader["cardcode"].ToString().Trim();
                                response.codcliente = ((reader["codcliente"]) is DBNull) ? string.Empty : reader["codcliente"].ToString().Trim();
                                response.codpaciente = ((reader["codpaciente"]) is DBNull) ? string.Empty : reader["codpaciente"].ToString().Trim();
                                response.nombre = ((reader["nombre"]) is DBNull) ? string.Empty : reader["nombre"].ToString().Trim();
                                response.codplan = ((reader["codplan"]) is DBNull) ? string.Empty : reader["codplan"].ToString().Trim();
                                response.porcentajedctoplan = ((reader["porcentajedctoplan"]) is DBNull) ? 0 : (decimal)reader["porcentajedctoplan"];
                                response.codtipocliente = ((reader["codtipocliente"]) is DBNull) ? string.Empty : reader["codtipocliente"].ToString().Trim();
                                response.porcentajecoaseguro = ((reader["porcentajecoaseguro"]) is DBNull) ? 0 : (decimal)reader["porcentajecoaseguro"];
                                response.porcentajeimpuesto = ((reader["porcentajeimpuesto"]) is DBNull) ? 0 : (decimal)reader["porcentajeimpuesto"];
                                response.montopaciente = ((reader["montopaciente"]) is DBNull) ? 0 : (decimal)reader["montopaciente"];
                                response.montoaseguradora = ((reader["montoaseguradora"]) is DBNull) ? 0 : (decimal)reader["montoaseguradora"];

                                response.fechaemision = ((reader["fechaemision"]) is DBNull) ? DateTime.MinValue : (DateTime)reader["fechaemision"];

                                response.nombreestado = ((reader["nombreestado"]) is DBNull) ? string.Empty : reader["nombreestado"].ToString().Trim();
                                response.nombretipocliente = ((reader["nombretipocliente"]) is DBNull) ? string.Empty : reader["nombretipocliente"].ToString().Trim();
                                //response.nombrealmacen = ((reader["nombrealmacen"]) is DBNull) ? string.Empty : (string)reader["nombrealmacen"];
                                response.nombreplan = ((reader["nombreplan"]) is DBNull) ? string.Empty : reader["nombreplan"].ToString().Trim();
                                //response.tienedevolucion = ((reader["tienedevolucion"]) is DBNull) ? false : (bool)reader["tienedevolucion"];
                                response.ruccliente = ((reader["ruccliente"]) is DBNull) ? string.Empty : reader["ruccliente"].ToString().Trim();
                                response.dircliente = ((reader["dircliente"]) is DBNull) ? string.Empty : reader["dircliente"].ToString().Trim();
                                response.tipdocidentidad = ((reader["tipdocidentidad"]) is DBNull) ? string.Empty : reader["tipdocidentidad"].ToString().Trim();
                                response.docidentidad = ((reader["docidentidad"]) is DBNull) ? string.Empty : reader["docidentidad"].ToString().Trim();
                                response.nombretipdocidentidad = ((reader["nombretipdocidentidad"]) is DBNull) ? string.Empty : reader["nombretipdocidentidad"].ToString().Trim();
                                response.autorizado = ((reader["autorizado"]) is DBNull) ? string.Empty : reader["autorizado"].ToString().Trim();
                                response.correocliente = ((reader["correocliente"]) is DBNull) ? string.Empty : reader["correocliente"].ToString().Trim();
                                response.moneda_comprobantes = ((reader["moneda_comprobantes"]) is DBNull) ? string.Empty : reader["moneda_comprobantes"].ToString().Trim();
                                response.flg_gratuito = ((reader["flg_gratuito"]) is DBNull) ? false : (bool)reader["flg_gratuito"];
                                response.strTienedevolucion = ((reader["tienedevolucion"]) is DBNull) ? string.Empty : reader["tienedevolucion"].ToString();
                                response.tipomovimiento = ((reader["tipomovimiento"]) is DBNull) ? string.Empty : (string)reader["tipomovimiento"];

                            }
                        }

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", 1);
                        vResultadoTransaccion.data = response;
                    }
                }
            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return vResultadoTransaccion;

        }
        public async Task<ResultadoTransaccion<BE_VentasCabecera>> ComprobanteElectronicoUpd(string campo,string nuevovalor,string xml, Byte[] codigobarrajpg,string codigo)
        {
            ResultadoTransaccion<BE_VentasCabecera> vResultadoTransaccion = new ResultadoTransaccion<BE_VentasCabecera>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_POST_CLINICA_COMPROBANTE_UPDATE, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@campo", campo));
                        cmd.Parameters.Add(new SqlParameter("@nuevovalor", nuevovalor));
                        cmd.Parameters.Add(new SqlParameter("@xml", xml));
                        cmd.Parameters.Add(new SqlParameter("@codigobarrajpg", codigobarrajpg));
                        cmd.Parameters.Add(new SqlParameter("@codigo", codigo));

                        BE_VentasCabecera response = new BE_VentasCabecera();

                        await conn.OpenAsync();

                        await cmd.ExecuteNonQueryAsync();

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", 1);
                        vResultadoTransaccion.data = response;

                    }
                }
            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = "Error al actualizar el archivo en la Base de Datos. Posibles errores: 1.- Ya sea porque existe el archivo en la BD. 2.- No se encuentra registrado el comprobante en la BD por eso no puede actualizar. otros:"+ ex.Message.ToString();
            }

            return vResultadoTransaccion;

        }


        public async Task<ResultadoTransaccion<string>> GetDatoCardCodeConsulta(string tipoCliente,string codCliente)
        {
            ResultadoTransaccion<string> vResultadoTransaccion = new ResultadoTransaccion<string>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            string cardcode = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_GET_DATOCARDCODE_CONSULTAR, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@tctip_cliente", tipoCliente));
                        cmd.Parameters.Add(new SqlParameter("@tccodigo", codCliente));

                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                cardcode = ((reader["cardcode"]) is DBNull) ? string.Empty : reader["cardcode"].ToString().Trim();
                            }
                        }

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", 1);
                        vResultadoTransaccion.data = cardcode;

                    }
                }
            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return vResultadoTransaccion;

        }
        public async Task<ResultadoTransaccion<BE_VentasDetalle>> GetVentaDetallePorCodVenta(string codVenta)
        {
            ResultadoTransaccion<BE_VentasDetalle> vResultadoTransaccion = new ResultadoTransaccion<BE_VentasDetalle>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_GET_VENTA_DETALLE_BY_CODVENTA, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codventa", codVenta));

                        List<BE_VentasDetalle> ListaResponse = new List<BE_VentasDetalle>();
                        var response = new BE_VentasDetalle();

                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response = new BE_VentasDetalle();
                                response.codproducto = ((reader["codproducto"]) is DBNull) ? string.Empty : reader["codproducto"].ToString().Trim();
                                response.nombreproducto = ((reader["nombreproducto"]) is DBNull) ? string.Empty : (string)reader["nombreproducto"].ToString().Trim();
                                response.cantidad = ((reader["cantidad"]) is DBNull) ? 0 : (int)reader["cantidad"];
                                response.cantidad_fraccion = ((reader["cantidad_fraccion"]) is DBNull) ? 0 : (int)reader["cantidad_fraccion"];
                                response.cnt_unitario = ((reader["cnt_unitario"]) is DBNull) ? 0 : (int)reader["cnt_unitario"];
                                response.prc_unitario = ((reader["prc_unitario"]) is DBNull) ? 0 : (decimal)reader["prc_unitario"];
                                response.precioventaPVP = ((reader["precioventaPVP"]) is DBNull) ? 0 : (decimal)reader["precioventaPVP"];
                                response.porcentajedctoproducto = ((reader["porcentajedctoproducto"]) is DBNull) ? 0 : (decimal)reader["porcentajedctoproducto"];
                                response.porcentajedctoplan = ((reader["porcentajedctoplan"]) is DBNull) ? 0 : (decimal)reader["porcentajedctoplan"];
                                response.montototal = ((reader["montototal"]) is DBNull) ? 0 : (decimal)reader["montototal"];
                                response.montopaciente = ((reader["montopaciente"]) is DBNull) ? 0 : (decimal)reader["montopaciente"];
                                response.montoaseguradora = ((reader["montoaseguradora"]) is DBNull) ? 0 : (decimal)reader["montoaseguradora"];
                                response.valorVVP = ((reader["valorVVP"]) is DBNull) ? 0 : (decimal)reader["valorVVP"];
                                ListaResponse.Add(response);

                            }

                        }

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", ListaResponse.Count());
                        vResultadoTransaccion.dataList = ListaResponse;

                    }
                }
            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return vResultadoTransaccion;

        }

        public async Task<ResultadoTransaccion<AsegContPaci>> GetRucConsultav2PorFiltro(string codPaciente,string codAseguradora,string codCia)
        {
            ResultadoTransaccion<AsegContPaci> vResultadoTransaccion = new ResultadoTransaccion<AsegContPaci>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxClinica))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_GET_RUC_CONSULTAV2_BY_FILTRO, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codpaciente", codPaciente));
                        cmd.Parameters.Add(new SqlParameter("@codaseguradora", codAseguradora));
                        cmd.Parameters.Add(new SqlParameter("@codcia", codCia));

                        SqlParameter oParamRucaseg = new SqlParameter("@rucaseg", SqlDbType.Char, 11)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(oParamRucaseg);

                        SqlParameter oParamNombreaseg = new SqlParameter("@nombreaseg", SqlDbType.Char, 60)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(oParamNombreaseg);

                        SqlParameter oParamDireccionaseg = new SqlParameter("@direccionaseg", SqlDbType.Char, 60)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(oParamDireccionaseg);

                        //
                        SqlParameter oParamRuccia = new SqlParameter("@ruccia", SqlDbType.Char, 11)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(oParamRuccia);

                        SqlParameter oParamNombrecia = new SqlParameter("@nombrecia", SqlDbType.Char, 60)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(oParamNombrecia);

                        SqlParameter oParamDireccioncia = new SqlParameter("@direccioncia", SqlDbType.Char, 60)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(oParamDireccioncia);

                        SqlParameter oParamRucpac = new SqlParameter("@rucpac", SqlDbType.NVarChar, 11)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(oParamRucpac);

                        SqlParameter oParamNombrepac = new SqlParameter("@nombrepac", SqlDbType.NVarChar, 60)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(oParamNombrepac);

                        SqlParameter oParamDireccionpac = new SqlParameter("@direccionpac", SqlDbType.NVarChar, 60)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(oParamDireccionpac);

                        SqlParameter oParamCorreoaseg = new SqlParameter("@correoaseg", SqlDbType.NVarChar, 60)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(oParamCorreoaseg);

                        SqlParameter oParamCorreocia = new SqlParameter("@correocia", SqlDbType.NVarChar, 60)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(oParamCorreocia);

                        SqlParameter oParamCorreopac = new SqlParameter("@correopac", SqlDbType.NVarChar, 60)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(oParamCorreopac);

                        SqlParameter oParamCardcodeA = new SqlParameter("@cardcodeA", SqlDbType.NVarChar, 15)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(oParamCardcodeA);

                        SqlParameter oParamCardcodeC = new SqlParameter("@cardcodeC", SqlDbType.NVarChar, 15)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(oParamCardcodeC);

                        SqlParameter oParamCardcodeP = new SqlParameter("@cardcodeP", SqlDbType.NVarChar, 15)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(oParamCardcodeP);


                        AsegContPaci response = new AsegContPaci();

                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        response.seguradoraRuc = oParamRucaseg.Value.ToString().Trim();
                        response.seguradoraNombre = oParamNombreaseg.Value.ToString().Trim();
                        response.seguradoraDireccion = oParamDireccionaseg.Value.ToString().Trim();
                        response.seguradoraCorreo = oParamCorreoaseg.Value.ToString().Trim();
                        response.seguradoraCardCode = oParamCardcodeA.Value.ToString().Trim();

                        response.contratanteRuc = oParamRuccia.Value.ToString().Trim();
                        response.contratanteNombre = oParamNombrecia.Value.ToString().Trim();
                        response.contratanteDireccion = oParamDireccioncia.Value.ToString().Trim();
                        response.contratanteCorreo = oParamCorreocia.Value.ToString().Trim();
                        response.contratanteCardCode = oParamCardcodeC.Value.ToString().Trim();

                        response.pacienteRuc = oParamRucpac.Value.ToString().Trim();
                        response.pacienteNombre = oParamNombrepac.Value.ToString().Trim();
                        response.pacienteDireccion = oParamDireccionpac.Value.ToString().Trim();
                        response.pacienteCorreo = oParamCorreopac.Value.ToString().Trim();
                        response.pacienteCardCode = oParamCardcodeP.Value.ToString().Trim();

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", 1);
                        vResultadoTransaccion.data = response;
                    }
                }
            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return vResultadoTransaccion;

        }

        public async Task<ResultadoTransaccion<B_MdsynPagos>> GetMdsynPagosConsulta(long ide_pagos_bot, int ide_mdsyn_reserva, int ide_correl_reserva,string cod_liquidacion,string cod_venta,int orden)
        {
            ResultadoTransaccion<B_MdsynPagos> vResultadoTransaccion = new ResultadoTransaccion<B_MdsynPagos>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxClinica))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_GET_MDSYN_PAGOS_CONSULTA_BY_FILTRO, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ide_pagos_bot", ide_pagos_bot));
                        cmd.Parameters.Add(new SqlParameter("@ide_mdsyn_reserva", ide_mdsyn_reserva));
                        cmd.Parameters.Add(new SqlParameter("@ide_correl_reserva", ide_correl_reserva));
                        cmd.Parameters.Add(new SqlParameter("@cod_liquidacion", cod_liquidacion));
                        cmd.Parameters.Add(new SqlParameter("@cod_venta", cod_venta));
                        cmd.Parameters.Add(new SqlParameter("@orden", orden));

                        B_MdsynPagos response = new B_MdsynPagos();

                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.ide_pagos_bot = ((reader["@ide_pagos_bot"]) is DBNull) ? 0 : (int)reader["@ide_pagos_bot"];
                                response.est_pagado = ((reader["@est_pagado"]) is DBNull) ? string.Empty : (string)reader["@est_pagado"];
                                response.nro_operacion = ((reader["@nro_operacion"]) is DBNull) ? string.Empty : (string)reader["@nro_operacion"];
                                response.tip_tarjeta = ((reader["@tip_tarjeta"]) is DBNull) ? string.Empty : (string)reader["@tip_tarjeta"];
                                response.num_tarjeta = ((reader["@num_tarjeta"]) is DBNull) ? string.Empty : (string)reader["@num_tarjeta"];
                                response.cnt_monto_pago = ((reader["@cnt_monto_pago"]) is DBNull) ? 0 : (decimal)reader["@cnt_monto_pago"];
                                response.terminal = ((reader["@terminal"]) is DBNull) ? string.Empty : (string)reader["@terminal"];
                                response.flg_pago_usado = ((reader["@flg_pago_usado"]) is DBNull) ? string.Empty : (string)reader["@flg_pago_usado"];
                                
                            }
                        }

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", 1);
                        vResultadoTransaccion.data = response;
                    }
                }
            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return vResultadoTransaccion;

        }

        public async Task<ResultadoTransaccion<object>> GetLimiteConsumoPersonalPorCodPersonal(string codPersonal)
        {
            ResultadoTransaccion<object> vResultadoTransaccion = new ResultadoTransaccion<object>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIMITE_CONSUMO_PERSONAL_BY_CODPERSONAL, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codpersonal", codPersonal));

                        await conn.OpenAsync();

                        var objSetVista = new object();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {

                               var  objVista = new { 
                                    fecha1 = ((reader["fecha1"]) is DBNull) ? string.Empty : reader["fecha1"].ToString(), 
                                    fecha2 = ((reader["fecha1"]) is DBNull) ? string.Empty : reader["fecha1"].ToString(),
                                    montolimite = (reader["montolimite"] is DBNull) ? 0 : (decimal)reader["montolimite"],
                                    montoconsumo = (reader["montoconsumo"] is DBNull) ? 0 : (decimal)reader["montoconsumo"],
                                    vender = ((reader["vender"]) is DBNull) ? string.Empty : reader["vender"].ToString(),
                                    codpersonal = ((reader["codpersonal"]) is DBNull) ? string.Empty : reader["codpersonal"].ToString(),
                                    nombre = ((reader["nombre"]) is DBNull) ? string.Empty : reader["nombre"].ToString()
                               };
                                objSetVista = objVista;
                            }
                        }

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", 1);
                        vResultadoTransaccion.data = objSetVista;
                    }
                }
            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return vResultadoTransaccion;

        }

        public async Task<ResultadoTransaccion<object>> GetCorrelativoConsulta()
        {
            // por el momento no se usa, se est usando SerieRepository GetListConfigDocumentoPorNombreMaquina
            
            ResultadoTransaccion<object> vResultadoTransaccion = new ResultadoTransaccion<object>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_GET_CORRELATIVO_CONSULTA, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        await conn.OpenAsync();

                        var objSetVista = new object();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {

                                var objVista = new
                                {
                                    boleta = ((reader["Boleta"]) is DBNull) ? string.Empty : reader["Boleta"].ToString(),
                                    factura = ((reader["Factura"]) is DBNull) ? string.Empty : reader["Factura"].ToString(),
                                    creditoB = (reader["CreditoB"] is DBNull) ? string.Empty : reader["CreditoB"],
                                    creditoF = (reader["CreditoF"] is DBNull) ? string.Empty : reader["CreditoF"],
                                    flg_electronico = ((reader["flg_electronico"]) is DBNull) ? string.Empty : reader["flg_electronico"].ToString(),
                                    generar_e = ((reader["generar_e"]) is DBNull) ? string.Empty : reader["generar_e"].ToString(),
                                    flg_otorgarf = (reader["flg_otorgarf"] is DBNull) ? string.Empty : reader["flg_otorgarf"],
                                    flg_otorgarb = (reader["flg_otorgarb"] is DBNull) ? string.Empty : reader["flg_otorgarb"]

                                };
                                objSetVista = objVista;
                            }
                        }

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", 1);
                        vResultadoTransaccion.data = objSetVista;
                    }
                }
            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return vResultadoTransaccion;

        }
        public async Task<ResultadoTransaccion<BE_ConsumoPersonal>> GetCsLimiteConsumoPersonalPorCodPersonal(string codPersonal)
        {
            ResultadoTransaccion<BE_ConsumoPersonal> vResultadoTransaccion = new ResultadoTransaccion<BE_ConsumoPersonal>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIMITE_CONSUMO_PERSONAL_BY_CODPERSONAL, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codpersonal", codPersonal));

                        await conn.OpenAsync();

                        BE_ConsumoPersonal modal = new BE_ConsumoPersonal();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                modal = new BE_ConsumoPersonal();
                                modal.fecha1 = ((reader["fecha1"]) is DBNull) ? string.Empty : reader["fecha1"].ToString();
                                modal.fecha2 = ((reader["fecha1"]) is DBNull) ? string.Empty : reader["fecha1"].ToString();
                                modal.montolimite = (reader["montolimite"] is DBNull) ? 0 : (decimal)reader["montolimite"];
                                modal.montoconsumo = (reader["montoconsumo"] is DBNull) ? 0 : (decimal)reader["montoconsumo"];
                                modal.vender = ((reader["vender"]) is DBNull) ? string.Empty : reader["vender"].ToString();
                                modal.codpersonal = ((reader["codpersonal"]) is DBNull) ? string.Empty : reader["codpersonal"].ToString();
                                modal.nombre = ((reader["nombre"]) is DBNull) ? string.Empty : reader["nombre"].ToString();
                            }
                        }

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", 1);
                        vResultadoTransaccion.data = modal;
                    }
                }
            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return vResultadoTransaccion;

        }


        public async Task<ResultadoTransaccion<string>> ComprobantesRegistrar(BE_Comprobante value,string tipocomprobante,string correo,string codTipoafectacionigv,bool wFlg_electronico,string maquina)
        {
            ResultadoTransaccion<string> vResultadoTransaccion = new ResultadoTransaccion<string>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            string codcomprobante = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        //REQ_ValeSalidaIns
                        using (SqlCommand cmd = new SqlCommand(SP_GET_COMPROBANTE_INSERT, conn, transaction))
                        {
                            
                            int li_contador = 0;
                            string variostipopago = "";
                            foreach (var item in value.cuadreCaja)
                            {
                                li_contador = li_contador + 1;
                                variostipopago = (li_contador == 1) ? "N" : "S";
                                cmd.Parameters.Clear();
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@tipocomprobante", tipocomprobante));
                                cmd.Parameters.Add(new SqlParameter("@codventa", value.codventa));
                                cmd.Parameters.Add(new SqlParameter("@monto", item.monto));
                                cmd.Parameters.Add(new SqlParameter("@moneda", value.moneda));
                                cmd.Parameters.Add(new SqlParameter("@anombrede", value.anombrede));
                                cmd.Parameters.Add(new SqlParameter("@ruc", value.ruc));
                                cmd.Parameters.Add(new SqlParameter("@direccion", value.direccion));
                                cmd.Parameters.Add(new SqlParameter("@tipopago", item.tipopago));
                                cmd.Parameters.Add(new SqlParameter("@nombreentidad", item.nombreentidad.ToString()));
                                cmd.Parameters.Add(new SqlParameter("@descripcionentidad", item.descripcionentidad.ToString()));
                                cmd.Parameters.Add(new SqlParameter("@numeroentidad", item.numeroentidad.ToString()));
                                cmd.Parameters.Add(new SqlParameter("@codterminal", item.codterminal));
                                cmd.Parameters.Add(new SqlParameter("@operacion", "T"));
                                cmd.Parameters.Add(new SqlParameter("@variostipopago", variostipopago));
                                cmd.Parameters.Add(new SqlParameter("@strcodcomprobante", codcomprobante));
                                cmd.Parameters.Add(new SqlParameter("@tipodecambio", value.tipodecambio));
                                cmd.Parameters.Add(new SqlParameter("@maquina", maquina));
                                
                                SqlParameter oParam = new SqlParameter("@codcomprobante", SqlDbType.Char, 11)
                                {
                                    Direction = ParameterDirection.Output
                                };
                                cmd.Parameters.Add(oParam);

                                await cmd.ExecuteNonQueryAsync();

                                codcomprobante  = cmd.Parameters["@codcomprobante"].Value.ToString();
                                if(codcomprobante.Equals("")) throw new Exception("Error al insertar comprobante !!");
                                vResultadoTransaccion.IdRegistro = 0;
                                vResultadoTransaccion.ResultadoCodigo = 0;
                                vResultadoTransaccion.ResultadoDescripcion = "SE REGISTRO CORRECTAMENTE";
                                vResultadoTransaccion.data = codcomprobante;

                            }
                        }

                        if (codcomprobante != string.Empty) {
                            value.codcomprobante = codcomprobante;
                                //Actualizando valores

                                using (SqlCommand cmd = new SqlCommand(SP_POST_COMPROBANTE_VAL_UPDATE, conn, transaction))
                                {
                                  
                                    cmd.Parameters.Clear();
                                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                    //cmd.Parameters.Add(new SqlParameter("@coduser", value.coduser));
                                    cmd.Parameters.Add(new SqlParameter("@idusuario", value.idusuario));
                                    //cmd.Parameters.Add(new SqlParameter("@codcentro", value.codcentro));// añadimos por bd  SBALogistica
                                    cmd.Parameters.Add(new SqlParameter("@cardcode", value.cardcode)); 
                                    cmd.Parameters.Add(new SqlParameter("@tipdocidentidad", value.tipdocidentidad));// default null
                                    cmd.Parameters.Add(new SqlParameter("@docidentidad", value.docidentidad));// default null
                                    // inicio clinica comprobante electronico
                                    cmd.Parameters.Add(new SqlParameter("@flg_gratuito", value.flg_gratuito));
                                    cmd.Parameters.Add(new SqlParameter("@correo", correo));
                                    cmd.Parameters.Add(new SqlParameter("@tipoafectacionigv", codTipoafectacionigv));
                                    // fin clinica comprobante electronico
                                    cmd.Parameters.Add(new SqlParameter("@codcomprobante", codcomprobante));

                                    await cmd.ExecuteNonQueryAsync();

                                    vResultadoTransaccion.IdRegistro = 0;
                                    vResultadoTransaccion.ResultadoCodigo = 0;
                                    vResultadoTransaccion.ResultadoDescripcion = "SE REGISTRO CORRECTAMENTE";
                                    vResultadoTransaccion.data = codcomprobante;
                             
                                }
                        }

                        if (wFlg_electronico) { 
                        


                        }

                        //bool wFlg_electronico

                        transaction.Commit();
                        transaction.Dispose();

                    }
                    catch (Exception ex)
                    {
                        vResultadoTransaccion.IdRegistro = -1;
                        vResultadoTransaccion.ResultadoCodigo = -1;
                        vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception ex3)
                        {

                            vResultadoTransaccion.ResultadoDescripcion = ex3.Message.ToString();
                        }
                        //throw;
                    }
                }
            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();

            }
            return vResultadoTransaccion;

        }

        public async Task<ResultadoTransaccion<BE_ComprobanteElectronico>> GetComprobanteElectroncioCodVenta(string codComprobante,
            int estadoRegistro, int estadoCdr,string fechaIni,string fechaFin,string codSistema,string tipoCompsunat,string tipoCompcsf,string codComprobanteElec, int orden)
        {
            ResultadoTransaccion<BE_ComprobanteElectronico> vResultadoTransaccion = new ResultadoTransaccion<BE_ComprobanteElectronico>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_POST_COMPROBANTE_ELECTRONICO_POR_CODCOMPROBANTE_TK, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codcomprobantepk", codComprobante)); 
                        cmd.Parameters.Add(new SqlParameter("@estadoregistro", estadoRegistro)); 
                        cmd.Parameters.Add(new SqlParameter("@estadocdr", estadoCdr)); 
                        cmd.Parameters.Add(new SqlParameter("@fechaini", fechaIni)); 
                        cmd.Parameters.Add(new SqlParameter("@fechafin", fechaFin)); 
                        cmd.Parameters.Add(new SqlParameter("@codsistema", codSistema)); 
                        cmd.Parameters.Add(new SqlParameter("@tipocompsunat", tipoCompsunat)); 
                        cmd.Parameters.Add(new SqlParameter("@tipocompcsf", tipoCompcsf)); 
                        cmd.Parameters.Add(new SqlParameter("@codcomprobanteelec", codComprobanteElec)); 
                        cmd.Parameters.Add(new SqlParameter("@orden", orden));

                        BE_ComprobanteElectronico response = new BE_ComprobanteElectronico();

                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.codcomprobante = ((reader["codcomprobante"]) is DBNull) ? string.Empty : reader["codcomprobante"].ToString().Trim();
                                response.codcomprobantee = ((reader["codcomprobantee"]) is DBNull) ? string.Empty : reader["codcomprobantee"].ToString().Trim();
                                response.tipo_otorgamiento = ((reader["tipo_otorgamiento"]) is DBNull) ? string.Empty : reader["tipo_otorgamiento"].ToString().Trim();
                                response.estado_cdr = ((reader["estado_cdr"]) is DBNull) ? string.Empty : reader["estado_cdr"].ToString().Trim();
                                response.flg_confirma = ((reader["flg_confirma"]) is DBNull) ? false : (bool)reader["flg_confirma"];
                                response.codigobarra = ((reader["codigobarra"]) is DBNull) ? null :  (Byte[])reader["codigobarra"];
                                response.fecha_registro_sis = ((reader["fecha_registro_sis"]) is DBNull) ? DateTime.MinValue : (DateTime)reader["fecha_registro_sis"];
                                response.tipo_comprobante = ((reader["tipo_comprobante"]) is DBNull) ? string.Empty : reader["tipo_comprobante"].ToString().Trim();
                                response.ruc_emisor = ((reader["ruc_emisor"]) is DBNull) ? string.Empty : reader["ruc_emisor"].ToString().Trim();
                                response.url_webservices = ((reader["url_webservices"]) is DBNull) ? string.Empty : reader["url_webservices"].ToString().Trim();

                            }
                        }

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", 1);
                        vResultadoTransaccion.data = response;
                    }
                }
            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return vResultadoTransaccion;

        }

        public async Task<ResultadoTransaccion<BE_ComprobanteElectronicoLogXmlCabPrint>> GetComprobanteElectroncioLogXmlCab_print(string codcomprobante,string maquina,
            int idusuario, int orden)
        {
            ResultadoTransaccion<BE_ComprobanteElectronicoLogXmlCabPrint> vResultadoTransaccion = new ResultadoTransaccion<BE_ComprobanteElectronicoLogXmlCabPrint>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;


            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_POST_COMPROBANTE_ELECTRONICO_LOG_XML_CAB_PRINT, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codcomprobante", codcomprobante));
                        cmd.Parameters.Add(new SqlParameter("@maquina", maquina));
                        cmd.Parameters.Add(new SqlParameter("@idusuario", idusuario));
                        cmd.Parameters.Add(new SqlParameter("@orden", orden));

                        List<BE_ComprobanteElectronicoLogXmlCabPrint> lista = new List<BE_ComprobanteElectronicoLogXmlCabPrint>();
                        BE_ComprobanteElectronicoLogXmlCabPrint response;

                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response = new BE_ComprobanteElectronicoLogXmlCabPrint();
                                response.codcomprobante  = ((reader["codcomprobante"]) is DBNull) ? string.Empty : reader["codcomprobante"].ToString().Trim();
                                response.codcomprobante_e = ((reader["codcomprobante_e"]) is DBNull) ? string.Empty : reader["codcomprobante_e"].ToString().Trim();
                                response.tipoplantilla = ((reader["tipoplantilla"]) is DBNull) ? string.Empty : reader["tipoplantilla"].ToString().Trim();
                                response.tipocomp_sunat = ((reader["tipocomp_sunat"]) is DBNull) ? string.Empty : reader["tipocomp_sunat"].ToString().Trim();
                                response.tipocomp_tci = ((reader["tipocomp_tci"]) is DBNull) ? string.Empty : reader["tipocomp_tci"].ToString().Trim();
                                response.estado = ((reader["estado"]) is DBNull) ? string.Empty : reader["estado"].ToString().Trim();
                                response.nombreestado = ((reader["nombreestado"]) is DBNull) ? string.Empty : reader["nombreestado"].ToString().Trim();
                                response.codtipocliente = ((reader["codtipocliente"]) is DBNull) ? string.Empty : reader["codtipocliente"].ToString().Trim();
                                response.estado = ((reader["estado"]) is DBNull) ? string.Empty : reader["estado"].ToString().Trim();
                                response.nombreestado = ((reader["nombreestado"]) is DBNull) ? string.Empty : reader["nombreestado"].ToString().Trim();
                                response.codtipocliente = ((reader["codtipocliente"]) is DBNull) ? string.Empty : reader["codtipocliente"].ToString().Trim();
                                response.tipomov_vta = ((reader["tipomov_vta"]) is DBNull) ? string.Empty : reader["tipomov_vta"].ToString().Trim();
                                response.codventa = ((reader["codventa"]) is DBNull) ? string.Empty : reader["codventa"].ToString().Trim();
                                response.fechaemision = ((reader["fechaemision"]) is DBNull) ? DateTime.MinValue : Convert.ToDateTime(reader["fechaemision"]);
                                response.anombrede = ((reader["anombrede"]) is DBNull) ? string.Empty : reader["anombrede"].ToString().Trim();
                                response.direccion = ((reader["direccion"]) is DBNull) ? string.Empty : reader["direccion"].ToString().Trim();
                                response.ruc = ((reader["ruc"]) is DBNull) ? string.Empty : reader["ruc"].ToString().Trim();
                                response.tipdocidentidad_sunat = ((reader["tipdocidentidad_sunat"]) is DBNull) ? string.Empty : reader["tipdocidentidad_sunat"].ToString().Trim();
                                response.ruc_sunat = ((reader["ruc_sunat"]) is DBNull) ? string.Empty : reader["ruc_sunat"].ToString().Trim();
                                response.receptor_correo = ((reader["receptor_correo"]) is DBNull) ? string.Empty : reader["receptor_correo"].ToString().Trim();
                                response.empresa_ruc = ((reader["empresa_ruc"]) is DBNull) ? string.Empty : reader["empresa_ruc"].ToString().Trim();
                                response.empresa_tipodocidentidad = ((reader["empresa_tipodocidentidad"]) is DBNull) ? string.Empty : reader["empresa_tipodocidentidad"].ToString().Trim();
                                response.empresa_nombre = ((reader["empresa_nombre"]) is DBNull) ? string.Empty : reader["empresa_nombre"].ToString().Trim();
                                response.empresa_direccion = ((reader["empresa_direccion"]) is DBNull) ? string.Empty : reader["empresa_direccion"].ToString().Trim();
                                response.empresa_telefono = ((reader["empresa_telefono"]) is DBNull) ? string.Empty : reader["empresa_telefono"].ToString().Trim();
                                response.concepto = ((reader["concepto"]) is DBNull) ? string.Empty : reader["concepto"].ToString().Trim();
                                response.codatencion = ((reader["codatencion"]) is DBNull) ? string.Empty : reader["codatencion"].ToString().Trim();
                                response.codpaciente = ((reader["codpaciente"]) is DBNull) ? string.Empty : reader["codpaciente"].ToString().Trim();
                                response.nombrepaciente = ((reader["nombrepaciente"]) is DBNull) ? string.Empty : reader["nombrepaciente"].ToString().Trim();
                                response.nombretitular = ((reader["nombretitular"]) is DBNull) ? string.Empty : reader["nombretitular"].ToString().Trim();
                                response.nombrecia = ((reader["nombrecia"]) is DBNull) ? string.Empty : reader["nombrecia"].ToString().Trim();
                                response.codpoliza = ((reader["codpoliza"]) is DBNull) ? string.Empty : reader["codpoliza"].ToString().Trim();
                                response.observaciones = ((reader["observaciones"]) is DBNull) ? string.Empty : reader["observaciones"].ToString().Trim();
                                response.texto1 = ((reader["texto1"]) is DBNull) ? string.Empty : reader["texto1"].ToString().Trim();
                                response.suc_direccion = ((reader["suc_direccion"]) is DBNull) ? string.Empty : reader["suc_direccion"].ToString().Trim();
                                response.tipopagotxt = ((reader["tipopagotxt"]) is DBNull) ? string.Empty : reader["tipopagotxt"].ToString().Trim();
                                response.c_moneda = ((reader["c_moneda"]) is DBNull) ? string.Empty : reader["c_moneda"].ToString().Trim();
                                response.c_nombremoneda = ((reader["c_nombremoneda"]) is DBNull) ? string.Empty : reader["c_nombremoneda"].ToString().Trim();
                                response.c_simbolomoneda = ((reader["c_simbolomoneda"]) is DBNull) ? string.Empty : reader["c_simbolomoneda"].ToString().Trim();
                                response.c_montodsctoplan = ((reader["c_montodsctoplan"]) is DBNull) ? 0 : (decimal)reader["c_montodsctoplan"];
                                response.c_montoafecto = ((reader["c_montoafecto"]) is DBNull) ? 0 : (decimal)reader["c_montoafecto"];
                                response.c_montoinafecto = ((reader["c_montoinafecto"]) is DBNull) ? 0 : (decimal)reader["c_montoinafecto"];
                                response.c_montoexonerado = ((reader["c_montoexonerado"]) is DBNull) ? 0 : (decimal)reader["c_montoexonerado"];
                                response.c_montoigv = ((reader["c_montoigv"]) is DBNull) ? 0 : (decimal)reader["c_montoigv"];
                                response.c_montoneto = ((reader["c_montoneto"]) is DBNull) ? 0 : (decimal)reader["c_montoneto"];
                                response.porcentajeimpuesto = ((reader["porcentajeimpuesto"]) is DBNull) ? 0 : (decimal)reader["porcentajeimpuesto"];
                                response.porcentajecoaseguro = ((reader["porcentajecoaseguro"]) is DBNull) ? 0 : (decimal)reader["porcentajecoaseguro"];
                                response.flg_gratuito = ((reader["flg_gratuito"]) is DBNull) ? string.Empty : (string)reader["flg_gratuito"];
                                //detalle:
                                response.d_orden = ((reader["d_orden"]) is DBNull) ? string.Empty : (string)reader["d_orden"];
                                response.d_unidad = ((reader["d_unidad"]) is DBNull) ? string.Empty : (string)reader["d_unidad"];
                                response.d_codproducto = ((reader["d_codproducto"]) is DBNull) ? string.Empty : (string)reader["d_codproducto"];
                                response.nombreproducto = ((reader["nombreproducto"]) is DBNull) ? string.Empty : (string)reader["nombreproducto"];
                                response.d_cant_sunat = ((reader["d_cant_sunat"]) is DBNull) ? 0 : (decimal)reader["d_cant_sunat"];
                                response.d_stockfraccion = ((reader["d_stockfraccion"]) is DBNull) ? 0 : (int)reader["d_stockfraccion"];
                                response.d_cantidad = ((reader["d_cantidad"]) is DBNull) ? 0 : (int)reader["d_cantidad"];
                                response.d_cantidad_fraccion = ((reader["d_cantidad_fraccion"]) is DBNull) ? 0 : (int)reader["d_cantidad_fraccion"];
                                response.d_valorVVP = ((reader["d_valorVVP"]) is DBNull) ? 0 : (decimal)reader["d_valorVVP"];
                                response.d_precioventaPVP = ((reader["d_codproducto"]) is DBNull) ? 0: (decimal)reader["d_precioventaPVP"];
                                response.d_porcdctoproducto = ((reader["d_porcdctoproducto"]) is DBNull) ? 0 : (decimal)reader["d_porcdctoproducto"];
                                response.d_porcdctoplan = ((reader["d_porcdctoplan"]) is DBNull) ? 0 : (decimal)reader["d_porcdctoplan"];
                                response.d_preciounidadcondcto = ((reader["d_preciounidadcondcto"]) is DBNull) ? 0 : (decimal)reader["d_preciounidadcondcto"];
                                response.d_montototal = ((reader["d_montototal"]) is DBNull) ? 0 : (decimal)reader["d_montototal"];
                                response.d_montopaciente = ((reader["d_montopaciente"]) is DBNull) ? 0 : (decimal)reader["d_montopaciente"];
                                response.d_montoaseguradora = ((reader["d_montoaseguradora"]) is DBNull) ? 0 : (decimal)reader["d_montoaseguradora"];
                                //detalle_sunat:
                                response.d_ventaunitario_sinigv = ((reader["d_ventaunitario_sinigv"]) is DBNull) ? 0 : (decimal)reader["d_ventaunitario_sinigv"];
                                response.d_ventaunitario_conigv = ((reader["d_ventaunitario_conigv"]) is DBNull) ? 0 : (decimal)reader["d_ventaunitario_conigv"];
                                response.d_dscto_sinigv = ((reader["d_dscto_sinigv"]) is DBNull) ? 0 : (decimal)reader["d_dscto_sinigv"];
                                response.d_dscto_conigv = ((reader["d_dscto_conigv"]) is DBNull) ? 0 : (decimal)reader["d_dscto_conigv"];
                                response.d_total_sinigv = ((reader["d_total_sinigv"]) is DBNull) ? 0 : (decimal)reader["d_total_sinigv"];
                                response.d_total_conigv = ((reader["d_total_conigv"]) is DBNull) ? 0 : (decimal)reader["d_total_conigv"];
                                response.d_total_sinigv2 = ((reader["d_total_sinigv2"]) is DBNull) ? 0 : (decimal)reader["d_total_sinigv2"];
                                response.d_montoigv = ((reader["d_montoigv"]) is DBNull) ? 0 : (decimal)reader["d_montoigv"];
                                response.d_determinante = ((reader["d_determinante"]) is DBNull) ? string.Empty : (string)reader["d_determinante"];
                                response.d_codigotipoprecio = ((reader["d_codigotipoprecio"]) is DBNull) ? string.Empty : (string)reader["d_codigotipoprecio"];
                                response.d_afectacionigv = ((reader["d_afectacionigv"]) is DBNull) ? string.Empty : (string)reader["d_afectacionigv"];
                                response.d_montobase = ((reader["d_montobase"]) is DBNull) ? 0 : (decimal)reader["d_montobase"];
                                response.tipo_operacion = ((reader["tipo_operacion"]) is DBNull) ? string.Empty : (string)reader["tipo_operacion"];
                                response.hora_emision = ((reader["hora_emision"]) is DBNull) ? string.Empty : (string)reader["hora_emision"];
                                response.total_impuesto = ((reader["total_impuesto"]) is DBNull) ? 0: (decimal)reader["total_impuesto"];
                                response.total_valorventa = ((reader["total_valorventa"]) is DBNull) ? 0: (decimal)reader["total_valorventa"];
                                response.total_precioventa = ((reader["total_precioventa"]) is DBNull) ? 0: (decimal)reader["total_precioventa"];
                                response.vs_ubl = ((reader["vs_ubl"]) is DBNull) ? string.Empty: (string)reader["vs_ubl"];
                                response.mt_total = ((reader["mt_total"]) is DBNull) ? 0: (decimal)reader["mt_total"];
                                response.mtg_Base = ((reader["mtg_Base"]) is DBNull) ? 0: (decimal)reader["mtg_Base"];
                                response.mtg_ValorImpuesto = ((reader["mtg_ValorImpuesto"]) is DBNull) ? 0: (decimal)reader["mtg_ValorImpuesto"];
                                response.mtg_Porcentaje = ((reader["mtg_Porcentaje"]) is DBNull) ? 0: (decimal)reader["mtg_Porcentaje"];
                                response.d_dscto_unitario = ((reader["d_dscto_unitario"]) is DBNull) ? 0: (decimal)reader["d_dscto_unitario"];
                                response.d_total_condsctoigv = ((reader["d_total_condsctoigv"]) is DBNull) ? 0: (decimal)reader["d_total_condsctoigv"];
                                response.d_dscto_montobase = ((reader["d_dscto_montobase"]) is DBNull) ? 0: (decimal)reader["d_dscto_montobase"];
                                response.CodigoEstabSUNAT = ((reader["CodigoEstabSUNAT"]) is DBNull) ? string.Empty: (string)reader["CodigoEstabSUNAT"];
                                response.CodDistrito = ((reader["CodDistrito"]) is DBNull) ? string.Empty: (string)reader["CodDistrito"];
                                response.forma_pago = ((reader["forma_pago"]) is DBNull) ? string.Empty: (string)reader["forma_pago"];
                                response.cod_tributo = ((reader["cod_tributo"]) is DBNull) ? string.Empty: (string)reader["cod_tributo"];
                                response.cod_afectacionIGV = ((reader["cod_afectacionIGV"]) is DBNull) ? string.Empty: (string)reader["cod_afectacionIGV"];
                                response.des_tributo = ((reader["des_tributo"]) is DBNull) ? string.Empty: (string)reader["des_tributo"];
                                response.cod_UN = ((reader["cod_UN"]) is DBNull) ? string.Empty: (string)reader["cod_UN"];
                                response.codProductoSUNAT = ((reader["CodProductoSUNAT"]) is DBNull) ? string.Empty: (string)reader["CodProductoSUNAT"];
                                response.cuadre = ((reader["cuadre"]) is DBNull) ? string.Empty: (string)reader["cuadre"];
                                response.efact_sunat_resol = ((reader["efact_sunat_resol"]) is DBNull) ? string.Empty: (string)reader["efact_sunat_resol"];
                                lista.Add(response);
                            }
                        }

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", lista.Count);
                        vResultadoTransaccion.dataList = lista;

                    }
                }
            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return vResultadoTransaccion;

        }


        public async Task<ResultadoTransaccion<BE_ComprobanteElectronico>> GetComprobanteElectroncioVB(
           string codEmpresa, string codComprobantePK, string codComprobante_e, string codSistema, string tipoCompsunat, int orden)
        {
            ResultadoTransaccion<BE_ComprobanteElectronico> vResultadoTransaccion = new ResultadoTransaccion<BE_ComprobanteElectronico>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
          
            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_POST_COMPROBANTE_ELECTRONICO_POR_CODCOMPROBANTE_VB, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codempresa", codEmpresa));
                        cmd.Parameters.Add(new SqlParameter("@codcomprobante", codComprobantePK));
                        cmd.Parameters.Add(new SqlParameter("@codcomprobante_e", codComprobante_e));
                        cmd.Parameters.Add(new SqlParameter("@codsistema", codSistema));
                        cmd.Parameters.Add(new SqlParameter("@tipocomp_sunat", tipoCompsunat));
                        cmd.Parameters.Add(new SqlParameter("@orden", orden));

                        BE_ComprobanteElectronico response = new BE_ComprobanteElectronico();

                        await conn.OpenAsync();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.flg_electronico = ((reader["flg_electronico"]) is DBNull) ? string.Empty : reader["flg_electronico"].ToString().Trim();
                                response.obtener_pdf = ((reader["obtener_pdf"]) is DBNull) ? string.Empty : reader["obtener_pdf"].ToString().Trim();
                                response.nombreestado_cdr = ((reader["nombreestado_cdr"]) is DBNull) ? string.Empty : reader["nombreestado_cdr"].ToString().Trim();
                                response.nombreestado_otorgamiento = ((reader["nombreestado_otorgamiento"]) is DBNull) ? string.Empty : reader["nombreestado_otorgamiento"].ToString().Trim();
                                response.mensaje = ((reader["mensaje"]) is DBNull) ? string.Empty : reader["mensaje"].ToString().Trim();
                            }
                        }

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", 1);
                        vResultadoTransaccion.data = response;
                    }
                }
            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return vResultadoTransaccion;

        }


        #region Vista de Impresion


        public async Task<ResultadoTransaccion<MemoryStream>> GenerarPreVistaPrint(string codcomprobante, string maquina,string archivoImg, int idusuario, int orden)
        {

            ResultadoTransaccion<MemoryStream> vResultadoTransaccion = new ResultadoTransaccion<MemoryStream>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();
            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            string Numero = "{0:###,###,###,###.00;-###,###,###,###.00;0.00;0.00}";

            if (maquina==null) {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = "el nombre de la maquina no puede ser nulo";
                return vResultadoTransaccion;
            }

            try
            {

                string xRutaCodigoBarra = string.Empty;
                


                var objCPE = await GetComprobanteElectroncioCodVenta(codcomprobante, 0, 0, "", "", "", "", "", "", 1);
                if (objCPE.data.codcomprobante !=null)
                {
                    var respCodigoBarra = objCPE.data.codigobarra;
                    if (respCodigoBarra.Length == 0) { 
                    
                    }

                    //Crea el directorio por si no existe
                    if (Directory.Exists(archivoImg) == false) Directory.CreateDirectory(archivoImg);
                    xRutaCodigoBarra = archivoImg + codcomprobante + ".jpg";
                    File.WriteAllBytes(xRutaCodigoBarra, respCodigoBarra); //Creamos el archivo 
                }
                else {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = "El comprobante que esta intentado obtener no es electrónico.";
                    return vResultadoTransaccion;
                }

                ///////////////////////////////////////////////////////////
                //zFarmacia2.Sp_ComprobantesElectronicos_ConsultaVB "001", pCodComprobantePK, "", "L", "", 4
                var data = await this.GetComprobanteElectroncioLogXmlCab_print(codcomprobante, maquina, 1, 0);
                if (data.dataList.Count() <= 0) throw new ArgumentException("NO SE ENCONTRO EL COMPROBANTE");

                var cabecera = data.dataList.ToList()[0];
                var Detalle = data.dataList.ToList();
                int xTamanio = 400 * ((Detalle.Count()==1) ? 2: Detalle.Count());


                decimal pC_MontoAfecto = cabecera.c_montoafecto;
                decimal pC_MontoInafecto = cabecera.c_montoinafecto;
                decimal pC_MontoGratuito = cabecera.c_montogratuito;
                decimal pC_MontoIgv = cabecera.c_montoigv;
                decimal pC_MontoNeto = cabecera.c_montoneto;
                DateTime cfecha = cabecera.fechaemision;
                string Moneda = cabecera.c_simbolomoneda;
                string wResolucion = cabecera.efact_sunat_resol;
                string pNombreEstado = cabecera.nombreestado;
                string pTipoPagotxt = cabecera.tipopagotxt;

                //////////////////////////////////////////////////////////

                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(_urlLogo);
                jpg.WidthPercentage = 37;
                jpg.Alignment = 1;
                PdfPCell imageCell = new PdfPCell();
                imageCell.AddElement(jpg);
                imageCell.Border = 0;
                imageCell.HorizontalAlignment = Element.ALIGN_CENTER;
                imageCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                iTextSharp.text.Image imgCodigoBarra = iTextSharp.text.Image.GetInstance(xRutaCodigoBarra);
                imgCodigoBarra.WidthPercentage = 60;
                imgCodigoBarra.Alignment = 1;
                PdfPCell imageCodigoBarraCell = new PdfPCell();
                imageCodigoBarraCell.AddElement(imgCodigoBarra);
                imageCodigoBarraCell.Border = 0;
                imageCodigoBarraCell.HorizontalAlignment = Element.ALIGN_CENTER;
                imageCodigoBarraCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                //Rectangle pagesize = new Rectangle(360f, 14400f);14100
                var pgSize = new iTextSharp.text.Rectangle(360f, xTamanio);
                Document doc = new Document(pgSize);
                // points to cm
                doc.SetMargins(20f, 20f, 15f, 2f);
                MemoryStream ms = new MemoryStream();
                PdfWriter write = PdfWriter.GetInstance(doc, ms);
                doc.AddAuthor("Grupo SBA");
                doc.AddTitle("Cliníca San Felipe");
                var pe = new PageEventHelper();
                write.PageEvent = pe;
                // Colocamos la fuente que deseamos que tenga el documento
                BaseFont helvetica = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
                BaseFont curier = BaseFont.CreateFont(BaseFont.COURIER, BaseFont.CP1250, true);

                // Titulo
                Font Header1= new iTextSharp.text.Font(helvetica, 8, iTextSharp.text.Font.BOLD, BaseColor.Black);
                Font Header2 = new iTextSharp.text.Font(curier, 10, iTextSharp.text.Font.BOLD, BaseColor.Black);
                Font subTitulo = new iTextSharp.text.Font(helvetica, 12f, iTextSharp.text.Font.BOLD, BaseColor.Black);
                Font parrafoNegroNegrita = new iTextSharp.text.Font(helvetica, 10f, iTextSharp.text.Font.BOLD, BaseColor.Black);
                Font subHeader3 = new iTextSharp.text.Font(helvetica, 8, iTextSharp.text.Font.BOLD, BaseColor.Black);
                Font subHeader4 = new iTextSharp.text.Font(helvetica, 8, iTextSharp.text.Font.NORMAL, BaseColor.Black);
                Font parrafoNegro = new iTextSharp.text.Font(helvetica, 10f, iTextSharp.text.Font.NORMAL, BaseColor.Black);
                Font parrafoItemDetalle = new iTextSharp.text.Font(helvetica, 8f, iTextSharp.text.Font.NORMAL, BaseColor.Black);

                pe.HeaderLeft = " ";
                pe.HeaderFont = parrafoNegroNegrita;
                pe.HeaderRight = " ";

                doc.Open();

                //30f, 40f, 30f
                var tbl = new PdfPTable(new float[] { 30f }) { WidthPercentage = 100 };
                tbl.AddCell(imageCell);

                var c1 = new PdfPCell(new Phrase("CLINICA SAN FELIPE S.A.", new Font(helvetica, 9, Font.BOLD, BaseColor.Black)));
                c1.Border = 0;
                c1.HorizontalAlignment = Element.ALIGN_CENTER;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbl.AddCell(c1);
                doc.Add(tbl);

                tbl = new PdfPTable(new float[] { 30f }) { WidthPercentage = 100 };
                c1 = new PdfPCell(new Phrase("Av. Gregorio Escobedo 650, Jesús María, Lima - Lima", subHeader4)) { Border = 0 };
                c1.HorizontalAlignment = Element.ALIGN_CENTER;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbl.AddCell(c1);
                doc.Add(tbl);

                tbl = new PdfPTable(new float[] { 30f }) { WidthPercentage = 100 };
                c1 = new PdfPCell(new Phrase("Teléfono: (01) 219-0000 RUC: 20100162742\n\n", subHeader4)) { Border = 0 };
                c1.HorizontalAlignment = Element.ALIGN_CENTER;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbl.AddCell(c1);
                doc.Add(tbl);
                //doc.Add(new Phrase(" "));

                tbl = new PdfPTable(new float[] { 30f }) { WidthPercentage = 100 };
                c1 = new PdfPCell(new Phrase("BOLETA DE VENTA ELECTRONICA", Header1)) { Border = 0 };
                c1.HorizontalAlignment = Element.ALIGN_CENTER;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbl.AddCell(c1);
                doc.Add(tbl);

                tbl = new PdfPTable(new float[] { 30f }) { WidthPercentage = 100 };
                c1 = new PdfPCell(new Phrase(cabecera.codcomprobante_e+ "\n\n", Header1)) { Border = 0 };
                c1.HorizontalAlignment = Element.ALIGN_CENTER;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbl.AddCell(c1);
                doc.Add(tbl);
                //doc.Add(new Phrase(" "));

                tbl = new PdfPTable(new float[] { 30f }) { WidthPercentage = 100 };
                c1 = new PdfPCell(new Phrase("ATENCION:", Header2)) { Border = 0 };
                c1.HorizontalAlignment = Element.ALIGN_CENTER;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbl.AddCell(c1);
                doc.Add(tbl);
                doc.Add(new Phrase(" "));


                //Obtenemos los datos de la venta

                ResultadoTransaccion<BE_VentasCabecera> resultadoTransaccionVenta = await GetVentaPorCodVenta("05563376");

                if (resultadoTransaccionVenta.ResultadoCodigo == -1)
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionVenta.ResultadoDescripcion;

                    return vResultadoTransaccion;
                }

                // Generamos la Cabecera
                tbl = new PdfPTable(new float[] { 15f,85f }) { WidthPercentage = 100 };
                //Linea 1
                c1 = new PdfPCell(new Phrase("Nombre", subHeader3)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(": "+cabecera.anombrede.Trim(), subHeader3)) { Border = 0 };
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase("Dirección", subHeader3)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(": " + cabecera.direccion.Trim(), subHeader3)) { Border = 0 };
                tbl.AddCell(c1);

                if (cabecera.tipocomp_sunat.Equals("03"))
                {
                    string etiquetaDoc = string.Empty;
                    switch (cabecera.tipdocidentidad_sunat)
                    {
                        case "0":
                            etiquetaDoc = "No Dom.";
                            break;
                        case "1":
                            etiquetaDoc = "DNI";
                            break;
                        case "4":
                            etiquetaDoc = "Carnet Ext.";
                            break;
                        case "7":
                            etiquetaDoc = "Pasaporte";
                            break;
                    }

                    c1 = new PdfPCell(new Phrase(etiquetaDoc, subHeader3)) { Border = 0 };
                    tbl.AddCell(c1);
                    c1 = new PdfPCell(new Phrase(": " + cabecera.ruc_sunat.Trim(), subHeader3)) { Border = 0 };
                    tbl.AddCell(c1);
                }
                else {
                    c1 = new PdfPCell(new Phrase("RUC", subHeader3)) { Border = 0 };
                    tbl.AddCell(c1);
                    c1 = new PdfPCell(new Phrase(": " + cabecera.ruc_sunat.Trim(), subHeader3)) { Border = 0 };
                    tbl.AddCell(c1);
                }

                c1 = new PdfPCell(new Phrase("F. Emisión", subHeader3)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(": " + cabecera.fechaemision.ToShortTimeString(), subHeader3)) { Border = 0 };
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase("H. Clinica", subHeader3)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(": " + cabecera.codpaciente, subHeader3)) { Border = 0 };
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase("Venta", subHeader3)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(": " + cabecera.codventa, subHeader3)) { Border = 0 };
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase("Paciente", subHeader3)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(": " + cabecera.nombrepaciente, subHeader3)) { Border = 0 };
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase("Coaseguro", subHeader3)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(": " + Convert.ToString(cabecera.porcentajecoaseguro), subHeader3)) { Border = 0 };
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase("Moneda", subHeader3)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(": " + cabecera.c_nombremoneda, subHeader3)) { Border = 0 };
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase("\n\n", subHeader3)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("\n\n", subHeader3)) { Border = 0 };
                tbl.AddCell(c1);
                doc.Add(tbl);

                // Generamos el detalle
                tbl = new PdfPTable(new float[] { 30f, 8f, 8f, 8f, 8f }) { WidthPercentage = 100 };
                c1 = new PdfPCell(new Phrase("Prod.", new Font(Font.HELVETICA, 8f, Font.NORMAL, BaseColor.Black)));
                c1.BorderWidth = 1;
                c1.DisableBorderSide(Rectangle.LEFT_BORDER);
                c1.DisableBorderSide(Rectangle.RIGHT_BORDER);
                c1.DisableBorderSide(Rectangle.TOP_BORDER);
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("Cant.", subHeader4));
                c1.BorderWidth = 1;
                c1.DisableBorderSide(Rectangle.LEFT_BORDER);
                c1.DisableBorderSide(Rectangle.RIGHT_BORDER);
                c1.DisableBorderSide(Rectangle.TOP_BORDER);
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("PUni.", subHeader4));
                c1.BorderWidth = 1;
                c1.DisableBorderSide(Rectangle.LEFT_BORDER);
                c1.DisableBorderSide(Rectangle.RIGHT_BORDER);
                c1.DisableBorderSide(Rectangle.TOP_BORDER);
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("Desc.", subHeader4));
                c1.BorderWidth = 1;
                c1.DisableBorderSide(Rectangle.LEFT_BORDER);
                c1.DisableBorderSide(Rectangle.RIGHT_BORDER);
                c1.DisableBorderSide(Rectangle.TOP_BORDER);
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("PVen.", subHeader4));
                c1.BorderWidth = 1;
                c1.DisableBorderSide(Rectangle.LEFT_BORDER);
                c1.DisableBorderSide(Rectangle.RIGHT_BORDER);
                c1.DisableBorderSide(Rectangle.TOP_BORDER);
                tbl.AddCell(c1);

                foreach (var item in Detalle)
                {
                    c1 = new PdfPCell(new Phrase(item.nombreproducto.Trim(), parrafoItemDetalle)) { Border = 0 };
                    tbl.AddCell(c1);
                    c1 = new PdfPCell(new Phrase(string.Format("{0:#0.0000}", item.d_cant_sunat), parrafoItemDetalle)) { Border = 0 };
                    tbl.AddCell(c1);
                    c1 = new PdfPCell(new Phrase(string.Format("{0:#0.00}", item.d_ventaunitario_conigv), parrafoItemDetalle)) { Border = 0 };
                    tbl.AddCell(c1);
                    c1 = new PdfPCell(new Phrase(string.Format("{0:#0.00}", item.d_dscto_conigv), parrafoItemDetalle)) { Border = 0 };
                    tbl.AddCell(c1);
                    c1 = new PdfPCell(new Phrase(string.Format("{0:#0.00}", item.d_total_conigv), parrafoItemDetalle)) { Border = 0 };
                    tbl.AddCell(c1);
                }
                doc.Add(tbl);

                
                tbl = new PdfPTable(new float[] { 46f, 8f, 8f }) { WidthPercentage = 100 };

                c1 = new PdfPCell(new Phrase("\n\n", subHeader4));
                c1.Colspan = 3;
                c1.Border = 0;
                c1.HorizontalAlignment = Element.ALIGN_RIGHT;
                tbl.AddCell(c1);

                // Totales
                c1 = new PdfPCell(new Phrase("Op. Gravada:", subHeader4));
                c1.Border = 0;
                c1.HorizontalAlignment = Element.ALIGN_RIGHT;
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase("S/.", subHeader4));
                c1.Border = 0;
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(string.Format(Numero, pC_MontoAfecto), subHeader4));
                c1.Border = 0;
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("Op. Inafectas:", subHeader4));
                c1.Border = 0;
                c1.HorizontalAlignment = Element.ALIGN_RIGHT;
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("S/.", subHeader4));
                c1.Border = 0;
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(string.Format(Numero, pC_MontoInafecto), subHeader4));
                c1.Border = 0;
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("Op. Gratuitas:", subHeader4));
                c1.Border = 0;
                c1.HorizontalAlignment = Element.ALIGN_RIGHT;
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("S/.", subHeader4));
                c1.Border = 0;
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(string.Format(Numero, pC_MontoGratuito), subHeader4));
                c1.Border = 0;
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("I.G.V:", subHeader4));
                c1.Border = 0;
                c1.HorizontalAlignment = Element.ALIGN_RIGHT;
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("S/.", subHeader4));
                c1.Border = 0;
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(string.Format(Numero, pC_MontoIgv), subHeader4));
                c1.Border = 0;
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("Importe Total:", subHeader4));
                c1.Border = 0;
                c1.HorizontalAlignment = Element.ALIGN_RIGHT;
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("S/.", subHeader4));
                c1.Border = 0;
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(string.Format(Numero, pC_MontoNeto), subHeader4));
                c1.Border = 0;
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase("\n\n")) { Border = 0 };
                c1.Colspan = 4;
                tbl.AddCell(c1);
                
                doc.Add(tbl);

                tbl = new PdfPTable(new float[] { 30f }) { WidthPercentage = 100 };
                tbl.AddCell(imageCodigoBarraCell);
                c1 = new PdfPCell(new Phrase("\n\n"));
                c1.Border = 0;
                tbl.AddCell(c1);
                doc.Add(tbl);

                if (cabecera.flg_gratuito.Equals("1"))
                {
                    tbl = new PdfPTable(new float[] { 30f }) { WidthPercentage = 100 };
                    c1 = new PdfPCell(new Phrase("TRANSFERENCIA GRATUITA DE UN BIEN Y/O SERVICIO PRESTADO GRATUITAMENTE", new Font(Font.HELVETICA, 7f, Font.NORMAL, BaseColor.Black)));
                    c1.Border = 0;
                    c1.HorizontalAlignment = Element.ALIGN_CENTER;
                    c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                    tbl.AddCell(c1);
                    doc.Add(tbl);
                    doc.Add(new Phrase(" "));
                }

                tbl = new PdfPTable(new float[] { 30f }) { WidthPercentage = 100 };

                c1 = new PdfPCell(new Phrase("Autorizado mediante", new Font(Font.HELVETICA, 8f, Font.NORMAL, BaseColor.Black)));
                c1.Border = 0;
                c1.HorizontalAlignment = Element.ALIGN_LEFT;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase("Resolución Nro: "+ wResolucion + " / SUNAT", new Font(Font.HELVETICA, 8f, Font.NORMAL, BaseColor.Black)));
                c1.Border = 0;
                c1.HorizontalAlignment = Element.ALIGN_LEFT;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase("www.clinicasanfelipe.com", new Font(Font.HELVETICA, 8f, Font.BOLD, BaseColor.Black)));
                c1.Border = 0;
                c1.HorizontalAlignment = Element.ALIGN_CENTER;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase("Estimado Cliente:", new Font(Font.HELVETICA, 8f, Font.NORMAL, BaseColor.Black)));
                c1.Border = 0;
                c1.HorizontalAlignment = Element.ALIGN_LEFT;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase("Antes de retirarse por favor verifique su compra,", new Font(Font.HELVETICA, 8f, Font.NORMAL, BaseColor.Black)));
                c1.Border = 0;
                c1.HorizontalAlignment = Element.ALIGN_LEFT;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase("no se aceptarán cambios ni devoluciones.", new Font(Font.HELVETICA, 8f, Font.NORMAL, BaseColor.Black)));
                c1.Border = 0;
                c1.HorizontalAlignment = Element.ALIGN_LEFT;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase("", new Font(Font.HELVETICA, 8f, Font.NORMAL, BaseColor.Black)));
                c1.Border = 0;
                c1.HorizontalAlignment = Element.ALIGN_CENTER;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase("Una vez emitido el comprobante, no se aceptará el cambio", new Font(Font.HELVETICA, 8f, Font.NORMAL, BaseColor.Black)));
                c1.Border = 0;
                c1.HorizontalAlignment = Element.ALIGN_LEFT;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase("por otro tipo de comprobante (factura/boleta) ni por cambio", new Font(Font.HELVETICA, 8f, Font.NORMAL, BaseColor.Black)));
                c1.Border = 0;
                c1.HorizontalAlignment = Element.ALIGN_LEFT;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase("de nombre o razón social.", new Font(Font.HELVETICA, 8f, Font.NORMAL, BaseColor.Black)));
                c1.Border = 0;
                c1.HorizontalAlignment = Element.ALIGN_CENTER;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase("", new Font(Font.HELVETICA, 8f, Font.NORMAL, BaseColor.Black)));
                c1.Border = 0;
                c1.HorizontalAlignment = Element.ALIGN_LEFT;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase("Fecha de Emisión: " + cfecha.ToShortDateString()+ " Hora: "+ cfecha.ToShortTimeString(), new Font(Font.HELVETICA, 8f, Font.NORMAL, BaseColor.Black)));
                c1.Border = 0;
                c1.HorizontalAlignment = Element.ALIGN_LEFT;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase("", new Font(Font.HELVETICA, 8f, Font.NORMAL, BaseColor.Black)));
                c1.Border = 0;
                c1.HorizontalAlignment = Element.ALIGN_LEFT;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase("Estado: "+ pNombreEstado, new Font(Font.HELVETICA, 8f, Font.NORMAL, BaseColor.Black)));
                c1.Border = 0;
                c1.HorizontalAlignment = Element.ALIGN_LEFT;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase("Tipo de pago:", new Font(Font.HELVETICA, 8f, Font.NORMAL, BaseColor.Black)));
                c1.Border = 0;
                c1.HorizontalAlignment = Element.ALIGN_LEFT;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase(pTipoPagotxt, new Font(Font.HELVETICA, 8f, Font.NORMAL, BaseColor.Black)));
                c1.Border = 0;
                c1.HorizontalAlignment = Element.ALIGN_LEFT;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbl.AddCell(c1);
                doc.Add(tbl);
                doc.Add(new Phrase(" "));

                write.Close();
                doc.Close();
                ms.Seek(0, SeekOrigin.Begin);
                var file = ms;

                vResultadoTransaccion.IdRegistro = 0;
                vResultadoTransaccion.ResultadoCodigo = 0;
                vResultadoTransaccion.ResultadoDescripcion = "Se genero correctamente Vale de Venta";
                vResultadoTransaccion.data = file;
            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return vResultadoTransaccion;

        }

        public async Task<ResultadoTransaccion<BE_VentasCabecera>> GetVentaPorCodVenta(string codventa)
        {
            ResultadoTransaccion<BE_VentasCabecera> vResultadoTransaccion = new ResultadoTransaccion<BE_VentasCabecera>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new BE_VentasCabecera();

                    using (SqlCommand cmd = new SqlCommand("VEN_ObtieneVentasCabeceraPorCodVentaGet", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codventa", codventa));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = context.Convert<BE_VentasCabecera>(reader);
                        }

                        conn.Close();

                    }

                    var responseDetalle = new List<BE_VentasDetalle>();

                    using (SqlCommand cmd = new SqlCommand("VEN_ListaVentaDetallePorCodVentaGet", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codventa", codventa));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            responseDetalle = (List<BE_VentasDetalle>)context.ConvertTo<BE_VentasDetalle>(reader);
                        }

                        conn.Close();
                    }

                    var responseDetalleLote = new List<BE_VentasDetalleLote>();

                    using (SqlCommand cmd = new SqlCommand("VEN_ListaVentaDetalleLotePorCodVentaDetalleGet", conn))
                    {
                        foreach (BE_VentasDetalle item in responseDetalle)
                        {
                            if (item.manBtchNum || item.binactivat)
                            {
                                cmd.Parameters.Clear();
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@coddetalle", item.coddetalle));

                                conn.Open();

                                using (var reader = await cmd.ExecuteReaderAsync())
                                {
                                    responseDetalleLote = (List<BE_VentasDetalleLote>)context.ConvertTo<BE_VentasDetalleLote>(reader);
                                }

                                conn.Close();

                                responseDetalle.Find(xFila => xFila.coddetalle == item.coddetalle).listVentasDetalleLotes = responseDetalleLote;
                            }
                        }
                    }

                    response.listaVentaDetalle = responseDetalle;

                    vResultadoTransaccion.IdRegistro = 0;
                    vResultadoTransaccion.ResultadoCodigo = 0;
                    vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", 1);
                    vResultadoTransaccion.data = response;
                }
            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return vResultadoTransaccion;

        }


        #endregion

        #region "Consulta Servicios"

        public async Task<ResultadoTransaccion<string>> GetWebServicesPorCodTabla(string codTabla)
        {
            ResultadoTransaccion<string> vResultadoTransaccion = new ResultadoTransaccion<string>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxClinica))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_GET_WEBSERVICES_CONSULTA_BY_CODTABLA, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codtabla", codTabla));
                       
                        await conn.OpenAsync();
                        string nombre = string.Empty;
                        vResultadoTransaccion.ResultadoDescripcion = string.Empty;

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                nombre = (reader["nombre"] is DBNull) ? string.Empty : (string)reader["nombre"];
                            }
                        }

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", 1);
                        vResultadoTransaccion.data = "test";

                    }
                }
            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return vResultadoTransaccion;

        }

        #endregion

    }
}
