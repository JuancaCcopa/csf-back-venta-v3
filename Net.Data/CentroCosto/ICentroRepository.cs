using Net.Business.Entities;
using Net.Connection;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface ICentroRepository
    {
        Task<ResultadoTransaccion<BE_Centro>> GetListCentroContains(string nombre);
        Task<ResultadoTransaccion<BE_Centro>> GetCentroPorCodigo(string codcentro);
    }
}
