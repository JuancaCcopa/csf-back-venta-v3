using Net.Business.Entities;
using Net.Connection;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface IAseguradoraxProductoRepository : IRepositoryBase<BE_AseguradoraxProducto>
    {
        Task<ResultadoTransaccion<BE_AseguradoraxProducto>> GetListAseguradoraxProductoPorFiltros(string codaseguradora, string codproducto, int tipoatencion);
    }
}
