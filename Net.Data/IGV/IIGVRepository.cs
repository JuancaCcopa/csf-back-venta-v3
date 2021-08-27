using Net.Business.Entities;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface IIGVRepository
    {
        Task<ResultadoTransaccion<BE_IGV>> GetIGVPorCodigo(string code);
    }
}
