using Net.Business.Entities;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface IUsuarioRepository
    {
        Task<ResultadoTransaccion<BE_Usuario>> GetUsuarioPorFiltro(string codtabla, string buscar, int key, int numerolineas, int orden);
        Task<ResultadoTransaccion<BE_UsuarioPersona>> GetUsuarioPersonaPorFiltroNombre(string Nombre);
    }
}
