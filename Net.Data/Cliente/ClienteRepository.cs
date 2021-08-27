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
    public class ClienteRepository: IClienteRepository
    {
        private string _aplicacionName;
        private string _metodoName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ConnectionServiceLayer _connectServiceLayer;

        public ClienteRepository(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
            _aplicacionName = this.GetType().Name;
            _connectServiceLayer = new ConnectionServiceLayer(_configuration, _clientFactory);
        }
        public async Task<ResultadoTransaccion<BE_Cliente>> GetListClientePorFiltro(string opcion, string ruc, string nombre)
        {

            ResultadoTransaccion<BE_Cliente> vResultadoTransaccion = new ResultadoTransaccion<BE_Cliente>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                var cadena = "BusinessPartners?$filter = CardType eq 'C'";
                var filter = "";
                var campos = "&$select= CardCode, CardName, FederalTaxID, MailAddress, Phone1, U_SYP_BPTP, U_SYP_BPAP, U_SYP_BPAM, U_SYP_BPNO, U_SYP_BPN2, U_SYP_BPTD";

                if (opcion == "RUC")
                {
                    filter = "&$filter = FederalTaxID eq '" + ruc + "'";
                }

                nombre = nombre == null ? string.Empty : nombre.ToUpper();

                if (opcion == "NOMBRE")
                {
                    filter = "&$filter=contains (CardName , '" + nombre + "' )";
                }

                cadena = cadena + filter + campos;

                List<BE_Cliente> data = await _connectServiceLayer.GetAsync<BE_Cliente>(cadena);

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
