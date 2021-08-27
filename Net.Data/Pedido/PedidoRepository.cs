using Net.Business.Entities;
using Net.Connection;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System;
using System.Text.RegularExpressions;
using Net.CrossCotting;

namespace Net.Data
{
    public class PedidoRepository : RepositoryBase<BE_Pedido>, IPedidoRepository
    {
        private readonly string _cnx;

        private string _aplicacionName;
        private string _metodoName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        const string DB_ESQUEMA = "";
        const string SP_GET_PEDIDOS_SEGUIMIENTO_POR_FILTRO = DB_ESQUEMA + "VEN_ListaSeguimientoPedidosPorFiltrosGet";
        const string SP_GET_PEDIDOS_POR_ATENCION = DB_ESQUEMA + "VEN_ListaPedidosPorAtencionGet";
        const string SP_GET_PEDIDODETALLE_POR_PEDIDO = DB_ESQUEMA + "VEN_ListaPedidoDetallePorPedidoGet";
        const string SP_GET_PEDIDO_POR_FILTRO = DB_ESQUEMA + "VEN_ListaPedidosPorFiltrosGet";
        const string SP_GET_DATOS_PEDIDO_POR_PEDIDO = DB_ESQUEMA + "VEN_DatosPedidoPorPedidoGet";
        const string SP_GET_LIST_PEDIDOS_VENTA_AUTOMATICA = DB_ESQUEMA + "VEN_ListaPedidoVentaAutomaticaGet";

        public PedidoRepository(IConnectionSQL context, IConfiguration configuration)
            : base(context)
        {
            _aplicacionName = this.GetType().Name;
            _cnx = configuration.GetConnectionString("cnnSqlLogistica");
        }

        public async Task<ResultadoTransaccion<BE_Pedido>> GetListaPedidosSeguimientoPorFiltro(DateTime fechainicio, DateTime fechafin, string ccosto, int opcion)
        {
            ResultadoTransaccion<BE_Pedido> vResultadoTransaccion = new ResultadoTransaccion<BE_Pedido>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            fechainicio = Utilidades.GetFechaHoraInicioActual(fechainicio);
            fechafin = Utilidades.GetFechaHoraFinActual(fechafin);

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new List<BE_Pedido>();
                    using (SqlCommand cmd = new SqlCommand(SP_GET_PEDIDOS_SEGUIMIENTO_POR_FILTRO, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@fechaini", fechainicio));
                        cmd.Parameters.Add(new SqlParameter("@fechafin", fechafin));
                        cmd.Parameters.Add(new SqlParameter("@ccosto", ccosto));
                        cmd.Parameters.Add(new SqlParameter("@opcion", opcion));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_Pedido>)context.ConvertTo<BE_Pedido>(reader);
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

        public async Task<ResultadoTransaccion<BE_Pedido>> GetListPedidosPorAtencion(string codatencion, string codtercero)
        {
            ResultadoTransaccion<BE_Pedido> vResultadoTransaccion = new ResultadoTransaccion<BE_Pedido>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new List<BE_Pedido>();
                    using (SqlCommand cmd = new SqlCommand(SP_GET_PEDIDOS_POR_ATENCION, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codatencion", codatencion));
                        cmd.Parameters.Add(new SqlParameter("@codtercero", codtercero));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_Pedido>)context.ConvertTo<BE_Pedido>(reader);
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

        public async Task<ResultadoTransaccion<BE_PedidoDetalle>> GetListPedidoDetallePorPedido(string codpedido)
        {
            ResultadoTransaccion<BE_PedidoDetalle> vResultadoTransaccion = new ResultadoTransaccion<BE_PedidoDetalle>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new List<BE_PedidoDetalle>();
                    using (SqlCommand cmd = new SqlCommand(SP_GET_PEDIDODETALLE_POR_PEDIDO, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codpedido", codpedido));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_PedidoDetalle>)context.ConvertTo<BE_PedidoDetalle>(reader);
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

        public async Task<ResultadoTransaccion<BE_Pedido>> GetListPedidosPorFiltro(DateTime fechainicio, DateTime fechafin, string codtipopedido, string codpedido)
        {
            ResultadoTransaccion<BE_Pedido> vResultadoTransaccion = new ResultadoTransaccion<BE_Pedido>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            fechainicio = Utilidades.GetFechaHoraInicioActual(fechainicio);
            fechafin = Utilidades.GetFechaHoraFinActual(fechafin);

            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new List<BE_Pedido>();
                    using (SqlCommand cmd = new SqlCommand(SP_GET_PEDIDO_POR_FILTRO, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@fechainicio", fechainicio));
                        cmd.Parameters.Add(new SqlParameter("@fechafin", fechafin));
                        cmd.Parameters.Add(new SqlParameter("@codtipopedido", codtipopedido == null ? string.Empty : codtipopedido));
                        cmd.Parameters.Add(new SqlParameter("@codpedido", codpedido == null ? string.Empty : codpedido));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_Pedido>)context.ConvertTo<BE_Pedido>(reader);
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

        public async Task<ResultadoTransaccion<BE_Pedido>> GetDatosPedidoPorPedido(string codpedido)
        {
            ResultadoTransaccion<BE_Pedido> vResultadoTransaccion = new ResultadoTransaccion<BE_Pedido>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new List<BE_Pedido>();
                    using (SqlCommand cmd = new SqlCommand(SP_GET_DATOS_PEDIDO_POR_PEDIDO, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codpedido", codpedido));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_Pedido>)context.ConvertTo<BE_Pedido>(reader);
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

        public async Task<ResultadoTransaccion<BE_Pedido>> GetDatosPedidoPorPedido(SqlConnection conn, string codpedido)
        {
            ResultadoTransaccion<BE_Pedido> vResultadoTransaccion = new ResultadoTransaccion<BE_Pedido>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {
                //using (SqlConnection conn = new SqlConnection(_cnx))
                //{
                var response = new List<BE_Pedido>();
                using (SqlCommand cmd = new SqlCommand(SP_GET_DATOS_PEDIDO_POR_PEDIDO, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@codpedido", codpedido));

                    //conn.Open();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        response = (List<BE_Pedido>)context.ConvertTo<BE_Pedido>(reader);
                    }

                    //conn.Close();

                    vResultadoTransaccion.IdRegistro = 0;
                    vResultadoTransaccion.ResultadoCodigo = 0;
                    vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", response.Count);
                    vResultadoTransaccion.dataList = response;
                }
                //}
            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return vResultadoTransaccion;

        }

        public async Task<ResultadoTransaccion<BE_Pedido>> GetListPedidosPorPedido(string codpedido)
        {
            ResultadoTransaccion<BE_Pedido> vResultadoTransaccion = new ResultadoTransaccion<BE_Pedido>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new List<BE_Pedido>();
                    using (SqlCommand cmd = new SqlCommand(SP_GET_PEDIDO_POR_FILTRO, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codpedido", codpedido));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_Pedido>)context.ConvertTo<BE_Pedido>(reader);
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

        public async Task<ResultadoTransaccion<BE_Pedido>> GetListaPedidoVentaAutomatica(string codpedido)
        {
            ResultadoTransaccion<BE_Pedido> vResultadoTransaccion = new ResultadoTransaccion<BE_Pedido>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new List<BE_Pedido>();
                    using (SqlCommand cmd = new SqlCommand(SP_GET_LIST_PEDIDOS_VENTA_AUTOMATICA, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codpedido", codpedido));
                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_Pedido>)context.ConvertTo<BE_Pedido>(reader);
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
