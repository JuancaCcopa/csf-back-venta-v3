using Net.Business.Entities;
using Net.Connection;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using System;

namespace Net.Data
{
    class PersonalClinicaRepository : RepositoryBase<BE_PersonalClinica>, IPersonalClinicaRepository
    {
        private readonly string _cnx;
        private readonly string _cnxLogistica;

        private string _aplicacionName;
        private string _metodoName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        const string DB_ESQUEMA = "";
        const string SP_GET = DB_ESQUEMA + "Fa_Personal_Consulta";

        const string SP_GET_LIMITE_CONSUMO = DB_ESQUEMA + "VEN_LimiteConsumoPersonalPorPersonalGet";

        public PersonalClinicaRepository(IConnectionSQL context, IConfiguration configuration)
            : base(context)
        {
            _aplicacionName = this.GetType().Name;
            _cnx = configuration.GetConnectionString("cnnSqlPlanilla");
            _cnxLogistica = configuration.GetConnectionString("cnnSqlLogistica");
        }
        public async Task<ResultadoTransaccion<BE_PersonalClinica>> GetListPersonalClinicaPorNombre(string nombre)
        {
            ResultadoTransaccion<BE_PersonalClinica> vResultadoTransaccion = new ResultadoTransaccion<BE_PersonalClinica>();
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
                        cmd.Parameters.Add(new SqlParameter("@buscar", nombre == null ? "" : nombre.ToUpper()));
                        cmd.Parameters.Add(new SqlParameter("@key", 1));
                        cmd.Parameters.Add(new SqlParameter("@numerolineas", 1));
                        cmd.Parameters.Add(new SqlParameter("@orden", 5));

                        var response = new List<BE_PersonalClinica>();

                        conn.Open();

                        using (IDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_PersonalClinica>)context.ConvertTo<BE_PersonalClinica>(reader);
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

        public async Task<ResultadoTransaccion<BE_PersonalLimiteConsumo>> GetListLimiteConsumoPorPersonal(string codpersonal)
        {
            ResultadoTransaccion<BE_PersonalLimiteConsumo> vResultadoTransaccion = new ResultadoTransaccion<BE_PersonalLimiteConsumo>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {
                using (SqlConnection conn = new SqlConnection(_cnxLogistica))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIMITE_CONSUMO, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codpersonal", codpersonal));

                        var response = new List<BE_PersonalLimiteConsumo>();

                        conn.Open();

                        using (IDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_PersonalLimiteConsumo>)context.ConvertTo<BE_PersonalLimiteConsumo>(reader);
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
