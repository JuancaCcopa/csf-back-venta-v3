using Net.Business.Entities;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Net.Http;
using Net.Connection.ServiceLayer;
using System.Text.RegularExpressions;
using System;

namespace Net.Data
{
    public class GenericoRepository : IGenericoRepository
    {
        private string _aplicacionName;
        private string _metodoName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ConnectionServiceLayer _connectServiceLayer;

        public GenericoRepository(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _aplicacionName = this.GetType().Name;
            _configuration = configuration;
            _clientFactory = clientFactory;
            _connectServiceLayer = new ConnectionServiceLayer(_configuration, _clientFactory);
        }

        public async Task<ResultadoTransaccion<BE_Generico>> GetListGenericoPorFiltro(string code, string name)
        {
            ResultadoTransaccion<BE_Generico> vResultadoTransaccion = new ResultadoTransaccion<BE_Generico>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {
                code = code == null ? "" : code.ToUpper();
                name = name == null ? "" : name.ToUpper();

                var modelo = "U_SYP_CS_DCI";
                var campos = "?$select= Code, Name ";
                var filter = "&$filter = U_SYP_CS_ESTADO eq 'Y' ";


                if (!string.IsNullOrEmpty(code))
                {
                    filter = filter + " and Code eq '" + code + "'";
                }

                if (string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(name))
                {
                    filter = filter + " and contains (Name,'" + name + "')";
                }

                modelo = modelo + campos + filter;

                List<BE_Generico> data = await _connectServiceLayer.GetAsync<BE_Generico>(modelo);

                vResultadoTransaccion.IdRegistro = 0;
                vResultadoTransaccion.ResultadoCodigo = 0;
                vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", data.Count);
                vResultadoTransaccion.dataList = data;
            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return vResultadoTransaccion;
        }

        public async Task<ResultadoTransaccion<BE_Generico>> GetListGenericoPorProDCI(string codprodci)
        {
            ResultadoTransaccion<BE_Generico> vResultadoTransaccion = new ResultadoTransaccion<BE_Generico>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {
                codprodci = codprodci == null ? "" : codprodci.ToUpper();

                List<BE_Stock> listDCI = await _connectServiceLayer.GetAsync<BE_Stock>("U_SYP_CS_PRODCI?$select=U_SYP_CS_DCI&$filter = Code eq '" + codprodci + "'");
                List<BE_Generico> data = new List<BE_Generico>();

                string code = string.Empty;

                if (listDCI.Count > 0)
                {
                    code = listDCI[0].U_SYP_CS_DCI == null ? string.Empty : listDCI[0].U_SYP_CS_DCI;

                    if (!string.IsNullOrEmpty(code))
                    {
                        var modelo = "U_SYP_CS_DCI";
                        var campos = "?$select= Code, Name ";
                        var filter = "&$filter = U_SYP_CS_ESTADO eq 'Y' ";

                        filter = filter + " and Code eq '" + code + "'";

                        modelo = modelo + campos + filter;

                        data = await _connectServiceLayer.GetAsync<BE_Generico>(modelo);
                    }
                }

                vResultadoTransaccion.IdRegistro = 0;
                vResultadoTransaccion.ResultadoCodigo = 0;
                vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", data.Count);
                vResultadoTransaccion.dataList = data;
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
