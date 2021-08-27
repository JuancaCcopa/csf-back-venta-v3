using Net.Business.Entities;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface IListaPrecioRepository
    {
        Task<ResultadoTransaccion<BE_ListaPrecio>> GetPrecioPorCodigo(string codproducto, int pricelist);
    }
}
