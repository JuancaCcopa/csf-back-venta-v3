using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Net.Business.Entities;
using Net.Connection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Net.Data
{
    public class TerminalRepository : RepositoryBase<BE_VentasCabecera>, ITerminalRepository
    {
        private readonly string _cnx;
        private readonly string _cnxClinica;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private string _aplicacionName;
        private string _metodoName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        const string DB_ESQUEMA = "";
        const string SP_GET_TERMINAL_CONSULTAR = DB_ESQUEMA + "Sp_Terminal_Consulta";

        public TerminalRepository(IHttpClientFactory clientFactory, IConnectionSQL context, IConfiguration configuration)
            : base(context)
        {
            _cnx = configuration.GetConnectionString("cnnSqlLogistica");
            _cnxClinica = configuration.GetConnectionString("cnnSqlClinica");
            _configuration = configuration;
            _clientFactory = clientFactory;
            _aplicacionName = this.GetType().Name;
        }

        public async Task<ResultadoTransaccion<BE_Terminal>> GetTerminal()
        {
            ResultadoTransaccion<BE_Terminal> vResultadoTransaccion = new ResultadoTransaccion<BE_Terminal>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxClinica))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_GET_TERMINAL_CONSULTAR, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        BE_Terminal response = new BE_Terminal();
                        List<BE_Terminal> lista = new List<BE_Terminal>();

                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                response = new BE_Terminal();
                                response.codterminal = ((reader["codterminal"]) is DBNull) ? string.Empty : (string)reader["codterminal"];
                                response.numeroterminal = ((reader["numeroterminal"]) is DBNull) ? string.Empty : (string)reader["numeroterminal"];
                                lista.Add(response);
                            }
                        }

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", 1);
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



    }
}
