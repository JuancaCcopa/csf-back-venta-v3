using Net.Business.Entities;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Net.Http;
using Net.Connection.ServiceLayer;
using System.Text.RegularExpressions;
using System;
using Microsoft.Data.SqlClient;
using Net.Connection;
using System.Linq;

namespace Net.Data
{
    public class ProductoRepository : RepositoryBase<BE_Producto>, IProductoRepository
    {
        private readonly string _cnx;
        private string _aplicacionName;
        private string _metodoName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ConnectionServiceLayer _connectServiceLayer;

        const string DB_ESQUEMA = "";
        const string SP_GET_ALTERNATIVO_POR_FILTRO = DB_ESQUEMA + "VEN_ListaAlternativoxProductoPorFiltroGet";

        public ProductoRepository(IHttpClientFactory clientFactory, IConfiguration configuration, IConnectionSQL context)
             : base(context)
        {
            _cnx = configuration.GetConnectionString("cnnSqlLogistica");
            _aplicacionName = this.GetType().Name;
            _configuration = configuration;
            _clientFactory = clientFactory;
            _connectServiceLayer = new ConnectionServiceLayer(_configuration, _clientFactory);
        }
        //public async Task<ResultadoTransaccion<BE_Producto>> GetListProductoPorFiltro(string codalmacen, string codigo, string nombre, string codaseguradora, string codcia, bool conStock = true)
        //{
        //    ResultadoTransaccion<BE_Producto> vResultadoTransaccion = new ResultadoTransaccion<BE_Producto>();
        //    _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

        //    vResultadoTransaccion.NombreMetodo = _metodoName;
        //    vResultadoTransaccion.NombreAplicacion = _aplicacionName;
        //    try
        //    {
        //        codigo = codigo == null ? "" : codigo.ToUpper();
        //        nombre = nombre == null ? "" : nombre.ToUpper();

        //        var cadena = "Items";
        //        var filter = "";
        //        var campos = "?$select=ItemCode, ItemName, U_SYP_CS_LABORATORIO, QuantityOnStock, AvgStdPrice, U_SYP_CS_FAMILIA, U_SYP_CS_SIC, U_SYP_CS_EABAS, U_SYP_CS_CLASIF, U_SYP_MONART";
        //        var filterConStock = " and QuantityOnStock gt 0";

        //        if (!string.IsNullOrEmpty(codigo))
        //        {
        //            filter = "&$filter =U_SYP_CS_SIC eq '" + codigo + "'";
        //        }

        //        if (string.IsNullOrEmpty(codigo) && !string.IsNullOrEmpty(nombre))
        //        {
        //            filter = "&$filter = contains (ItemName,'" + nombre + "')";
        //        }

        //        if (conStock)
        //        {
        //            cadena = cadena + campos + filter + filterConStock;
        //        }
        //        else
        //        {
        //            cadena = cadena + campos + filter;
        //        }

        //        List<BE_Producto> data = await _connectServiceLayer.GetAsync<BE_Producto>(cadena);

        //        if (!string.IsNullOrEmpty(codaseguradora) && !string.IsNullOrEmpty(codcia))
        //        {
        //            for (int i = 0; i < data.Count; i++)
        //            {
        //                var dataProducto = data[i];
        //                ResultadoTransaccion<BE_Producto> dataAlternativo = await GetListProductoAlternativoPorCodigo(dataProducto.codproducto, codaseguradora, codcia);

        //                if (((List<BE_Producto>)dataAlternativo.dataList).Count > 0)
        //                {
        //                    data[i].flgrestringido = true;
        //                }
        //            }
        //        }

        //        vResultadoTransaccion.IdRegistro = 0;
        //        vResultadoTransaccion.ResultadoCodigo = 0;
        //        vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", data.Count);
        //        vResultadoTransaccion.dataList = data;
        //    }
        //    catch (Exception ex)
        //    {
        //        vResultadoTransaccion.IdRegistro = -1;
        //        vResultadoTransaccion.ResultadoCodigo = -1;
        //        vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
        //    }

        //    return vResultadoTransaccion;

        //}
        //public async Task<ResultadoTransaccion<BE_Producto>> GetListProductoGenericoPorCodigo(string codigo, bool conStock = true)
        //{
        //    ResultadoTransaccion<BE_Producto> vResultadoTransaccion = new ResultadoTransaccion<BE_Producto>();
        //    _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

        //    vResultadoTransaccion.NombreMetodo = _metodoName;
        //    vResultadoTransaccion.NombreAplicacion = _aplicacionName;
        //    try
        //    {
        //        codigo = codigo == null ? "" : codigo.ToUpper();

        //        var cadena = "Items";
        //        var filter = "";
        //        var campos = "?$select=ItemCode, ItemName, U_SYP_CS_LABORATORIO, QuantityOnStock, AvgStdPrice, U_SYP_CS_FAMILIA, U_SYP_CS_SIC, U_SYP_CS_EABAS, U_SYP_CS_CLASIF, U_SYP_MONART";
        //        var filterConStock = " and QuantityOnStock gt 0";

        //        if (!string.IsNullOrEmpty(codigo))
        //        {
        //            filter = "&$filter = U_SYP_CS_FAMILIA eq '" + codigo + "'";
        //        }

        //        if (conStock)
        //        {
        //            cadena = cadena + campos + filter + filterConStock;
        //        }
        //        else
        //        {
        //            cadena = cadena + campos + filter;
        //        }

        //        List<BE_Producto> data = await _connectServiceLayer.GetAsync<BE_Producto>(cadena);

        //        vResultadoTransaccion.IdRegistro = 0;
        //        vResultadoTransaccion.ResultadoCodigo = 0;
        //        vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", data.Count);
        //        vResultadoTransaccion.dataList = data;
        //    }
        //    catch (Exception ex)
        //    {
        //        vResultadoTransaccion.IdRegistro = -1;
        //        vResultadoTransaccion.ResultadoCodigo = -1;
        //        vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
        //    }

        //    return vResultadoTransaccion;

        //}

        public async Task<ResultadoTransaccion<BE_Producto>> GetListProductoAlternativoPorCodigo(string codproducto, string codaseguradora, string codcia)
        {
            ResultadoTransaccion<BE_Producto> vResultadoTransaccion = new ResultadoTransaccion<BE_Producto>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx))
                {
                    var response = new List<BE_Producto>();

                    using (SqlCommand cmd = new SqlCommand(SP_GET_ALTERNATIVO_POR_FILTRO, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codproducto", codproducto));
                        cmd.Parameters.Add(new SqlParameter("@codaseguradora", codaseguradora));
                        cmd.Parameters.Add(new SqlParameter("@codcia", codcia));

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = (List<BE_Producto>)context.ConvertTo<BE_Producto>(reader);
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

        public async Task<ResultadoTransaccion<BE_Producto>> GetProductoPorCodigo(string codalmacen, string codproducto, string codaseguradora, string codcia, string tipomovimiento, string codtipocliente, string codcliente, string codpaciente, int tipoatencion)
        {
            ResultadoTransaccion<BE_Producto> vResultadoTransaccion = new ResultadoTransaccion<BE_Producto>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {
                codproducto = codproducto == null ? "" : codproducto.ToUpper();

                var cadena = "Items";
                var filter = "&$filter=ItemCode eq '" + codproducto + "'";
                var campos = "?$select=ItemCode, ItemName, U_SYP_CS_LABORATORIO, U_SYP_FAMILIA, U_SYP_CS_EABAS, U_SYP_CS_CLASIF, U_SYP_MONART, ItemsGroupCode, ManageBatchNumbers, ArTaxCode, Properties1, U_SYP_CS_DCTO, U_SYP_CS_FRVTA, U_SYP_CS_PRODCI ";

                cadena = cadena + campos + filter;

                List<BE_Producto> listProductos = await _connectServiceLayer.GetAsync<BE_Producto>(cadena);

                if (listProductos.Any())
                {

                    BE_Producto data = listProductos[0];

                    if (!string.IsNullOrEmpty(codaseguradora) && !string.IsNullOrEmpty(codcia))
                    {
                        ResultadoTransaccion<BE_Producto> dataAlternativo = await GetListProductoAlternativoPorCodigo(codproducto, codaseguradora, codcia);

                        if (dataAlternativo.ResultadoCodigo == -1)
                        {
                            vResultadoTransaccion.IdRegistro = -1;
                            vResultadoTransaccion.ResultadoCodigo = -1;
                            vResultadoTransaccion.ResultadoDescripcion = dataAlternativo.ResultadoDescripcion;
                            return vResultadoTransaccion;
                        }

                        if (((List<BE_Producto>)dataAlternativo.dataList).Count > 0)
                        {
                            data.flgrestringido = true;
                        }
                    }

                    IGVRepository iGVRepository = new IGVRepository(_clientFactory, _configuration);
                    ResultadoTransaccion<BE_IGV> resultadoTransaccionIGV = await iGVRepository.GetIGVPorCodigo(data.ArTaxCode);

                    if (resultadoTransaccionIGV.ResultadoCodigo == -1)
                    {
                        vResultadoTransaccion.IdRegistro = -1;
                        vResultadoTransaccion.ResultadoCodigo = -1;
                        vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionIGV.ResultadoDescripcion;
                        return vResultadoTransaccion;
                    }

                    if (((List<BE_IGV>)resultadoTransaccionIGV.dataList).Count > 0)
                    {
                        data.valorIGV = ((List<BE_IGV>)resultadoTransaccionIGV.dataList)[0].Rate;
                    }
                    else
                    {
                        data.valorIGV = 0;
                    }

                    StockRepository stockRepository = new StockRepository(_clientFactory, _configuration);
                    ResultadoTransaccion<BE_Stock> resultadoTransaccionStock = await stockRepository.GetListStockPorProductoAlmacen(codalmacen, codproducto);

                    if (resultadoTransaccionStock.ResultadoCodigo == -1)
                    {
                        vResultadoTransaccion.IdRegistro = -1;
                        vResultadoTransaccion.ResultadoCodigo = -1;
                        vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionStock.ResultadoDescripcion;
                        return vResultadoTransaccion;
                    }

                    if (resultadoTransaccionStock.dataList.Any())
                    {
                        BE_Stock stockProducto = ((List<BE_Stock>)resultadoTransaccionStock.dataList)[0];

                        data.ProductoStock = stockProducto.OnHandALM;
                        data.valorVVP = (decimal)stockProducto.Price;
                    }

                    ConveniosRepository conveniosRepository = new ConveniosRepository(_clientFactory, context, _configuration);

                    ResultadoTransaccion<BE_ConveniosListaPrecio> resultadoTransaccionConvenios = new ResultadoTransaccion<BE_ConveniosListaPrecio>();

                    if (codtipocliente.Equals("01"))
                    {
                        resultadoTransaccionConvenios = await conveniosRepository.GetConveniosPorFiltros(null, tipomovimiento, codtipocliente, null, codpaciente, codaseguradora, codcia, codproducto);
                    }
                    else
                    {
                        resultadoTransaccionConvenios = await conveniosRepository.GetConveniosPorFiltros(null, tipomovimiento, codtipocliente, null, null, null, null, codproducto);
                    }

                    if (resultadoTransaccionConvenios.ResultadoCodigo == -1)
                    {
                        vResultadoTransaccion.IdRegistro = -1;
                        vResultadoTransaccion.ResultadoCodigo = -1;
                        vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionConvenios.ResultadoDescripcion;
                        return vResultadoTransaccion;
                    }

                    if (((List<BE_ConveniosListaPrecio>)resultadoTransaccionConvenios.dataList).Count == 0)
                    {
                        ListaPrecioRepository listaPrecioRepository = new ListaPrecioRepository(_clientFactory, _configuration);
                        ResultadoTransaccion<BE_ListaPrecio> resultadoTransaccionListaPrecio = await listaPrecioRepository.GetPrecioPorCodigo(codproducto, 1);

                        if (resultadoTransaccionListaPrecio.ResultadoCodigo == -1)
                        {
                            vResultadoTransaccion.IdRegistro = -1;
                            vResultadoTransaccion.ResultadoCodigo = -1;
                            vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionListaPrecio.ResultadoDescripcion;
                            return vResultadoTransaccion;
                        }

                        BE_ListaPrecio precio = ((List<BE_ListaPrecio>)resultadoTransaccionListaPrecio.dataList)[0];

                        if (((List<BE_ListaPrecio>)resultadoTransaccionListaPrecio.dataList).Count > 0)
                        {
                            var datap = precio.Price == null ? 0 : precio.Price;

                            data.valorVVP = (decimal)datap;
                        }
                        else
                        {
                            data.valorVVP = 0;
                        }

                    }
                    else
                    {
                        BE_ConveniosListaPrecio conveniosListaPrecio = ((List<BE_ConveniosListaPrecio>)resultadoTransaccionConvenios.dataList)[0];

                        if (!conveniosListaPrecio.monto.Equals(0))
                        {
                            data.valorVVP = decimal.Parse(conveniosListaPrecio.monto.ToString());
                        }
                        data.FlgConvenio = true;
                    }

                    VentaRepository ventaRepository = new VentaRepository(_clientFactory, context, _configuration);
                    ResultadoTransaccion<bool> resultadoTransaccionGastoCubierto = await ventaRepository.GetGastoCubiertoPorFiltro(codaseguradora, codproducto, 1);

                    data.GastoCubierto = false;

                    if (resultadoTransaccionGastoCubierto.ResultadoCodigo == -1)
                    {
                        vResultadoTransaccion.IdRegistro = -1;
                        vResultadoTransaccion.ResultadoCodigo = -1;
                        vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionGastoCubierto.ResultadoDescripcion;
                        return vResultadoTransaccion;
                    }

                    data.GastoCubierto = resultadoTransaccionGastoCubierto.data;

                    ResultadoTransaccion<BE_Producto> resultadoTransaccionListProductoAlternativo = await GetListProductoAlternativoPorCodigo(codproducto, codaseguradora, codcia);

                    if (resultadoTransaccionListProductoAlternativo.ResultadoCodigo == -1)
                    {
                        vResultadoTransaccion.IdRegistro = -1;
                        vResultadoTransaccion.ResultadoCodigo = -1;
                        vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionListProductoAlternativo.ResultadoDescripcion;
                        return vResultadoTransaccion;
                    }

                    data.ProductoRestringido = false;

                    if (resultadoTransaccionListProductoAlternativo.dataList.Any())
                    {
                        data.ProductoRestringido = true;
                    }

                    vResultadoTransaccion.IdRegistro = 0;
                    vResultadoTransaccion.ResultadoCodigo = 0;
                    vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", 1);
                    vResultadoTransaccion.data = data;
                    vResultadoTransaccion.dataList = listProductos;
                }
                else
                {
                    vResultadoTransaccion.IdRegistro = 0;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = string.Format("Producto no existe", 0);
                    vResultadoTransaccion.data = null;
                    vResultadoTransaccion.dataList = listProductos;
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

        public async Task<ResultadoTransaccion<BE_Producto>> GetProductoyStockAlmacenesPorCodigo(string codproducto)
        {
            ResultadoTransaccion<BE_Producto> vResultadoTransaccion = new ResultadoTransaccion<BE_Producto>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {
                codproducto = codproducto == null ? "" : codproducto.ToUpper();

                var cadena = "Items";
                var filter = "&$filter=ItemCode eq '" + codproducto + "'";
                var campos = "?$select=ItemCode, ItemName, U_SYP_CS_LABORATORIO, U_SYP_FAMILIA, U_SYP_CS_EABAS, U_SYP_CS_CLASIF, U_SYP_MONART, ItemsGroupCode, Properties1, ManageBatchNumbers, U_SYP_CS_PRODCI ";

                cadena = cadena + campos + filter;

                List<BE_Producto> listproducto = await _connectServiceLayer.GetAsync<BE_Producto>(cadena);

                BE_Producto data = new BE_Producto();

                if (listproducto.Count > 0)
                {

                    data = listproducto[0];

                    StockRepository stockRepository = new StockRepository(_clientFactory, _configuration);
                    ResultadoTransaccion<BE_Stock> resultadoTransaccionStock = await stockRepository.GetListStockPorProducto(codproducto, true);

                    if (resultadoTransaccionStock.ResultadoCodigo == -1)
                    {
                        vResultadoTransaccion.IdRegistro = -1;
                        vResultadoTransaccion.ResultadoCodigo = -1;
                        vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionStock.ResultadoDescripcion;

                        return vResultadoTransaccion;
                    }

                    data.ListStockAlmacen = (List<BE_Stock>)resultadoTransaccionStock.dataList;
                } else
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = "Producto no existe en SAP HANA";
                    return vResultadoTransaccion;
                }

                vResultadoTransaccion.IdRegistro = 0;
                vResultadoTransaccion.ResultadoCodigo = 0;
                vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", 1);
                vResultadoTransaccion.data = data;
            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return vResultadoTransaccion;
        }

        public async Task<ResultadoTransaccion<BE_Producto>> GetListDetalleProductoPorPedido(string codpedido, string codalmacen, string codaseguradora, string codcia, string tipomovimiento, string codtipocliente, string codcliente, string codpaciente, int tipoatencion)
        {
            ResultadoTransaccion<BE_Producto> vResultadoTransaccion = new ResultadoTransaccion<BE_Producto>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {

                List<BE_Producto> listProductos = new List<BE_Producto>();

                PedidoRepository pedidoRepository = new PedidoRepository(context, _configuration);
                ResultadoTransaccion<BE_PedidoDetalle> resultadoTransaccionDetallePedido = await pedidoRepository.GetListPedidoDetallePorPedido(codpedido);

                if (resultadoTransaccionDetallePedido.ResultadoCodigo == -1)
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionDetallePedido.ResultadoDescripcion;
                    return vResultadoTransaccion;
                }

                if (resultadoTransaccionDetallePedido.dataList.Any())
                {

                    List<BE_PedidoDetalle> listPedidoDetalle = (List<BE_PedidoDetalle>)resultadoTransaccionDetallePedido.dataList;


                    foreach (BE_PedidoDetalle item in listPedidoDetalle)
                    {
                        ResultadoTransaccion<BE_Producto> resultadoTransaccionProducto = await GetProductoPorCodigo(codalmacen, item.codproducto, codaseguradora, codcia, tipomovimiento, codtipocliente, codcliente, codpaciente, tipoatencion);

                        if (resultadoTransaccionProducto.ResultadoCodigo == -1 && resultadoTransaccionProducto.IdRegistro == -1)
                        {
                            vResultadoTransaccion.IdRegistro = -1;
                            vResultadoTransaccion.ResultadoCodigo = -1;
                            vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionProducto.ResultadoDescripcion;
                            return vResultadoTransaccion;
                        }

                        if (resultadoTransaccionProducto.IdRegistro == 0 && resultadoTransaccionProducto.ResultadoCodigo == -1)
                        {

                        } else
                        {
                            resultadoTransaccionProducto.data.CantidadPedido = (decimal)item.cantidad;
                            resultadoTransaccionProducto.data.CodPedido = item.codpedido;

                            listProductos.Add(resultadoTransaccionProducto.data);
                        }

                    }

                    vResultadoTransaccion.IdRegistro = 0;
                    vResultadoTransaccion.ResultadoCodigo = 0;
                    vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", listProductos.Count);
                    vResultadoTransaccion.dataList = listProductos;

                } 
                else
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = "No existe detalle para el pedido nro. " + codpedido;
                    return vResultadoTransaccion;
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
        public async Task<ResultadoTransaccion<BE_Producto>> GetListDetalleProductoPorReceta(int idereceta, string codalmacen, string codaseguradora, string codcia, string tipomovimiento, string codtipocliente, string codcliente, string codpaciente, int tipoatencion)
        {
            ResultadoTransaccion<BE_Producto> vResultadoTransaccion = new ResultadoTransaccion<BE_Producto>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {

                List<BE_Producto> listProductos = new List<BE_Producto>();

                RecetaRepository recetaRepository = new RecetaRepository(context, _configuration);
                ResultadoTransaccion<BE_RecetaDetalle> resultadoTransaccionDetalleReceta = await recetaRepository.GetListRecetaDetallePorReceta(idereceta);

                if (resultadoTransaccionDetalleReceta.ResultadoCodigo == -1)
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionDetalleReceta.ResultadoDescripcion;
                    return vResultadoTransaccion;
                }

                if (resultadoTransaccionDetalleReceta.dataList.Any())
                {

                    List<BE_RecetaDetalle> lisDetalleReceta = (List<BE_RecetaDetalle>)resultadoTransaccionDetalleReceta.dataList;


                    foreach (BE_RecetaDetalle item in lisDetalleReceta)
                    {
                        ResultadoTransaccion<BE_Producto> resultadoTransaccionProducto = await GetProductoPorCodigo(codalmacen, item.codproducto, codaseguradora, codcia, tipomovimiento, codtipocliente, codcliente, codpaciente, tipoatencion);

                        if (resultadoTransaccionProducto.ResultadoCodigo == -1 && resultadoTransaccionProducto.IdRegistro == -1)
                        {
                            vResultadoTransaccion.IdRegistro = -1;
                            vResultadoTransaccion.ResultadoCodigo = -1;
                            vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionProducto.ResultadoDescripcion;
                            return vResultadoTransaccion;
                        }

                        if (resultadoTransaccionProducto.IdRegistro == 0 && resultadoTransaccionProducto.ResultadoCodigo == -1)
                        {

                        }
                        else
                        {
                            //resultadoTransaccionProducto.data.CantidadPedido = (decimal)item.cantidad;
                            //resultadoTransaccionProducto.data.CodPedido = item.codpedido;

                            listProductos.Add(resultadoTransaccionProducto.data);
                        }

                    }

                    vResultadoTransaccion.IdRegistro = 0;
                    vResultadoTransaccion.ResultadoCodigo = 0;
                    vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", listProductos.Count);
                    vResultadoTransaccion.dataList = listProductos;

                }
                else
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = "No existe detalle para la receta nro. " + idereceta.ToString();
                    return vResultadoTransaccion;
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
