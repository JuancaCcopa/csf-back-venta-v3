using Net.Business.Entities;
using Net.Connection;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface IPacienteRepository : IRepositoryBase<BE_Paciente>
    {
        Task<ResultadoTransaccion<BE_Paciente>> GetPacientePorAtencion(string codAtencion);
    }
}