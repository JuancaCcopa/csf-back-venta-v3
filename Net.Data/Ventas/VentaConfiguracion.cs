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
    public class VentaConfiguracion : RepositoryBase<BE_VentasConfiguracion>, IVentaConfiguracion
    {
        private readonly string _cnx;
        private string _aplicacionName;
        private string _metodoName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        const string DB_ESQUEMA = "";
        const string SP_GET = DB_ESQUEMA + "VEN_VentaConfiguracionGet";
        const string SP_GET_ID = DB_ESQUEMA + "VEN_VentaConfiguracionById";
        const string SP_INSERT = DB_ESQUEMA + "VEN_VentaConfiguracionIns";
        const string SP_UPDATE = DB_ESQUEMA + "VEN_VentaConfiguracionUpd";
        const string SP_DELETE = DB_ESQUEMA + "VEN_VentaConfiguracionDel";

        public VentaConfiguracion(IConnectionSQL context, IConfiguration configuration)
            : base(context)
        {
            _aplicacionName = this.GetType().Name;
            _cnx = configuration.GetConnectionString("cnnSqlLogistica");
        }

        public Task<IEnumerable<BE_VentasConfiguracion>> GetByFiltros(BE_VentasConfiguracion value)
        {
            return Task.Run(() => {
                value.nombre = value.nombre == null ? "" : value.nombre;
                return context.ExecuteSqlViewFindByCondition<BE_VentasConfiguracion>(SP_GET, new BE_VentasConfiguracion { nombre = value.nombre }, _cnx);
            });
        }
        public Task<BE_VentasConfiguracion> GetbyId(BE_VentasConfiguracion value)
        {
            return Task.Run(() => {
                return context.ExecuteSqlViewId<BE_VentasConfiguracion>(SP_GET_ID, new BE_VentasConfiguracion { idconfiguracion = value.idconfiguracion }, _cnx);
            });

        }

        public async Task<ResultadoTransaccion<BE_VentasConfiguracion>> Registrar(BE_VentasConfiguracion item)
        {
            ResultadoTransaccion<BE_VentasConfiguracion> vResultadoTransaccion = new ResultadoTransaccion<BE_VentasConfiguracion>();
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

                        SqlParameter oParam = new SqlParameter("@idconfiguracion", item.idconfiguracion);
                        oParam.SqlDbType = SqlDbType.Int;
                        oParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(oParam);

                        cmd.Parameters.Add(new SqlParameter("@nombre", item.nombre));
                        cmd.Parameters.Add(new SqlParameter("@flgautomatico", item.flgautomatico));
                        cmd.Parameters.Add(new SqlParameter("@flgmanual", item.flgmanual));
                        cmd.Parameters.Add(new SqlParameter("@flgpedido", item.flgpedido));
                        cmd.Parameters.Add(new SqlParameter("@flgreceta", item.flgreceta));
                        cmd.Parameters.Add(new SqlParameter("@flgimpresionautomatico", item.flgimpresionautomatico));
                        cmd.Parameters.Add(new SqlParameter("@codalmacen", item.codalmacen));
                        cmd.Parameters.Add(new SqlParameter("@desalmacen", item.desalmacen));
                        cmd.Parameters.Add(new SqlParameter("@RegIdUsuario", item.RegIdUsuario));

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

                        vResultadoTransaccion.IdRegistro = (int)cmd.Parameters["@idconfiguracion"].Value;
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

        public async Task<ResultadoTransaccion<BE_VentasConfiguracion>> Modificar(BE_VentasConfiguracion item)
        {

            ResultadoTransaccion<BE_VentasConfiguracion> vResultadoTransaccion = new ResultadoTransaccion<BE_VentasConfiguracion>();
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
                        cmd.Parameters.Add(new SqlParameter("@idconfiguracion", item.idconfiguracion));
                        cmd.Parameters.Add(new SqlParameter("@nombre", item.nombre));
                        cmd.Parameters.Add(new SqlParameter("@flgautomatico", item.flgautomatico));
                        cmd.Parameters.Add(new SqlParameter("@flgmanual", item.flgmanual));
                        cmd.Parameters.Add(new SqlParameter("@flgpedido", item.flgpedido));
                        cmd.Parameters.Add(new SqlParameter("@flgreceta", item.flgreceta));
                        cmd.Parameters.Add(new SqlParameter("@flgimpresionautomatico", item.flgimpresionautomatico));
                        cmd.Parameters.Add(new SqlParameter("@RegIdUsuario", item.RegIdUsuario));

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

                        vResultadoTransaccion.IdRegistro = int.Parse(item.idconfiguracion.ToString());
                        vResultadoTransaccion.ResultadoCodigo = int.Parse(outputIdTransaccionParam.Value.ToString());
                        vResultadoTransaccion.ResultadoDescripcion = (string)outputMsjTransaccionParam.Value;
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
        public async Task<ResultadoTransaccion<BE_VentasConfiguracion>> Eliminar(BE_VentasConfiguracion value)
        {
            ResultadoTransaccion<BE_VentasConfiguracion> vResultadoTransaccion = new ResultadoTransaccion<BE_VentasConfiguracion>();
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

                        cmd.Parameters.Add(new SqlParameter("@idconfiguracion", value.idconfiguracion));
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

                        vResultadoTransaccion.IdRegistro = int.Parse(value.idconfiguracion.ToString());
                        vResultadoTransaccion.ResultadoCodigo = int.Parse(outputIdTransaccionParam.Value.ToString());
                        vResultadoTransaccion.ResultadoDescripcion = (string)outputMsjTransaccionParam.Value;
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
    }
}