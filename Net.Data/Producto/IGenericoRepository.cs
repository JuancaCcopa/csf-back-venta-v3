using Net.Business.Entities;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface IGenericoRepository
    {
        Task<ResultadoTransaccion<BE_Generico>> GetListGenericoPorFiltro(string code, string name);

        Task<ResultadoTransaccion<BE_Generico>> GetListGenericoPorProDCI(string codprodci);
    }
}
