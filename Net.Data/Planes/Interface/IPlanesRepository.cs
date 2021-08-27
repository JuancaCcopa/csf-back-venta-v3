using Net.Business.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface IPlanesRepository
    {
        Task<ResultadoTransaccion<BE_Planes>> GetByFiltros(BE_Planes value);
        Task<ResultadoTransaccion<BE_Planes>> GetbyId(BE_Planes value);
        Task<ResultadoTransaccion<BE_Planes>> GetbyCodigo(BE_Planes value);
        Task<ResultadoTransaccion<BE_Planes>> Eliminar(BE_Planes value);
        Task<ResultadoTransaccion<BE_Planes>> Registrar(BE_Planes value);
        Task<ResultadoTransaccion<BE_Planes>> Modificar(BE_Planes value);

    }

}