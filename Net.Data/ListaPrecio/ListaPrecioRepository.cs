using Net.Business.Entities;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Net.Http;
using Net.Connection.ServiceLayer;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;

namespace Net.Data
{
    public class ListaPrecioRepository: IListaPrecioRepository
    {
        private string _aplicacionName;
        private string _metodoName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ConnectionServiceLayer _connectServiceLayer;

        public ListaPrecioRepository(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _aplicacionName = this.GetType().Name;
            _configuration = configuration;
            _clientFactory = clientFactory;
            _connectServiceLayer = new ConnectionServiceLayer(_configuration, _clientFactory);
        }
        public async Task<ResultadoTransaccion<BE_ListaPrecio>> GetPrecioPorCodigo(string codproducto, int pricelist)
        {
            ResultadoTransaccion<BE_ListaPrecio> vResultadoTransaccion = new ResultadoTransaccion<BE_ListaPrecio>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {
                codproducto = codproducto == null ? "" : codproducto.ToUpper();

                var cadena = "sml.svc/SBALDPRParameters(CODITEM='" + codproducto + "',PRICELIST=" + pricelist + ")/SBALDPR";
                var filter = "&$filter = ItemCode eq '" + codproducto + "' and PriceList eq " + pricelist;
                var campos = "?$select=ItemCode, PriceList, Price, Factor ";

                cadena = cadena + campos + filter;

                List<BE_ListaPrecio> data = await _connectServiceLayer.GetAsync<BE_ListaPrecio>(cadena);

                vResultadoTransaccion.IdRegistro = 0;
                vResultadoTransaccion.ResultadoCodigo = 0;
                vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", 1);
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
