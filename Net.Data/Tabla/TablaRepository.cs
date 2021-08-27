using Net.Business.Entities;
using Net.Connection;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;
using System;

namespace Net.Data
{
    public class TablaRepository : RepositoryBase<BE_Tabla>, ITablaRepository
    {
        private readonly string _cnx_clinica;
        private readonly string _cnx_logistica;

        private string _aplicacionName;
        private string _metodoName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        const string DB_ESQUEMA = "";
        const string SP_GET_CLINICA = DB_ESQUEMA + "Sp_Tablas_Consulta";
        const string SP_GET_LOGISTICA = DB_ESQUEMA + "SHR_TablasGet";
        const string SP_GET_TCI_WEBSERVICE = DB_ESQUEMA + "Sp_TCI_WebService_Consulta";

        public TablaRepository(IConnectionSQL context, IConfiguration configuration)
            : base(context)
        {
            _aplicacionName = this.GetType().Name;
            _cnx_clinica = configuration.GetConnectionString("cnnSqlClinica");
            _cnx_logistica = configuration.GetConnectionString("cnnSqlLogistica");
        }
        public async Task<ResultadoTransaccion<BE_Tabla>> GetTablasClinicaPorFiltros(string codtabla, string buscar, int key, int numerolineas, int orden)
        {
            ResultadoTransaccion<BE_Tabla> vResultadoTransaccion = new ResultadoTransaccion<BE_Tabla>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx_clinica))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_GET_CLINICA, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codtabla", codtabla));
                        cmd.Parameters.Add(new SqlParameter("@buscar", buscar));
                        cmd.Parameters.Add(new SqlParameter("@key", key));
                        cmd.Parameters.Add(new SqlParameter("@numerolineas", numerolineas));
                        cmd.Parameters.Add(new SqlParameter("@orden", orden));

                        var response = new BE_Tabla();

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = context.Convert<BE_Tabla>(reader);

                            if (response == null)
                            {
                                response = new BE_Tabla();
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

        public async Task<ResultadoTransaccion<BE_Tabla>> GetListTablaClinicaPorFiltros(string codtabla, string buscar, int key, int numerolineas, int orden)
        {
            ResultadoTransaccion<BE_Tabla> vResultadoTransaccion = new ResultadoTransaccion<BE_Tabla>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx_clinica))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_GET_CLINICA, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codtabla", codtabla));
                        cmd.Parameters.Add(new SqlParameter("@buscar", buscar == null ? string.Empty : buscar ));
                        cmd.Parameters.Add(new SqlParameter("@key", key));
                        cmd.Parameters.Add(new SqlParameter("@numerolineas", numerolineas));
                        cmd.Parameters.Add(new SqlParameter("@orden", orden));

                        var response = new List<BE_Tabla>();

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_Tabla>)context.ConvertTo<BE_Tabla>(reader);
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

        public async Task<ResultadoTransaccion<BE_Tabla>> GetListTablaLogisticaPorFiltros(string codtabla, string buscar, int key, int numerolineas, int orden)
        {
            ResultadoTransaccion<BE_Tabla> vResultadoTransaccion = new ResultadoTransaccion<BE_Tabla>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx_logistica))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_GET_LOGISTICA, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codtabla", codtabla));
                        cmd.Parameters.Add(new SqlParameter("@buscar", buscar == null ? string.Empty : buscar));
                        cmd.Parameters.Add(new SqlParameter("@key", key));
                        cmd.Parameters.Add(new SqlParameter("@numerolineas", numerolineas));
                        cmd.Parameters.Add(new SqlParameter("@orden", orden));

                        var response = new List<BE_Tabla>();

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_Tabla>)context.ConvertTo<BE_Tabla>(reader);
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

        public async Task<ResultadoTransaccion<BE_Tabla>> GetTablaLogisticaPorFiltros(string codtabla, string buscar, int key, int numerolineas, int orden)
        {
            ResultadoTransaccion<BE_Tabla> vResultadoTransaccion = new ResultadoTransaccion<BE_Tabla>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx_logistica))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_GET_LOGISTICA, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codtabla", codtabla));
                        cmd.Parameters.Add(new SqlParameter("@buscar", buscar == null ? string.Empty : buscar));
                        cmd.Parameters.Add(new SqlParameter("@key", key));
                        cmd.Parameters.Add(new SqlParameter("@numerolineas", numerolineas));
                        cmd.Parameters.Add(new SqlParameter("@orden", orden));

                        var response = new BE_Tabla();

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = context.Convert<BE_Tabla>(reader);

                            if (response == null)
                            {
                                response = new BE_Tabla();
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

        //public async Task<IEnumerable<BE_Tabla>> GetListTablasClinicaPorFiltros(string codtabla, string buscar, int key, int numerolineas, int orden)
        //{
        //    using (SqlConnection conn = new SqlConnection(_cnx))
        //    {
        //        using (SqlCommand cmd = new SqlCommand(SP_GET, conn))
        //        {
        //            cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //            cmd.Parameters.Add(new SqlParameter("@codtabla", codtabla));
        //            cmd.Parameters.Add(new SqlParameter("@buscar", buscar));
        //            cmd.Parameters.Add(new SqlParameter("@key", key));
        //            cmd.Parameters.Add(new SqlParameter("@numerolineas", numerolineas));
        //            cmd.Parameters.Add(new SqlParameter("@orden", orden));

        //            var response = new List<BE_Tabla>();

        //            conn.Open();

        //            using (var reader = await cmd.ExecuteReaderAsync())
        //            {
        //                response = (List<BE_Tabla>)context.ConvertTo<BE_Tabla>(reader);
        //            }

        //            conn.Close();

        //            return response;
        //        }
        //    }
        //}
        public async Task<ResultadoTransaccion<BE_Tabla>> GetTablasTCIWebService(string codtabla)
        {
            ResultadoTransaccion<BE_Tabla> vResultadoTransaccion = new ResultadoTransaccion<BE_Tabla>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx_clinica))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_GET_TCI_WEBSERVICE, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codtabla", codtabla));

                        var response = new BE_Tabla();

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = context.Convert<BE_Tabla>(reader);

                            if (response == null)
                            {
                                response = new BE_Tabla();
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

    }
}