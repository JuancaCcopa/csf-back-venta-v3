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
    public class TipoCambioRepository : ITipoCambioRepository
    {
        private string _metodoName;
        private string _aplicacionName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        private readonly ConnectionServiceLayer _connectServiceLayer;

        public TipoCambioRepository(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _aplicacionName = this.GetType().Name;
            _connectServiceLayer = new ConnectionServiceLayer(configuration, clientFactory);
        }
        public async Task<ResultadoTransaccion<BE_TipoCambio>> GetObtieneTipoCambio()
        {
            ResultadoTransaccion<BE_TipoCambio> vResultadoTransaccion = new ResultadoTransaccion<BE_TipoCambio>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {
                var fechaActual = string.Format("{0}-{1}-{2}", DateTime.Now.Year, DateTime.Now.Month.ToString().PadLeft(2,'0'), DateTime.Now.Day);

                var cadena = "sml.svc/SBATIDC";
                var filter = "&$filter = RateDate eq '" + fechaActual + "'";
                var campos = "?$select = Currency, RateDate, Rate ";

                cadena = cadena + campos + filter;

                List<BE_TipoCambio> data = await _connectServiceLayer.GetAsync<BE_TipoCambio>(cadena);

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
