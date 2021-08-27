using Net.Business.Entities;
using Net.Connection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface IMedicoRepository : IRepositoryBase<BE_Medico>
    {
        Task<IEnumerable<BE_Medico>> GetListMedicoPorNombre(string nombre);
        Task<IEnumerable<BE_Medico>> GetListMedicoPorAtencion(string codAtencion);
    }
}
