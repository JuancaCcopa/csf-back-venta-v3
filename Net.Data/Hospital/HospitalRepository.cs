using Net.Business.Entities;
using Net.Connection;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

namespace Net.Data
{
    public class HospitalRepository : RepositoryBase<BE_HospitalDatos>, IHospitalRepository
    {
        private readonly string _cnx;
        private string _aplicacionName;
        private string _metodoName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        const string DB_ESQUEMA = "";
        const string SP_GET_DATOS = DB_ESQUEMA + "Sp_HospitalDatos_Consulta";
        const string SP_GET_EXCLUSIONES = DB_ESQUEMA + "Sp_HospitalExclusiones_Consulta";
        const string SP_GET_HOSPITAL_PACIENTE_CLINICA = DB_ESQUEMA + "Sp_HospitalxPiso_Consulta1";

        public HospitalRepository(IConnectionSQL context, IConfiguration configuration)
            : base(context)
        {
            _aplicacionName = this.GetType().Name;
            _cnx = configuration.GetConnectionString("cnnSqlClinica");
        }

        public async Task<ResultadoTransaccion<BE_HospitalDatos>> GetHospitalDatosPorAtencion(string codatencion)
        {
            ResultadoTransaccion<BE_HospitalDatos> vResultadoTransaccion = new ResultadoTransaccion<BE_HospitalDatos>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_GET_DATOS, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codatencion", codatencion));

                        var response = new BE_HospitalDatos();

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = context.Convert<BE_HospitalDatos>(reader);

                            if (response == null) {
                                response = new BE_HospitalDatos();
                            }
                        }

                        conn.Close();

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", 1);
                        vResultadoTransaccion.data = response;
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

        public async Task<ResultadoTransaccion<BE_HospitalExclusiones>> GetListHospitalExclusionesPorAtencion(string codatencion)
        {
            ResultadoTransaccion<BE_HospitalExclusiones> vResultadoTransaccion = new ResultadoTransaccion<BE_HospitalExclusiones>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_GET_EXCLUSIONES, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codigo", codatencion));

                        var response = new List<BE_HospitalExclusiones>();

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_HospitalExclusiones>)context.ConvertTo<BE_HospitalExclusiones>(reader);
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

        public async Task<ResultadoTransaccion<BE_Hospital>> GetListHospitalPacienteClinicaPorFiltros(string pabellon, string piso, string local)
        {
            ResultadoTransaccion<BE_Hospital> vResultadoTransaccion = new ResultadoTransaccion<BE_Hospital>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_GET_HOSPITAL_PACIENTE_CLINICA, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@pabellon", pabellon));
                        cmd.Parameters.Add(new SqlParameter("@piso", piso));
                        cmd.Parameters.Add(new SqlParameter("@local", local));

                        var response = new List<BE_Hospital>();

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_Hospital>)context.ConvertTo<BE_Hospital>(reader);
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
