using Net.Business.Entities;
using Net.Connection;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface IConveniosRepository : IRepositoryBase<BE_ConveniosListaPrecio>
    {
        Task<ResultadoTransaccion<BE_ConveniosListaPrecio>> GetConveniosPorFiltros(string codalmacen, string tipomovimiento, string codtipocliente, string codcliente, string codpaciente, string codaseguradora, string codcia, string codproducto);
    }
}
