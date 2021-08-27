using Net.Business.Entities;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface IStockRepository
    {
        Task<ResultadoTransaccion<BE_Stock>> GetListStockPorFiltro(string codalmacen, string nombre, string codproducto, bool constock);
        Task<ResultadoTransaccion<BE_Stock>> GetListStockPorProductoAlmacen(string codalmacen, string codproducto);
        Task<ResultadoTransaccion<BE_StockLote>> GetListStockLotePorFiltro(string codalmacen, string codproducto, bool constock);
        Task<ResultadoTransaccion<BE_StockLote>> GetListStockUbicacionPorFiltro(string codalmacen, string codproducto, bool constock);
        Task<ResultadoTransaccion<BE_Stock>> GetListStockPorProducto(string codproducto, bool constock);
        Task<ResultadoTransaccion<BE_Stock>> GetListProductoGenericoPorCodigo(string codalmacen, string codprodci, bool constock);

        Task<ResultadoTransaccion<BE_Stock>> GetListProductoGenericoPorDCI(string codalmacen, string coddci, bool constock);
    }
}
