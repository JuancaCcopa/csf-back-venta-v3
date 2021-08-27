using Net.Business.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface ICuadreCajaRepository
    {
        Task<ResultadoTransaccion<BE_CuadreCaja>> GetListaCuadredeCajaPorFiltro(string documento);
        Task<ResultadoTransaccion<BE_CuadreCaja>> GetListaCuadredeCajaGeneralPorFiltro(string fecha1, string fecha2, string coddocumentoe, string codcentro, string orden);
        Task<ResultadoTransaccion<BE_CuadreCaja>> GetRpCuadreCajaPorFiltro(string fechainicio, string fechafin, string coduser, string codcentro, string numeroplanilla, decimal dolares);
        Task<ResultadoTransaccion<BE_CuadreCaja>> GetRpCuadreCajaDetalladoPorFiltro(string fechainicio, string fechafin, string coduserp, string codcentro, string numeroplanilla, string orden);
        Task<ResultadoTransaccion<string>> CuadreCajaRegistrar(List<BE_CuadreCaja> value);

    }
}
