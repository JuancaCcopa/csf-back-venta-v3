using Net.Business.Entities;
using Net.Connection;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Data;

namespace Net.Data
{
    public class SeriePorMaquinaRepository: RepositoryBase<BE_SeriePorMaquina>, ISeriePorMaquinaRepository
    {
        private readonly string _cnx;

        private string _aplicacionName;
        private string _metodoName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        const string DB_ESQUEMA = "";
        const string SP_GET = DB_ESQUEMA + "VEN_ListaSeriexMaquinaPorFiltro";
        const string SP_GET_ID = DB_ESQUEMA + "VEN_ListaSeriexMaquinaPorId";
        const string SP_INSERT = DB_ESQUEMA + "VEN_SeriexMaquinaIns";
        const string SP_UPDATE = DB_ESQUEMA + "VEN_SeriexMaquinaUpd";
        const string SP_DELETE = DB_ESQUEMA + "VEN_SeriexMaquinaDel";

        public SeriePorMaquinaRepository(IConnectionSQL context, IConfiguration configuration)
            : base(context)
        {
            _aplicacionName = this.GetType().Name;
            _cnx = configuration.GetConnectionString("cnnSqlLogistica");
        }

        public async Task<ResultadoTransaccion<BE_SeriePorMaquina>> GetListSeriePorMaquinaPorFiltro(BE_SeriePorMaquina value)
        {
            ResultadoTransaccion<BE_SeriePorMaquina> vResultadoTransaccion = new ResultadoTransaccion<BE_SeriePorMaquina>();
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
                        cmd.Parameters.Add(new SqlParameter("@nombremaquina", value.nombremaquina == null ? string.Empty : value.nombremaquina.ToUpper()));

                        var response = new List<BE_SeriePorMaquina>();

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_SeriePorMaquina>)context.ConvertTo<BE_SeriePorMaquina>(reader);
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

        public async Task<ResultadoTransaccion<BE_SeriePorMaquina>> GetSeriePorMaquinaPorId(int id)
        {
            ResultadoTransaccion<BE_SeriePorMaquina> vResultadoTransaccion = new ResultadoTransaccion<BE_SeriePorMaquina>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_GET_ID, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@id", id));

                        var response = new BE_SeriePorMaquina();

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = context.Convert<BE_SeriePorMaquina>(reader);

                            if (response == null)
                            {
                                response = new BE_SeriePorMaquina();
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

        public async Task<ResultadoTransaccion<BE_SeriePorMaquina>> Registrar(BE_SeriePorMaquina value)
        {
            ResultadoTransaccion<BE_SeriePorMaquina> vResultadoTransaccion = new ResultadoTransaccion<BE_SeriePorMaquina>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {

                    using (SqlCommand cmd = new SqlCommand(SP_INSERT, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@nombremaquina", value.nombremaquina.ToUpper()));
                        cmd.Parameters.Add(new SqlParameter("@seriefactura", value.seriefactura));
                        cmd.Parameters.Add(new SqlParameter("@serieboleta", value.serieboleta));
                        cmd.Parameters.Add(new SqlParameter("@serienotacredito", value.serienotacredito));
                        cmd.Parameters.Add(new SqlParameter("@serienotadebito", value.serienotadebito));
                        cmd.Parameters.Add(new SqlParameter("@serieguia", value.serieguia));
                        cmd.Parameters.Add(new SqlParameter("@codcentro", value.codcentro));
                        cmd.Parameters.Add(new SqlParameter("@codalmacen", value.codalmacen));
                        cmd.Parameters.Add(new SqlParameter("@RegIdUsuario", value.RegIdUsuario));

                        SqlParameter outputIdTransaccionParam = new SqlParameter("@IdTransaccion", SqlDbType.Int, 3)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputIdTransaccionParam);

                        SqlParameter outputMsjTransaccionParam = new SqlParameter("@MsjTransaccion", SqlDbType.VarChar, 700)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputMsjTransaccionParam);

                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = int.Parse(outputIdTransaccionParam.Value.ToString());
                        vResultadoTransaccion.ResultadoDescripcion = (string)outputMsjTransaccionParam.Value;
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

        public async Task<ResultadoTransaccion<BE_SeriePorMaquina>> Modificar(BE_SeriePorMaquina value)
        {
            ResultadoTransaccion<BE_SeriePorMaquina> vResultadoTransaccion = new ResultadoTransaccion<BE_SeriePorMaquina>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {

                    using (SqlCommand cmd = new SqlCommand(SP_UPDATE, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@id", value.id));
                        cmd.Parameters.Add(new SqlParameter("@seriefactura", value.seriefactura));
                        cmd.Parameters.Add(new SqlParameter("@serieboleta", value.serieboleta));
                        cmd.Parameters.Add(new SqlParameter("@serienotacredito", value.serienotacredito));
                        cmd.Parameters.Add(new SqlParameter("@serienotadebito", value.serienotadebito));
                        cmd.Parameters.Add(new SqlParameter("@serieguia", value.serieguia));
                        cmd.Parameters.Add(new SqlParameter("@codcentro", value.codcentro));
                        cmd.Parameters.Add(new SqlParameter("@codalmacen", value.codalmacen));
                        cmd.Parameters.Add(new SqlParameter("@RegIdUsuario", value.RegIdUsuario));

                        SqlParameter outputIdTransaccionParam = new SqlParameter("@IdTransaccion", SqlDbType.Int, 3)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputIdTransaccionParam);

                        SqlParameter outputMsjTransaccionParam = new SqlParameter("@MsjTransaccion", SqlDbType.VarChar, 700)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputMsjTransaccionParam);

                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = int.Parse(outputIdTransaccionParam.Value.ToString());
                        vResultadoTransaccion.ResultadoDescripcion = (string)outputMsjTransaccionParam.Value;
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

        public async Task<ResultadoTransaccion<BE_SeriePorMaquina>> Eliminar(BE_SeriePorMaquina value)
        {
            ResultadoTransaccion<BE_SeriePorMaquina> vResultadoTransaccion = new ResultadoTransaccion<BE_SeriePorMaquina>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {

                    using (SqlCommand cmd = new SqlCommand(SP_DELETE, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@id", value.id));
                        cmd.Parameters.Add(new SqlParameter("@RegIdUsuario", value.RegIdUsuario));

                        SqlParameter outputIdTransaccionParam = new SqlParameter("@IdTransaccion", SqlDbType.Int, 3)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputIdTransaccionParam);

                        SqlParameter outputMsjTransaccionParam = new SqlParameter("@MsjTransaccion", SqlDbType.VarChar, 700)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(outputMsjTransaccionParam);

                        await conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = int.Parse(outputIdTransaccionParam.Value.ToString());
                        vResultadoTransaccion.ResultadoDescripcion = (string)outputMsjTransaccionParam.Value;
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
