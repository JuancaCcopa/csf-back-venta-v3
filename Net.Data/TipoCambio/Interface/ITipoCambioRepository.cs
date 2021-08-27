using Net.Business.Entities;
using Net.Connection;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface ITipoCambioRepository
    {
        Task<ResultadoTransaccion<BE_TipoCambio>> GetObtieneTipoCambio();
    }
}
