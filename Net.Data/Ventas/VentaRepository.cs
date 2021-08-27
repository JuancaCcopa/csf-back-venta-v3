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
using System.Linq;
using System.Net.Http;
using Net.CrossCotting;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;

namespace Net.Data
{
    public class VentaRepository : RepositoryBase<BE_VentasCabecera>, IVentaRepository
    {
        private readonly string _cnx;
        private readonly string _cnxClinica;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private string _aplicacionName;
        private string _metodoName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        const string DB_ESQUEMA = "";
        const string SP_GET = DB_ESQUEMA + "VEN_ListaVentasCabeceraPorFiltrosGet";
        const string SP_GET_SIN_STOCK = DB_ESQUEMA + "VEN_ListaVentasCabeceraSinStockPorFiltrosGet";
        const string SP_GET_VENTA_CABECERA = DB_ESQUEMA + "VEN_ListaVentasCabeceraPorCodVentaGet";
        const string SP_GET_CABECERA_VENTA_POR_CODVENTA = DB_ESQUEMA + "VEN_ObtieneVentasCabeceraPorCodVentaGet";
        const string SP_GET_DETALLEVENTA_POR_CODVENTA = DB_ESQUEMA + "VEN_ListaVentaDetallePorCodVentaGet";
        const string SP_GET_DETALLEVENTA_LOTE_POR_CODVENTA = DB_ESQUEMA + "VEN_ListaVentaDetalleLotePorCodVentaDetalleGet";
        const string SP_GET_CABECERA_VENTA_PENDIENTE_POR_FILTRO = DB_ESQUEMA + "VEN_ListaVentasCabeceraPendientesPorFiltrosGet";
        const string SP_GET_VENTAS_CHEQUEA_1MES_ANTES = DB_ESQUEMA + "VEN_VentasChequea1MesAntesGet";
        const string SP_GET_VALIDA_EXISTE_VENTA_ANULACION = DB_ESQUEMA + "VEN_VentaValidaExisteAnulacionGet";

        // SIN STOCK
        const string SP_UPDATE_CABECERA_SIN_STOCK = DB_ESQUEMA + "VEN_VentaSinStockUpd";
        const string SP_UPDATE_CABECERA_SAP = DB_ESQUEMA + "VEN_VentaSAPUpd";

        // Validaciones de la venta
        const string SP_GET_TIPOPRODUCTOPRESTACIONES = DB_ESQUEMA + "VEN_TipoproductoPrestacionesPorFiltroGet";
        const string SP_GET_GNCXVENTA = DB_ESQUEMA + "VEN_GncxVentaPorVentaGet";
        const string SP_GET_VALIDA_PRESOTOR = DB_ESQUEMA + "VEN_ValidaPresotorPorFiltroGet";
        const string SP_GET_CAMA_POR_ATENCION = DB_ESQUEMA + "VEN_ListaCamaPorAtencionGet";
        const string SP_GET_TIPOPRODUCTO_PRESTACION = DB_ESQUEMA + "VEN_ListaTipoProductoPrestacionVentaAutomaticaGet";

        const string SP_UPDATE_CABECERA_VENTA_ENVIO_PISO = DB_ESQUEMA + "VEN_VentaCabeceraEnvioPisoUpd";

        const string SP_INSERT = DB_ESQUEMA + "VEN_VentaCabeceraIns";
        const string SP_INSERT_XML = DB_ESQUEMA + "VEN_VentaXmlIns";
        const string SP_INSERT_DETALLE = DB_ESQUEMA + "VEN_VentaDetalleIns";
        const string SP_INSERT_DETALLE_DATOS = DB_ESQUEMA + "VEN_VentaDetalleDatosIns";
        const string SP_INSERT_DETALLE_LOTE = DB_ESQUEMA + "VEN_VentaDetalleLoteIns";
        const string SP_INSERT_DEVOLUCION = DB_ESQUEMA + "VEN_VentaDevolucionIns";

        const string SP_UPDATE = DB_ESQUEMA + "VEN_VentaCabeceraUpd";
        const string SP_UPDATE_DSCTO_DETALLE = DB_ESQUEMA + "VEN_VentaCabeceraUpdDscDet";
        const string SP_INSERT_VENTA_ANULAR = DB_ESQUEMA + "VEN_VentaAnularIns";
        const string SP_INSERT_VENTA_PEDIDO_ERROR = DB_ESQUEMA + "VEN_VentasPedidoErrorIns";


        // Clinica
        const string SP_PEDIDOXDEVOLVER_RECALCULO = DB_ESQUEMA + "VEN_ClinicaPedidosxDevolver_Recalculo";
        const string SP_GET_PRESOTOR_CONSULTAVARIOS = DB_ESQUEMA + "VEN_ClinicaPresotorConsultaVariosGet";
        const string SP_UPDATE_PRESOTOR = DB_ESQUEMA + "VEN_ClinicaPresotorUpd";
        const string SP_UPDATE_PEDIDO = DB_ESQUEMA + "VEN_ClinicaPedidosUpd";
        const string SP_FARMACIA_PRESTACION_INSERT = DB_ESQUEMA + "VEN_ClinicaFarmaciaPrestacionIns";
        const string SP_PRESOTOR_CONSULTA = DB_ESQUEMA + "Sp_Presotor_Consulta";

        public VentaRepository(IHttpClientFactory clientFactory, IConnectionSQL context, IConfiguration configuration)
            : base(context)
        {
            _cnx = configuration.GetConnectionString("cnnSqlLogistica");
            _cnxClinica = configuration.GetConnectionString("cnnSqlClinica");
            _configuration = configuration;
            _clientFactory = clientFactory;
            _aplicacionName = this.GetType().Name;
        }
        public async Task<ResultadoTransaccion<BE_VentasCabecera>> GetAll(string codcomprobante, string codventa, DateTime fecinicio, DateTime fecfin)
        {
            ResultadoTransaccion<BE_VentasCabecera> vResultadoTransaccion = new ResultadoTransaccion<BE_VentasCabecera>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            fecinicio = Utilidades.GetFechaHoraInicioActual(fecinicio);
            fecfin = Utilidades.GetFechaHoraFinActual(fecfin);

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new List<BE_VentasCabecera>();
                    using (SqlCommand cmd = new SqlCommand(SP_GET, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codcomprobante", codcomprobante));
                        cmd.Parameters.Add(new SqlParameter("@codventa", codventa));
                        cmd.Parameters.Add(new SqlParameter("@fecinicio", fecinicio));
                        cmd.Parameters.Add(new SqlParameter("@fecfin", fecfin));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_VentasCabecera>)context.ConvertTo<BE_VentasCabecera>(reader);
                        }

                        conn.Close();

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", response.Count);
                        vResultadoTransaccion.dataList = response;
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
        public async Task<ResultadoTransaccion<BE_VentasCabecera>> GetAllSinStock(string codcomprobante, string codventa, DateTime fecinicio, DateTime fecfin)
        {
            ResultadoTransaccion<BE_VentasCabecera> vResultadoTransaccion = new ResultadoTransaccion<BE_VentasCabecera>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            fecinicio = Utilidades.GetFechaHoraInicioActual(fecinicio);
            fecfin = Utilidades.GetFechaHoraFinActual(fecfin);

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new List<BE_VentasCabecera>();
                    using (SqlCommand cmd = new SqlCommand(SP_GET_SIN_STOCK, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codcomprobante", codcomprobante));
                        cmd.Parameters.Add(new SqlParameter("@codventa", codventa));
                        cmd.Parameters.Add(new SqlParameter("@fecinicio", fecinicio));
                        cmd.Parameters.Add(new SqlParameter("@fecfin", fecfin));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_VentasCabecera>)context.ConvertTo<BE_VentasCabecera>(reader);
                        }

                        conn.Close();

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", response.Count);
                        vResultadoTransaccion.dataList = response;
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

        public async Task<ResultadoTransaccion<BE_VentasCabecera>> GetListVentaCabecera(string codventa)
        {
            ResultadoTransaccion<BE_VentasCabecera> vResultadoTransaccion = new ResultadoTransaccion<BE_VentasCabecera>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new List<BE_VentasCabecera>();
                    using (SqlCommand cmd = new SqlCommand(SP_GET_VENTA_CABECERA, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codventa", codventa));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_VentasCabecera>)context.ConvertTo<BE_VentasCabecera>(reader);
                        }

                        conn.Close();

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", response.Count);
                        vResultadoTransaccion.dataList = response;
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

                    using (SqlCommand cmd = new SqlCommand(SP_GET_CABECERA_VENTA_POR_CODVENTA, conn))
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

                    using (SqlCommand cmd = new SqlCommand(SP_GET_DETALLEVENTA_POR_CODVENTA, conn))
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

                    using (SqlCommand cmd = new SqlCommand(SP_GET_DETALLEVENTA_LOTE_POR_CODVENTA, conn))
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

        public async Task<ResultadoTransaccion<BE_VentasCabecera>> GetVentaCabeceraPendientePorFiltro(DateTime fecha)
        {
            ResultadoTransaccion<BE_VentasCabecera> vResultadoTransaccion = new ResultadoTransaccion<BE_VentasCabecera>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            var fecinicio = Utilidades.GetFechaHoraInicioActual(fecha);
            var fecfin = Utilidades.GetFechaHoraFinActual(fecha);

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new List<BE_VentasCabecera>();
                    using (SqlCommand cmd = new SqlCommand(SP_GET_CABECERA_VENTA_PENDIENTE_POR_FILTRO, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@fechaini", fecinicio));
                        cmd.Parameters.Add(new SqlParameter("@fechafin", fecfin));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_VentasCabecera>)context.ConvertTo<BE_VentasCabecera>(reader);
                        }

                        conn.Close();
                    }

                    vResultadoTransaccion.IdRegistro = 0;
                    vResultadoTransaccion.ResultadoCodigo = 0;
                    vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", response.Count);
                    vResultadoTransaccion.dataList = response;

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

        public async Task<ResultadoTransaccion<BE_VentasDetalle>> GetVentaChequea1MesPorFiltro(string codpaciente, int cuantosmesesantes)
        {
            ResultadoTransaccion<BE_VentasDetalle> vResultadoTransaccion = new ResultadoTransaccion<BE_VentasDetalle>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new List<BE_VentasDetalle>();
                    using (SqlCommand cmd = new SqlCommand(SP_GET_CABECERA_VENTA_PENDIENTE_POR_FILTRO, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codpaciente", codpaciente));
                        cmd.Parameters.Add(new SqlParameter("@cuantosmesesantes", cuantosmesesantes));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_VentasDetalle>)context.ConvertTo<BE_VentasDetalle>(reader);
                        }

                        conn.Close();
                    }

                    vResultadoTransaccion.IdRegistro = 0;
                    vResultadoTransaccion.ResultadoCodigo = 0;
                    vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", response.Count);
                    vResultadoTransaccion.dataList = response;

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

        public async Task<ResultadoTransaccion<BE_VentasCabecera>> ModificarVentaCabeceraEnvioPiso(BE_VentasCabecera value)
        {
            ResultadoTransaccion<BE_VentasCabecera> vResultadoTransaccion = new ResultadoTransaccion<BE_VentasCabecera>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    using (CommittableTransaction transaction = new CommittableTransaction())
                    {
                        await conn.OpenAsync();
                        conn.EnlistTransaction(transaction);
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(SP_UPDATE_CABECERA_VENTA_ENVIO_PISO, conn))
                            {
                                cmd.Parameters.Clear();

                                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@codventa", value.codventa));
                                cmd.Parameters.Add(new SqlParameter("@RegIdUsuario", value.RegIdUsuario));

                                SqlParameter outputIdTransaccionParam = new SqlParameter("@IdTransaccion", SqlDbType.Int, 3)
                                {
                                    Direction = ParameterDirection.Output
                                };
                                cmd.Parameters.Add(outputIdTransaccionParam);

                                SqlParameter outputMsjTransaccionParam = new SqlParameter("@MsjTransaccion", SqlDbType.VarChar, 700)
                                {
                                    Direction = ParameterDirection.Output
                                };
                                cmd.Parameters.Add(outputMsjTransaccionParam);

                                await cmd.ExecuteNonQueryAsync();

                                vResultadoTransaccion.IdRegistro = 0;
                                vResultadoTransaccion.ResultadoCodigo = int.Parse(outputIdTransaccionParam.Value.ToString());
                                vResultadoTransaccion.ResultadoDescripcion = (string)outputMsjTransaccionParam.Value;
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            vResultadoTransaccion.IdRegistro = -1;
                            vResultadoTransaccion.ResultadoCodigo = -1;
                            vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                        }
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

        public async Task<ResultadoTransaccion<Boolean>> GetGastoCubiertoPorFiltro(string codaseguradora, string codproducto, int tipoatencion)
        {
            ResultadoTransaccion<Boolean> vResultadoTransaccion = new ResultadoTransaccion<Boolean>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                TablaRepository tablaRepository = new TablaRepository(context, _configuration);

                ResultadoTransaccion<BE_Tabla> resultadoTransaccionTabla = await tablaRepository.GetTablaLogisticaPorFiltros("ESTADOVENTAGASTOCUBIERTOV2", "01", 0, 0, 4);

                if (resultadoTransaccionTabla.ResultadoCodigo == -1)
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionTabla.ResultadoDescripcion;
                    return vResultadoTransaccion;
                }

                if (resultadoTransaccionTabla.data.estado.Equals("X")) 
                {

                    AseguradoraxProductoRepository aseguradoraxProductoRepository = new AseguradoraxProductoRepository(context, _configuration);
                    //Lista los productos que no se encuentran cubierto por la aseguradora
                    ResultadoTransaccion<BE_AseguradoraxProducto> resultadoTransaccionAseguradora = await aseguradoraxProductoRepository.GetListAseguradoraxProductoPorFiltros(codaseguradora, codproducto, tipoatencion);

                    if (resultadoTransaccionAseguradora.ResultadoCodigo == -1)
                    {
                        vResultadoTransaccion.IdRegistro = -1;
                        vResultadoTransaccion.ResultadoCodigo = -1;
                        vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionAseguradora.ResultadoDescripcion;
                        return vResultadoTransaccion;
                    }

                    if (((List<BE_AseguradoraxProducto>)resultadoTransaccionAseguradora.dataList).Count > 0)
                    {
                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.data = true;
                    } else
                    {
                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.data = false;
                    }

                } else
                {
                    vResultadoTransaccion.IdRegistro = 0;
                    vResultadoTransaccion.ResultadoCodigo = 0;
                    vResultadoTransaccion.data = true;
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

        public async Task<ResultadoTransaccion<BE_VentasCabecera>> ValidacionRegistraVentaCabecera(BE_VentasCabecera value)
        {
            ResultadoTransaccion<BE_VentasCabecera> vResultadoTransaccion = new ResultadoTransaccion<BE_VentasCabecera>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {

                SeriePorMaquinaRepository seriePorMaquinaRepository = new SeriePorMaquinaRepository(context, _configuration);
                ResultadoTransaccion<BE_SeriePorMaquina> resultadoTransaccionSeriePorMaquina = await seriePorMaquinaRepository.GetListSeriePorMaquinaPorFiltro(new BE_SeriePorMaquina { nombremaquina = value.nombremaquina });

                if (resultadoTransaccionSeriePorMaquina.ResultadoCodigo == -1)
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionSeriePorMaquina.ResultadoDescripcion;

                    return vResultadoTransaccion;
                }

                if (resultadoTransaccionSeriePorMaquina.dataList.Any())
                {
                    BE_SeriePorMaquina seriePorMaquina = ((List<BE_SeriePorMaquina>)resultadoTransaccionSeriePorMaquina.dataList)[0];

                    if (!seriePorMaquina.codalmacen.Equals(value.codalmacen) && !seriePorMaquina.codalmacen.Equals(string.Empty))
                    {
                        vResultadoTransaccion.IdRegistro = -1;
                        vResultadoTransaccion.ResultadoCodigo = -1;
                        vResultadoTransaccion.ResultadoDescripcion = string.Format("Ud. solo puede vender del almacén: {0} - {1} ...Favor de revisar", seriePorMaquina.codalmacen, seriePorMaquina.desalmacen);

                        return vResultadoTransaccion;
                    }

                }
                else
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = "Ud. no tiene acceso al almacén seleccionado...Favor de revisar";

                    return vResultadoTransaccion;
                }

                if (value.codtipocliente.Equals("01"))
                {
                    PacienteRepository atencionRepository = new PacienteRepository(context, _configuration);

                    ResultadoTransaccion<BE_Paciente> resultadoTransaccionPaciente = await atencionRepository.GetExistenciaPaciente(value.codatencion);

                    if (resultadoTransaccionPaciente.ResultadoCodigo == -1)
                    {
                        vResultadoTransaccion.IdRegistro = -1;
                        vResultadoTransaccion.ResultadoCodigo = -1;
                        vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionPaciente.ResultadoDescripcion;

                        return vResultadoTransaccion;
                    }

                    List<BE_Paciente> listPaciente = (List<BE_Paciente>)resultadoTransaccionPaciente.dataList;

                    if (listPaciente.Count == 0)
                    {
                        vResultadoTransaccion.IdRegistro = -1;
                        vResultadoTransaccion.ResultadoCodigo = -1;
                        vResultadoTransaccion.ResultadoDescripcion = "Atención no existe...Favor de revisar";

                        return vResultadoTransaccion;
                    }
                    else
                    {
                        if (!listPaciente[0].activo.Equals(1))
                        {
                            vResultadoTransaccion.IdRegistro = -1;
                            vResultadoTransaccion.ResultadoCodigo = -1;
                            vResultadoTransaccion.ResultadoDescripcion = "Atención desactivada...Favor de revisar";

                            return vResultadoTransaccion;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(value.codpedido))
                {
                    if (value.codpedido.Length.Equals(14))
                    {
                        PedidoRepository pedidoRepository = new PedidoRepository(context, _configuration);
                        ResultadoTransaccion<BE_Pedido> resultadoTransaccionPedido = await pedidoRepository.GetDatosPedidoPorPedido(value.codpedido);

                        if (resultadoTransaccionPedido.ResultadoCodigo == -1)
                        {
                            vResultadoTransaccion.IdRegistro = -1;
                            vResultadoTransaccion.ResultadoCodigo = -1;
                            vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionPedido.ResultadoDescripcion;

                            return vResultadoTransaccion;
                        }

                        List<BE_Pedido> listPedido = (List<BE_Pedido>)resultadoTransaccionPedido.dataList;

                        if (listPedido.Count > 0)
                        {
                            if (listPedido[0].TieneVenta)
                            {
                                vResultadoTransaccion.IdRegistro = -1;
                                vResultadoTransaccion.ResultadoCodigo = -1;
                                vResultadoTransaccion.ResultadoDescripcion = "Pedido ya fue atendido:" + value.codpedido + "en el almacén: " + listPedido[0].codalmacen;

                                return vResultadoTransaccion;
                            }

                            if (!string.IsNullOrEmpty(listPedido[0].codalmacen))
                            {
                                if (listPedido[0].codalmacen != value.codalmacen)
                                {
                                    vResultadoTransaccion.IdRegistro = -1;
                                    vResultadoTransaccion.ResultadoCodigo = -1;
                                    vResultadoTransaccion.ResultadoDescripcion = "Pedido ya fue atendido:" + value.codpedido + "en el almacén: " + listPedido[0].codalmacen;

                                    return vResultadoTransaccion;
                                }

                            }
                            else
                            {
                                vResultadoTransaccion.IdRegistro = -1;
                                vResultadoTransaccion.ResultadoCodigo = -1;
                                vResultadoTransaccion.ResultadoDescripcion = "El centro de costo del pedido no tiene asignado un almacén";

                                return vResultadoTransaccion;
                            }
                        }
                    }
                }  

                // Controla monto de venta a personal
                if (value.codtipocliente.Equals("03"))
                {
                    PersonalClinicaRepository personalClinicaRepository = new PersonalClinicaRepository(context, _configuration);
                    ResultadoTransaccion<BE_PersonalLimiteConsumo> resultadoTransaccionPersonalClinica = await personalClinicaRepository.GetListLimiteConsumoPorPersonal(value.codcliente);
                    //ResultadoTransaccion<BE_PersonalLimiteConsumo> resultadoTransaccionPersonalClinica = new ResultadoTransaccion<BE_PersonalLimiteConsumo>();

                    if (resultadoTransaccionPersonalClinica.ResultadoCodigo == -1)
                    {
                        vResultadoTransaccion.IdRegistro = -1;
                        vResultadoTransaccion.ResultadoCodigo = -1;
                        vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionPersonalClinica.ResultadoDescripcion;

                        return vResultadoTransaccion;
                    }

                    List<BE_PersonalLimiteConsumo> personalClinicas = ((List<BE_PersonalLimiteConsumo>)resultadoTransaccionPersonalClinica.dataList);

                    if (personalClinicas.Count > 0)
                    {
                        if (personalClinicas[0].vender.Equals("N"))
                        {
                            vResultadoTransaccion.IdRegistro = -1;
                            vResultadoTransaccion.ResultadoCodigo = -1;
                            vResultadoTransaccion.ResultadoDescripcion = string.Format("El consumo al CREDITO es mayor al límite de consumo <br> en el periodo del {0} al {1} <br> Monto consumo (NO incluye esta venta): {2} <br> Monto Limite de Consumo {3}...Favor de revisar", personalClinicas[0].fecha1, personalClinicas[0].fecha2, personalClinicas[0].montoconsumo, personalClinicas[0].montolimite);

                            return vResultadoTransaccion;
                        }
                    }
                    else
                    {
                        var nuevoConsumo = personalClinicas[0].montoconsumo + value.montoneto;

                        if (nuevoConsumo <= personalClinicas[0].montolimite)
                        {
                            vResultadoTransaccion.ResultadoDescripcion = string.Format("El consumo al CREDITO en el periodo del {0} al {1} <br> Monto consumo (INCLUYE esta venta): {2} <br> Monto Limite de Consumo {3}...Favor de revisar", personalClinicas[0].fecha1, personalClinicas[0].fecha2, personalClinicas[0].montoconsumo, personalClinicas[0].montolimite);
                        }
                    }
                }

                vResultadoTransaccion.IdRegistro = 0;
                vResultadoTransaccion.ResultadoCodigo = 0;
                vResultadoTransaccion.ResultadoDescripcion = "Validaciones Correctamente";
            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return vResultadoTransaccion;
        }

        public async Task<ResultadoTransaccion<BE_VentasGenerado>> RegistrarVentaCabecera(BE_VentasCabecera value, bool ventaAutomatica)
        {
            ResultadoTransaccion<BE_VentasGenerado> vResultadoTransaccion = new ResultadoTransaccion<BE_VentasGenerado>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {

                TablaRepository tablaRepository = new TablaRepository(context, _configuration);
                ResultadoTransaccion<BE_Tabla> resultadoTransaccionTablaUsuarioAcceso = await tablaRepository.GetListTablaLogisticaPorFiltros("PERMISOUSERMOVDV", value.RegIdUsuario.ToString(), 50, 0, 2);

                if (resultadoTransaccionTablaUsuarioAcceso.ResultadoCodigo == -1)
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionTablaUsuarioAcceso.ResultadoDescripcion;

                    return vResultadoTransaccion;
                }

                if (resultadoTransaccionTablaUsuarioAcceso.dataList.Any())
                {

                    BE_Tabla tablaUsuarioAcceso = ((List<BE_Tabla>)resultadoTransaccionTablaUsuarioAcceso.dataList)[0];

                    if (!tablaUsuarioAcceso.estado.Equals("G"))
                    {
                        vResultadoTransaccion.IdRegistro = -1;
                        vResultadoTransaccion.ResultadoCodigo = -1;
                        vResultadoTransaccion.ResultadoDescripcion = "Usted no tiene permiso para este tipo de Movimiento";

                        return vResultadoTransaccion;
                    }

                }
                else
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = "Usted no tiene permiso para este tipo de Movimiento";

                    return vResultadoTransaccion;
                }

                // Repetido
                if (value.codtipocliente.Equals("01"))
                {
                    PacienteRepository atencionRepository = new PacienteRepository(context, _configuration);

                    ResultadoTransaccion<BE_Paciente> resultadoTransaccionPaciente = await atencionRepository.GetExistenciaPaciente(value.codatencion);

                    if (resultadoTransaccionPaciente.ResultadoCodigo == -1)
                    {
                        vResultadoTransaccion.IdRegistro = -1;
                        vResultadoTransaccion.ResultadoCodigo = -1;
                        vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionPaciente.ResultadoDescripcion;

                        return vResultadoTransaccion;
                    }

                    List<BE_Paciente> listPaciente = (List<BE_Paciente>)resultadoTransaccionPaciente.dataList;

                    if (listPaciente.Count == 0)
                    {
                        vResultadoTransaccion.IdRegistro = -1;
                        vResultadoTransaccion.ResultadoCodigo = -1;
                        vResultadoTransaccion.ResultadoDescripcion = "Atención no existe...Favor de revisar";

                        return vResultadoTransaccion;
                    }
                    else
                    {
                        if (!listPaciente[0].activo.Equals(1))
                        {
                            vResultadoTransaccion.IdRegistro = -1;
                            vResultadoTransaccion.ResultadoCodigo = -1;
                            vResultadoTransaccion.ResultadoDescripcion = "Atención desactivada...Favor de revisar";

                            return vResultadoTransaccion;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(value.codpedido))
                {
                    if (value.codpedido.Length.Equals(14))
                    {
                        PedidoRepository pedidoRepository = new PedidoRepository(context, _configuration);
                        ResultadoTransaccion<BE_Pedido> resultadoTransaccionPedido = await pedidoRepository.GetDatosPedidoPorPedido(value.codpedido);

                        if (resultadoTransaccionPedido.ResultadoCodigo == -1)
                        {
                            vResultadoTransaccion.IdRegistro = -1;
                            vResultadoTransaccion.ResultadoCodigo = -1;
                            vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionPedido.ResultadoDescripcion;

                            return vResultadoTransaccion;
                        }

                        List<BE_Pedido> listPedido = (List<BE_Pedido>)resultadoTransaccionPedido.dataList;

                        if (listPedido.Count > 0)
                        {
                            if (listPedido[0].TieneVenta)
                            {
                                vResultadoTransaccion.IdRegistro = -1;
                                vResultadoTransaccion.ResultadoCodigo = -1;
                                vResultadoTransaccion.ResultadoDescripcion = "Pedido ya fue atendido:" + value.codpedido + "en el almacén: " + listPedido[0].codalmacen;

                                return vResultadoTransaccion;
                            }
                        }
                    }
                }

                /*Validar si se tiene stock en el Almacen 0000002 + 0000019 => Para el tema de consignacion PENDIENTE DE COLOCAR VALIDACIÓN*/

                /*Iniciamos a grabar la venta*/

                List<BE_VentasDetalleQuiebre> listVentasDetalleQuiebres = new List<BE_VentasDetalleQuiebre>();

                // Lista para asignar el detalle por quiebre
                List<BE_VentasDetalle> listQuiebreVentasDetalle = new List<BE_VentasDetalle>();

                foreach (var item in value.listaVentaDetalle)
                {
                    // Inicializamos la variable
                    listQuiebreVentasDetalle = new List<BE_VentasDetalle>();

                    // Buscamos si existe
                    int vExiste = listVentasDetalleQuiebres.FindAll(xFila => xFila.codtipoproducto == item.codtipoproducto && xFila.igvproducto == item.igvproducto && xFila.narcotico == item.narcotico).Count;

                    if (vExiste.Equals(0))
                    {
                        // Clonamos el registro
                        var cabecera = value.Clone();
                        // Asignamos solo linea de detalle que le pertenece
                        cabecera.listaVentaDetalle = cabecera.listaVentaDetalle.FindAll(xFila => xFila.codtipoproducto == item.codtipoproducto && xFila.igvproducto == item.igvproducto && xFila.narcotico == item.narcotico);
                        // Agregamos linea por cada venta a realizar
                        listVentasDetalleQuiebres.Add(new BE_VentasDetalleQuiebre { codtipoproducto = item.codtipoproducto, narcotico = item.narcotico, igvproducto = item.igvproducto, ventascabecera = cabecera }) ;
                    }
                }

                string vCodCentroCSF = string.Empty;

                if (ventaAutomatica)
                {
                    vCodCentroCSF = value.codcentro;
                    value.codcentro = null;
                }

                BE_VentasCabecera newVentasCabecera = new BE_VentasCabecera();
                List<BE_VentaXml> listNewVentasCabeceraXml = new List<BE_VentaXml>();

                // Insertaresmos el detalle por venta
                List<BE_VentasDetalle> listVentasDetalle = new List<BE_VentasDetalle>();
                List<BE_VentasDetalle> listNewVentasDetalle = new List<BE_VentasDetalle>();

                // Insertaremos los lotes por detalle
                List<BE_VentasDetalleLote> listNewVentasDetalleLote = new List<BE_VentasDetalleLote>();

                string vCodigoPrestacion = string.Empty;
                decimal vGncPorVenta = 0;
                BE_ValidaPresotor vValidaPresotor = new BE_ValidaPresotor();
                string vNC = string.Empty;
                bool vNoCubierto = false;
                string vFlagPaquete = "N";

                if (!listVentasDetalleQuiebres.Count.Equals(0))
                {
                    foreach (BE_VentasDetalleQuiebre itemQuiebreOrigen in listVentasDetalleQuiebres)
                    {
                        //Obtenemos la cabecera con los montos totales actualizados
                        newVentasCabecera = new BE_VentasCabecera();
                        newVentasCabecera = CalculaTotales(itemQuiebreOrigen, itemQuiebreOrigen.ventascabecera);

                        // Asignamos el porcentaje de Igv
                        newVentasCabecera.porcentajeimpuesto = itemQuiebreOrigen.igvproducto;

                        // Obtenemos el detalle por venta
                        listVentasDetalle = new List<BE_VentasDetalle>();
                        listNewVentasDetalle = new List<BE_VentasDetalle>();

                        listVentasDetalle = itemQuiebreOrigen.ventascabecera.listaVentaDetalle;

                        newVentasCabecera.listaVentaDetalle = new List<BE_VentasDetalle>();

                        // Realizamos la logica para la asignacion del lote
                        foreach (BE_VentasDetalle item in listVentasDetalle)
                        {
                            if (vFlagPaquete.Equals("S"))
                            {
                                item.gnc = "N";
                            }

                            if (vNC.Equals("S"))
                            {
                                vNoCubierto = true;
                            }

                            // Inicializamos la variables por cada linea de detalle
                            listNewVentasDetalleLote = new List<BE_VentasDetalleLote>();

                            // Validamos si el articulo trabaja por Lote
                            if (item.manBtchNum)
                            {
                                // Validamos si los lotes son seleccionados manuealmente
                                if (item.flgbtchnum)
                                {
                                    if (item.listStockLote.Count > 0)
                                    {
                                        foreach (var lote in item.listStockLote)
                                        {
                                            if (lote.Quantityinput > 0)
                                            {
                                                var itemLote = new BE_VentasDetalleLote { codproducto = lote.ItemCode, lote = lote.BatchNum, cantidad = lote.Quantityinput, coddetalle = item.coddetalle, fechavencimiento = (DateTime)lote.ExpDate };

                                                listNewVentasDetalleLote.Add(itemLote);
                                            }
                                        }

                                        item.listVentasDetalleLotes = listNewVentasDetalleLote;
                                    }
                                }
                                else
                                {
                                    // Si no es seleccionado manualmente, obtenemos los lotes de hana
                                    StockRepository stockRepository = new StockRepository(_clientFactory, _configuration);
                                    ResultadoTransaccion<BE_StockLote> resultadoTransaccionListLote = await stockRepository.GetListStockLotePorFiltro(item.codalmacen, item.codproducto, true);

                                    if (resultadoTransaccionListLote.ResultadoCodigo == -1)
                                    {
                                        vResultadoTransaccion.IdRegistro = -1;
                                        vResultadoTransaccion.ResultadoCodigo = -1;
                                        vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionListLote.ResultadoDescripcion + " ; [GetListStockLotePorFiltro]";
                                        return vResultadoTransaccion;
                                    }

                                    decimal cantidadInput = item.cantidad;
                                    List<BE_StockLote> listLote = new List<BE_StockLote>();

                                    listLote = (List<BE_StockLote>)resultadoTransaccionListLote.dataList;

                                    if (listLote.Any())
                                    {
                                        foreach (BE_StockLote lote in listLote)
                                        {
                                            if (cantidadInput > 0)
                                            {
                                                if (cantidadInput <= lote.QuantityLote)
                                                {
                                                    listLote.Find(xFila => xFila.BatchNum == lote.BatchNum).Quantityinput = cantidadInput;
                                                    cantidadInput = 0;
                                                }
                                                else
                                                {
                                                    listLote.Find(xFila => xFila.BatchNum == lote.BatchNum).Quantityinput = 0;

                                                    decimal cantidad = (decimal)listLote.Find(xFila => xFila.BatchNum == lote.BatchNum).QuantityLote;

                                                    decimal newResul = (cantidadInput - cantidad);

                                                    listLote.Find(xFila => xFila.BatchNum == lote.BatchNum).Quantityinput = cantidadInput - newResul;

                                                    cantidadInput = newResul;
                                                }
                                            }
                                        }

                                        if (cantidadInput > 0)
                                        {
                                            vResultadoTransaccion.IdRegistro = -1;
                                            vResultadoTransaccion.ResultadoCodigo = -1;
                                            vResultadoTransaccion.ResultadoDescripcion = "No existe suficiente stock para el producto: " + item.codproducto;
                                            return vResultadoTransaccion;
                                        }

                                        if (listLote.Count > 0)
                                        {
                                            foreach (BE_StockLote lote in listLote)
                                            {
                                                if (lote.Quantityinput > 0)
                                                {
                                                    var itemLote = new BE_VentasDetalleLote { codproducto = lote.ItemCode, lote = lote.BatchNum, cantidad = lote.Quantityinput, coddetalle = item.coddetalle, fechavencimiento = (DateTime)lote.ExpDate };
                                                    listNewVentasDetalleLote.Add(itemLote);
                                                }
                                            }
                                            item.listVentasDetalleLotes = listNewVentasDetalleLote;
                                        }
                                    }
                                    else
                                    {
                                        vResultadoTransaccion.IdRegistro = -1;
                                        vResultadoTransaccion.ResultadoCodigo = -1;
                                        vResultadoTransaccion.ResultadoDescripcion = "No se encuentran lotes con stock para el producto " + item.codproducto;
                                        return vResultadoTransaccion;
                                    }
                                }
                            }
                            // Asignamos el detalle a la venta
                            listNewVentasDetalle.Add(item);
                        }

                        newVentasCabecera.listaVentaDetalle = listNewVentasDetalle;

                        // Realizamos la conversion a XML
                        var entiDom = new BE_VentaXml();
                        var ser = new Serializador();
                        var ms = new MemoryStream();
                        ser.SerializarXml(newVentasCabecera, ms);
                        entiDom.XmlData = Encoding.UTF8.GetString(ms.ToArray());
                        ms.Dispose();

                        var venta = new BE_VentaXml
                        {
                            XmlData = entiDom.XmlData,
                            RegIdUsuario = newVentasCabecera.RegIdUsuario
                        };

                        listNewVentasCabeceraXml.Add(venta);
                    }
                }

                // Conexion de Logistica
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    using (CommittableTransaction transaction = new CommittableTransaction())
                    {
                        // Conexion de Logistica
                        await conn.OpenAsync();
                        conn.EnlistTransaction(transaction);

                        try
                        {
                            // Insertaremos las ventas generadas
                            List<BE_VentasGenerado> listVentasGenerados = new List<BE_VentasGenerado>();

                            foreach (BE_VentaXml ventaXml in listNewVentasCabeceraXml)
                            {
                                ResultadoTransaccion<BE_VentasGenerado> resultadoTransaccionVentasGenerado = await RegistraVentaXml(conn, ventaXml, (int)value.RegIdUsuario);

                                if (resultadoTransaccionVentasGenerado.ResultadoCodigo == -1)
                                {
                                    transaction.Rollback();
                                    vResultadoTransaccion.IdRegistro = -1;
                                    vResultadoTransaccion.ResultadoCodigo = -1;
                                    vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionVentasGenerado.ResultadoDescripcion + " ; [RegistraVentaXml]";
                                    return vResultadoTransaccion;
                                }

                                listVentasGenerados.Add(new BE_VentasGenerado { codventa = resultadoTransaccionVentasGenerado.data.codventa, codpresotor = resultadoTransaccionVentasGenerado.data.codpresotor });
                            }

                            vResultadoTransaccion.IdRegistro = 0;
                            vResultadoTransaccion.ResultadoCodigo = 0;
                            vResultadoTransaccion.ResultadoDescripcion = "Se genero correctamente";

                            vResultadoTransaccion.dataList = listVentasGenerados;

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            vResultadoTransaccion.IdRegistro = -1;
                            vResultadoTransaccion.ResultadoCodigo = -1;
                            vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                        }
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

        public async Task<ResultadoTransaccion<BE_VentasGenerado>> RegistraVentaXml(SqlConnection conn, BE_VentaXml item, int RegIdUsuario)
        {
            ResultadoTransaccion<BE_VentasGenerado> vResultadoTransaccion = new ResultadoTransaccion<BE_VentasGenerado>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlCommand cmdDatos = new SqlCommand(SP_INSERT_XML, conn))
                {
                    cmdDatos.Parameters.Clear();
                    cmdDatos.CommandType = System.Data.CommandType.StoredProcedure;

                    SqlParameter outputCodVentaParamDato = new SqlParameter("@codventa", SqlDbType.Char, 8)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmdDatos.Parameters.Add(outputCodVentaParamDato);
                    SqlParameter outputCodPresotorParamDato = new SqlParameter("@codpresotor", SqlDbType.Char, 12)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmdDatos.Parameters.Add(outputCodPresotorParamDato);
                    cmdDatos.Parameters.Add(new SqlParameter("@xmldata", item.XmlData));
                    // Datos de Auditoria
                    cmdDatos.Parameters.Add(new SqlParameter("@RegIdUsuario", RegIdUsuario));
                    // Datos de Transaccion
                    SqlParameter outputIdTransaccionParamDato = new SqlParameter("@IdTransaccion", SqlDbType.Int, 3)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmdDatos.Parameters.Add(outputIdTransaccionParamDato);

                    SqlParameter outputMsjTransaccionParamDato = new SqlParameter("@MsjTransaccion", SqlDbType.VarChar, 700)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmdDatos.Parameters.Add(outputMsjTransaccionParamDato);

                    //await conn.OpenAsync();
                    await cmdDatos.ExecuteNonQueryAsync();

                    vResultadoTransaccion.IdRegistro = 0;
                    vResultadoTransaccion.ResultadoCodigo = int.Parse(outputIdTransaccionParamDato.Value.ToString());
                    vResultadoTransaccion.ResultadoDescripcion = (string)outputMsjTransaccionParamDato.Value;

                    var result = new BE_VentasGenerado { 
                        codventa = (string)outputCodVentaParamDato.Value,
                        codpresotor = (string)outputCodPresotorParamDato.Value
                    };

                    vResultadoTransaccion.data = result;
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

        //public async Task<ResultadoTransaccion<BE_VentasGenerado>> RegistrarVentaCabecera_Old(BE_VentasCabecera value, bool ventaAutomatica)
        //{
        //    ResultadoTransaccion<BE_VentasGenerado> vResultadoTransaccion = new ResultadoTransaccion<BE_VentasGenerado>();
        //    _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

        //    vResultadoTransaccion.NombreMetodo = _metodoName;
        //    vResultadoTransaccion.NombreAplicacion = _aplicacionName;

        //    try
        //    {
        //        TablaRepository tablaRepository = new TablaRepository(context, _configuration);
        //        ResultadoTransaccion<BE_Tabla> resultadoTransaccionTablaUsuarioAcceso = await tablaRepository.GetListTablaLogisticaPorFiltros("PERMISOUSERMOVDV", value.RegIdUsuario.ToString(), 50, 0, 2);

        //        if (resultadoTransaccionTablaUsuarioAcceso.ResultadoCodigo == -1)
        //        {
        //            vResultadoTransaccion.IdRegistro = -1;
        //            vResultadoTransaccion.ResultadoCodigo = -1;
        //            vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionTablaUsuarioAcceso.ResultadoDescripcion;

        //            return vResultadoTransaccion;
        //        }

        //        if (resultadoTransaccionTablaUsuarioAcceso.dataList.Any())
        //        {
        //            BE_Tabla tablaUsuarioAcceso = ((List<BE_Tabla>)resultadoTransaccionTablaUsuarioAcceso.dataList)[0];

        //            if (!tablaUsuarioAcceso.estado.Equals("G"))
        //            {
        //                vResultadoTransaccion.IdRegistro = -1;
        //                vResultadoTransaccion.ResultadoCodigo = -1;
        //                vResultadoTransaccion.ResultadoDescripcion = "Usted no tiene permiso para este tipo de Movimiento";

        //                return vResultadoTransaccion;
        //            }

        //        }
        //        else
        //        {
        //            vResultadoTransaccion.IdRegistro = -1;
        //            vResultadoTransaccion.ResultadoCodigo = -1;
        //            vResultadoTransaccion.ResultadoDescripcion = "Usted no tiene permiso para este tipo de Movimiento";

        //            return vResultadoTransaccion;
        //        }

        //        // Repetido
        //        if (value.codtipocliente.Equals("01"))
        //        {
        //            PacienteRepository atencionRepository = new PacienteRepository(context, _configuration);

        //            ResultadoTransaccion<BE_Paciente> resultadoTransaccionPaciente = await atencionRepository.GetExistenciaPaciente(value.codatencion);

        //            if (resultadoTransaccionPaciente.ResultadoCodigo == -1)
        //            {
        //                vResultadoTransaccion.IdRegistro = -1;
        //                vResultadoTransaccion.ResultadoCodigo = -1;
        //                vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionPaciente.ResultadoDescripcion;

        //                return vResultadoTransaccion;
        //            }

        //            List<BE_Paciente> listPaciente = (List<BE_Paciente>)resultadoTransaccionPaciente.dataList;

        //            if (listPaciente.Count == 0)
        //            {
        //                vResultadoTransaccion.IdRegistro = -1;
        //                vResultadoTransaccion.ResultadoCodigo = -1;
        //                vResultadoTransaccion.ResultadoDescripcion = "Atención no existe...Favor de revisar";

        //                return vResultadoTransaccion;
        //            }
        //            else
        //            {
        //                if (!listPaciente[0].activo.Equals(1))
        //                {
        //                    vResultadoTransaccion.IdRegistro = -1;
        //                    vResultadoTransaccion.ResultadoCodigo = -1;
        //                    vResultadoTransaccion.ResultadoDescripcion = "Atención desactivada...Favor de revisar";

        //                    return vResultadoTransaccion;
        //                }
        //            }
        //        }

        //        if (!string.IsNullOrEmpty(value.codpedido))
        //        {
        //            if (value.codpedido.Length.Equals(14))
        //            {
        //                PedidoRepository pedidoRepository = new PedidoRepository(context, _configuration);
        //                ResultadoTransaccion<BE_Pedido> resultadoTransaccionPedido = await pedidoRepository.GetDatosPedidoPorPedido(value.codpedido);

        //                if (resultadoTransaccionPedido.ResultadoCodigo == -1)
        //                {
        //                    vResultadoTransaccion.IdRegistro = -1;
        //                    vResultadoTransaccion.ResultadoCodigo = -1;
        //                    vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionPedido.ResultadoDescripcion;

        //                    return vResultadoTransaccion;
        //                }

        //                List<BE_Pedido> listPedido = (List<BE_Pedido>)resultadoTransaccionPedido.dataList;

        //                if (listPedido.Count > 0)
        //                {
        //                    if (listPedido[0].TieneVenta)
        //                    {
        //                        vResultadoTransaccion.IdRegistro = -1;
        //                        vResultadoTransaccion.ResultadoCodigo = -1;
        //                        vResultadoTransaccion.ResultadoDescripcion = "Pedido ya fue atendido:" + value.codpedido + "en el almacén: " + listPedido[0].codalmacen;

        //                        return vResultadoTransaccion;
        //                    }
        //                }
        //            }
        //        }

        //        /*Validar si se tiene stock en el Almacen 0000002 + 0000019 => Para el tema de consignacion PENDIENTE DE COLOCAR VALIDACIÓN*/

        //        /*Iniciamos a grabar la venta*/

        //        List<BE_VentasDetalleQuiebre> listVentasDetalleQuiebres = new List<BE_VentasDetalleQuiebre>();

        //        foreach (var item in value.listaVentaDetalle)
        //        {
        //            int vExiste = listVentasDetalleQuiebres.FindAll(xFila => xFila.codtipoproducto == item.codtipoproducto && xFila.igvproducto == item.igvproducto && xFila.narcotico == item.narcotico).Count;

        //            if (vExiste.Equals(0))
        //            {
        //                listVentasDetalleQuiebres.Add(new BE_VentasDetalleQuiebre { codtipoproducto = item.codtipoproducto, narcotico = item.narcotico, igvproducto = item.igvproducto });
        //            }
        //        }

        //        string vCodCentroCSF = string.Empty;

        //        if (ventaAutomatica)
        //        {
        //            vCodCentroCSF = value.codcentro;
        //            value.codcentro = null;
        //        }

        //        // Conexion de Logistica
        //        using (SqlConnection conn = new SqlConnection(_cnx))
        //        {
        //            using (CommittableTransaction transaction = new CommittableTransaction())
        //            {
        //                // Conexion de Logistica
        //                await conn.OpenAsync();
        //                conn.EnlistTransaction(transaction);

        //                try
        //                {
        //                    List<BE_VentasGenerado> listVentasGenerados = new List<BE_VentasGenerado>();
        //                    string vCodigoPresotor = string.Empty;

        //                    if (!listVentasDetalleQuiebres.Count.Equals(0))
        //                    {

        //                        string vCodigoPrestacion = string.Empty;
        //                        decimal vGncPorVenta = 0;
        //                        BE_ValidaPresotor vValidaPresotor = new BE_ValidaPresotor();
        //                        //string vGnc = string.Empty;
        //                        string vNC = string.Empty;
        //                        bool vNoCubierto = false;
        //                        string vFlagPaquete = "N";

        //                        foreach (BE_VentasDetalleQuiebre itemQuiebre in listVentasDetalleQuiebres)
        //                        {

        //                            //ResultadoTransaccion<BE_VentasCabecera> resultadoTransaccionCalculoCabeceraVenta = CalculaTotales(itemQuiebre, value);

        //                            //if (resultadoTransaccionCalculoCabeceraVenta.ResultadoCodigo == -1)
        //                            //{
        //                            //    transaction.Rollback();
        //                            //    vResultadoTransaccion.IdRegistro = -1;
        //                            //    vResultadoTransaccion.ResultadoCodigo = -1;
        //                            //    vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionCalculoCabeceraVenta.ResultadoDescripcion + " ; [CalculaTotales]";
        //                            //    return vResultadoTransaccion;
        //                            //}

        //                            value = CalculaTotales(itemQuiebre, value);

        //                            value.porcentajeimpuesto = itemQuiebre.igvproducto;

        //                            ResultadoTransaccion<string> resultadoTransaccionRegistraCabeceraVenta = await RegistraVentaCabecera(conn, value);

        //                            if (resultadoTransaccionRegistraCabeceraVenta.ResultadoCodigo == -1)
        //                            {
        //                                transaction.Rollback();
        //                                vResultadoTransaccion.IdRegistro = -1;
        //                                vResultadoTransaccion.ResultadoCodigo = -1;
        //                                vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionRegistraCabeceraVenta.ResultadoDescripcion + " ; [RegistraVentaCabecera]";
        //                                return vResultadoTransaccion;
        //                            }

        //                            value.codventa = resultadoTransaccionRegistraCabeceraVenta.data;

        //                            if (value.codpedido != null)
        //                            {
        //                                if (value.codpedido.Length.Equals(14))
        //                                {
        //                                    // Obtiene los datos del pedido
        //                                    PedidoRepository pedidoRepository = new PedidoRepository(context, _configuration);
        //                                    ResultadoTransaccion<BE_Pedido> resultadoTransaccionPedido = await pedidoRepository.GetDatosPedidoPorPedido(conn, value.codpedido);

        //                                    if (resultadoTransaccionPedido.ResultadoCodigo == -1)
        //                                    {
        //                                        transaction.Rollback();
        //                                        vResultadoTransaccion.IdRegistro = -1;
        //                                        vResultadoTransaccion.ResultadoCodigo = -1;
        //                                        vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionPedido.ResultadoDescripcion + " ; [GetDatosPedidoPorPedido]";
        //                                        return vResultadoTransaccion;
        //                                    }

        //                                    if (resultadoTransaccionPedido.dataList.Any())
        //                                    {
        //                                        vFlagPaquete = ((List<BE_Pedido>)resultadoTransaccionPedido.dataList)[0].flg_paquete;
        //                                    }

        //                                }
        //                            }

        //                            List<BE_VentasDetalle> listVentasDetalle = value.listaVentaDetalle.FindAll(xFila => xFila.codtipoproducto == itemQuiebre.codtipoproducto && xFila.igvproducto == itemQuiebre.igvproducto && xFila.narcotico == itemQuiebre.narcotico).ToList();

        //                            foreach (BE_VentasDetalle item in listVentasDetalle)
        //                            {
        //                                if (vFlagPaquete.Equals("S"))
        //                                {
        //                                    item.gnc = "N";
        //                                }

        //                                if (vNC.Equals("S"))
        //                                {
        //                                    vNoCubierto = true;
        //                                }

        //                                item.codventa = value.codventa;

        //                                ResultadoTransaccion<string> resultadoTransaccionDetalle = await RegistraVentaDetalle(conn, item, (int)value.RegIdUsuario);

        //                                if (resultadoTransaccionDetalle.ResultadoCodigo == -1)
        //                                {
        //                                    transaction.Rollback();
        //                                    vResultadoTransaccion.IdRegistro = -1;
        //                                    vResultadoTransaccion.ResultadoCodigo = -1;
        //                                    vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionDetalle.ResultadoDescripcion + " ; [RegistraVentaDetalle]";
        //                                    return vResultadoTransaccion;
        //                                }

        //                                item.coddetalle = resultadoTransaccionDetalle.data;

        //                                if (item.manBtchNum)
        //                                {
        //                                    BE_VentasDetalleLote itemLote = new BE_VentasDetalleLote();

        //                                    if (item.flgbtchnum)
        //                                    {
        //                                        if (item.listStockLote.Count > 0)
        //                                        {
        //                                            foreach (var lote in item.listStockLote)
        //                                            {
        //                                                if (lote.Quantityinput > 0)
        //                                                {
        //                                                    itemLote = new BE_VentasDetalleLote { lote = lote.BatchNum, cantidad = lote.Quantityinput, coddetalle = item.coddetalle, fechavencimiento = (DateTime)lote.ExpDate };

        //                                                    ResultadoTransaccion<bool> resultadoTransaccionRegistraVentaLote = await RegistraVentaDetalleLote(conn, itemLote, (int)value.RegIdUsuario);

        //                                                    if (resultadoTransaccionRegistraVentaLote.ResultadoCodigo == -1)
        //                                                    {
        //                                                        transaction.Rollback();
        //                                                        vResultadoTransaccion.IdRegistro = -1;
        //                                                        vResultadoTransaccion.ResultadoCodigo = -1;
        //                                                        vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionRegistraVentaLote.ResultadoDescripcion + " ; [RegistraVentaDetalleLote]";
        //                                                        return vResultadoTransaccion;
        //                                                    }
        //                                                }
        //                                            }
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        StockRepository stockRepository = new StockRepository(_clientFactory, _configuration);
        //                                        ResultadoTransaccion<BE_StockLote> resultadoTransaccionListLote = await stockRepository.GetListStockLotePorFiltro(item.codalmacen, item.codproducto, true);

        //                                        if (resultadoTransaccionListLote.ResultadoCodigo == -1)
        //                                        {
        //                                            transaction.Rollback();
        //                                            vResultadoTransaccion.IdRegistro = -1;
        //                                            vResultadoTransaccion.ResultadoCodigo = -1;
        //                                            vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionListLote.ResultadoDescripcion + " ; [GetListStockLotePorFiltro]";
        //                                            return vResultadoTransaccion;
        //                                        }

        //                                        decimal cantidadInput = item.cantidad;
        //                                        List<BE_StockLote> listLote = new List<BE_StockLote>();

        //                                        listLote = (List<BE_StockLote>)resultadoTransaccionListLote.dataList;

        //                                        if (listLote.Any())
        //                                        {
        //                                            foreach (BE_StockLote lote in listLote)
        //                                            {
        //                                                if (cantidadInput > 0)
        //                                                {
        //                                                    if (cantidadInput <= lote.QuantityLote)
        //                                                    {
        //                                                        listLote.Find(xFila => xFila.BatchNum == lote.BatchNum).Quantityinput = cantidadInput;
        //                                                        cantidadInput = 0;
        //                                                    }
        //                                                    else
        //                                                    {
        //                                                        listLote.Find(xFila => xFila.BatchNum == lote.BatchNum).Quantityinput = 0;

        //                                                        decimal cantidad = (decimal)listLote.Find(xFila => xFila.BatchNum == lote.BatchNum).QuantityLote;

        //                                                        decimal newResul = (cantidadInput - cantidad);

        //                                                        listLote.Find(xFila => xFila.BatchNum == lote.BatchNum).Quantityinput = cantidadInput - newResul;

        //                                                        cantidadInput = newResul;
        //                                                    }
        //                                                }
        //                                            }

        //                                            if (cantidadInput > 0)
        //                                            {
        //                                                transaction.Rollback();
        //                                                vResultadoTransaccion.IdRegistro = -1;
        //                                                vResultadoTransaccion.ResultadoCodigo = -1;
        //                                                vResultadoTransaccion.ResultadoDescripcion = "No existe suficiente stock para el producto: " + item.codproducto;
        //                                                return vResultadoTransaccion;
        //                                            }

        //                                            if (listLote.Count > 0)
        //                                            {
        //                                                foreach (BE_StockLote lote in listLote)
        //                                                {
        //                                                    if (lote.Quantityinput > 0)
        //                                                    {
        //                                                        itemLote = new BE_VentasDetalleLote { lote = lote.BatchNum, cantidad = lote.Quantityinput, coddetalle = item.coddetalle, fechavencimiento = (DateTime)lote.ExpDate };

        //                                                        ResultadoTransaccion<bool> resultadoTransaccionRegistraVentaLote = await RegistraVentaDetalleLote(conn, itemLote, (int)value.RegIdUsuario);

        //                                                        if (resultadoTransaccionRegistraVentaLote.ResultadoCodigo == -1)
        //                                                        {
        //                                                            transaction.Rollback();
        //                                                            vResultadoTransaccion.IdRegistro = -1;
        //                                                            vResultadoTransaccion.ResultadoCodigo = -1;
        //                                                            vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionRegistraVentaLote.ResultadoDescripcion + " ; [RegistraVentaDetalleLote]";
        //                                                            return vResultadoTransaccion;
        //                                                        }
        //                                                    }
        //                                                }
        //                                            }

        //                                        }
        //                                        else
        //                                        {
        //                                            transaction.Rollback();
        //                                            vResultadoTransaccion.IdRegistro = -1;
        //                                            vResultadoTransaccion.ResultadoCodigo = -1;
        //                                            vResultadoTransaccion.ResultadoDescripcion = "No se encuentran lotes con stock para el producto " + item.codproducto;
        //                                            return vResultadoTransaccion;
        //                                        }

        //                                    }
        //                                }

        //                                if (item.VentasDetalleDatos != null)
        //                                {
        //                                    if (!item.VentasDetalleDatos.tipodocumentoautorizacion.Equals("00"))
        //                                    {
        //                                        ResultadoTransaccion<bool> resultadoTransaccionRegistraVentaDatos = await RegistraVentaDatos(conn, item, (int)value.RegIdUsuario);

        //                                        if (resultadoTransaccionRegistraVentaDatos.ResultadoCodigo == -1)
        //                                        {
        //                                            transaction.Rollback();
        //                                            vResultadoTransaccion.IdRegistro = -1;
        //                                            vResultadoTransaccion.ResultadoCodigo = -1;
        //                                            vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionRegistraVentaDatos.ResultadoDescripcion + " ; [RegistraVentaDatos]";
        //                                            return vResultadoTransaccion;
        //                                        }
        //                                    }
        //                                }

        //                                // Validar
        //                                if (value.codtipocliente.Equals("03"))
        //                                {
        //                                    if (value.codpedido != null)
        //                                    {
        //                                        if (value.codpedido.Length.Equals(14))
        //                                        {

        //                                            ResultadoTransaccion<bool> vResultadoTransaccionPedido = await PedidoxDevolverRecalculo(conn, item.codpedido, item.codproducto, item.cantidad, 0, 0);

        //                                            if (vResultadoTransaccionPedido.ResultadoCodigo == -1)
        //                                            {
        //                                                transaction.Rollback();
        //                                                vResultadoTransaccion.IdRegistro = -1;
        //                                                vResultadoTransaccion.ResultadoCodigo = -1;
        //                                                vResultadoTransaccion.ResultadoDescripcion = vResultadoTransaccionPedido.ResultadoDescripcion + " ; [PedidoxDevolverRecalculo]";
        //                                                return vResultadoTransaccion;
        //                                            }
        //                                        }
        //                                    }
        //                                }

        //                            }

        //                            /*Genera transferencia automatica*/

        //                            if (value.codtipocliente.Equals("01"))
        //                            {
        //                                if (value.codpedido != null)
        //                                {
        //                                    if (value.codpedido.Length.Equals(14))
        //                                    {
        //                                        ResultadoTransaccion<string> resultadoTransaccionPrestacion = await GetTipoProductoPrestaciones(conn, value.codventa, itemQuiebre.codtipoproducto, vCodCentroCSF);

        //                                        if (resultadoTransaccionPrestacion.ResultadoCodigo == -1)
        //                                        {
        //                                            transaction.Rollback();
        //                                            vResultadoTransaccion.IdRegistro = -1;
        //                                            vResultadoTransaccion.ResultadoCodigo = -1;
        //                                            vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionPrestacion.ResultadoDescripcion + " ; [GetTipoProductoPrestaciones]";
        //                                            return vResultadoTransaccion;
        //                                        }

        //                                        vCodigoPrestacion = resultadoTransaccionPrestacion.data;
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    ResultadoTransaccion<string> resultadoTransaccionPrestacion = await GetTipoProductoPrestaciones(conn, value.codventa, itemQuiebre.codtipoproducto, string.Empty);

        //                                    if (resultadoTransaccionPrestacion.ResultadoCodigo == -1)
        //                                    {
        //                                        transaction.Rollback();
        //                                        vResultadoTransaccion.IdRegistro = -1;
        //                                        vResultadoTransaccion.ResultadoCodigo = -1;
        //                                        vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionPrestacion.ResultadoDescripcion + " ; [GetTipoProductoPrestaciones]";
        //                                        return vResultadoTransaccion;
        //                                    }

        //                                    vCodigoPrestacion = resultadoTransaccionPrestacion.data;
        //                                }

        //                                // Validar codigo de prestación que viene del pedido -- Falta Indicar por Cesar Medrano
        //                                if (ventaAutomatica)
        //                                {
        //                                    if (value.flagpaquete.Equals("S"))
        //                                    {
        //                                        vCodigoPrestacion = vCodigoPrestacion;
        //                                    }
        //                                }

        //                                if (!ventaAutomatica)
        //                                {
        //                                    ResultadoTransaccion<decimal> resultadoTransaccionGncxVenta = await GetGncPorVenta(conn, value.codventa);

        //                                    if (resultadoTransaccionGncxVenta.ResultadoCodigo == -1)
        //                                    {
        //                                        transaction.Rollback();
        //                                        vResultadoTransaccion.IdRegistro = -1;
        //                                        vResultadoTransaccion.ResultadoCodigo = -1;
        //                                        vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionGncxVenta.ResultadoDescripcion + " ; [GetGncPorVenta]";
        //                                        return vResultadoTransaccion;
        //                                    }

        //                                    vGncPorVenta = resultadoTransaccionGncxVenta.data;

        //                                    ResultadoTransaccion<BE_ValidaPresotor> resultadoTransaccionValidaPresotor = await GetValidaPresotor(conn, value.codatencion, value.codventa);

        //                                    if (resultadoTransaccionValidaPresotor.ResultadoCodigo == -1)
        //                                    {
        //                                        transaction.Rollback();
        //                                        vResultadoTransaccion.IdRegistro = -1;
        //                                        vResultadoTransaccion.ResultadoCodigo = -1;
        //                                        vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionValidaPresotor.ResultadoDescripcion + " ; [GetValidaPresotor]";
        //                                        return vResultadoTransaccion;
        //                                    }

        //                                    vValidaPresotor = resultadoTransaccionValidaPresotor.data;

        //                                    if (vValidaPresotor.seguir_venta.Equals("N"))
        //                                    {
        //                                        transaction.Rollback();
        //                                        vResultadoTransaccion.IdRegistro = -1;
        //                                        vResultadoTransaccion.ResultadoCodigo = -1;
        //                                        vResultadoTransaccion.ResultadoDescripcion = "Error al generar la venta...!!!, Por favor grabar la venta de nuevo";
        //                                        return vResultadoTransaccion;
        //                                    }
        //                                }

        //                                ResultadoTransaccion<string> resultadoTransaccionCodigoPresotor = await ResgistraPrestacion(conn, value.codatencion, vCodigoPrestacion, (float)value.montototal, (float)vGncPorVenta, (float)value.montopaciente, 0, (float)value.porcentajecoaseguro, 0, value.codventa, 0, value.observacion, value.tipomovimiento);

        //                                if (resultadoTransaccionCodigoPresotor.ResultadoCodigo == -1)
        //                                {
        //                                    transaction.Rollback();
        //                                    vResultadoTransaccion.IdRegistro = -1;
        //                                    vResultadoTransaccion.ResultadoCodigo = -1;
        //                                    vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionCodigoPresotor.ResultadoDescripcion + " ; [ResgistraPrestacion]";
        //                                    return vResultadoTransaccion;
        //                                }

        //                                vCodigoPresotor = resultadoTransaccionCodigoPresotor.data;

        //                                if (vValidaPresotor.codatencion_ver != vCodigoPresotor.Substring(0, 8))
        //                                {
        //                                    transaction.Rollback();
        //                                    vResultadoTransaccion.IdRegistro = -1;
        //                                    vResultadoTransaccion.ResultadoCodigo = -1;
        //                                    vResultadoTransaccion.ResultadoDescripcion = "Error al generar la venta...!!!, Por favor grabar la venta de nuevo";
        //                                    return vResultadoTransaccion;
        //                                }

        //                                ResultadoTransaccion<bool> resultadoTransaccionActualizarVenta = await ActualizarVenta(conn, value.codventa, vCodigoPresotor, (int)value.RegIdUsuario);

        //                                if (resultadoTransaccionActualizarVenta.ResultadoCodigo == -1)
        //                                {
        //                                    transaction.Rollback();
        //                                    vResultadoTransaccion.IdRegistro = -1;
        //                                    vResultadoTransaccion.ResultadoCodigo = -1;
        //                                    vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionActualizarVenta.ResultadoDescripcion + " ; [ActualizarVenta]";
        //                                    return vResultadoTransaccion;
        //                                }

        //                                ResultadoTransaccion<string> resultadoTransaccionPresotorConsultaVarios = await GetPresotorConsultaVarios(conn, value.codatencion, vCodigoPresotor, 3);

        //                                if (resultadoTransaccionPresotorConsultaVarios.ResultadoCodigo == -1)
        //                                {
        //                                    transaction.Rollback();
        //                                    vResultadoTransaccion.IdRegistro = -1;
        //                                    vResultadoTransaccion.ResultadoCodigo = -1;
        //                                    vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionPresotorConsultaVarios.ResultadoDescripcion + " ; [GetPresotorConsultaVarios]";
        //                                    return vResultadoTransaccion;
        //                                }

        //                                if (resultadoTransaccionPresotorConsultaVarios.data.Equals("N"))
        //                                {
        //                                    transaction.Rollback();
        //                                    vResultadoTransaccion.IdRegistro = -1;
        //                                    vResultadoTransaccion.ResultadoCodigo = -1;
        //                                    vResultadoTransaccion.ResultadoDescripcion = "Error al generar la venta...!!!, Por favor grabar la venta de nuevo";
        //                                    return vResultadoTransaccion;
        //                                }

        //                                ResultadoTransaccion<bool> resultadoTransaccionActualizarPresotor = new ResultadoTransaccion<bool>();

        //                                if (vFlagPaquete.Equals("S"))
        //                                {
        //                                    resultadoTransaccionActualizarPresotor = await ActualizarPresotor(conn, "flagpaquete", vCodigoPresotor, "S");
        //                                }
        //                                else
        //                                {
        //                                    resultadoTransaccionActualizarPresotor = await ActualizarPresotor(conn, "flagpaquete", vCodigoPresotor, "N");
        //                                }

        //                                if (resultadoTransaccionActualizarPresotor.ResultadoCodigo == -1)
        //                                {
        //                                    transaction.Rollback();
        //                                    vResultadoTransaccion.IdRegistro = -1;
        //                                    vResultadoTransaccion.ResultadoCodigo = -1;
        //                                    vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionActualizarPresotor.ResultadoDescripcion + " ; [ActualizarPresotor]";
        //                                    return vResultadoTransaccion;
        //                                }

        //                                if (!value.codatencion.Substring(1, 1).Equals("A"))
        //                                {
        //                                    if (vNoCubierto)
        //                                    {
        //                                        ResultadoTransaccion<bool> resultadoTransaccionValorGNC = await ActualizarPresotor(conn, "valorgnc", vCodigoPresotor, vGncPorVenta.ToString());

        //                                        if (resultadoTransaccionValorGNC.ResultadoCodigo == -1)
        //                                        {
        //                                            transaction.Rollback();
        //                                            vResultadoTransaccion.IdRegistro = -1;
        //                                            vResultadoTransaccion.ResultadoCodigo = -1;
        //                                            vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionValorGNC.ResultadoDescripcion + " ; [ActualizarPresotor]";
        //                                            return vResultadoTransaccion;
        //                                        }

        //                                        ResultadoTransaccion<bool> resultadoTransaccionLiqTerceroContratante = await ActualizarPresotor(conn, "liqtercerocontratante", vCodigoPresotor, "S");

        //                                        if (resultadoTransaccionLiqTerceroContratante.ResultadoCodigo == -1)
        //                                        {
        //                                            transaction.Rollback();
        //                                            vResultadoTransaccion.IdRegistro = -1;
        //                                            vResultadoTransaccion.ResultadoCodigo = -1;
        //                                            vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionLiqTerceroContratante.ResultadoDescripcion + " ; [ActualizarPresotor]";
        //                                            return vResultadoTransaccion;
        //                                        }
        //                                    }
        //                                }

        //                                ResultadoTransaccion<bool> resultadoTransaccionAfectoImpuesto = new ResultadoTransaccion<bool>();

        //                                if (itemQuiebre.igvproducto.Equals(0))
        //                                {
        //                                    resultadoTransaccionAfectoImpuesto = await ActualizarPresotor(conn, "afectoimpuesto", vCodigoPresotor, "N");
        //                                }
        //                                else
        //                                {
        //                                    resultadoTransaccionAfectoImpuesto = await ActualizarPresotor(conn, "afectoimpuesto", vCodigoPresotor, "S");
        //                                }

        //                                if (resultadoTransaccionAfectoImpuesto.ResultadoCodigo == -1)
        //                                {
        //                                    transaction.Rollback();
        //                                    vResultadoTransaccion.IdRegistro = -1;
        //                                    vResultadoTransaccion.ResultadoCodigo = -1;
        //                                    vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionAfectoImpuesto.ResultadoDescripcion + " ; [ActualizarPresotor]";
        //                                    return vResultadoTransaccion;
        //                                }

        //                                ResultadoTransaccion<bool> resultadoTransaccionCodMedicoEnvia = await ActualizarPresotor(conn, "codmedicoenvia", vCodigoPresotor, value.codmedico);

        //                                if (resultadoTransaccionCodMedicoEnvia.ResultadoCodigo == -1)
        //                                {
        //                                    transaction.Rollback();
        //                                    vResultadoTransaccion.IdRegistro = -1;
        //                                    vResultadoTransaccion.ResultadoCodigo = -1;
        //                                    vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionCodMedicoEnvia.ResultadoDescripcion + " ; [ActualizarPresotor]";
        //                                    return vResultadoTransaccion;
        //                                }
        //                            }

        //                            if (value.codtipocliente.Equals("03"))
        //                            {
        //                                if (value.codpedido != null)
        //                                {
        //                                    if (value.codpedido.Length.Equals(14))
        //                                    {

        //                                        ResultadoTransaccion<bool> vResultadoTransaccionPedido = await PedidoxDevolverRecalculo(conn, value.codpedido, string.Empty, 0, 0, 1);

        //                                        if (vResultadoTransaccionPedido.ResultadoCodigo == -1)
        //                                        {
        //                                            transaction.Rollback();
        //                                            vResultadoTransaccion.IdRegistro = -1;
        //                                            vResultadoTransaccion.ResultadoCodigo = -1;
        //                                            vResultadoTransaccion.ResultadoDescripcion = vResultadoTransaccionPedido.ResultadoDescripcion + " ; [PedidoxDevolverRecalculo]";
        //                                            return vResultadoTransaccion;
        //                                        }
        //                                    }
        //                                }
        //                            }

        //                            listVentasGenerados.Add(new BE_VentasGenerado { codventa = value.codventa, codpresotor = vCodigoPresotor });

        //                        }
        //                    }

        //                    foreach (BE_VentasGenerado itemVenta in listVentasGenerados)
        //                    {
        //                        ResultadoTransaccion<bool> resultadoTransaccionActualizarVentaDcstDetalle = await ActualizarVentaDcstDetalle(conn, value.codventa, vCodigoPresotor, (int)value.RegIdUsuario);

        //                        if (resultadoTransaccionActualizarVentaDcstDetalle.ResultadoCodigo == -1)
        //                        {
        //                            transaction.Rollback();
        //                            vResultadoTransaccion.IdRegistro = -1;
        //                            vResultadoTransaccion.ResultadoCodigo = -1;
        //                            vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionActualizarVentaDcstDetalle.ResultadoDescripcion + " ; [ActualizarVentaDcstDetalle]";
        //                            return vResultadoTransaccion;
        //                        }
        //                    }

        //                    if (!ventaAutomatica)
        //                    {
        //                        if (value.codtipocliente.Equals("01"))
        //                        {
        //                            if (value.codpedido != null)
        //                            {
        //                                if (value.codpedido.Length.Equals(14))
        //                                {
        //                                    ResultadoTransaccion<bool> resultadoTransaccionVistoBueno = await ActualizarPedido(conn, "vistobueno", value.codpedido, "L", value.RegIdUsuario.ToString());

        //                                    if (resultadoTransaccionVistoBueno.ResultadoCodigo == -1)
        //                                    {
        //                                        transaction.Rollback();
        //                                        vResultadoTransaccion.IdRegistro = -1;
        //                                        vResultadoTransaccion.ResultadoCodigo = -1;
        //                                        vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionVistoBueno.ResultadoDescripcion + " ; [ActualizarPedido]";
        //                                        return vResultadoTransaccion;
        //                                    }

        //                                    ResultadoTransaccion<bool> resultadoTransaccionFechaAtencion = await ActualizarPedido(conn, "fechaatencion_servidor", value.codpedido, "L", DateTime.Now.ToString());

        //                                    if (resultadoTransaccionFechaAtencion.ResultadoCodigo == -1)
        //                                    {
        //                                        transaction.Rollback();
        //                                        vResultadoTransaccion.IdRegistro = -1;
        //                                        vResultadoTransaccion.ResultadoCodigo = -1;
        //                                        vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionFechaAtencion.ResultadoDescripcion + " ; [ActualizarPedido]";
        //                                        return vResultadoTransaccion;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }

        //                    vResultadoTransaccion.IdRegistro = 0;
        //                    vResultadoTransaccion.ResultadoCodigo = 0;
        //                    vResultadoTransaccion.ResultadoDescripcion = "Se genero correctamente";

        //                    vResultadoTransaccion.dataList = listVentasGenerados;

        //                    transaction.Commit();
        //                }
        //                catch (Exception ex)
        //                {
        //                    transaction.Rollback();
        //                    vResultadoTransaccion.IdRegistro = -1;
        //                    vResultadoTransaccion.ResultadoCodigo = -1;
        //                    vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        vResultadoTransaccion.IdRegistro = -1;
        //        vResultadoTransaccion.ResultadoCodigo = -1;
        //        vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
        //    }

        //    return vResultadoTransaccion;
        //}

        //public async Task<ResultadoTransaccion<string>> RegistraVentaCabecera(SqlConnection conn, BE_VentasCabecera value)
        //{
        //    ResultadoTransaccion<string> vResultadoTransaccion = new ResultadoTransaccion<string>();
        //    _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

        //    vResultadoTransaccion.NombreMetodo = _metodoName;
        //    vResultadoTransaccion.NombreAplicacion = _aplicacionName;

        //    //using (SqlConnection conn = new SqlConnection(_cnx))
        //    //{
        //    try
        //    {
        //        using (SqlCommand cmd = new SqlCommand(SP_INSERT, conn))
        //        {
        //            cmd.Parameters.Clear();
        //            cmd.CommandType = System.Data.CommandType.StoredProcedure;

        //            SqlParameter oParam = new SqlParameter("@codigo", SqlDbType.Char, 8)
        //            {
        //                Direction = ParameterDirection.Output
        //            };
        //            cmd.Parameters.Add(oParam);

        //            cmd.Parameters.Add(new SqlParameter("@codatencion", value.codatencion));
        //            cmd.Parameters.Add(new SqlParameter("@codpedido", value.codpedido));
        //            cmd.Parameters.Add(new SqlParameter("@codalmacen", value.codalmacen));
        //            cmd.Parameters.Add(new SqlParameter("@tipomovimiento", value.tipomovimiento));
        //            cmd.Parameters.Add(new SqlParameter("@codtipocliente", value.codtipocliente));
        //            cmd.Parameters.Add(new SqlParameter("@codempresa", value.codempresa));
        //            cmd.Parameters.Add(new SqlParameter("@codcliente", value.codcliente));
        //            cmd.Parameters.Add(new SqlParameter("@codpaciente", value.codpaciente));
        //            cmd.Parameters.Add(new SqlParameter("@nombre", value.nombre));
        //            cmd.Parameters.Add(new SqlParameter("@cama", value.cama));
        //            cmd.Parameters.Add(new SqlParameter("@codmedico", value.codmedico));
        //            cmd.Parameters.Add(new SqlParameter("@planpoliza", value.planpoliza));
        //            cmd.Parameters.Add(new SqlParameter("@codpoliza", value.codpoliza));
        //            cmd.Parameters.Add(new SqlParameter("@deducible", value.deducible));
        //            cmd.Parameters.Add(new SqlParameter("@codaseguradora", value.codaseguradora));
        //            cmd.Parameters.Add(new SqlParameter("@codcia", value.codcia));
        //            cmd.Parameters.Add(new SqlParameter("@porcentajecoaseguro", value.porcentajecoaseguro));
        //            cmd.Parameters.Add(new SqlParameter("@porcentajeimpuesto", value.porcentajeimpuesto));
        //            cmd.Parameters.Add(new SqlParameter("@montodctoplan", value.montodctoplan));
        //            cmd.Parameters.Add(new SqlParameter("@porcentajedctoplan", value.porcentajedctoplan));
        //            cmd.Parameters.Add(new SqlParameter("@moneda", value.moneda));
        //            cmd.Parameters.Add(new SqlParameter("@montototal", value.montototal));
        //            cmd.Parameters.Add(new SqlParameter("@montoigv", value.montoigv));
        //            cmd.Parameters.Add(new SqlParameter("@montoneto", value.montoneto));
        //            cmd.Parameters.Add(new SqlParameter("@codplan", value.codplan));
        //            cmd.Parameters.Add(new SqlParameter("@montopaciente", value.montopaciente));
        //            cmd.Parameters.Add(new SqlParameter("@montoaseguradora", value.montoaseguradora));
        //            cmd.Parameters.Add(new SqlParameter("@observacion", value.observacion));
        //            cmd.Parameters.Add(new SqlParameter("@nombremedico", value.nombremedico));
        //            cmd.Parameters.Add(new SqlParameter("@nombreaseguradora", value.nombreaseguradora));
        //            cmd.Parameters.Add(new SqlParameter("@nombrecia", value.nombrecia));
        //            cmd.Parameters.Add(new SqlParameter("@tipocambio", value.tipocambio));
        //            cmd.Parameters.Add(new SqlParameter("@nombrediagnostico", value.nombrediagnostico));
        //            cmd.Parameters.Add(new SqlParameter("@flagpaquete", value.flagpaquete));
        //            cmd.Parameters.Add(new SqlParameter("@flg_gratuito", value.flg_gratuito));
        //            cmd.Parameters.Add(new SqlParameter("@usuario", value.usuario));
        //            cmd.Parameters.Add(new SqlParameter("@codcentro", value.codcentro));
        //            cmd.Parameters.Add(new SqlParameter("@flgsinstock", value.flgsinstock));

        //            // Datos de Auditoria
        //            cmd.Parameters.Add(new SqlParameter("@RegIdUsuario", value.RegIdUsuario));
        //            // Datos de Transaccion
        //            SqlParameter outputIdTransaccionParam = new SqlParameter("@IdTransaccion", SqlDbType.Int, 3)
        //            {
        //                Direction = ParameterDirection.Output
        //            };
        //            cmd.Parameters.Add(outputIdTransaccionParam);

        //            SqlParameter outputMsjTransaccionParam = new SqlParameter("@MsjTransaccion", SqlDbType.VarChar, 700)
        //            {
        //                Direction = ParameterDirection.Output
        //            };
        //            cmd.Parameters.Add(outputMsjTransaccionParam);

        //            await cmd.ExecuteNonQueryAsync();

        //            value.codventa = (string)oParam.Value;

        //            vResultadoTransaccion.IdRegistro = 0;
        //            vResultadoTransaccion.ResultadoCodigo = int.Parse(outputIdTransaccionParam.Value.ToString());
        //            vResultadoTransaccion.ResultadoDescripcion = (string)outputMsjTransaccionParam.Value;
        //            vResultadoTransaccion.data = value.codventa;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        vResultadoTransaccion.IdRegistro = -1;
        //        vResultadoTransaccion.ResultadoCodigo = -1;
        //        vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
        //    }
        //    //}

        //    return vResultadoTransaccion;
        //}

        //public async Task<ResultadoTransaccion<string>> RegistraVentaDetalle(SqlConnection conn, BE_VentasDetalle item, int RegIdUsuario)
        //{
        //    ResultadoTransaccion<string> vResultadoTransaccion = new ResultadoTransaccion<string>();
        //    _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

        //    vResultadoTransaccion.NombreMetodo = _metodoName;
        //    vResultadoTransaccion.NombreAplicacion = _aplicacionName;

        //    //using (SqlConnection conn = new SqlConnection(_cnx))
        //    //{
        //    try
        //    {
        //        using (SqlCommand cmdDetalle = new SqlCommand(SP_INSERT_DETALLE, conn))
        //        {
        //            cmdDetalle.Parameters.Clear();
        //            cmdDetalle.CommandType = System.Data.CommandType.StoredProcedure;

        //            cmdDetalle.Parameters.Add(new SqlParameter("@codventa", item.codventa));

        //            SqlParameter oParamDetalle = new SqlParameter("@coddetalle", SqlDbType.Char, 10)
        //            {
        //                Direction = ParameterDirection.Output
        //            };
        //            cmdDetalle.Parameters.Add(oParamDetalle);

        //            cmdDetalle.Parameters.Add(new SqlParameter("@codalmacen", item.codalmacen));
        //            cmdDetalle.Parameters.Add(new SqlParameter("@tipomovimiento", item.tipomovimiento));
        //            cmdDetalle.Parameters.Add(new SqlParameter("@codtipoproducto", item.codtipoproducto));
        //            cmdDetalle.Parameters.Add(new SqlParameter("@codproducto", item.codproducto));
        //            cmdDetalle.Parameters.Add(new SqlParameter("@dsc_producto", item.nombreproducto));
        //            cmdDetalle.Parameters.Add(new SqlParameter("@cnt_unitario", item.cantidad));
        //            cmdDetalle.Parameters.Add(new SqlParameter("@prc_unitario", item.valorVVP));
        //            cmdDetalle.Parameters.Add(new SqlParameter("@preciounidadcondcto", item.preciounidadcondcto));
        //            cmdDetalle.Parameters.Add(new SqlParameter("@precioventaPVP", item.precioventaPVP));
        //            cmdDetalle.Parameters.Add(new SqlParameter("@valorVVP", item.valorVVP));
        //            cmdDetalle.Parameters.Add(new SqlParameter("@porcentajedctoproducto", item.porcentajedctoproducto));
        //            cmdDetalle.Parameters.Add(new SqlParameter("@montototal", item.montototal));
        //            cmdDetalle.Parameters.Add(new SqlParameter("@montopaciente", item.montopaciente));
        //            cmdDetalle.Parameters.Add(new SqlParameter("@montoaseguradora", item.montoaseguradora));
        //            cmdDetalle.Parameters.Add(new SqlParameter("@codpedido", item.codpedido));
        //            cmdDetalle.Parameters.Add(new SqlParameter("@gnc", item.gnc));
        //            cmdDetalle.Parameters.Add(new SqlParameter("@manbtchnum", item.manBtchNum));
        //            cmdDetalle.Parameters.Add(new SqlParameter("@flgbtchnum", item.flgbtchnum));
        //            cmdDetalle.Parameters.Add(new SqlParameter("@flgnarcotico", item.flgnarcotico));
        //            cmdDetalle.Parameters.Add(new SqlParameter("@stockfraccion", item.stockfraccion));
        //            cmdDetalle.Parameters.Add(new SqlParameter("@stockalmacen", item.stockalmacen));

        //            // Datos de Auditoria
        //            cmdDetalle.Parameters.Add(new SqlParameter("@RegIdUsuario", RegIdUsuario));
        //            // Datos de Transaccion
        //            SqlParameter outputIdTransaccionParamDetalle = new SqlParameter("@IdTransaccion", SqlDbType.Int, 3)
        //            {
        //                Direction = ParameterDirection.Output
        //            };
        //            cmdDetalle.Parameters.Add(outputIdTransaccionParamDetalle);

        //            SqlParameter outputMsjTransaccionParamDetalle = new SqlParameter("@MsjTransaccion", SqlDbType.VarChar, 700)
        //            {
        //                Direction = ParameterDirection.Output
        //            };
        //            cmdDetalle.Parameters.Add(outputMsjTransaccionParamDetalle);

        //            //await conn.OpenAsync();
        //            await cmdDetalle.ExecuteNonQueryAsync();

        //            item.coddetalle = (string)oParamDetalle.Value;

        //            vResultadoTransaccion.IdRegistro = 0;
        //            vResultadoTransaccion.ResultadoCodigo = int.Parse(outputIdTransaccionParamDetalle.Value.ToString());
        //            vResultadoTransaccion.ResultadoDescripcion = (string)outputMsjTransaccionParamDetalle.Value;
        //            vResultadoTransaccion.data = item.coddetalle;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        vResultadoTransaccion.IdRegistro = -1;
        //        vResultadoTransaccion.ResultadoCodigo = -1;
        //        vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
        //    }
        //    //}

        //    return vResultadoTransaccion;
        //}

        //public async Task<ResultadoTransaccion<bool>> RegistraVentaDetalleLote(SqlConnection conn, BE_VentasDetalleLote item, int RegIdUsuario)
        //{
        //    ResultadoTransaccion<bool> vResultadoTransaccion = new ResultadoTransaccion<bool>();
        //    _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

        //    vResultadoTransaccion.NombreMetodo = _metodoName;
        //    vResultadoTransaccion.NombreAplicacion = _aplicacionName;

        //    try
        //    {
        //        using (SqlCommand cmdDetalle = new SqlCommand(SP_INSERT_DETALLE_LOTE, conn))
        //        {
        //            cmdDetalle.Parameters.Clear();
        //            cmdDetalle.CommandType = System.Data.CommandType.StoredProcedure;

        //            cmdDetalle.Parameters.Add(new SqlParameter("@coddetalle", item.coddetalle));

        //            cmdDetalle.Parameters.Add(new SqlParameter("@lote", item.lote));
        //            //cmdDetalle.Parameters.Add(new SqlParameter("@ubicacion", item.ubicacion));
        //            cmdDetalle.Parameters.Add(new SqlParameter("@fechavencimiento", item.fechavencimiento));
        //            cmdDetalle.Parameters.Add(new SqlParameter("@cantidad", item.cantidad));

        //            // Datos de Auditoria
        //            cmdDetalle.Parameters.Add(new SqlParameter("@RegIdUsuario", RegIdUsuario));
        //            // Datos de Transaccion
        //            SqlParameter outputIdTransaccionParamDetalle = new SqlParameter("@IdTransaccion", SqlDbType.Int, 3)
        //            {
        //                Direction = ParameterDirection.Output
        //            };
        //            cmdDetalle.Parameters.Add(outputIdTransaccionParamDetalle);

        //            SqlParameter outputMsjTransaccionParamDetalle = new SqlParameter("@MsjTransaccion", SqlDbType.VarChar, 700)
        //            {
        //                Direction = ParameterDirection.Output
        //            };
        //            cmdDetalle.Parameters.Add(outputMsjTransaccionParamDetalle);

        //            await cmdDetalle.ExecuteNonQueryAsync();

        //            vResultadoTransaccion.IdRegistro = 0;
        //            vResultadoTransaccion.ResultadoCodigo = int.Parse(outputIdTransaccionParamDetalle.Value.ToString());
        //            vResultadoTransaccion.ResultadoDescripcion = (string)outputMsjTransaccionParamDetalle.Value;
        //            vResultadoTransaccion.data = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        vResultadoTransaccion.IdRegistro = -1;
        //        vResultadoTransaccion.ResultadoCodigo = -1;
        //        vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
        //    }

        //    return vResultadoTransaccion;
        //}

        //public async Task<ResultadoTransaccion<bool>> RegistraVentaDatos(SqlConnection conn, BE_VentasDetalle item, int RegIdUsuario)
        //{
        //    ResultadoTransaccion<bool> vResultadoTransaccion = new ResultadoTransaccion<bool>();
        //    _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

        //    vResultadoTransaccion.NombreMetodo = _metodoName;
        //    vResultadoTransaccion.NombreAplicacion = _aplicacionName;

        //    //using (SqlConnection conn = new SqlConnection(_cnx))
        //    //{
        //    try
        //    {
        //        using (SqlCommand cmdDatos = new SqlCommand(SP_INSERT_DETALLE_DATOS, conn))
        //        {
        //            cmdDatos.Parameters.Clear();
        //            cmdDatos.CommandType = System.Data.CommandType.StoredProcedure;

        //            cmdDatos.Parameters.Add(new SqlParameter("@codventa", item.codventa));
        //            cmdDatos.Parameters.Add(new SqlParameter("@coddetalle", item.coddetalle));

        //            cmdDatos.Parameters.Add(new SqlParameter("@codproducto", item.codproducto));
        //            cmdDatos.Parameters.Add(new SqlParameter("@tipodocumentoautorizacion", item.VentasDetalleDatos.tipodocumentoautorizacion));
        //            cmdDatos.Parameters.Add(new SqlParameter("@numerodocumentoautorizacion", item.VentasDetalleDatos.numerodocumentoautorizacion));

        //            // Datos de Auditoria
        //            cmdDatos.Parameters.Add(new SqlParameter("@RegIdUsuario", RegIdUsuario));
        //            // Datos de Transaccion
        //            SqlParameter outputIdTransaccionParamDato = new SqlParameter("@IdTransaccion", SqlDbType.Int, 3)
        //            {
        //                Direction = ParameterDirection.Output
        //            };
        //            cmdDatos.Parameters.Add(outputIdTransaccionParamDato);

        //            SqlParameter outputMsjTransaccionParamDato = new SqlParameter("@MsjTransaccion", SqlDbType.VarChar, 700)
        //            {
        //                Direction = ParameterDirection.Output
        //            };
        //            cmdDatos.Parameters.Add(outputMsjTransaccionParamDato);

        //            //await conn.OpenAsync();
        //            await cmdDatos.ExecuteNonQueryAsync();

        //            vResultadoTransaccion.IdRegistro = 0;
        //            vResultadoTransaccion.ResultadoCodigo = int.Parse(outputIdTransaccionParamDato.Value.ToString());
        //            vResultadoTransaccion.ResultadoDescripcion = (string)outputMsjTransaccionParamDato.Value;
        //            vResultadoTransaccion.data = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        vResultadoTransaccion.IdRegistro = -1;
        //        vResultadoTransaccion.ResultadoCodigo = -1;
        //        vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
        //    }
        //    //}

        //    return vResultadoTransaccion;
        //}

        //public async Task<ResultadoTransaccion<bool>> ActualizarVenta(SqlConnection conn, string codventa, string codigopresotor, int RegIdUsuario)
        //{
        //    ResultadoTransaccion<bool> vResultadoTransaccion = new ResultadoTransaccion<bool>();
        //    _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

        //    vResultadoTransaccion.NombreMetodo = _metodoName;
        //    vResultadoTransaccion.NombreAplicacion = _aplicacionName;

        //    //using (SqlConnection conn = new SqlConnection(_cnx))
        //    //{
        //    try
        //    {
        //        using (SqlCommand cmdUpdate = new SqlCommand(SP_UPDATE, conn))
        //        {
        //            cmdUpdate.Parameters.Clear();
        //            cmdUpdate.CommandType = System.Data.CommandType.StoredProcedure;

        //            cmdUpdate.Parameters.Add(new SqlParameter("@codventa", codventa));

        //            cmdUpdate.Parameters.Add(new SqlParameter("@codpresotor", codigopresotor));

        //            // Datos de Auditoria
        //            cmdUpdate.Parameters.Add(new SqlParameter("@RegIdUsuario", RegIdUsuario));
        //            // Datos de Transaccion
        //            SqlParameter outputIdTransaccionParam = new SqlParameter("@IdTransaccion", SqlDbType.Int, 3)
        //            {
        //                Direction = ParameterDirection.Output
        //            };
        //            cmdUpdate.Parameters.Add(outputIdTransaccionParam);

        //            SqlParameter outputMsjTransaccionParam = new SqlParameter("@MsjTransaccion", SqlDbType.VarChar, 700)
        //            {
        //                Direction = ParameterDirection.Output
        //            };
        //            cmdUpdate.Parameters.Add(outputMsjTransaccionParam);

        //            //await conn.OpenAsync();
        //            await cmdUpdate.ExecuteNonQueryAsync();

        //            vResultadoTransaccion.IdRegistro = 0;
        //            vResultadoTransaccion.ResultadoCodigo = int.Parse(outputIdTransaccionParam.Value.ToString());
        //            vResultadoTransaccion.ResultadoDescripcion = (string)outputMsjTransaccionParam.Value;
        //            vResultadoTransaccion.data = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        vResultadoTransaccion.IdRegistro = -1;
        //        vResultadoTransaccion.ResultadoCodigo = -1;
        //        vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
        //    }
        //    //}

        //    return vResultadoTransaccion;
        //}

        //public async Task<ResultadoTransaccion<bool>> ActualizarVentaDcstDetalle(SqlConnection conn, string codventa, string codigopresotor, int RegIdUsuario)
        //{
        //    ResultadoTransaccion<bool> vResultadoTransaccion = new ResultadoTransaccion<bool>();
        //    _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

        //    vResultadoTransaccion.NombreMetodo = _metodoName;
        //    vResultadoTransaccion.NombreAplicacion = _aplicacionName;

        //    //using (SqlConnection conn = new SqlConnection(_cnx))
        //    //{
        //    try
        //    {
        //        using (SqlCommand cmdUpdate = new SqlCommand(SP_UPDATE_DSCTO_DETALLE, conn))
        //        {
        //            cmdUpdate.Parameters.Clear();
        //            cmdUpdate.CommandType = System.Data.CommandType.StoredProcedure;

        //            cmdUpdate.Parameters.Add(new SqlParameter("@codventa", codventa));
        //            cmdUpdate.Parameters.Add(new SqlParameter("@codpresotor", codigopresotor));

        //            // Datos de Auditoria
        //            //cmdUpdate.Parameters.Add(new SqlParameter("@RegIdUsuario", RegIdUsuario));
        //            // Datos de Transaccion
        //            SqlParameter outputIdTransaccionParam = new SqlParameter("@IdTransaccion", SqlDbType.Int, 3)
        //            {
        //                Direction = ParameterDirection.Output
        //            };
        //            cmdUpdate.Parameters.Add(outputIdTransaccionParam);

        //            SqlParameter outputMsjTransaccionParam = new SqlParameter("@MsjTransaccion", SqlDbType.VarChar, 700)
        //            {
        //                Direction = ParameterDirection.Output
        //            };
        //            cmdUpdate.Parameters.Add(outputMsjTransaccionParam);

        //            //await conn.OpenAsync();
        //            await cmdUpdate.ExecuteNonQueryAsync();

        //            vResultadoTransaccion.IdRegistro = 0;
        //            vResultadoTransaccion.ResultadoCodigo = int.Parse(outputIdTransaccionParam.Value.ToString());
        //            vResultadoTransaccion.ResultadoDescripcion = (string)outputMsjTransaccionParam.Value;
        //            vResultadoTransaccion.data = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        vResultadoTransaccion.IdRegistro = -1;
        //        vResultadoTransaccion.ResultadoCodigo = -1;
        //        vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
        //    }
        //    //}

        //    return vResultadoTransaccion;
        //}

        //public async Task<ResultadoTransaccion<bool>> PedidoxDevolverRecalculo(SqlConnection conn, string codpedido, string codproducto, decimal entero, decimal menudeo,int orden)
        //{
        //    ResultadoTransaccion<bool> vResultadoTransaccion = new ResultadoTransaccion<bool>();
        //    _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

        //    vResultadoTransaccion.NombreMetodo = _metodoName;
        //    vResultadoTransaccion.NombreAplicacion = _aplicacionName;

        //    //using (SqlConnection conn = new SqlConnection(_cnxClinica))
        //    //{
        //    try
        //    {
        //        using (SqlCommand cmdPedido = new SqlCommand(SP_PEDIDOXDEVOLVER_RECALCULO, conn))
        //        {
        //            cmdPedido.Parameters.Clear();
        //            cmdPedido.CommandType = System.Data.CommandType.StoredProcedure;

        //            cmdPedido.Parameters.Add(new SqlParameter("@codpedido", codpedido));
        //            cmdPedido.Parameters.Add(new SqlParameter("@codproducto", codproducto));

        //            cmdPedido.Parameters.Add(new SqlParameter("@cantE", entero));
        //            cmdPedido.Parameters.Add(new SqlParameter("@cantM", menudeo));
        //            cmdPedido.Parameters.Add(new SqlParameter("@orden", orden));

        //            // Datos de Transaccion
        //            // Datos de Transaccion
        //            SqlParameter outputIdTransaccionParam = new SqlParameter("@IdTransaccion", SqlDbType.Int, 3)
        //            {
        //                Direction = ParameterDirection.Output
        //            };
        //            cmdPedido.Parameters.Add(outputIdTransaccionParam);

        //            SqlParameter outputMsjTransaccionParam = new SqlParameter("@MsjTransaccion", SqlDbType.VarChar, 700)
        //            {
        //                Direction = ParameterDirection.Output
        //            };
        //            cmdPedido.Parameters.Add(outputMsjTransaccionParam);

        //            await cmdPedido.ExecuteNonQueryAsync();

        //            vResultadoTransaccion.IdRegistro = 0;
        //            vResultadoTransaccion.ResultadoCodigo = int.Parse(outputIdTransaccionParam.Value.ToString());
        //            vResultadoTransaccion.ResultadoDescripcion = (string)outputMsjTransaccionParam.Value;
        //            vResultadoTransaccion.data = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        vResultadoTransaccion.IdRegistro = -1;
        //        vResultadoTransaccion.ResultadoCodigo = -1;
        //        vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
        //    }
        //    //}

        //    return vResultadoTransaccion;
        //}

        //public async Task<ResultadoTransaccion<string>> ResgistraPrestacion(SqlConnection conn, string codatencion, string codpresentacion, float montototal, float gnc, float montopaciente, float porcentajededucible, float coaseguro, float descuento, string codventa, float deducible, string observacion, string tipomovimiento)
        //{
        //    ResultadoTransaccion<string> vResultadoTransaccion = new ResultadoTransaccion<string>();
        //    _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

        //    vResultadoTransaccion.NombreMetodo = _metodoName;
        //    vResultadoTransaccion.NombreAplicacion = _aplicacionName;

        //    //using (SqlConnection connPedido = new SqlConnection(_cnxClinica))
        //    //{
        //    try
        //    {

        //        string vCodPresotor = string.Empty;

        //        using (SqlCommand cmdPedido = new SqlCommand(SP_FARMACIA_PRESTACION_INSERT, conn))
        //        {
        //            cmdPedido.Parameters.Clear();
        //            cmdPedido.CommandType = System.Data.CommandType.StoredProcedure;

        //            cmdPedido.Parameters.Add(new SqlParameter("@codatencion", codatencion));
        //            cmdPedido.Parameters.Add(new SqlParameter("@codprestacion", codpresentacion));
        //            cmdPedido.Parameters.Add(new SqlParameter("@valorcorregido", montototal));
        //            cmdPedido.Parameters.Add(new SqlParameter("@valorgnc", gnc));
        //            cmdPedido.Parameters.Add(new SqlParameter("@valorcoaseguro", montopaciente));
        //            cmdPedido.Parameters.Add(new SqlParameter("@porcentajededucible", porcentajededucible));
        //            cmdPedido.Parameters.Add(new SqlParameter("@porcentajecoaseguro", coaseguro));
        //            cmdPedido.Parameters.Add(new SqlParameter("@descuento", descuento));
        //            cmdPedido.Parameters.Add(new SqlParameter("@documento", codventa));
        //            cmdPedido.Parameters.Add(new SqlParameter("@deducible", deducible));
        //            cmdPedido.Parameters.Add(new SqlParameter("@observaciones", observacion));
        //            cmdPedido.Parameters.Add(new SqlParameter("@tipomovimiento", tipomovimiento));

        //            SqlParameter outputCodPresotorParam = new SqlParameter("@codpresotor", SqlDbType.Char, 12)
        //            {
        //                Direction = ParameterDirection.Output
        //            };
        //            cmdPedido.Parameters.Add(outputCodPresotorParam);

        //            SqlParameter outputIdTransaccionParam = new SqlParameter("@IdTransaccion", SqlDbType.Int, 3)
        //            {
        //                Direction = ParameterDirection.Output
        //            };
        //            cmdPedido.Parameters.Add(outputIdTransaccionParam);

        //            SqlParameter outputMsjTransaccionParam = new SqlParameter("@MsjTransaccion", SqlDbType.VarChar, 700)
        //            {
        //                Direction = ParameterDirection.Output
        //            };
        //            cmdPedido.Parameters.Add(outputMsjTransaccionParam);

        //            //await conn.OpenAsync();
        //            await cmdPedido.ExecuteNonQueryAsync();

        //            vCodPresotor = (string)outputCodPresotorParam.Value;

        //            vResultadoTransaccion.IdRegistro = 0;
        //            vResultadoTransaccion.ResultadoCodigo = int.Parse(outputIdTransaccionParam.Value.ToString());
        //            vResultadoTransaccion.ResultadoDescripcion = (string)outputMsjTransaccionParam.Value;
        //            vResultadoTransaccion.data = vCodPresotor;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        vResultadoTransaccion.IdRegistro = -1;
        //        vResultadoTransaccion.ResultadoCodigo = -1;
        //        vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
        //    }
        //    //}

        //    return vResultadoTransaccion;
        //}

        //public async Task<ResultadoTransaccion<string>> GetTipoProductoPrestaciones(SqlConnection conn, string codventa, string codtipproducto, string codcentropedido)
        //{
        //    ResultadoTransaccion<string> vResultadoTransaccion = new ResultadoTransaccion<string>();
        //    _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

        //    vResultadoTransaccion.NombreMetodo = _metodoName;
        //    vResultadoTransaccion.NombreAplicacion = _aplicacionName;

        //    try
        //    {
        //        //using (SqlConnection connTipoProducto = new SqlConnection(_cnx))
        //        //{
        //        using (SqlCommand cmdTipoProductoPrestaciones = new SqlCommand(SP_GET_TIPOPRODUCTOPRESTACIONES, conn))
        //        {
        //            cmdTipoProductoPrestaciones.CommandType = System.Data.CommandType.StoredProcedure;
        //            cmdTipoProductoPrestaciones.Parameters.Add(new SqlParameter("@codventa", codventa));
        //            cmdTipoProductoPrestaciones.Parameters.Add(new SqlParameter("@tipoproducto", codtipproducto));
        //            cmdTipoProductoPrestaciones.Parameters.Add(new SqlParameter("@centrocsf", codcentropedido));

        //            string response = string.Empty;

        //            //await connTipoProducto.OpenAsync();

        //            using (var reader = await cmdTipoProductoPrestaciones.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    response = ((reader["codprestacion"]) is DBNull) ? string.Empty : (string)reader["codprestacion"];
        //                }
        //            }

        //            vResultadoTransaccion.IdRegistro = 0;
        //            vResultadoTransaccion.ResultadoCodigo = 0;
        //            vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", 1);
        //            vResultadoTransaccion.data = response;
        //        }
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        vResultadoTransaccion.IdRegistro = -1;
        //        vResultadoTransaccion.ResultadoCodigo = -1;
        //        vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
        //    }

        //    return vResultadoTransaccion;

        //}

        //public async Task<ResultadoTransaccion<decimal>> GetGncPorVenta(SqlConnection conn, string codventa)
        //{
        //    ResultadoTransaccion<decimal> vResultadoTransaccion = new ResultadoTransaccion<decimal>();
        //    _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

        //    vResultadoTransaccion.NombreMetodo = _metodoName;
        //    vResultadoTransaccion.NombreAplicacion = _aplicacionName;

        //    try
        //    {
        //        //using (SqlConnection connGncPorVenta = new SqlConnection(_cnx))
        //        //{
        //        using (SqlCommand cmdGncPorVenta = new SqlCommand(SP_GET_GNCXVENTA, conn))
        //        {
        //            cmdGncPorVenta.CommandType = System.Data.CommandType.StoredProcedure;
        //            cmdGncPorVenta.Parameters.Add(new SqlParameter("@codventa", codventa));

        //            decimal response = 0;

        //            //await connGncPorVenta.OpenAsync();

        //            using (var reader = await cmdGncPorVenta.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    response = ((reader["valorgnc"]) is DBNull) ? 0 : (decimal)reader["valorgnc"];
        //                }
        //            }


        //            vResultadoTransaccion.IdRegistro = 0;
        //            vResultadoTransaccion.ResultadoCodigo = 0;
        //            vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", 1);
        //            vResultadoTransaccion.data = response;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        vResultadoTransaccion.IdRegistro = -1;
        //        vResultadoTransaccion.ResultadoCodigo = -1;
        //        vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
        //    }

        //    return vResultadoTransaccion;

        //}

        //public async Task<ResultadoTransaccion<BE_ValidaPresotor>> GetValidaPresotor(SqlConnection conn, string codatencion, string codventa)
        //{
        //    ResultadoTransaccion<BE_ValidaPresotor> vResultadoTransaccion = new ResultadoTransaccion<BE_ValidaPresotor>();
        //    _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

        //    vResultadoTransaccion.NombreMetodo = _metodoName;
        //    vResultadoTransaccion.NombreAplicacion = _aplicacionName;

        //    try
        //    {
        //        //using (SqlConnection connValidaPresotor = new SqlConnection(_cnx))
        //        //{
        //        using (SqlCommand cmdValidaPresotor = new SqlCommand(SP_GET_VALIDA_PRESOTOR, conn))
        //        {
        //            cmdValidaPresotor.CommandType = System.Data.CommandType.StoredProcedure;
        //            cmdValidaPresotor.Parameters.Add(new SqlParameter("@codatencion", codatencion));
        //            cmdValidaPresotor.Parameters.Add(new SqlParameter("@codventa", codventa));

        //            BE_ValidaPresotor response = new BE_ValidaPresotor();

        //            //await connValidaPresotor.OpenAsync();

        //            using (var reader = await cmdValidaPresotor.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    response.seguir_venta = ((reader["seguir_venta"]) is DBNull) ? string.Empty : (string)reader["seguir_venta"];
        //                    response.codatencion_ver = ((reader["codatencion_ver"]) is DBNull) ? string.Empty : (string)reader["codatencion_ver"];
        //                    response.codpresotor_ver = ((reader["codpresotor_ver"]) is DBNull) ? string.Empty : (string)reader["codpresotor_ver"];
        //                }
        //            }

        //            vResultadoTransaccion.IdRegistro = 0;
        //            vResultadoTransaccion.ResultadoCodigo = 0;
        //            vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", 1);
        //            vResultadoTransaccion.data = response;
        //        }
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        vResultadoTransaccion.IdRegistro = -1;
        //        vResultadoTransaccion.ResultadoCodigo = -1;
        //        vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
        //    }

        //    return vResultadoTransaccion;

        //}

        //public async Task<ResultadoTransaccion<string>> GetPresotorConsultaVarios(SqlConnection conn, string codatencion, string codpresotor, int orden)
        //{
        //    ResultadoTransaccion<string> vResultadoTransaccion = new ResultadoTransaccion<string>();
        //    _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

        //    vResultadoTransaccion.NombreMetodo = _metodoName;
        //    vResultadoTransaccion.NombreAplicacion = _aplicacionName;

        //    try
        //    {
        //        //using (SqlConnection connPresotorConsultaVarios = new SqlConnection(_cnx))
        //        //{
        //        using (SqlCommand cmdPresotorConsultaVarios = new SqlCommand(SP_GET_PRESOTOR_CONSULTAVARIOS, conn))
        //        {
        //            cmdPresotorConsultaVarios.CommandType = System.Data.CommandType.StoredProcedure;
        //            cmdPresotorConsultaVarios.Parameters.Add(new SqlParameter("@codigo", codatencion));
        //            cmdPresotorConsultaVarios.Parameters.Add(new SqlParameter("@codpresotor", codpresotor));
        //            cmdPresotorConsultaVarios.Parameters.Add(new SqlParameter("@orden", orden));

        //            string response = string.Empty;

        //            //await conn.OpenAsync();

        //            using (var reader = await cmdPresotorConsultaVarios.ExecuteReaderAsync())
        //            {
        //                while (await reader.ReadAsync())
        //                {
        //                    response = ((reader["seguir_venta"]) is DBNull) ? string.Empty : (string)reader["seguir_venta"];
        //                }
        //            }


        //            vResultadoTransaccion.IdRegistro = 0;
        //            vResultadoTransaccion.ResultadoCodigo = 0;
        //            vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", 1);
        //            vResultadoTransaccion.data = response;
        //        }
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        vResultadoTransaccion.IdRegistro = -1;
        //        vResultadoTransaccion.ResultadoCodigo = -1;
        //        vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
        //    }

        //    return vResultadoTransaccion;

        //}

        //public async Task<ResultadoTransaccion<bool>> ActualizarPresotor(SqlConnection conn, string campo, string codpresotor, string nuevovalor)
        //{
        //    ResultadoTransaccion<bool> vResultadoTransaccion = new ResultadoTransaccion<bool>();
        //    _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

        //    vResultadoTransaccion.NombreMetodo = _metodoName;
        //    vResultadoTransaccion.NombreAplicacion = _aplicacionName;

        //    //using (SqlConnection connPresotor = new SqlConnection(_cnxClinica))
        //    //{
        //    try
        //    {
        //        using (SqlCommand cmdActualizarPresotor = new SqlCommand(SP_UPDATE_PRESOTOR, conn))
        //        {
        //            cmdActualizarPresotor.Parameters.Clear();
        //            cmdActualizarPresotor.CommandType = System.Data.CommandType.StoredProcedure;

        //            cmdActualizarPresotor.Parameters.Add(new SqlParameter("@campo", campo));
        //            cmdActualizarPresotor.Parameters.Add(new SqlParameter("@codigo", codpresotor));
        //            cmdActualizarPresotor.Parameters.Add(new SqlParameter("@nuevovalor", nuevovalor));
        //            SqlParameter outputIdTransaccionParam = new SqlParameter("@IdTransaccion", SqlDbType.Int, 3)
        //            {
        //                Direction = ParameterDirection.Output
        //            };
        //            cmdActualizarPresotor.Parameters.Add(outputIdTransaccionParam);

        //            SqlParameter outputMsjTransaccionParam = new SqlParameter("@MsjTransaccion", SqlDbType.VarChar, 700)
        //            {
        //                Direction = ParameterDirection.Output
        //            };
        //            cmdActualizarPresotor.Parameters.Add(outputMsjTransaccionParam);
        //            //await connPresotor.OpenAsync();
        //            await cmdActualizarPresotor.ExecuteNonQueryAsync();

        //            vResultadoTransaccion.IdRegistro = 0;
        //            vResultadoTransaccion.ResultadoCodigo = int.Parse(outputIdTransaccionParam.Value.ToString());
        //            vResultadoTransaccion.ResultadoDescripcion = (string)outputMsjTransaccionParam.Value;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        vResultadoTransaccion.IdRegistro = -1;
        //        vResultadoTransaccion.ResultadoCodigo = -1;
        //        vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
        //    }
        //    //}

        //    return vResultadoTransaccion;
        //}

        //public async Task<ResultadoTransaccion<bool>> ActualizarPedido(SqlConnection conn, string campo, string codigo, string nuevovalor, string coduser)
        //{
        //    ResultadoTransaccion<bool> vResultadoTransaccion = new ResultadoTransaccion<bool>();
        //    _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

        //    vResultadoTransaccion.NombreMetodo = _metodoName;
        //    vResultadoTransaccion.NombreAplicacion = _aplicacionName;

        //    //using (SqlConnection connPedido = new SqlConnection(conn))
        //    //{
        //    try
        //    {
        //        using (SqlCommand cmdActualizarPedido = new SqlCommand(SP_UPDATE_PEDIDO, conn))
        //        {
        //            cmdActualizarPedido.Parameters.Clear();
        //            cmdActualizarPedido.CommandType = System.Data.CommandType.StoredProcedure;

        //            cmdActualizarPedido.Parameters.Add(new SqlParameter("@wcampo", campo));
        //            cmdActualizarPedido.Parameters.Add(new SqlParameter("@codigo", codigo));
        //            cmdActualizarPedido.Parameters.Add(new SqlParameter("@nuevovalor", nuevovalor));
        //            cmdActualizarPedido.Parameters.Add(new SqlParameter("@coduser", coduser));
        //            SqlParameter outputIdTransaccionParam = new SqlParameter("@IdTransaccion", SqlDbType.Int, 3)
        //            {
        //                Direction = ParameterDirection.Output
        //            };
        //            cmdActualizarPedido.Parameters.Add(outputIdTransaccionParam);

        //            SqlParameter outputMsjTransaccionParam = new SqlParameter("@MsjTransaccion", SqlDbType.VarChar, 700)
        //            {
        //                Direction = ParameterDirection.Output
        //            };
        //            cmdActualizarPedido.Parameters.Add(outputMsjTransaccionParam);
        //            //await connPedido.OpenAsync();
        //            await cmdActualizarPedido.ExecuteNonQueryAsync();

        //            vResultadoTransaccion.IdRegistro = 0;
        //            vResultadoTransaccion.ResultadoCodigo = int.Parse(outputIdTransaccionParam.Value.ToString());
        //            vResultadoTransaccion.ResultadoDescripcion = (string)outputMsjTransaccionParam.Value;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        vResultadoTransaccion.IdRegistro = -1;
        //        vResultadoTransaccion.ResultadoCodigo = -1;
        //        vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
        //    }
        //    //}

        //    return vResultadoTransaccion;
        //}

        private BE_VentasCabecera CalculaTotales(BE_VentasDetalleQuiebre itemQuiebre, BE_VentasCabecera value)
        {
            BE_VentasCabecera ventasCabecera = new BE_VentasCabecera();

            bool isTrabajaVariosIGV = true;
            decimal isIGVTemp = 0;
            decimal isIGV = 0;
            decimal isSubTotal = 0;
            decimal isTotalPaciente = 0;
            decimal isTotalAseguradora = 0;

            decimal isSubTotal_0 = 0;
            decimal isTotalPaciente_0 = 0;
            decimal isTotalAseguradora_0 = 0;

            ventasCabecera = value;

            List<BE_VentasDetalle> ventasDetalles = new List<BE_VentasDetalle>();

            if (ventasCabecera.listaVentaDetalle.Any())
            {
                ventasDetalles = ventasCabecera.listaVentaDetalle;//.FindAll(xFila => xFila.codtipoproducto == itemQuiebre.codtipoproducto && xFila.igvproducto == itemQuiebre.igvproducto && xFila.narcotico == itemQuiebre.narcotico).ToList();

                foreach (BE_VentasDetalle item in ventasDetalles)
                {
                    isIGV = item.igvproducto;

                    if (isIGV > isIGVTemp)
                    {
                        isIGVTemp = isIGV;
                    }

                    if (!isTrabajaVariosIGV)
                    {
                        isSubTotal = isSubTotal + item.totalsinigv;
                        isTotalPaciente = isTotalPaciente + item.montopaciente;
                        isTotalAseguradora = isTotalAseguradora + item.montoaseguradora;
                    }
                    else
                    {
                        if (isIGV == 0)
                        {
                            isSubTotal_0 = isSubTotal_0 + item.totalsinigv;
                            isTotalPaciente_0 = isTotalPaciente_0 + item.montopaciente;
                            isTotalAseguradora_0 = isTotalAseguradora_0 + item.montoaseguradora;
                        }
                        else
                        {
                            isSubTotal = isSubTotal + item.totalsinigv;
                            isTotalPaciente = isTotalPaciente + item.montopaciente;
                            isTotalAseguradora = isTotalAseguradora + item.montoaseguradora;
                        }
                    }
                }

                isIGV = isIGVTemp;

                if (!isTrabajaVariosIGV)
                {
                    ventasCabecera.montototal = Math.Round(isSubTotal, 2);
                    ventasCabecera.montodctoplan = Math.Round(isSubTotal * (ventasCabecera.porcentajedctoplan / 100), 2);
                    ventasCabecera.montoigv = Math.Round(isSubTotal * (isIGV / 100), 2);
                    ventasCabecera.montoneto = Math.Round(isSubTotal + (isSubTotal * (isIGV / 100)), 2);
                    ventasCabecera.montopaciente = Math.Round(isTotalPaciente, 2);
                    ventasCabecera.montoaseguradora = Math.Round(isTotalAseguradora, 2);
                }
                else
                {
                    ventasCabecera.montototal = Math.Round(isSubTotal + isSubTotal_0, 2);
                    ventasCabecera.montodctoplan = Math.Round((isSubTotal + isSubTotal_0) * (ventasCabecera.porcentajedctoplan / 100), 2);
                    ventasCabecera.montoigv = Math.Round(isSubTotal * (isIGV / 100), 2);
                    ventasCabecera.montoneto = Math.Round(isSubTotal + isSubTotal_0 + (isSubTotal * (isIGV / 100)), 2);
                    ventasCabecera.montopaciente = Math.Round(isTotalPaciente + isTotalPaciente_0 + (isTotalPaciente * (isIGV / 100)), 2);
                    ventasCabecera.montoaseguradora = Math.Round(isTotalAseguradora + isTotalAseguradora_0 + (isTotalAseguradora * (isIGV / 100)), 2);
                }
            }
            else
            {
                ventasCabecera.montototal = Math.Round(isSubTotal + isSubTotal_0, 2);
                ventasCabecera.montodctoplan = Math.Round((isSubTotal + isSubTotal_0) * (ventasCabecera.porcentajedctoplan / 100), 2);
                ventasCabecera.montoigv = Math.Round(isSubTotal * (isIGV / 100), 2);
                ventasCabecera.montoneto = Math.Round((isSubTotal + isSubTotal_0) + (isSubTotal * (isIGV / 100)), 2);
                ventasCabecera.montopaciente = Math.Round(isTotalPaciente + isTotalPaciente_0 + (isTotalPaciente * (isIGV / 100)), 2);
                ventasCabecera.montoaseguradora = Math.Round(isTotalAseguradora + isTotalAseguradora_0 + (isTotalAseguradora * (isIGV / 100)), 2);
            }

            return ventasCabecera;

        }
        public async Task<ResultadoTransaccion<BE_VentasDetalle>> GetVentasChequea1MesAntes(string codpaciente, int cuantosmesesantes)
        {
            ResultadoTransaccion<BE_VentasDetalle> vResultadoTransaccion = new ResultadoTransaccion<BE_VentasDetalle>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new List<BE_VentasDetalle>();
                    using (SqlCommand cmd = new SqlCommand(SP_GET_VENTAS_CHEQUEA_1MES_ANTES, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codpaciente", codpaciente));
                        cmd.Parameters.Add(new SqlParameter("@cuantosmesesantes", cuantosmesesantes));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_VentasDetalle>)context.ConvertTo<BE_VentasDetalle>(reader);
                        }

                        conn.Close();

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", response.Count);
                        vResultadoTransaccion.dataList = response;
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
        public async Task<ResultadoTransaccion<BE_VentasCabecera>> ValidacionAnularVenta(BE_VentasCabecera value)
        {
            ResultadoTransaccion<BE_VentasCabecera> vResultadoTransaccion = new ResultadoTransaccion<BE_VentasCabecera>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {

                if (value.codtipocliente.Equals("01"))
                {
                    PacienteRepository atencionRepository = new PacienteRepository(context, _configuration);

                    ResultadoTransaccion<BE_Paciente> resultadoTransaccionPaciente = await atencionRepository.GetExistenciaPaciente(value.codatencion);

                    if (resultadoTransaccionPaciente.ResultadoCodigo == -1)
                    {
                        vResultadoTransaccion.IdRegistro = -1;
                        vResultadoTransaccion.ResultadoCodigo = -1;
                        vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionPaciente.ResultadoDescripcion;

                        return vResultadoTransaccion;
                    }

                    List<BE_Paciente> listPaciente = (List<BE_Paciente>)resultadoTransaccionPaciente.dataList;

                    if (listPaciente.Count == 0)
                    {
                        vResultadoTransaccion.IdRegistro = -1;
                        vResultadoTransaccion.ResultadoCodigo = -1;
                        vResultadoTransaccion.ResultadoDescripcion = "Atención no existe...Favor de revisar";

                        return vResultadoTransaccion;
                    }
                    else
                    {
                        if (!listPaciente[0].activo.Equals(1))
                        {
                            vResultadoTransaccion.IdRegistro = -1;
                            vResultadoTransaccion.ResultadoCodigo = -1;
                            vResultadoTransaccion.ResultadoDescripcion = "Atención desactivada...Favor de revisar";

                            return vResultadoTransaccion;
                        }
                    }

                    ResultadoTransaccion<BE_Presotor> resultadoTransaccionPresotor = await GetConsultaPresotorPorCodigo(value.codpresotor);

                    if (resultadoTransaccionPresotor.ResultadoCodigo == -1)
                    {
                        vResultadoTransaccion.IdRegistro = -1;
                        vResultadoTransaccion.ResultadoCodigo = -1;
                        vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionPresotor.ResultadoDescripcion;

                        return vResultadoTransaccion;
                    }

                    if (resultadoTransaccionPresotor.data.existepresotor)
                    {
                        if (!string.IsNullOrEmpty(resultadoTransaccionPresotor.data.codliqpaciente.Trim()) ||
                            !string.IsNullOrEmpty(resultadoTransaccionPresotor.data.codliqaseguradora.Trim()) ||
                            !string.IsNullOrEmpty(resultadoTransaccionPresotor.data.codliqcontratante.Trim()))
                        {
                            vResultadoTransaccion.IdRegistro = -1;
                            vResultadoTransaccion.ResultadoCodigo = -1;
                            vResultadoTransaccion.ResultadoDescripcion = "Venta tiene liquidación asociada";

                            return vResultadoTransaccion;
                        }
                    }
                    else
                    {
                        vResultadoTransaccion.IdRegistro = -1;
                        vResultadoTransaccion.ResultadoCodigo = -1;
                        vResultadoTransaccion.ResultadoDescripcion = "Presetor no se encuentra";

                        return vResultadoTransaccion;
                    }
                }

                if (value.tienedevolucion)
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = "No puede anular una venta que tiene una devolución asociada o esta anulada";

                    return vResultadoTransaccion;
                }

                if (!value.tipomovimiento.Equals("DV"))
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = "No puede anular este movimiento";

                    return vResultadoTransaccion;
                }

                ResultadoTransaccion<int> resultadoTransaccionValidaExisteVentaAnulacion = await GetValidaExisteVentaAnulacion(value.codventa);

                if (resultadoTransaccionValidaExisteVentaAnulacion.ResultadoCodigo == -1)
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionValidaExisteVentaAnulacion.ResultadoDescripcion;

                    return vResultadoTransaccion;
                }

                if (resultadoTransaccionValidaExisteVentaAnulacion.data > 0)
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = "La venta tiene generada una anulación";

                    return vResultadoTransaccion;
                }

                ResultadoTransaccion<BE_VentasCabecera> resultadoTransaccionVentaCabecera = await GetListVentaCabecera(value.codventa);

                if (resultadoTransaccionVentaCabecera.ResultadoCodigo == -1)
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionVentaCabecera.ResultadoDescripcion;

                    return vResultadoTransaccion;
                }

                if (resultadoTransaccionVentaCabecera.dataList.Any())
                {
                    BE_VentasCabecera ventasCabecera = ((List<BE_VentasCabecera>)resultadoTransaccionVentaCabecera.dataList)[0];

                    if(ventasCabecera.estado.Trim().Equals("C") && !string.IsNullOrEmpty(ventasCabecera.codcomprobante))
                    {
                        vResultadoTransaccion.IdRegistro = -1;
                        vResultadoTransaccion.ResultadoCodigo = -1;
                        vResultadoTransaccion.ResultadoDescripcion = "No puede Anular la venta, tiene comprobante";

                        return vResultadoTransaccion;
                    }
                }

                vResultadoTransaccion.IdRegistro = 0;
                vResultadoTransaccion.ResultadoCodigo = 0;
                vResultadoTransaccion.ResultadoDescripcion = "Validaciones Correctamente";
            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return vResultadoTransaccion;
        }
        public async Task<ResultadoTransaccion<BE_Presotor>> GetConsultaPresotorPorCodigo(string codpresotor)
        {
            ResultadoTransaccion<BE_Presotor> vResultadoTransaccion = new ResultadoTransaccion<BE_Presotor>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxClinica))
                {
                    using (SqlCommand cmdGncPorVenta = new SqlCommand(SP_PRESOTOR_CONSULTA, conn))
                    {
                        cmdGncPorVenta.CommandType = System.Data.CommandType.StoredProcedure;
                        cmdGncPorVenta.Parameters.Add(new SqlParameter("@codpresotor", codpresotor));

                        BE_Presotor response = new BE_Presotor();

                        await conn.OpenAsync();

                        using (var reader = await cmdGncPorVenta.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response.existepresotor = true;
                                response.codliqpaciente = ((reader["codliqpaciente"]) is DBNull) ? string.Empty : (string)reader["codliqpaciente"];
                                response.codliqaseguradora = ((reader["codliqaseguradora"]) is DBNull) ? string.Empty : (string)reader["codliqaseguradora"];
                                response.codliqcontratante = ((reader["codliqcontratante"]) is DBNull) ? string.Empty : (string)reader["codliqcontratante"];
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
        public async Task<ResultadoTransaccion<int>> GetValidaExisteVentaAnulacion(string codventa)
        {
            ResultadoTransaccion<int> vResultadoTransaccion = new ResultadoTransaccion<int>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    using (SqlCommand cmdGncPorVenta = new SqlCommand(SP_GET_VALIDA_EXISTE_VENTA_ANULACION, conn))
                    {
                        cmdGncPorVenta.CommandType = System.Data.CommandType.StoredProcedure;
                        cmdGncPorVenta.Parameters.Add(new SqlParameter("@codventa", codventa));

                        int response = 0;

                        await conn.OpenAsync();

                        using (var reader = await cmdGncPorVenta.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response = ((reader["cantidad"]) is DBNull) ? 0 : (int)reader["cantidad"];
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
        public async Task<ResultadoTransaccion<BE_VentasCabecera>> RegistrarAnularVenta(BE_VentasCabecera value)
        {
            ResultadoTransaccion<BE_VentasCabecera> vResultadoTransaccion = new ResultadoTransaccion<BE_VentasCabecera>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                TablaRepository tablaRepository = new TablaRepository(context, _configuration);
                ResultadoTransaccion<BE_Tabla> resultadoTransaccionTablaUsuarioAcceso = await tablaRepository.GetListTablaLogisticaPorFiltros("PERMISOUSERMOVCA", value.RegIdUsuario.ToString(), 50, 0, 2);

                if (resultadoTransaccionTablaUsuarioAcceso.ResultadoCodigo == -1)
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionTablaUsuarioAcceso.ResultadoDescripcion;

                    return vResultadoTransaccion;
                }

                if (resultadoTransaccionTablaUsuarioAcceso.dataList.Any())
                {
                    BE_Tabla tablaUsuarioAcceso = ((List<BE_Tabla>)resultadoTransaccionTablaUsuarioAcceso.dataList)[0];

                    if (!tablaUsuarioAcceso.estado.Equals("G"))
                    {
                        vResultadoTransaccion.IdRegistro = -1;
                        vResultadoTransaccion.ResultadoCodigo = -1;
                        vResultadoTransaccion.ResultadoDescripcion = "Usted no tiene permiso para este tipo de Movimiento";

                        return vResultadoTransaccion;
                    }

                }
                else
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = "Usted no tiene permiso para este tipo de Movimiento";

                    return vResultadoTransaccion;
                }

                ResultadoTransaccion<BE_VentasCabecera> resultadoTransaccionValidacionAnularVenta = await ValidacionAnularVenta(value);

                if (resultadoTransaccionValidacionAnularVenta.ResultadoCodigo == -1)
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionValidacionAnularVenta.ResultadoDescripcion;

                    return vResultadoTransaccion;
                }

                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    using (CommittableTransaction transaction = new CommittableTransaction())
                    {
                        // Conexion de Logistica
                        await conn.OpenAsync();
                        conn.EnlistTransaction(transaction);

                        try
                        {
                            // Generar Aanulacion
                            ResultadoTransaccion<bool> resultadoTransaccionAnularVenta = await AnularVenta(conn, value);

                            if (resultadoTransaccionAnularVenta.ResultadoCodigo == -1)
                            {
                                transaction.Rollback();
                                vResultadoTransaccion.IdRegistro = -1;
                                vResultadoTransaccion.ResultadoCodigo = -1;
                                vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionAnularVenta.ResultadoDescripcion;
                                return vResultadoTransaccion;
                            }

                            // Enviar a SAP Anulacion

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            vResultadoTransaccion.IdRegistro = -1;
                            vResultadoTransaccion.ResultadoCodigo = -1;
                            vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                        }
                    }
                }

                vResultadoTransaccion.IdRegistro = 0;
                vResultadoTransaccion.ResultadoCodigo = 0;
                vResultadoTransaccion.ResultadoDescripcion = "Validaciones Correctamente";
            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return vResultadoTransaccion;
        }
        public async Task<ResultadoTransaccion<bool>> AnularVenta(SqlConnection conn, BE_VentasCabecera value)
        {
            ResultadoTransaccion<bool> vResultadoTransaccion = new ResultadoTransaccion<bool>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            //using (SqlConnection connPresotor = new SqlConnection(_cnxClinica))
            //{
            try
            {
                using (SqlCommand cmdActualizarPresotor = new SqlCommand(SP_INSERT_VENTA_ANULAR, conn))
                {
                    cmdActualizarPresotor.Parameters.Clear();
                    cmdActualizarPresotor.CommandType = System.Data.CommandType.StoredProcedure;

                    cmdActualizarPresotor.Parameters.Add(new SqlParameter("@codventa", value.codventa));
                    cmdActualizarPresotor.Parameters.Add(new SqlParameter("@usuario", value.usuario));
                    cmdActualizarPresotor.Parameters.Add(new SqlParameter("@motivoanulacion", value.motivoanulacion));
                    cmdActualizarPresotor.Parameters.Add(new SqlParameter("@RegIdUsuario", value.RegIdUsuario));

                    SqlParameter outputIdTransaccionParam = new SqlParameter("@IdTransaccion", SqlDbType.Int, 3)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmdActualizarPresotor.Parameters.Add(outputIdTransaccionParam);

                    SqlParameter outputMsjTransaccionParam = new SqlParameter("@MsjTransaccion", SqlDbType.VarChar, 700)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmdActualizarPresotor.Parameters.Add(outputMsjTransaccionParam);
                    //await connPresotor.OpenAsync();
                    await cmdActualizarPresotor.ExecuteNonQueryAsync();

                    vResultadoTransaccion.IdRegistro = 0;
                    vResultadoTransaccion.ResultadoCodigo = int.Parse(outputIdTransaccionParam.Value.ToString());
                    vResultadoTransaccion.ResultadoDescripcion = (string)outputMsjTransaccionParam.Value;
                    vResultadoTransaccion.data = true;
                }
            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }
            //}

            return vResultadoTransaccion;
        }
        public async Task<ResultadoTransaccion<bool>> GeneraVentaAutomatica(string codpedido)
        {
            ResultadoTransaccion<bool> vResultadoTransaccion = new ResultadoTransaccion<bool>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                PedidoRepository pedidoRepository = new PedidoRepository(context, _configuration);
                ResultadoTransaccion<BE_Pedido> resultadoTransaccionPedido = await pedidoRepository.GetListaPedidoVentaAutomatica(codpedido);

                if (resultadoTransaccionPedido.ResultadoCodigo == -1)
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionPedido.ResultadoDescripcion;

                    return vResultadoTransaccion;
                }

                if (resultadoTransaccionPedido.dataList.Any())
                {

                    List<BE_Pedido> listPedidos = ((List<BE_Pedido>)resultadoTransaccionPedido.dataList);

                    foreach (BE_Pedido pedido in listPedidos)
                    {

                        ResultadoTransaccion<bool> resultadoTransaccionVentaAutomatica = await VentaAutomatica(pedido);

                        if (resultadoTransaccionVentaAutomatica.ResultadoCodigo == -1)
                        {
                            vResultadoTransaccion.IdRegistro = -1;
                            vResultadoTransaccion.ResultadoCodigo = -1;
                            vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionVentaAutomatica.ResultadoDescripcion;

                            ResultadoTransaccion<bool> resultadoTransaccionVentaPedidoError = await RegistrarVentaPedidoError(pedido.codpedido, resultadoTransaccionVentaAutomatica.ResultadoDescripcion, 1);

                            if (resultadoTransaccionVentaPedidoError.ResultadoCodigo == -1)
                            {
                                vResultadoTransaccion.IdRegistro = -1;
                                vResultadoTransaccion.ResultadoCodigo = -1;
                                vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionVentaPedidoError.ResultadoDescripcion;

                                return vResultadoTransaccion;
                            }
                        }
                    }

                    vResultadoTransaccion.IdRegistro = 0;
                    vResultadoTransaccion.ResultadoCodigo = 0;
                    vResultadoTransaccion.ResultadoDescripcion = "Se generó correctamente";
                    vResultadoTransaccion.data = true;
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
        public async Task<ResultadoTransaccion<bool>> VentaAutomatica(BE_Pedido pedido)
        {
            ResultadoTransaccion<bool> vResultadoTransaccion = new ResultadoTransaccion<bool>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                PacienteRepository pacienteRepository = new PacienteRepository(context, _configuration);
                ResultadoTransaccion<BE_Paciente> resultadoTransaccionPaciente = await pacienteRepository.GetExistenciaPaciente(pedido.codatencion);

                if (resultadoTransaccionPaciente.ResultadoCodigo == -1)
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionPaciente.ResultadoDescripcion;

                    return vResultadoTransaccion;
                }

                if (resultadoTransaccionPaciente.dataList.Any())
                {

                    TipoCambioRepository tipoCambioRepository = new TipoCambioRepository(_clientFactory, _configuration);
                    ResultadoTransaccion<BE_TipoCambio> resultadoTransaccionTipoCambio = await tipoCambioRepository.GetObtieneTipoCambio();

                    if (resultadoTransaccionTipoCambio.ResultadoCodigo == -1)
                    {
                        vResultadoTransaccion.IdRegistro = -1;
                        vResultadoTransaccion.ResultadoCodigo = -1;
                        vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionTipoCambio.ResultadoDescripcion;

                        return vResultadoTransaccion;
                    }

                    BE_TipoCambio tipoCambio = new BE_TipoCambio();

                    if (resultadoTransaccionTipoCambio.dataList.Any())
                    {
                        tipoCambio = ((List<BE_TipoCambio>)resultadoTransaccionTipoCambio.dataList)[0];
                    }


                    BE_Paciente paciente = ((List<BE_Paciente>)resultadoTransaccionPaciente.dataList)[0];

                    BE_VentasCabecera ventasCabecera = new BE_VentasCabecera
                    {
                        codalmacen = pedido.codalmacen,
                        codatencion = paciente.codatencion,
                        codpaciente = paciente.codpaciente,
                        nombre = paciente.nombrepaciente,
                        dircliente = paciente.direccion,
                        codaseguradora = paciente.codaseguradora,
                        nombreaseguradora = paciente.nombreaseguradora,
                        codcia = paciente.codcia,
                        nombrecia = paciente.nombrecontratante,
                        codpoliza = paciente.poliza,
                        planpoliza = paciente.planpoliza,
                        cama = paciente.cama,
                        deducible = paciente.deducible,
                        observacion = paciente.observacionespaciente,
                        tipocambio = tipoCambio.Rate,
                        codmedico = pedido.codmedico,
                        flagpaquete = "N",
                        codpedido = pedido.codpedido,
                        codcentro = pedido.codcentro,
                        codtipocliente = "01",
                        moneda = "S",
                        usuario = "admin",
                        tipomovimiento = "DV",
                        RegIdUsuario = 1
                    };

                    ResultadoTransaccion<string> resultadoTransaccionCama = await GetListaCamaPorAtencion(pedido.codatencion, pedido.codcentro);

                    if (resultadoTransaccionCama.ResultadoCodigo == -1)
                    {
                        vResultadoTransaccion.IdRegistro = -1;
                        vResultadoTransaccion.ResultadoCodigo = -1;
                        vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionCama.ResultadoDescripcion;

                        return vResultadoTransaccion;
                    }

                    string cama = resultadoTransaccionCama.data;

                    ventasCabecera.cama = cama;

                    ResultadoTransaccion<string> resultadoTransaccionTipoProductoPrestacion = new ResultadoTransaccion<string>();

                    if (!string.IsNullOrEmpty(pedido.codprestacion))
                    {
                        resultadoTransaccionTipoProductoPrestacion = await GetListaTipoProductoPrestacionVentaAutomatica(pedido.codprestacion);

                        if (resultadoTransaccionTipoProductoPrestacion.ResultadoCodigo == -1)
                        {
                            vResultadoTransaccion.IdRegistro = -1;
                            vResultadoTransaccion.ResultadoCodigo = -1;
                            vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionTipoProductoPrestacion.ResultadoDescripcion;

                            return vResultadoTransaccion;
                        }

                        ventasCabecera.flagpaquete = "S";

                    }
                    PedidoRepository pedidoRepository = new PedidoRepository(context, _configuration);
                    ResultadoTransaccion<BE_PedidoDetalle> resultadoTransaccionDetallePedido = await pedidoRepository.GetListPedidoDetallePorPedido(pedido.codpedido);

                    if (resultadoTransaccionDetallePedido.ResultadoCodigo == -1)
                    {
                        vResultadoTransaccion.IdRegistro = -1;
                        vResultadoTransaccion.ResultadoCodigo = -1;
                        vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionDetallePedido.ResultadoDescripcion;

                        return vResultadoTransaccion;
                    }

                    PlanesRepository planesRepository = new PlanesRepository(context, _configuration);

                    if (paciente.codaseguradora == "0019")
                    {
                        paciente.codplan = "00000006";
                        ResultadoTransaccion<BE_Planes> planes = await planesRepository.GetbyCodigo(new BE_Planes { CodPlan = "00000006" });

                        if (planes.ResultadoCodigo == -1)
                        {
                            vResultadoTransaccion.IdRegistro = -1;
                            vResultadoTransaccion.ResultadoCodigo = -1;
                            vResultadoTransaccion.ResultadoDescripcion = planes.ResultadoDescripcion;
                            return vResultadoTransaccion;
                        }

                        paciente.porcentajeplan = (decimal)planes.data.PorcentajeDescuento;

                    }
                    else
                    {
                        paciente.codplan = null;
                        paciente.porcentajeplan = 0;
                    }

                    if (resultadoTransaccionDetallePedido.dataList.Any())
                    {
                        List<BE_PedidoDetalle> listPedidoDetalle = ((List<BE_PedidoDetalle>)resultadoTransaccionDetallePedido.dataList);

                        List<BE_VentasDetalle> listVentasDetalles = new List<BE_VentasDetalle>();

                        foreach (BE_PedidoDetalle itemPedido in listPedidoDetalle)
                        {
                            ProductoRepository productoRepository = new ProductoRepository(_clientFactory, _configuration, context);
                            ResultadoTransaccion<BE_Producto> resultadoTransaccionProducto = await productoRepository.GetProductoPorCodigo(pedido.codalmacen, itemPedido.codproducto, paciente.codaseguradora, paciente.codcia, "DV", "01", string.Empty, paciente.codpaciente, paciente.idegctipoatencionmae);

                            if (resultadoTransaccionProducto.dataList.Any())
                            {
                                BE_Producto producto = ((List<BE_Producto>)resultadoTransaccionProducto.dataList)[0];

                                decimal isValorVVP = 0;
                                decimal isValorPVP = 0;
                                string isGrupoArticulo = string.Empty;

                                if (producto.FlgConvenio)
                                {
                                    isValorVVP = producto.valorVVP;
                                    isValorPVP = Math.Round((isValorVVP * (producto.valorIGV / 100 + 1)), 2);
                                }
                                else
                                {
                                    isValorVVP = producto.valorVVP;
                                    isValorPVP = producto.valorPVP;
                                }

                                if (producto.U_SYP_MONART != null)
                                {
                                    if (producto.U_SYP_MONART == "D")
                                    {
                                        isValorPVP = isValorPVP * tipoCambio.Rate;
                                        isValorVVP = isValorVVP * tipoCambio.Rate;
                                    }
                                }

                                switch (producto.ItemsGroupCode)
                                {
                                    case 111:
                                        isGrupoArticulo = "F";
                                        break;
                                    case 132:
                                        isGrupoArticulo = "T";
                                        break;
                                    case 134:
                                        isGrupoArticulo = "X";
                                        break;
                                    case 101:
                                        isGrupoArticulo = "A";
                                        break;
                                    default:
                                        isGrupoArticulo = string.Empty;
                                        break;
                                }

                                if (ventasCabecera.flagpaquete.Equals("S"))
                                {
                                    isGrupoArticulo = resultadoTransaccionTipoProductoPrestacion.data;
                                }

                                //calcula el preciounidadcondcto
                                //decimal valorDescuento = paciente.porcentajeplan;
                                decimal precioUnidadConDcto = isValorVVP - ((paciente.porcentajeplan / 100) * isValorVVP);

                                //calcula montontotaltemp
                                decimal montoTotalTemp = ((decimal)itemPedido.cantidad * precioUnidadConDcto);

                                decimal montoTotal = montoTotalTemp;

                                //calcula monto coaseguro para tipocliente = '01' = Paciente

                                decimal montoCoaseguro = montoTotal * ((decimal)paciente.coaseguro / 100); // Paga Paciente

                                decimal montoSeguro = montoTotal - montoCoaseguro; //Paga la Aseguradora

                                //montontotal con IGV

                                decimal porcentajeIgv = itemPedido.igv / 100;

                                decimal montoTotalIGV = montoTotal * (1 + porcentajeIgv);

                                string gnc = "N";

                                if (itemPedido.codproducto.Equals("00085516") || itemPedido.codproducto.Equals("00089245"))
                                {
                                    if ((paciente.codaseguradora.Equals("0013") || paciente.codaseguradora.Equals("0111")) && (paciente.codatencion.Substring(0, 1).Equals("E") || paciente.codatencion.Substring(0, 1).Equals("H")))
                                    {
                                        gnc = "S";
                                    }
                                }

                                if (!paciente.codaseguradora.Equals("0001") && !paciente.codatencion.Substring(0, 1).Equals("A"))
                                {
                                    gnc = producto.GastoCubierto ? "S" : "N";
                                }

                                if (ventasCabecera.flagpaquete.Equals("S"))
                                {
                                    gnc = "N";
                                }

                                BE_VentasDetalle ventasDetalle = new BE_VentasDetalle
                                {
                                    manBtchNum = producto.manbtchnum,
                                    codalmacen = pedido.codalmacen,
                                    tipomovimiento = "DV",
                                    codproducto = producto.ItemCode,
                                    nombreproducto = producto.ItemName,
                                    cantidad = (int)itemPedido.cantidad,
                                    precioventaPVP = isValorPVP,
                                    valorVVP = isValorVVP,
                                    porcentajedctoproducto = 0,
                                    porcentajedctoplan = paciente.porcentajeplan,
                                    montototal = montoTotal,
                                    montopaciente = montoCoaseguro,
                                    montoaseguradora = montoSeguro,
                                    codpedido = pedido.codpedido,
                                    gnc = gnc,
                                    codtipoproducto = isGrupoArticulo,
                                    preciounidadcondcto = precioUnidadConDcto,
                                    igvproducto = itemPedido.igv,
                                    narcotico = producto.Narcotico,
                                    flgbtchnum = false,
                                    stockfraccion = producto.U_SYP_CS_FRVTA == null ? 0 : (int)producto.U_SYP_CS_FRVTA,
                                    totalconigv = montoTotalIGV,
                                    totalsinigv = montoTotal
                                };

                                listVentasDetalles.Add(ventasDetalle);

                            }
                        }

                        ventasCabecera.listaVentaDetalle = listVentasDetalles;
                    }

                    ResultadoTransaccion<BE_VentasGenerado> newVenta = await RegistrarVentaCabecera(ventasCabecera, true);

                    if (newVenta.ResultadoCodigo == -1)
                    {
                        vResultadoTransaccion.IdRegistro = -1;
                        vResultadoTransaccion.ResultadoCodigo = -1;
                        vResultadoTransaccion.ResultadoDescripcion = newVenta.ResultadoDescripcion;
                        return vResultadoTransaccion;
                    }

                }

                vResultadoTransaccion.IdRegistro = 0;
                vResultadoTransaccion.ResultadoCodigo = 0;
                vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", 1);
                vResultadoTransaccion.data = true;

            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return vResultadoTransaccion;
        }
        public async Task<ResultadoTransaccion<string>> GetListaCamaPorAtencion(string codatencion, string codcentro)
        {
            ResultadoTransaccion<string> vResultadoTransaccion = new ResultadoTransaccion<string>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    using (SqlCommand cmdGncPorVenta = new SqlCommand(SP_GET_CAMA_POR_ATENCION, conn))
                    {
                        cmdGncPorVenta.CommandType = System.Data.CommandType.StoredProcedure;
                        cmdGncPorVenta.Parameters.Add(new SqlParameter("@codatencion", codatencion));
                        cmdGncPorVenta.Parameters.Add(new SqlParameter("@codcentro", codcentro));

                        string response = string.Empty;

                        await conn.OpenAsync();

                        using (var reader = await cmdGncPorVenta.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response = ((reader["cama"]) is DBNull) ? string.Empty : (string)reader["cama"];
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
        public async Task<ResultadoTransaccion<string>> GetListaTipoProductoPrestacionVentaAutomatica(string pcodprestacionpaq)
        {
            ResultadoTransaccion<string> vResultadoTransaccion = new ResultadoTransaccion<string>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    using (SqlCommand cmdGncPorVenta = new SqlCommand(SP_GET_TIPOPRODUCTO_PRESTACION, conn))
                    {
                        cmdGncPorVenta.CommandType = System.Data.CommandType.StoredProcedure;
                        cmdGncPorVenta.Parameters.Add(new SqlParameter("@pcodprestacionpaq", pcodprestacionpaq));

                        string response = string.Empty;

                        await conn.OpenAsync();

                        using (var reader = await cmdGncPorVenta.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response = ((reader["codtipoproducto"]) is DBNull) ? string.Empty : (string)reader["codtipoproducto"];
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
        public async Task<ResultadoTransaccion<bool>> RegistrarVentaPedidoError(string codpedido, string observacion, int RegIdUsuario)
        {
            ResultadoTransaccion<bool> vResultadoTransaccion = new ResultadoTransaccion<bool>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            using (SqlConnection conn = new SqlConnection(_cnx))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(SP_INSERT_VENTA_PEDIDO_ERROR, conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@codpedido", codpedido));
                        cmd.Parameters.Add(new SqlParameter("@observacion", observacion));
                        cmd.Parameters.Add(new SqlParameter("@RegIdUsuario", RegIdUsuario));

                        SqlParameter outputIdTransaccionParam = new SqlParameter("@IdTransaccion", SqlDbType.Int, 3)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputIdTransaccionParam);

                        SqlParameter outputMsjTransaccionParam = new SqlParameter("@MsjTransaccion", SqlDbType.VarChar, 700)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputMsjTransaccionParam);
                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = int.Parse(outputIdTransaccionParam.Value.ToString());
                        vResultadoTransaccion.ResultadoDescripcion = (string)outputMsjTransaccionParam.Value;
                        vResultadoTransaccion.data = true;
                    }
                }
                catch (Exception ex)
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                }
            }

            return vResultadoTransaccion;
        }
        public async Task<ResultadoTransaccion<MemoryStream>> GenerarValeVentaPrint(string codventa)
        {

            ResultadoTransaccion<MemoryStream> vResultadoTransaccion = new ResultadoTransaccion<MemoryStream>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                Document doc = new Document();
                doc.SetPageSize(PageSize.Letter);
                // points to cm
                doc.SetMargins(20f, 20f, 15f, 15f);
                MemoryStream ms = new MemoryStream();
                PdfWriter write = PdfWriter.GetInstance(doc, ms);
                doc.AddAuthor("Grupo SBA");
                doc.AddTitle("Cliníca San Felipe");
                var pe = new PageEventHelper();
                write.PageEvent = pe;
                // Colocamos la fuente que deseamos que tenga el documento
                BaseFont helvetica = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
                // Titulo
                iTextSharp.text.Font titulo = new iTextSharp.text.Font(helvetica, 14f, iTextSharp.text.Font.BOLD, BaseColor.Black);
                iTextSharp.text.Font subTitulo = new iTextSharp.text.Font(helvetica, 12f, iTextSharp.text.Font.BOLD, BaseColor.Black);
                iTextSharp.text.Font parrafoNegroNegrita = new iTextSharp.text.Font(helvetica, 10f, iTextSharp.text.Font.BOLD, BaseColor.Black);
                iTextSharp.text.Font parrafoNegro = new iTextSharp.text.Font(helvetica, 10f, iTextSharp.text.Font.NORMAL, BaseColor.Black);
                pe.HeaderLeft = " ";
                pe.HeaderFont = parrafoNegroNegrita;
                pe.HeaderRight = " ";
                doc.Open();

                var tbl = new PdfPTable(new float[] { 30f, 40f, 30f }) { WidthPercentage = 100 };

                var title = string.Format("VENTAS Nro {0}", codventa, titulo);

                var c1 = new PdfPCell(new Phrase("Cliníca San Felipe S.A.", parrafoNegro)) { Border = 0 };
                c1.HorizontalAlignment = Element.ALIGN_LEFT;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase(title, titulo)) { Border = 0 };
                c1.HorizontalAlignment = Element.ALIGN_CENTER;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbl.AddCell(c1);
                
                c1 = new PdfPCell(new Phrase(DateTime.Now.ToString(), parrafoNegro)) { Border = 0 };
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                c1.HorizontalAlignment = Element.ALIGN_RIGHT;
                tbl.AddCell(c1);
                doc.Add(tbl);

                doc.Add(new Phrase(" "));
                //Obtenemos los datos de la venta

                ResultadoTransaccion<BE_VentasCabecera> resultadoTransaccionVenta = await GetVentaPorCodVenta(codventa);

                if (resultadoTransaccionVenta.ResultadoCodigo == -1)
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionVenta.ResultadoDescripcion;

                    return vResultadoTransaccion;
                }

                // Generamos la Cabecera del vale de venta
                tbl = new PdfPTable(new float[] { 12f, 45f, 3f, 12f, 28f }) { WidthPercentage = 100 };
                //Linea 1
                c1 = new PdfPCell(new Phrase("Nombre", parrafoNegroNegrita)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(string.Format(": {0}", resultadoTransaccionVenta.data.nombre), parrafoNegro)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("", parrafoNegro)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("Plan", parrafoNegroNegrita)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(string.Format(": {0}", resultadoTransaccionVenta.data.porcentajedctoplan.ToString()), parrafoNegro)) { Border = 0 };
                tbl.AddCell(c1);

                //Linea 2
                c1 = new PdfPCell(new Phrase("Atención", parrafoNegroNegrita)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(string.Format(": {0}", resultadoTransaccionVenta.data.codatencion), parrafoNegro)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("", parrafoNegro)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("Aseguradora", parrafoNegroNegrita)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(string.Format(": {0}", resultadoTransaccionVenta.data.nombreaseguradora), parrafoNegro)) { Border = 0 };
                tbl.AddCell(c1);

                //Linea 3
                c1 = new PdfPCell(new Phrase("H. Cliníca", parrafoNegroNegrita)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(string.Format(": {0}", resultadoTransaccionVenta.data.codpaciente), parrafoNegro)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("", parrafoNegro)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("Coaseguro", parrafoNegroNegrita)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(string.Format(": {0}", resultadoTransaccionVenta.data.porcentajecoaseguro.ToString()), parrafoNegro)) { Border = 0 };
                tbl.AddCell(c1);

                //Linea 4
                c1 = new PdfPCell(new Phrase("Médico", parrafoNegroNegrita)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(string.Format(": {0}", resultadoTransaccionVenta.data.nombremedico), parrafoNegro)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("", parrafoNegro)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("Cama", parrafoNegroNegrita)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(string.Format(": {0}", resultadoTransaccionVenta.data.cama), parrafoNegro)) { Border = 0 };
                tbl.AddCell(c1);

                //Linea 5
                c1 = new PdfPCell(new Phrase("Observación", parrafoNegroNegrita)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(string.Format(": {0}", resultadoTransaccionVenta.data.observacion), parrafoNegro)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("", parrafoNegro)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("F.Emisión", parrafoNegroNegrita)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(string.Format(": {0}", resultadoTransaccionVenta.data.fechaemision.ToString()), parrafoNegro)) { Border = 0 };
                tbl.AddCell(c1);
                
                //Linea 6
                c1 = new PdfPCell(new Phrase("", parrafoNegroNegrita)) { Border = 0 };
                c1.Colspan = 3;
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("Paquete", parrafoNegroNegrita)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(string.Format(": {0}", ""), parrafoNegro)) { Border = 0 };
                tbl.AddCell(c1);
                doc.Add(tbl);

                doc.Add(new Phrase(" "));

                // Generamos el detalle del vale de venta
                tbl = new PdfPTable(new float[] { 30f, 15f, 8f, 8f, 8f, 8f, 8f }) { WidthPercentage = 100 };
                c1 = new PdfPCell(new Phrase("Producto", parrafoNegroNegrita));
                c1.BorderWidth = 1;
                c1.DisableBorderSide(Rectangle.LEFT_BORDER);
                c1.DisableBorderSide(Rectangle.RIGHT_BORDER);
                c1.DisableBorderSide(Rectangle.TOP_BORDER);
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("Lab.", parrafoNegroNegrita));
                c1.BorderWidth = 1;
                c1.DisableBorderSide(Rectangle.LEFT_BORDER);
                c1.DisableBorderSide(Rectangle.RIGHT_BORDER);
                c1.DisableBorderSide(Rectangle.TOP_BORDER);
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("P.Unit", parrafoNegroNegrita));
                c1.BorderWidth = 1;
                c1.DisableBorderSide(Rectangle.LEFT_BORDER);
                c1.DisableBorderSide(Rectangle.RIGHT_BORDER);
                c1.DisableBorderSide(Rectangle.TOP_BORDER);
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("Cant.", parrafoNegroNegrita));
                c1.BorderWidth = 1;
                c1.DisableBorderSide(Rectangle.LEFT_BORDER);
                c1.DisableBorderSide(Rectangle.RIGHT_BORDER);
                c1.DisableBorderSide(Rectangle.TOP_BORDER);
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("D.Prod.", parrafoNegroNegrita));
                c1.BorderWidth = 1;
                c1.DisableBorderSide(Rectangle.LEFT_BORDER);
                c1.DisableBorderSide(Rectangle.RIGHT_BORDER);
                c1.DisableBorderSide(Rectangle.TOP_BORDER);
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("D.Plan", parrafoNegroNegrita));
                c1.BorderWidth = 1;
                c1.DisableBorderSide(Rectangle.LEFT_BORDER);
                c1.DisableBorderSide(Rectangle.RIGHT_BORDER);
                c1.DisableBorderSide(Rectangle.TOP_BORDER);
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("Valor", parrafoNegroNegrita));
                c1.BorderWidth = 1;
                c1.DisableBorderSide(Rectangle.LEFT_BORDER);
                c1.DisableBorderSide(Rectangle.RIGHT_BORDER);
                c1.DisableBorderSide(Rectangle.TOP_BORDER);
                tbl.AddCell(c1);

                foreach (BE_VentasDetalle item in resultadoTransaccionVenta.data.listaVentaDetalle)
                {
                    c1 = new PdfPCell(new Phrase(item.nombreproducto, parrafoNegro)) { Border = 0};
                    tbl.AddCell(c1);
                    c1 = new PdfPCell(new Phrase("", parrafoNegro)) { Border = 0 };
                    tbl.AddCell(c1);
                    c1 = new PdfPCell(new Phrase(item.preciounidadcondcto.ToString(), parrafoNegro)) { Border = 0 };
                    tbl.AddCell(c1);
                    c1 = new PdfPCell(new Phrase(item.cantidad.ToString(), parrafoNegro)) { Border = 0 };
                    tbl.AddCell(c1);
                    c1 = new PdfPCell(new Phrase(item.porcentajedctoproducto.ToString(), parrafoNegro)) { Border = 0 };
                    tbl.AddCell(c1);
                    c1 = new PdfPCell(new Phrase(item.porcentajedctoplan.ToString(), parrafoNegro)) { Border = 0 };
                    tbl.AddCell(c1);
                    c1 = new PdfPCell(new Phrase(item.montototal.ToString(), parrafoNegro)) { Border = 0 };
                    tbl.AddCell(c1);
                }
                doc.Add(tbl);

                doc.Add(new Phrase(" "));
                doc.Add(new Phrase(" "));
                // Totales
                tbl = new PdfPTable(new float[] { 70f, 15f, 15f }) { WidthPercentage = 100 };
                c1 = new PdfPCell(new Phrase("NO INCLUYE IGV", parrafoNegroNegrita));
                c1.BorderWidth = 1;
                c1.DisableBorderSide(Rectangle.LEFT_BORDER);
                c1.DisableBorderSide(Rectangle.RIGHT_BORDER);
                c1.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("Total :", parrafoNegroNegrita));
                c1.BorderWidth = 1;
                c1.DisableBorderSide(Rectangle.LEFT_BORDER);
                c1.DisableBorderSide(Rectangle.RIGHT_BORDER);
                c1.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(resultadoTransaccionVenta.data.montototal.ToString(), parrafoNegroNegrita));
                c1.BorderWidth = 1;
                c1.DisableBorderSide(Rectangle.LEFT_BORDER);
                c1.DisableBorderSide(Rectangle.RIGHT_BORDER);
                c1.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(resultadoTransaccionVenta.data.usuario, parrafoNegroNegrita)) { Border = 0};
                c1.Colspan = 3;
                tbl.AddCell(c1);
                doc.Add(tbl);

                doc.Add(new Phrase(" "));
                doc.Add(new Phrase(" "));

                tbl = new PdfPTable(new float[] { 25f, 25f, 25f, 25f }) { WidthPercentage = 100 };
                c1 = new PdfPCell(new Phrase("Vendedor", parrafoNegroNegrita));
                c1.BorderWidth = 1;
                c1.HorizontalAlignment = Element.ALIGN_CENTER;
                c1.DisableBorderSide(Rectangle.LEFT_BORDER);
                c1.DisableBorderSide(Rectangle.RIGHT_BORDER);
                c1.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("Personal de Reparto", parrafoNegroNegrita));
                c1.BorderWidth = 1;
                c1.HorizontalAlignment = Element.ALIGN_CENTER;
                c1.DisableBorderSide(Rectangle.LEFT_BORDER);
                c1.DisableBorderSide(Rectangle.RIGHT_BORDER);
                c1.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("Personal Recepciona", parrafoNegroNegrita));
                c1.BorderWidth = 1;
                c1.HorizontalAlignment = Element.ALIGN_CENTER;
                c1.DisableBorderSide(Rectangle.LEFT_BORDER);
                c1.DisableBorderSide(Rectangle.RIGHT_BORDER);
                c1.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("Paciente", parrafoNegroNegrita));
                c1.BorderWidth = 1;
                c1.HorizontalAlignment = Element.ALIGN_CENTER;
                c1.DisableBorderSide(Rectangle.LEFT_BORDER);
                c1.DisableBorderSide(Rectangle.RIGHT_BORDER);
                c1.DisableBorderSide(Rectangle.BOTTOM_BORDER);
                tbl.AddCell(c1);
                doc.Add(tbl);

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
        public async Task<ResultadoTransaccion<MemoryStream>> GenerarValeVentaLotePrint(string codventa)
        {

            ResultadoTransaccion<MemoryStream> vResultadoTransaccion = new ResultadoTransaccion<MemoryStream>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                Document doc = new Document();
                doc.SetPageSize(PageSize.Letter);
                // points to cm
                doc.SetMargins(20f, 20f, 15f, 15f);
                MemoryStream ms = new MemoryStream();
                PdfWriter write = PdfWriter.GetInstance(doc, ms);
                doc.AddAuthor("Grupo SBA");
                doc.AddTitle("Cliníca San Felipe");
                var pe = new PageEventHelper();
                write.PageEvent = pe;
                // Colocamos la fuente que deseamos que tenga el documento
                BaseFont helvetica = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1250, true);
                // Titulo
                iTextSharp.text.Font titulo = new iTextSharp.text.Font(helvetica, 14f, iTextSharp.text.Font.BOLD, BaseColor.Black);
                iTextSharp.text.Font subTitulo = new iTextSharp.text.Font(helvetica, 12f, iTextSharp.text.Font.BOLD, BaseColor.Black);
                iTextSharp.text.Font parrafoNegroNegrita = new iTextSharp.text.Font(helvetica, 10f, iTextSharp.text.Font.BOLD, BaseColor.Black);
                iTextSharp.text.Font parrafoNegro = new iTextSharp.text.Font(helvetica, 10f, iTextSharp.text.Font.NORMAL, BaseColor.Black);
                pe.HeaderLeft = " ";
                pe.HeaderFont = parrafoNegroNegrita;
                pe.HeaderRight = " ";
                doc.Open();

                var tbl = new PdfPTable(new float[] { 30f, 40f, 30f }) { WidthPercentage = 100 };

                var title = string.Format("VENTAS Nro {0}", codventa, titulo);

                var c1 = new PdfPCell(new Phrase("Cliníca San Felipe S.A.", parrafoNegro)) { Border = 0 };
                c1.HorizontalAlignment = Element.ALIGN_LEFT;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase(title, titulo)) { Border = 0 };
                c1.HorizontalAlignment = Element.ALIGN_CENTER;
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                tbl.AddCell(c1);

                c1 = new PdfPCell(new Phrase(DateTime.Now.ToString(), parrafoNegro)) { Border = 0 };
                c1.VerticalAlignment = Element.ALIGN_MIDDLE;
                c1.HorizontalAlignment = Element.ALIGN_RIGHT;
                tbl.AddCell(c1);
                doc.Add(tbl);

                doc.Add(new Phrase(" "));
                //Obtenemos los datos de la venta

                ResultadoTransaccion<BE_VentasCabecera> resultadoTransaccionVenta = await GetVentaPorCodVenta(codventa);

                if (resultadoTransaccionVenta.ResultadoCodigo == -1)
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionVenta.ResultadoDescripcion;

                    return vResultadoTransaccion;
                }

                // Generamos la Cabecera del vale de venta
                tbl = new PdfPTable(new float[] { 15f, 75f }) { WidthPercentage = 100 };
                //Linea 1
                c1 = new PdfPCell(new Phrase("ALMACEN", parrafoNegroNegrita)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(string.Format(": {0}", resultadoTransaccionVenta.data.nombrealmacen), parrafoNegro)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("FECHA MOV", parrafoNegroNegrita)) { Border = 0 };
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase(string.Format(": {0}", resultadoTransaccionVenta.data.fechaemision.ToString()), parrafoNegro)) { Border = 0 };
                tbl.AddCell(c1);
                doc.Add(tbl);

                doc.Add(new Phrase(" "));

                // Generamos el detalle del vale de venta
                tbl = new PdfPTable(new float[] { 40f, 20f, 10f, 15f, 15f }) { WidthPercentage = 100 };
                c1 = new PdfPCell(new Phrase("Producto", parrafoNegroNegrita));
                c1.BorderWidth = 1;
                c1.DisableBorderSide(Rectangle.LEFT_BORDER);
                c1.DisableBorderSide(Rectangle.RIGHT_BORDER);
                c1.DisableBorderSide(Rectangle.TOP_BORDER);
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("Lab.", parrafoNegroNegrita));
                c1.BorderWidth = 1;
                c1.DisableBorderSide(Rectangle.LEFT_BORDER);
                c1.DisableBorderSide(Rectangle.RIGHT_BORDER);
                c1.DisableBorderSide(Rectangle.TOP_BORDER);
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("Cant.", parrafoNegroNegrita));
                c1.BorderWidth = 1;
                c1.DisableBorderSide(Rectangle.LEFT_BORDER);
                c1.DisableBorderSide(Rectangle.RIGHT_BORDER);
                c1.DisableBorderSide(Rectangle.TOP_BORDER);
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("Lote", parrafoNegroNegrita));
                c1.BorderWidth = 1;
                c1.DisableBorderSide(Rectangle.LEFT_BORDER);
                c1.DisableBorderSide(Rectangle.RIGHT_BORDER);
                c1.DisableBorderSide(Rectangle.TOP_BORDER);
                tbl.AddCell(c1);
                c1 = new PdfPCell(new Phrase("F. Vencimiento", parrafoNegroNegrita));
                c1.BorderWidth = 1;
                c1.DisableBorderSide(Rectangle.LEFT_BORDER);
                c1.DisableBorderSide(Rectangle.RIGHT_BORDER);
                c1.DisableBorderSide(Rectangle.TOP_BORDER);
                tbl.AddCell(c1);

                foreach (BE_VentasDetalle item in resultadoTransaccionVenta.data.listaVentaDetalle)
                {
                    if (item.listVentasDetalleLotes != null)
                    {
                        if (item.listVentasDetalleLotes.Any())
                        {
                            foreach (var itemLote in item.listVentasDetalleLotes)
                            {
                                c1 = new PdfPCell(new Phrase(item.nombreproducto, parrafoNegro)) { Border = 0 };
                                tbl.AddCell(c1);
                                c1 = new PdfPCell(new Phrase("", parrafoNegro)) { Border = 0 };
                                tbl.AddCell(c1);
                                c1 = new PdfPCell(new Phrase(itemLote.cantidad.ToString(), parrafoNegro)) { Border = 0 };
                                tbl.AddCell(c1);
                                c1 = new PdfPCell(new Phrase(itemLote.lote, parrafoNegro)) { Border = 0 };
                                tbl.AddCell(c1);
                                c1 = new PdfPCell(new Phrase(((DateTime)itemLote.fechavencimiento).ToShortDateString(), parrafoNegro)) { Border = 0 };
                                tbl.AddCell(c1);
                            }
                        }
                    }
                }
                doc.Add(tbl);

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
        public async Task<ResultadoTransaccion<bool>> UpdateSAPVenta(BE_VentasCabecera value)
        {
            ResultadoTransaccion<bool> vResultadoTransaccion = new ResultadoTransaccion<bool>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            using (SqlConnection conn = new SqlConnection(_cnx))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(SP_UPDATE_CABECERA_SAP, conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@codventa", value.codventa));
                        cmd.Parameters.Add(new SqlParameter("@ide_docentrysap", value.ide_docentrysap));
                        cmd.Parameters.Add(new SqlParameter("@fec_docentrysap", value.fec_docentrysap));
                        cmd.Parameters.Add(new SqlParameter("@RegIdUsuario", value.RegIdUsuario));

                        SqlParameter outputIdTransaccionParam = new SqlParameter("@IdTransaccion", SqlDbType.Int, 3)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputIdTransaccionParam);

                        SqlParameter outputMsjTransaccionParam = new SqlParameter("@MsjTransaccion", SqlDbType.VarChar, 700)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputMsjTransaccionParam);
                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = int.Parse(outputIdTransaccionParam.Value.ToString());
                        vResultadoTransaccion.ResultadoDescripcion = (string)outputMsjTransaccionParam.Value;
                        vResultadoTransaccion.data = true;
                    }
                }
                catch (Exception ex)
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                }
            }

            return vResultadoTransaccion;
        }
        public async Task<ResultadoTransaccion<bool>> UpdateSinStockVenta(BE_VentasCabecera value)
        {
            ResultadoTransaccion<bool> vResultadoTransaccion = new ResultadoTransaccion<bool>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            using (SqlConnection conn = new SqlConnection(_cnx))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(SP_UPDATE_CABECERA_SIN_STOCK, conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@codventa", value.codventa));
                        cmd.Parameters.Add(new SqlParameter("@RegIdUsuario", value.RegIdUsuario));

                        SqlParameter outputIdTransaccionParam = new SqlParameter("@IdTransaccion", SqlDbType.Int, 3)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputIdTransaccionParam);

                        SqlParameter outputMsjTransaccionParam = new SqlParameter("@MsjTransaccion", SqlDbType.VarChar, 700)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputMsjTransaccionParam);
                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = int.Parse(outputIdTransaccionParam.Value.ToString());
                        vResultadoTransaccion.ResultadoDescripcion = (string)outputMsjTransaccionParam.Value;
                        vResultadoTransaccion.data = true;
                    }
                }
                catch (Exception ex)
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                }
            }

            return vResultadoTransaccion;
        }
        public async Task<ResultadoTransaccion<BE_VentasGenerado>> RegistrarVentaDevolucion(BE_VentaXml value)
        {
            ResultadoTransaccion<BE_VentasGenerado> vResultadoTransaccion = new ResultadoTransaccion<BE_VentasGenerado>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            List<BE_VentasGenerado> ventasGenerados = new List<BE_VentasGenerado>();

            using (SqlConnection conn = new SqlConnection(_cnx))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(SP_INSERT_DEVOLUCION, conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        SqlParameter oParam = new SqlParameter("@codventa", SqlDbType.Char, 8)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(oParam);

                        cmd.Parameters.Add(new SqlParameter("@xmldata", value.XmlData));
                        cmd.Parameters.Add(new SqlParameter("@RegIdUsuario", value.RegIdUsuario));
                        SqlParameter outputIdTransaccionParam = new SqlParameter("@IdTransaccion", SqlDbType.Int, 3)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputIdTransaccionParam);

                        SqlParameter outputMsjTransaccionParam = new SqlParameter("@MsjTransaccion", SqlDbType.VarChar, 700)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputMsjTransaccionParam);
                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        ventasGenerados.Add( new BE_VentasGenerado { codventa = (string)oParam.Value, codpresotor = string.Empty });

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = int.Parse(outputIdTransaccionParam.Value.ToString());
                        vResultadoTransaccion.ResultadoDescripcion = (string)outputMsjTransaccionParam.Value;
                        vResultadoTransaccion.dataList = ventasGenerados;
                    }
                }
                catch (Exception ex)
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                }
            }

            return vResultadoTransaccion;
        }
    }
}
