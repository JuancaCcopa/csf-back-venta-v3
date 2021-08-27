using Net.Business.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface IVentaConfiguracion
    {
        Task<IEnumerable<BE_VentasConfiguracion>> GetByFiltros(BE_VentasConfiguracion value);
        Task<BE_VentasConfiguracion> GetbyId(BE_VentasConfiguracion value);
        Task<ResultadoTransaccion<BE_VentasConfiguracion>> Eliminar(BE_VentasConfiguracion value);
        Task<ResultadoTransaccion<BE_VentasConfiguracion>> Registrar(BE_VentasConfiguracion value);
        Task<ResultadoTransaccion<BE_VentasConfiguracion>> Modificar(BE_VentasConfiguracion value);
    }
}
