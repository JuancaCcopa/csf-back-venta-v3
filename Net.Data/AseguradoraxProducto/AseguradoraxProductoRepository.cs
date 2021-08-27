using Net.Business.Entities;
using Net.Connection;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System;
using System.Text.RegularExpressions;

namespace Net.Data
{
    public class AseguradoraxProductoRepository : RepositoryBase<BE_AseguradoraxProducto>, IAseguradoraxProductoRepository
    {
        private readonly string _cnx;
        private string _aplicacionName;
        private string _metodoName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        const string DB_ESQUEMA = "";
        const string SP_GET = DB_ESQUEMA + "VEN_AseguradoraxProductoGNCPorFiltroGet";

        public AseguradoraxProductoRepository(IConnectionSQL context, IConfiguration configuration)
            : base(context)
        {
            _aplicacionName = this.GetType().Name;
            _cnx = configuration.GetConnectionString("cnnSqlLogistica");
        }
        public async Task<ResultadoTransaccion<BE_AseguradoraxProducto>> GetListAseguradoraxProductoPorFiltros(string codaseguradora, string codproducto, int tipoatencion)
        {
            ResultadoTransaccion<BE_AseguradoraxProducto> vResultadoTransaccion = new ResultadoTransaccion<BE_AseguradoraxProducto>();
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
                        cmd.Parameters.Add(new SqlParameter("@codaseguradora", codaseguradora));
                        cmd.Parameters.Add(new SqlParameter("@codproducto", codproducto));
                        cmd.Parameters.Add(new SqlParameter("@cod_tipoatencion_mae", tipoatencion));

                        var response = new List<BE_AseguradoraxProducto>();

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_AseguradoraxProducto>)context.ConvertTo<BE_AseguradoraxProducto>(reader);
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