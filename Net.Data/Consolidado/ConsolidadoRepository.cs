using Net.Business.Entities;
using Net.Connection;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Data;
using Net.CrossCotting;

namespace Net.Data
{
    public class ConsolidadoRepository : RepositoryBase<BE_Consolidado>, IConsolidadoRepository
    {
        private readonly string _cnx;
        private string _aplicacionName;
        private string _metodoName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        const string DB_ESQUEMA = "";
        const string SP_GET_CONSOLIDADO_POR_FILTRO = DB_ESQUEMA + "VEN_ConsolidadosPorFiltrosGet";
        const string SP_GET_CONSOLIDADO_POR_PICKING = DB_ESQUEMA + "VEN_ConsolidadosPedidoPickingGet";
        const string SP_GET_CONSOLIDADO_PICKING_POR_ID = DB_ESQUEMA + "VEN_ConsolidadosPedidoPickingPorIdConsolidadoGet";
        const string SP_GET_DETALLE_CONSOLIDADO_POR_ID = DB_ESQUEMA + "VEN_ConsolidadosPedidoPorIdGet";
        const string SP_GET_DETALLE_CONSOLIDADO_PICKING_POR_ID = DB_ESQUEMA + "VEN_ConsolidadosPedidoPickingPorIdGet";

        const string SP_INSERT_CONSOLIDADO_PICKING = DB_ESQUEMA + "VEN_ConsolidadosPedidoPickingIns";
        const string SP_UPDATE_CONSOLIDADO_PICKING = DB_ESQUEMA + "VEN_ConsolidadosPedidoPickingUpd";
        const string SP_UPDATE_CONSOLIDADO_PICKING_ESTADO = DB_ESQUEMA + "VEN_ConsolidadosPedidoPickingEstadoUpd";
        const string SP_DELETE_CONSOLIDADO_PICKING = DB_ESQUEMA + "VEN_ConsolidadosPedidoPickingDel";

        public ConsolidadoRepository(IConnectionSQL context, IConfiguration configuration)
            : base(context)
        {
            _cnx = configuration.GetConnectionString("cnnSqlLogistica");
            _aplicacionName = this.GetType().Name;
        }

        public async Task<ResultadoTransaccion<BE_Consolidado>> GetListConsolidadoPorFiltro(DateTime fechainicio, DateTime fechafin)
        {
            ResultadoTransaccion<BE_Consolidado> vResultadoTransaccion = new ResultadoTransaccion<BE_Consolidado>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            fechainicio = Utilidades.GetFechaHoraInicioActual(fechainicio);
            fechafin = Utilidades.GetFechaHoraFinActual(fechafin);

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new List<BE_Consolidado>();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_CONSOLIDADO_POR_FILTRO, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@fechainicio", fechainicio));
                        cmd.Parameters.Add(new SqlParameter("@fechafin", fechafin));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_Consolidado>)context.ConvertTo<BE_Consolidado>(reader);
                        }

                        conn.Close();
                    }

                    vResultadoTransaccion.IdRegistro = 0;
                    vResultadoTransaccion.ResultadoCodigo = 0;
                    vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", response.Count);
                    vResultadoTransaccion.dataList = response;
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
        public async Task<ResultadoTransaccion<BE_ConsolidadoPedidoPicking>> GetListConsolidadoPickingPorId(int idconsolidado)
        {
            ResultadoTransaccion<BE_ConsolidadoPedidoPicking> vResultadoTransaccion = new ResultadoTransaccion<BE_ConsolidadoPedidoPicking>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new List<BE_ConsolidadoPedidoPicking>();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_CONSOLIDADO_PICKING_POR_ID, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@idconsolidado", idconsolidado));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_ConsolidadoPedidoPicking>)context.ConvertTo<BE_ConsolidadoPedidoPicking>(reader);
                        }

                        conn.Close();
                    }

                    vResultadoTransaccion.IdRegistro = 0;
                    vResultadoTransaccion.ResultadoCodigo = 0;
                    vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", response.Count);
                    vResultadoTransaccion.dataList = response;
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
        public async Task<ResultadoTransaccion<BE_ConsolidadoPedidoPicking>> GetListConsolidadoPicking(string codpedido, string codproducto)
        {
            ResultadoTransaccion<BE_ConsolidadoPedidoPicking> vResultadoTransaccion = new ResultadoTransaccion<BE_ConsolidadoPedidoPicking>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new List<BE_ConsolidadoPedidoPicking>();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_CONSOLIDADO_POR_PICKING, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codpedido", codpedido));
                        cmd.Parameters.Add(new SqlParameter("@codproducto", codproducto));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_ConsolidadoPedidoPicking>)context.ConvertTo<BE_ConsolidadoPedidoPicking>(reader);
                        }

                        conn.Close();
                    }

                    vResultadoTransaccion.IdRegistro = 0;
                    vResultadoTransaccion.ResultadoCodigo = 0;
                    vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", response.Count);
                    vResultadoTransaccion.dataList = response;
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
        public async Task<ResultadoTransaccion<BE_ConsolidadoPedido>> GetListDetalleConsolidado(int idconsolidado)
        {
            ResultadoTransaccion<BE_ConsolidadoPedido> vResultadoTransaccion = new ResultadoTransaccion<BE_ConsolidadoPedido>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new List<BE_ConsolidadoPedido>();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_DETALLE_CONSOLIDADO_POR_ID, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@idconsolidado", idconsolidado));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_ConsolidadoPedido>)context.ConvertTo<BE_ConsolidadoPedido>(reader);
                        }

                        conn.Close();
                    }

                    vResultadoTransaccion.IdRegistro = 0;
                    vResultadoTransaccion.ResultadoCodigo = 0;
                    vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", response.Count);
                    vResultadoTransaccion.dataList = response;
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
        public async Task<ResultadoTransaccion<BE_ConsolidadoPedidoPicking>> GetConsolidadoPedidoPickingPorId(int idconsolidadopicking)
        {
            ResultadoTransaccion<BE_ConsolidadoPedidoPicking> vResultadoTransaccion = new ResultadoTransaccion<BE_ConsolidadoPedidoPicking>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new BE_ConsolidadoPedidoPicking();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_DETALLE_CONSOLIDADO_PICKING_POR_ID, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@idconsolidadopicking", idconsolidadopicking));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = context.Convert<BE_ConsolidadoPedidoPicking>(reader);

                            if (response == null)
                            {
                                response = new BE_ConsolidadoPedidoPicking();
                            }
                        }

                        conn.Close();
                    }

                    vResultadoTransaccion.IdRegistro = 0;
                    vResultadoTransaccion.ResultadoCodigo = 0;
                    vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", 1);
                    vResultadoTransaccion.data = response;
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
        public async Task<ResultadoTransaccion<BE_ConsolidadoPedidoPicking>> RegistrarConsolidadoPicking(BE_ConsolidadoPedidoPicking item)
        {
            ResultadoTransaccion<BE_ConsolidadoPedidoPicking> vResultadoTransaccion = new ResultadoTransaccion<BE_ConsolidadoPedidoPicking>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    using (CommittableTransaction transaction = new CommittableTransaction())
                    {
                        await conn.OpenAsync();
                        conn.EnlistTransaction(transaction);
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(SP_INSERT_CONSOLIDADO_PICKING, conn))
                            {
                                cmd.Parameters.Clear();

                                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                                SqlParameter oParam = new SqlParameter("@idconsolidadopicking", item.idconsolidadopicking);
                                oParam.SqlDbType = SqlDbType.Int;
                                oParam.Direction = ParameterDirection.Output;
                                cmd.Parameters.Add(oParam);
                                cmd.Parameters.Add(new SqlParameter("@idconsolidado", item.idconsolidado));
                                cmd.Parameters.Add(new SqlParameter("@codpedido", item.codpedido));
                                cmd.Parameters.Add(new SqlParameter("@codproducto", item.codproducto));
                                cmd.Parameters.Add(new SqlParameter("@cantidad", item.cantidad));
                                cmd.Parameters.Add(new SqlParameter("@cantidadpicking", item.cantidadpicking));
                                cmd.Parameters.Add(new SqlParameter("@lote", item.lote));
                                cmd.Parameters.Add(new SqlParameter("@fechavencimiento", item.fechavencimiento));
                                cmd.Parameters.Add(new SqlParameter("@codalmacen", item.codalmacen));
                                cmd.Parameters.Add(new SqlParameter("@ubicacion", item.ubicacion));
                                cmd.Parameters.Add(new SqlParameter("@codusuarioapu", item.codusuarioapu));
                                cmd.Parameters.Add(new SqlParameter("@estado", item.estado));
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

                                await cmd.ExecuteNonQueryAsync();

                                item.idconsolidadopicking = (int)cmd.Parameters["@idconsolidadopicking"].Value;

                                vResultadoTransaccion.IdRegistro = item.idconsolidadopicking;
                                vResultadoTransaccion.ResultadoCodigo = int.Parse(outputIdTransaccionParam.Value.ToString());
                                vResultadoTransaccion.ResultadoDescripcion = (string)outputMsjTransaccionParam.Value;
                                vResultadoTransaccion.data = item;
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            vResultadoTransaccion.IdRegistro = -1;
                            vResultadoTransaccion.ResultadoCodigo = -1;
                            vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                        }
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
        public async Task<ResultadoTransaccion<BE_ConsolidadoPedidoPicking>> ModificarConsolidadoPicking(BE_ConsolidadoPedidoPicking item)
        {
            ResultadoTransaccion<BE_ConsolidadoPedidoPicking> vResultadoTransaccion = new ResultadoTransaccion<BE_ConsolidadoPedidoPicking>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    using (CommittableTransaction transaction = new CommittableTransaction())
                    {
                        await conn.OpenAsync();
                        conn.EnlistTransaction(transaction);
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand(SP_UPDATE_CONSOLIDADO_PICKING, conn))
                            {
                                cmd.Parameters.Clear();

                                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                                cmd.Parameters.Add(new SqlParameter("@idconsolidadopicking", item.idconsolidadopicking));
                                cmd.Parameters.Add(new SqlParameter("@cantidadpicking", item.cantidadpicking));
                                cmd.Parameters.Add(new SqlParameter("@codusuarioapu", item.codusuarioapu));
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

                                await cmd.ExecuteNonQueryAsync();

                                vResultadoTransaccion.IdRegistro = 0;
                                vResultadoTransaccion.ResultadoCodigo = int.Parse(outputIdTransaccionParam.Value.ToString());
                                vResultadoTransaccion.ResultadoDescripcion = (string)outputMsjTransaccionParam.Value;
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            vResultadoTransaccion.IdRegistro = -1;
                            vResultadoTransaccion.ResultadoCodigo = -1;
                            vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
                        }
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
        public async Task<ResultadoTransaccion<BE_ConsolidadoPedidoPicking>> EliminarConsolidadoPicking(BE_ConsolidadoPedidoPicking value)
        {
            ResultadoTransaccion<BE_ConsolidadoPedidoPicking> vResultadoTransaccion = new ResultadoTransaccion<BE_ConsolidadoPedidoPicking>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            using (SqlConnection conn = new SqlConnection(_cnx))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(SP_DELETE_CONSOLIDADO_PICKING, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@idconsolidadopicking", value.idconsolidadopicking));
                        cmd.Parameters.Add(new SqlParameter("@codusuarioapu", value.codusuarioapu));
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

                        vResultadoTransaccion.IdRegistro = int.Parse(value.idconsolidadopicking.ToString());
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
        public async Task<ResultadoTransaccion<BE_ConsolidadoPedidoPicking>> ModificarEstadoPedido(BE_ConsolidadoPedidoPicking item)
        {
            ResultadoTransaccion<BE_ConsolidadoPedidoPicking> vResultadoTransaccion = new ResultadoTransaccion<BE_ConsolidadoPedidoPicking>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            using (SqlConnection conn = new SqlConnection(_cnx))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand(SP_UPDATE_CONSOLIDADO_PICKING_ESTADO, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@idconsolidado", item.idconsolidado));
                        cmd.Parameters.Add(new SqlParameter("@estado", item.estado));
                        cmd.Parameters.Add(new SqlParameter("@codusuarioapu", item.codusuarioapu));
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

                        vResultadoTransaccion.IdRegistro = int.Parse(item.idconsolidado.ToString());
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
