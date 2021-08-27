using Net.Business.Entities;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface IPlanillaRepository
    {
        Task<ResultadoTransaccion<string>> RegistrarPorUsuario(BE_Planilla value);
        Task<ResultadoTransaccion<BE_Planilla>> Modificar(BE_Planilla values);
        Task<ResultadoTransaccion<BE_Planilla>> Elminar(string numeroplanilla);
        Task<ResultadoTransaccion<BE_Planilla>> GetPlanillasPorFiltro(string buscar, int key, int numerolineas, int orden, string serie);
        Task<ResultadoTransaccion<BE_Planilla>> GetListaPlanillasPorFiltro(string buscar, int key, int numerolineas, int orden, string serie, string codcentro, string idusuario, string numeroPlanilla, string fechaInicio, string fechaFin);
    }
}
