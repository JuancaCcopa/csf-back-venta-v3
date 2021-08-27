using Net.Business.Entities;
using Net.Connection;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using System;
using System.Net.Http;
using System.Collections.Generic;

namespace Net.Data
{
    class ConveniosRepository : RepositoryBase<BE_ConveniosListaPrecio>, IConveniosRepository
    {
        private readonly string _cnx;
        private string _aplicacionName;
        private string _metodoName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;

        const string DB_ESQUEMA = "";
        //const string SP_GET = DB_ESQUEMA + "VEN_ConveniosPorFiltroGet";
        const string SP_GET = DB_ESQUEMA + "VEN_ConveniosListaPrecioPorFiltroGet";

        public ConveniosRepository(IHttpClientFactory clientFactory, IConnectionSQL context, IConfiguration configuration)
            : base(context)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
            _cnx = configuration.GetConnectionString("cnnSqlLogistica");
            _aplicacionName = this.GetType().Name;
        }

        public async Task<ResultadoTransaccion<BE_ConveniosListaPrecio>> GetConveniosPorFiltros(string codalmacen, string tipomovimiento, string codtipocliente, string codcliente, string codpaciente, string codaseguradora, string codcia, string codproducto)
        {
            ResultadoTransaccion<BE_ConveniosListaPrecio> vResultadoTransaccion = new ResultadoTransaccion<BE_ConveniosListaPrecio>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {

                var response = new List<BE_ConveniosListaPrecio>();

                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_GET, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codalmacen", codalmacen == null ? string.Empty : codalmacen));
                        cmd.Parameters.Add(new SqlParameter("@tipomovimiento", tipomovimiento == null ? string.Empty : tipomovimiento));
                        cmd.Parameters.Add(new SqlParameter("@codtipocliente", codtipocliente == null ? string.Empty : codtipocliente));
                        cmd.Parameters.Add(new SqlParameter("@codcliente", codcliente == null ? string.Empty : codcliente));
                        cmd.Parameters.Add(new SqlParameter("@codpaciente", codpaciente == null ? string.Empty : codpaciente));
                        cmd.Parameters.Add(new SqlParameter("@codaseguradora", codaseguradora == null ? string.Empty : codaseguradora));
                        cmd.Parameters.Add(new SqlParameter("@codcia", codcia == null ? string.Empty : codcia));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_ConveniosListaPrecio>)context.ConvertTo<BE_ConveniosListaPrecio>(reader);
                        }

                        conn.Close();

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", 1);
                        vResultadoTransaccion.dataList = response;
                    }
                }

                if (response.Count > 0)
                {
                    ListaPrecioRepository listaPrecioRepository = new ListaPrecioRepository(_clientFactory, _configuration);
                    ResultadoTransaccion<BE_ListaPrecio> resultadoTransaccionListaPrecio = await listaPrecioRepository.GetPrecioPorCodigo(codproducto, response[0].pricelist);

                    if (resultadoTransaccionListaPrecio.ResultadoCodigo == -1)
                    {
                        vResultadoTransaccion.IdRegistro = -1;
                        vResultadoTransaccion.ResultadoCodigo = -1;
                        vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionListaPrecio.ResultadoDescripcion;
                        return vResultadoTransaccion;
                    }

                    if (((List<BE_ListaPrecio>)resultadoTransaccionListaPrecio.dataList).Count > 0)
                    {
                        response[0].monto = double.Parse(((List<BE_ListaPrecio>)resultadoTransaccionListaPrecio.dataList)[0].Price.ToString());
                    } else
                    {
                        response[0].monto = 0;
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
