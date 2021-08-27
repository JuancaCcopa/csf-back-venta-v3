using Net.Business.Entities;
using Net.Connection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface ISeriePorMaquinaRepository: IRepositoryBase<BE_SeriePorMaquina>
    {
        Task<ResultadoTransaccion<BE_SeriePorMaquina>> GetListSeriePorMaquinaPorFiltro(BE_SeriePorMaquina value);
        Task<ResultadoTransaccion<BE_SeriePorMaquina>> GetSeriePorMaquinaPorId(int id);
        Task<ResultadoTransaccion<BE_SeriePorMaquina>> Registrar(BE_SeriePorMaquina value);
        Task<ResultadoTransaccion<BE_SeriePorMaquina>> Modificar(BE_SeriePorMaquina value);
        Task<ResultadoTransaccion<BE_SeriePorMaquina>> Eliminar(BE_SeriePorMaquina value);
    }
}
