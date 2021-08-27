using Net.Business.Entities;
using Net.Connection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface IAtencionRepository : IRepositoryBase<BE_Atencion>
    {
        Task<IEnumerable<BE_Atencion>> GetListPacientePorFiltros(string opcion, string codpaciente, string nombres);
    }
}
