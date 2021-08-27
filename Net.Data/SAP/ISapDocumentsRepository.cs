using Net.Business.Entities;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface ISapDocumentsRepository
    {
        Task<ResultadoTransaccion<SapDocument>> SetCreateDocument(BE_VentasCabecera value);
    }
}
