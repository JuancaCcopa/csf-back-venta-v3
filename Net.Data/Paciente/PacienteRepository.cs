using Net.Business.Entities;
using Net.Connection;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;

namespace Net.Data
{
    class PacienteRepository : RepositoryBase<BE_Paciente>, IPacienteRepository
    {
        private readonly string _cnx;
        private string _aplicacionName;
        private string _metodoName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        private readonly IConfiguration _configuration;
        const string DB_ESQUEMA = "";
        const string SP_GET = DB_ESQUEMA + "VEN_PacientesInfoFarmaPorAtencionGet";

        public PacienteRepository(IConnectionSQL context, IConfiguration configuration)
            : base(context)
        {
            _cnx = configuration.GetConnectionString("cnnSqlLogistica");
            _aplicacionName = this.GetType().Name;
            _configuration = configuration;
        }
        public async Task<ResultadoTransaccion<BE_Paciente>> GetPacientePorAtencion(string codAtencion)
        {
            ResultadoTransaccion<BE_Paciente> vResultadoTransaccion = new ResultadoTransaccion<BE_Paciente>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {

                // Variables
                double vValidaMedicoEmergencia = 1;

                if (codAtencion.Substring(0, 1) == "E")
                {
                    TablaRepository tablaRepository = new TablaRepository(context, _configuration);
                    ResultadoTransaccion<BE_Tabla> vResultadoTabla = new ResultadoTransaccion<BE_Tabla>();


                    HospitalRepository hospitalRepository = new HospitalRepository(context, _configuration);

                    ResultadoTransaccion<BE_HospitalDatos> vResultadoHospitalDatos = new ResultadoTransaccion<BE_HospitalDatos>();

                    if (codAtencion.Substring(0, 2) == "E0")
                    {
                        vResultadoTabla = await tablaRepository.GetTablasClinicaPorFiltros("LOG_VALIDAMEDICOEMERGENCIA", codAtencion.Substring(0, 2), 50, 1, -1);

                        if (vResultadoTabla.ResultadoCodigo == -1)
                        {
                            vResultadoTransaccion.IdRegistro = -1;
                            vResultadoTransaccion.ResultadoCodigo = -1;
                            vResultadoTransaccion.ResultadoDescripcion = vResultadoTabla.ResultadoDescripcion;
                            return vResultadoTransaccion;
                        }

                        vValidaMedicoEmergencia = vResultadoTabla.data.valor;
                    }

                    if (codAtencion.Substring(0, 2) == "E1")
                    {
                        vResultadoTabla = await tablaRepository.GetTablasClinicaPorFiltros("LOG_VALIDAMEDICOEMERGENCIA", codAtencion.Substring(0, 2), 50, 1, -1);

                        if (vResultadoTabla.ResultadoCodigo == -1)
                        {
                            vResultadoTransaccion.IdRegistro = -1;
                            vResultadoTransaccion.ResultadoCodigo = -1;
                            vResultadoTransaccion.ResultadoDescripcion = vResultadoTabla.ResultadoDescripcion;
                            return vResultadoTransaccion;
                        }

                        vValidaMedicoEmergencia = vResultadoTabla.data.valor;
                    }

                    if (vValidaMedicoEmergencia == 0)
                    {
                        vResultadoHospitalDatos = await hospitalRepository.GetHospitalDatosPorAtencion(codAtencion);

                        if (vResultadoHospitalDatos.ResultadoCodigo == -1)
                        {
                            vResultadoTransaccion.IdRegistro = -1;
                            vResultadoTransaccion.ResultadoCodigo = -1;
                            vResultadoTransaccion.ResultadoDescripcion = vResultadoHospitalDatos.ResultadoDescripcion;
                            return vResultadoTransaccion;
                        }

                        if (string.IsNullOrEmpty(vResultadoHospitalDatos.data.codmedicoemergencia))
                        {
                            vResultadoTransaccion.IdRegistro = -1;
                            vResultadoTransaccion.ResultadoCodigo = -1;
                            vResultadoTransaccion.ResultadoDescripcion = "Atención no tiene asignado el médico emergencista";
                            return vResultadoTransaccion;
                        }
                    }

                }

                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_GET, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codatencion", codAtencion));

                        var response = new BE_Paciente();

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = context.Convert<BE_Paciente>(reader);
                        }

                        conn.Close();

                        if (response == null)
                        {
                            vResultadoTransaccion.IdRegistro = -1;
                            vResultadoTransaccion.ResultadoCodigo = -1;
                            vResultadoTransaccion.ResultadoDescripcion = "Atención no existe";
                            return vResultadoTransaccion;
                        }

                        PlanesRepository planesRepository = new PlanesRepository(context, _configuration);

                        if (response.codaseguradora == "0019")
                        {
                            response.codplan = "00000006";
                            ResultadoTransaccion<BE_Planes>  planes = await planesRepository.GetbyCodigo(new BE_Planes { CodPlan = "00000006" });

                            if (planes.ResultadoCodigo == -1)
                            {
                                vResultadoTransaccion.IdRegistro = -1;
                                vResultadoTransaccion.ResultadoCodigo = -1;
                                vResultadoTransaccion.ResultadoDescripcion = planes.ResultadoDescripcion;
                                return vResultadoTransaccion;
                            }

                            response.porcentajeplan = (decimal)planes.data.PorcentajeDescuento;

                        }

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

        public async Task<ResultadoTransaccion<BE_Paciente>> GetExistenciaPaciente(string codAtencion)
        {
            ResultadoTransaccion<BE_Paciente> vResultadoTransaccion = new ResultadoTransaccion<BE_Paciente>();
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
                        cmd.Parameters.Add(new SqlParameter("@codatencion", codAtencion));

                        var response = new List<BE_Paciente>();

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_Paciente>)context.ConvertTo<BE_Paciente>(reader);
                        }

                        conn.Close();

                        if (response.Count == 0)
                        {
                            vResultadoTransaccion.IdRegistro = -1;
                            vResultadoTransaccion.ResultadoCodigo = -1;
                            vResultadoTransaccion.ResultadoDescripcion = "Atención no existe";
                            vResultadoTransaccion.dataList = response;
                            return vResultadoTransaccion;
                        }

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
