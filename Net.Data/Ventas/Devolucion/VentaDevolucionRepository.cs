using Microsoft.Data.SqlClient;
using Net.Business.Entities;
using Net.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Net.Data
{
    public class VentaDevolucionRepository : RepositoryBase<BE_VentasDevolucion>, IVentaDevolucionRepository
    {
        private readonly string _cnx;
        private string _aplicacionName;
        private string _metodoName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        const string DB_ESQUEMA = "";

        // GET
        const string SP_GET = DB_ESQUEMA + "VEN_ProductosxAtencionGet";
        const string SP_GET_ID = DB_ESQUEMA + "";

        // SET
        const string SP_INSERT = DB_ESQUEMA + "";
        const string SP_UPDATE = DB_ESQUEMA + "";
        const string SP_DELETE = DB_ESQUEMA + "";

        public VentaDevolucionRepository(IConnectionSQL context, IConfiguration configuration)
            : base(context)
        {
            _aplicacionName = this.GetType().Name;
            _cnx = configuration.GetConnectionString("cnnSqlLogistica");
        }

        public async Task<ResultadoTransaccion<BE_VentasDevolucion>> GetVentasPorAtencion(BE_VentasDevolucion value)
        {
            ResultadoTransaccion<BE_VentasDevolucion> vResultadoTransaccion = new ResultadoTransaccion<BE_VentasDevolucion>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new List<BE_VentasDevolucion>();
                    using (SqlCommand cmd = new SqlCommand(SP_GET, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codatencion", value.codatencion));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_VentasDevolucion>)context.ConvertTo<BE_VentasDevolucion>(reader);
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
    }
}
