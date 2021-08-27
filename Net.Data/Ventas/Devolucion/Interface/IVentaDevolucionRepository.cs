using Net.Business.Entities;
using Net.Connection;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface IVentaDevolucionRepository : IRepositoryBase<BE_VentasDevolucion>
    {
        Task<ResultadoTransaccion<BE_VentasDevolucion>> GetVentasPorAtencion(BE_VentasDevolucion value);
    }
}
