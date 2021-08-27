using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Net.Business.Entities;
using Net.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Net.Data
{
    public class PlanillaRepository : RepositoryBase<BE_Planilla>, IPlanillaRepository
    {
        private readonly string _cnx;
        private string _aplicacionName;
        private string _metodoName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        const string DB_ESQUEMA = "";
        const string SP_INSERTXUSUER = DB_ESQUEMA + "VEN_PlanillaxUserIns";
        const string SP_UPDATE = DB_ESQUEMA + "VEN_PlanillasUpd";
        const string SP_DELETE = DB_ESQUEMA + "VEN_PlanillasDel";
        const string SP_PLANILLA_POR_FILTRO = DB_ESQUEMA + "VEN_ListaPlanillasPorFiltroGet";

        public PlanillaRepository(IConnectionSQL context, IConfiguration configuration)
            : base(context)
        {
            _aplicacionName = this.GetType().Name;
            _cnx = configuration.GetConnectionString("cnnSqlLogistica");
        }

        public async Task<ResultadoTransaccion<string>> RegistrarPorUsuario(BE_Planilla value)
        {
            ResultadoTransaccion<string> vResultadoTransaccion = new ResultadoTransaccion<string>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();
            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;


            using (SqlConnection conn = new SqlConnection(_cnx))
            {

                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    using (SqlCommand cmd = new SqlCommand(SP_INSERTXUSUER, conn, transaction))
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@idusuario", value.idusuario));
                        cmd.Parameters.Add(new SqlParameter("@codcentro", value.codcentro));
                        cmd.Parameters.Add(new SqlParameter("@monto", value.montodolares));
                        cmd.Parameters.Add(new SqlParameter("@codcomprobante", string.Empty));//value.codcomprobante
                        cmd.Parameters.Add(new SqlParameter("@ingresoegreso", string.Empty));// value.ingresoegreso
                        cmd.Parameters.Add(new SqlParameter("@accion", "1"));

                        SqlParameter oParam = new SqlParameter("@numeroplanilla", SqlDbType.Char,8)
                        {
                            Direction = ParameterDirection.Output,
                        };
                        cmd.Parameters.Add(oParam);

                        var result = await cmd.ExecuteNonQueryAsync();
                        value.numeroplanilla = (string)oParam.Value;

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.ResultadoDescripcion = $"PLANILLA PROCESADA {value.numeroplanilla}";
                        vResultadoTransaccion.data = value.numeroplanilla;

                    }

                    if (value.numeroplanilla.Length>0)
                    {

                        foreach (var item in value.planilladetalle)
                        {
                            using (SqlCommand cmd = new SqlCommand(SP_INSERTXUSUER, conn, transaction))
                            {
                                cmd.Parameters.Clear();
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@idusuario", value.idusuario));
                                cmd.Parameters.Add(new SqlParameter("@codcentro", string.Empty));
                                cmd.Parameters.Add(new SqlParameter("@monto", item.monto));
                                cmd.Parameters.Add(new SqlParameter("@codcomprobante", item.codcomprobante));//value.codcomprobante
                                cmd.Parameters.Add(new SqlParameter("@ingresoegreso", item.ingresoegreso));// value.ingresoegreso
                                cmd.Parameters.Add(new SqlParameter("@accion", "2"));
                                cmd.Parameters.Add(new SqlParameter("@strnumeroplanilla", value.numeroplanilla));

                                SqlParameter oParam = new SqlParameter("@numeroplanilla", SqlDbType.Char, 8)
                                {
                                    Direction = ParameterDirection.Output,
                                };
                                cmd.Parameters.Add(oParam);

                                var result = await cmd.ExecuteNonQueryAsync();

                                vResultadoTransaccion.IdRegistro = 0;
                                vResultadoTransaccion.ResultadoCodigo = 0;
                                vResultadoTransaccion.ResultadoDescripcion = "PLANILLA PROCESADA.";
                                vResultadoTransaccion.data = value.numeroplanilla;
                            }
                        }
                    }

                    transaction.Commit();
                    transaction.Dispose();

                }
                catch (Exception ex)
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex3)
                    {
                        vResultadoTransaccion.ResultadoDescripcion = ex3.Message.ToString();
                    }

                }
            }


            return vResultadoTransaccion;
        }
        public async Task<ResultadoTransaccion<BE_Planilla>> Modificar(BE_Planilla values)
        {
            ResultadoTransaccion<BE_Planilla> vResultadoTransaccion = new ResultadoTransaccion<BE_Planilla>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            using (SqlConnection conn = new SqlConnection(_cnx))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(SP_UPDATE, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@campo", values.campo));
                        cmd.Parameters.Add(new SqlParameter("@codigo", values.numeroplanilla));
                        cmd.Parameters.Add(new SqlParameter("@nuevovalor", values.fecha));

                        await conn.OpenAsync();
                        int result = await cmd.ExecuteNonQueryAsync();

                        vResultadoTransaccion.IdRegistro = result;
                        vResultadoTransaccion.ResultadoCodigo = result;
                        vResultadoTransaccion.ResultadoDescripcion = "Grabación exitosa.";
                    }
                }
                catch (Exception ex)
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                }
            }

            return vResultadoTransaccion;
        }

        public async Task<ResultadoTransaccion<BE_Planilla>> Elminar(string numeroplanilla)
        {
            ResultadoTransaccion<BE_Planilla> vResultadoTransaccion = new ResultadoTransaccion<BE_Planilla>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            using (SqlConnection conn = new SqlConnection(_cnx))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(SP_DELETE, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@numeroplanilla", numeroplanilla));

                        await conn.OpenAsync();
                        var result = await cmd.ExecuteNonQueryAsync();

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.ResultadoDescripcion = "La planilla fue eliminada.";
                    }
                }
                catch (Exception ex)
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                }
            }

            return vResultadoTransaccion;
        }

        public async Task<ResultadoTransaccion<BE_Planilla>> GetPlanillasPorFiltro(string buscar, int key, int numerolineas, int orden, string serie)
        {
            ResultadoTransaccion<BE_Planilla> vResultadoTransaccion = new ResultadoTransaccion<BE_Planilla>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new BE_Planilla();
                    using (SqlCommand cmd = new SqlCommand(SP_PLANILLA_POR_FILTRO, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@buscar", buscar));
                        cmd.Parameters.Add(new SqlParameter("@key", key));
                        cmd.Parameters.Add(new SqlParameter("@numerolineas", numerolineas));
                        cmd.Parameters.Add(new SqlParameter("@orden", orden));
                        cmd.Parameters.Add(new SqlParameter("@serie", serie));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = context.Convert<BE_Planilla>(reader);

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

        public async Task<ResultadoTransaccion<BE_Planilla>> GetListaPlanillasPorFiltro(string buscar, int key, int numerolineas, int orden, string serie, string codcentro, string idusuario, string numeroPlanilla, string fechaInicio, string fechaFin)
        {
            ResultadoTransaccion<BE_Planilla> vResultadoTransaccion = new ResultadoTransaccion<BE_Planilla>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new List<BE_Planilla>();
                    using (SqlCommand cmd = new SqlCommand(SP_PLANILLA_POR_FILTRO, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@buscar", buscar));
                        cmd.Parameters.Add(new SqlParameter("@key", key));
                        cmd.Parameters.Add(new SqlParameter("@numerolineas", numerolineas));
                        cmd.Parameters.Add(new SqlParameter("@orden", orden));
                        cmd.Parameters.Add(new SqlParameter("@serie", serie));
                        cmd.Parameters.Add(new SqlParameter("@numeroPlanilla", (numeroPlanilla == string.Empty) ? null : numeroPlanilla));
                        cmd.Parameters.Add(new SqlParameter("@idusuario", ( Convert.ToInt32(idusuario)==0)? null: idusuario));
                        cmd.Parameters.Add(new SqlParameter("@fechaInicio", (fechaInicio == string.Empty) ? null : fechaInicio));
                        cmd.Parameters.Add(new SqlParameter("@fechaFin", (fechaFin == string.Empty) ? null : fechaFin));

                        //cmd.Parameters.Add(new SqlParameter("@codcentro", (codcentro == string.Empty) ? null : codcentro));


                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_Planilla>)context.ConvertTo<BE_Planilla>(reader);
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
