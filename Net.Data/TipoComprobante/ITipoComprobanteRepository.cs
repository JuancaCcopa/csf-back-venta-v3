using Net.Business.Entities;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface ITipoComprobanteRepository
    {

        Task<ResultadoTransaccion<BE_TipoComprobante>> VentasTipoComprobantes();
        Task<ResultadoTransaccion<BE_TipoComprobante>> getSeriePorCodDocumento(string code);


    }
}
