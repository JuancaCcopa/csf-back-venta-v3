using Net.Business.Entities;
using Net.Connection;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface IHospitalRepository : IRepositoryBase<BE_HospitalDatos>
    {
        Task<ResultadoTransaccion<BE_HospitalDatos>> GetHospitalDatosPorAtencion(string codatencion);

        Task<ResultadoTransaccion<BE_HospitalExclusiones>> GetListHospitalExclusionesPorAtencion(string codatencion);

        Task<ResultadoTransaccion<BE_Hospital>> GetListHospitalPacienteClinicaPorFiltros(string pabellon, string piso, string local);
    }
}
