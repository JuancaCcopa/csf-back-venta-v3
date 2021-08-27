using Net.Business.Entities;
using Net.Connection;
using System;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface ISeguimientoRepository : IRepositoryBase<BE_SeguimientoVenta>
    {
        Task<ResultadoTransaccion<BE_SeguimientoVenta>> SetUpdateSeguimiento(BE_SeguimientoXml value);
    }
}
