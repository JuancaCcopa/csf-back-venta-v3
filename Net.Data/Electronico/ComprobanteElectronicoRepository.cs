using Net.Business.Entities;
using Net.Connection;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using Net.TCI;

namespace Net.Data
{
    public class ComprobanteElectronicoRepository : RepositoryBase<BE_TCIComprobanteElectronico>, IComprobanteElectronicoRepository
    {
        private readonly string _cnx;
        private string _aplicacionName;
        private string _metodoName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        private readonly IConfiguration _configuration;
        const string DB_ESQUEMA = "";
        const string SP_GET = DB_ESQUEMA + "Sp_ComprobantesElectronicos_ConsultaVB";
        const string SP_GET_COMPROBANTEELECTRONICO = DB_ESQUEMA + "VEN_ComprobantesElectronicosLogXmlGet";
        const string SP_GET_NOTAELECTRONICO = DB_ESQUEMA + "VEN_NotaElectronicaLogXmlGet";
        const string SP_UPD_COMPROBANTEELECTRONICO = DB_ESQUEMA + "VEN_ComprobantesElectronicosUpd";

        public ComprobanteElectronicoRepository(IConnectionSQL context, IConfiguration configuration)
           : base(context)
        {
            _cnx = configuration.GetConnectionString("cnnSqlClinica");
            _aplicacionName = this.GetType().Name;
            _configuration = configuration;
        }

        public async Task<ResultadoTransaccion<BE_TCIComprobanteElectronico>> GetListComprobanteElectronicoPorFiltro(string codempresa, string codcomprobante, string codcomprobante_e, string codsistema, string tipocomp_sunat, int orden)
        {
            ResultadoTransaccion<BE_TCIComprobanteElectronico> vResultadoTransaccion = new ResultadoTransaccion<BE_TCIComprobanteElectronico>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {

                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_GET, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codempresa", codempresa));
                        cmd.Parameters.Add(new SqlParameter("@codcomprobante", codcomprobante));
                        cmd.Parameters.Add(new SqlParameter("@codcomprobante_e", codcomprobante_e));
                        cmd.Parameters.Add(new SqlParameter("@codsistema", codsistema));
                        cmd.Parameters.Add(new SqlParameter("@tipocomp_sunat", tipocomp_sunat));
                        cmd.Parameters.Add(new SqlParameter("@orden", orden));

                        var response = new List<BE_TCIComprobanteElectronico>();

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_TCIComprobanteElectronico>)context.ConvertTo<BE_TCIComprobanteElectronico>(reader);
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
        public async Task<int> Registrar(string codcomprobante, string tipoCodigo_BarraHash, string tipoOtorgamiento, string Xml)
        {
            string wCuadre;
            string wCodComprobanteE;
            string wTipoComp_TCI;
            string wTipoCodigo;
            string wOtorgar;
            string[] respuesta = new string[1];
            string secuencia = "\r";

            wCodComprobanteE = "";
            wTipoComp_TCI = "";
            wTipoCodigo = tipoCodigo_BarraHash; // '"1" 'Se envía "0" si se desea obtener el código de barras; se envía "1" si se desea obtener el código hash.
            wOtorgar = tipoOtorgamiento;        //'"1" 'Se envía "0" si se otorgará manualmente, se envía "1" si se otorgará automáticamente
            wCuadre = "";

            //Datos de comprobante
            ResultadoTransaccion<BE_TCIComprobanteElectronico> vResultadoTransaccion = new ResultadoTransaccion<BE_TCIComprobanteElectronico>();

            if (codcomprobante.Substring(0, 1) == "F" | codcomprobante.Substring(0, 1) == "B")
            {
                vResultadoTransaccion = await GetComprobantesElectronicosXml("", "", "", 0);
            }
            else if (codcomprobante.Substring(0, 1) == "C" | codcomprobante.Substring(0, 1) == "D")
            {
                vResultadoTransaccion = await GetNotaElectronicaLogXml("", 0);
            }

            if (vResultadoTransaccion.IdRegistro == -1)
            {
                Xml = "";
                return -4; //'Error en Stored
            }

            List<BE_TCIComprobanteElectronico> response = (List<BE_TCIComprobanteElectronico>)vResultadoTransaccion.dataList;

            if (response.Count > 0)
            {
                wCodComprobanteE = response[0].ref_codcomprobantee;
                wTipoComp_TCI = response[0].tipocomp_tci; //'Regla_TCI: "Factura", "Boleta"
                wCuadre = response[0].cuadre;
                if (wCuadre == "")
                {
                    Xml = "";
                    return -1; //'Detalle tiene Negativo
                }
            }
            else
            {
                Xml = "";
                return -2; //'Comprobante no existe o No hay detalle
            }

            //' ------------------- Invocación del Web Service
            string XML_Cabecera;

            FacturacionElectronica _facturacionElectronica = new FacturacionElectronica();

            XML_Cabecera = _facturacionElectronica.GetComprobateElectronicoXml(response);

            if (XML_Cabecera != "")
            {
                //'I-TCI
                string strSOAPAction;
                string strURL;
                string Metodo;

                strURL = "http://200.106.52.10/WS_TCI/Service.asmx?wsdl";
                strSOAPAction = "http://tempuri.org/Registrar";

                //'I-Ali-csf
                Metodo = "";
                Metodo = Metodo + "<?xml version= " + "1.0" + " encoding= " + "utf-8" + "?> " + secuencia;
                Metodo = Metodo + "<soap:Envelope xmlns:xsi=" + "http://www.w3.org/2001/XMLSchema-instance" + " xmlns:xsd=" + "http://www.w3.org/2001/XMLSchema" + " ";
                Metodo = Metodo + " xmlns:soap=" + "http://schemas.xmlsoap.org/soap/envelope/" + ">" + secuencia +
                        "<soap:Body>" + secuencia +
                            "<Registrar xmlns=" + "http://tempuri.org/" + ">" + secuencia +
                                "<oGeneral>" + secuencia + XML_Cabecera + "</oGeneral>" + secuencia +
                                "<oTipoComprobante>" + wTipoComp_TCI + "</oTipoComprobante>" + secuencia +
                                "<Cadena></Cadena>" + secuencia +
                                "<TipoCodigo>" + wTipoCodigo + "</TipoCodigo>" + secuencia +
                                "<CogigoBarras></CogigoBarras>" + secuencia +
                                "<CogigoHash></CogigoHash>" + secuencia +
                                "<Otorgar>" + wOtorgar + "</Otorgar>" + secuencia +
                                "<IdComprobanteCliente>0</IdComprobanteCliente>" + secuencia +
                            "</Registrar>" + secuencia +
                        "</soap:Body>" + secuencia +
                    "</soap:Envelope>";
                //'F-Ali-csf
                Xml = Metodo;
            }
            else
            {
                Xml = "";
                return -3; // 'Error al construir XML
            }

            return 0;
        }


        private async Task<ResultadoTransaccion<BE_TCIComprobanteElectronico>> GetComprobantesElectronicosXml(string codcomprobante, string xcoduser, string maquina, int orden)
        {
            ResultadoTransaccion<BE_TCIComprobanteElectronico> vResultadoTransaccion = new ResultadoTransaccion<BE_TCIComprobanteElectronico>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {

                    using (SqlCommand cmd = new SqlCommand(SP_GET_COMPROBANTEELECTRONICO, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codcomprobante", codcomprobante));
                        cmd.Parameters.Add(new SqlParameter("@xcoduser", xcoduser));
                        cmd.Parameters.Add(new SqlParameter("@maquina", maquina));
                        cmd.Parameters.Add(new SqlParameter("@orden", orden));

                        conn.Open();

                        var response = new List<BE_TCIComprobanteElectronico>();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_TCIComprobanteElectronico>)context.ConvertTo<BE_TCIComprobanteElectronico>(reader);
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

        private async Task<ResultadoTransaccion<BE_TCIComprobanteElectronico>> GetNotaElectronicaLogXml(string codnota, int orden)
        {
            ResultadoTransaccion<BE_TCIComprobanteElectronico> vResultadoTransaccion = new ResultadoTransaccion<BE_TCIComprobanteElectronico>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {

                    using (SqlCommand cmd = new SqlCommand(SP_GET_NOTAELECTRONICO, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codnota", codnota));
                        cmd.Parameters.Add(new SqlParameter("@orden", orden));

                        conn.Open();

                        var response = new List<BE_TCIComprobanteElectronico>();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_TCIComprobanteElectronico>)context.ConvertTo<BE_TCIComprobanteElectronico>(reader);
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

        public async Task<ResultadoTransaccion<BE_TCIComprobanteElectronico>> ModificarComprobanteElectronico(string campo, string nuevoValor, string XML, string codigo)
        {
            ResultadoTransaccion<BE_TCIComprobanteElectronico> vResultadoTransaccion = new ResultadoTransaccion<BE_TCIComprobanteElectronico>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {

                    using (SqlCommand cmd = new SqlCommand(SP_UPD_COMPROBANTEELECTRONICO, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@campo", campo));
                        cmd.Parameters.Add(new SqlParameter("@nuevovalor", nuevoValor));
                        cmd.Parameters.Add(new SqlParameter("@XML", XML));
                        cmd.Parameters.Add(new SqlParameter("@codigo", codigo));

                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.ResultadoDescripcion = "Comprobante electrónico procesado con éxito.";
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

    }
}
