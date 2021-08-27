using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Net.Business.Entities;
using Net.Connection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Net.Data
{
    public class CuadreCajaRepository : RepositoryBase<BE_CuadreCaja>, ICuadreCajaRepository
    {
        private readonly string _cnx;

        private string _aplicacionName;
        private string _metodoName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        const string DB_ESQUEMA = "";
        const string SP_GET_CUADRECAJA_POR_FILTRO = DB_ESQUEMA + "VEN_ListaCuadredeCajaPorFiltroGet";
        const string SP_GET_CUADRECAJA_GENERAL_POR_FILTRO = DB_ESQUEMA + "VEN_ListaCuadredeCajaGralPorFiltroGet";
        const string SP_GET_RPCUADRECAJA_POR_FILTRO = DB_ESQUEMA + "VEN_Rp_CuadreCajaPorFiltroGet";
        const string SP_GET_RPCUADRECAJADETALLADO_POR_FILTRO = DB_ESQUEMA + "VEN_Rp_CuadreCajaDetalladoPorFiltroGet";
        const string SP_GET_CUADRECAJA_INSERT = DB_ESQUEMA + "VEN_CuadredeCaja_Ins";
        const string SP_GET_CUADRECAJA_DELETE = DB_ESQUEMA + "VEN_CuadredeCaja_Del";

        public CuadreCajaRepository(IConnectionSQL context, IConfiguration configuration)
            : base(context)
        {
            _aplicacionName = this.GetType().Name;
            _cnx = configuration.GetConnectionString("cnnSqlLogistica");
        }
        public async Task<ResultadoTransaccion<BE_CuadreCaja>> GetListaCuadredeCajaPorFiltro(string documento)
        {
            ResultadoTransaccion<BE_CuadreCaja> vResultadoTransaccion = new ResultadoTransaccion<BE_CuadreCaja>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new List<BE_CuadreCaja>();
                    using (SqlCommand cmd = new SqlCommand(SP_GET_CUADRECAJA_POR_FILTRO, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@documento", documento));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_CuadreCaja>)context.ConvertTo<BE_CuadreCaja>(reader);
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

        public async Task<ResultadoTransaccion<BE_CuadreCaja>> GetListaCuadredeCajaGeneralPorFiltro(string fecha1, string fecha2, string coduser, string codcentro, string orden)
        {
            ResultadoTransaccion<BE_CuadreCaja> vResultadoTransaccion = new ResultadoTransaccion<BE_CuadreCaja>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new List<BE_CuadreCaja>();
                    BE_CuadreCaja responseEnty;

                    using (SqlCommand cmd = new SqlCommand(SP_GET_CUADRECAJA_GENERAL_POR_FILTRO, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@fecha1", fecha1));
                        cmd.Parameters.Add(new SqlParameter("@fecha2", fecha2));
                        cmd.Parameters.Add(new SqlParameter("@idusuario", coduser));
                        cmd.Parameters.Add(new SqlParameter("@codcentro", codcentro));
                        cmd.Parameters.Add(new SqlParameter("@orden", orden));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                responseEnty = new BE_CuadreCaja()
                                {
                                    documento = (reader["documento"] is DBNull) ? string.Empty : (string)reader["documento"],
                                    documentoe = (reader["documentoe"] is DBNull) ? string.Empty : (string)reader["documentoe"],
                                    nombres = (reader["nombres"] is DBNull) ? string.Empty : (string)reader["nombres"],
                                    docmonto = (reader["docmonto"] is DBNull) ? 0 : Convert.ToDecimal(reader["docmonto"]),
                                    movimiento = (reader["movimiento"] is DBNull) ? string.Empty : (string)reader["movimiento"]
                                };
                                response.Add(responseEnty);
                                //response = (List<BE_CuadreCaja>)context.ConvertTo<BE_CuadreCaja>(reader);

                            }
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

        public async Task<ResultadoTransaccion<BE_CuadreCaja>> GetRpCuadreCajaPorFiltro(string fechainicio, string fechafin, string coduser, string codcentro, string numeroplanilla, decimal dolares)
        {
            ResultadoTransaccion<BE_CuadreCaja> vResultadoTransaccion = new ResultadoTransaccion<BE_CuadreCaja>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new List<BE_CuadreCaja>();
                    using (SqlCommand cmd = new SqlCommand(SP_GET_RPCUADRECAJA_POR_FILTRO, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@fechainicio", fechainicio));
                        cmd.Parameters.Add(new SqlParameter("@fechafin", fechafin));
                        cmd.Parameters.Add(new SqlParameter("@coduser", coduser));
                        cmd.Parameters.Add(new SqlParameter("@codcentro", codcentro));
                        cmd.Parameters.Add(new SqlParameter("@numeroplanilla", numeroplanilla));
                        cmd.Parameters.Add(new SqlParameter("@dolares", dolares));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_CuadreCaja>)context.ConvertTo<BE_CuadreCaja>(reader);
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
        public async Task<ResultadoTransaccion<BE_CuadreCaja>> GetRpCuadreCajaDetalladoPorFiltro(string fechainicio, string fechafin, string coduserp, string codcentro, string numeroplanilla, string orden)
        {
            ResultadoTransaccion<BE_CuadreCaja> vResultadoTransaccion = new ResultadoTransaccion<BE_CuadreCaja>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new List<BE_CuadreCaja>();
                    using (SqlCommand cmd = new SqlCommand(SP_GET_RPCUADRECAJADETALLADO_POR_FILTRO, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@fechainicio", fechainicio));
                        cmd.Parameters.Add(new SqlParameter("@fechafin", fechafin));
                        cmd.Parameters.Add(new SqlParameter("@coduserp", coduserp));
                        cmd.Parameters.Add(new SqlParameter("@codcentro", codcentro));
                        cmd.Parameters.Add(new SqlParameter("@numeroplanilla", numeroplanilla));
                        cmd.Parameters.Add(new SqlParameter("@orden", orden));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_CuadreCaja>)context.ConvertTo<BE_CuadreCaja>(reader);
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

        public async Task<ResultadoTransaccion<string>> CuadreCajaRegistrar(List<BE_CuadreCaja> value)
        {
            ResultadoTransaccion<string> vResultadoTransaccion = new ResultadoTransaccion<string>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            string correlativo = string.Empty;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        var documento = value[0].documento.ToString();
                        using (SqlCommand cmd = new SqlCommand(SP_GET_CUADRECAJA_DELETE, conn, transaction))
                        {
                            cmd.Parameters.Clear();
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@documento", documento));
                            await cmd.ExecuteNonQueryAsync();
                            vResultadoTransaccion.IdRegistro = 0;
                            vResultadoTransaccion.ResultadoCodigo = 0;
                            vResultadoTransaccion.ResultadoDescripcion = "SE ELIMINO EL REGISTRO CORRECTAMENTE";
                            vResultadoTransaccion.data = correlativo;
                        }

                        using (SqlCommand cmd = new SqlCommand(SP_GET_CUADRECAJA_INSERT, conn, transaction))
                        {

                            foreach (var item in value)
                            {
                                cmd.Parameters.Clear();
                                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                SqlParameter oParam = new SqlParameter("@correlativo", SqlDbType.Char, 11)
                                {
                                    Direction = ParameterDirection.Output
                                };
                                cmd.Parameters.Add(oParam);

                                cmd.Parameters.Add(new SqlParameter("@codcomprobante", item.codcomprobante));
                                cmd.Parameters.Add(new SqlParameter("@monto", item.monto));
                                cmd.Parameters.Add(new SqlParameter("@tipopago", item.tipopago));
                                cmd.Parameters.Add(new SqlParameter("@nombreentidad", item.nombreentidad.ToString()));
                                cmd.Parameters.Add(new SqlParameter("@descripcionentidad", item.descripcionentidad));
                                cmd.Parameters.Add(new SqlParameter("@numeroentidad", item.numeroentidad.ToString()));
                                cmd.Parameters.Add(new SqlParameter("@operacion", "P"));
                                cmd.Parameters.Add(new SqlParameter("@variostipopago", "S"));
                                cmd.Parameters.Add(new SqlParameter("@moneda", item.moneda));                                
                                cmd.Parameters.Add(new SqlParameter("@fechaCancelacion", item.fechacancelacion));                                
                                cmd.Parameters.Add(new SqlParameter("@codterminal", item.codterminal));
                                cmd.Parameters.Add(new SqlParameter("@tipodecambio", item.tipodecambio));

                                await cmd.ExecuteNonQueryAsync();

                                correlativo = cmd.Parameters["@correlativo"].Value.ToString();
                                if (correlativo.Equals("")) throw new Exception("Error al insertar comprobante !!");
                                vResultadoTransaccion.IdRegistro = 0;
                                vResultadoTransaccion.ResultadoCodigo = 0;
                                vResultadoTransaccion.ResultadoDescripcion = "SE MODIFICO EL TIPO DE PAGO CORRECTAMENTE";
                                vResultadoTransaccion.data = correlativo;

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
                        //throw;
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
