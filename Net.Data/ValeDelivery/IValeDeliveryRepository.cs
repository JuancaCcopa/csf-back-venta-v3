using Net.Business.Entities;
using Net.Connection;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface IValeDeliveryRepository : IRepositoryBase<BE_Receta>
    {
        Task<ResultadoTransaccion<BE_ValeDelivery>> GetListValeDeliveryPorCodAtencion(string codatencion);
        Task<ResultadoTransaccion<BE_ValeDelivery>> RegistrarValeDelivery(BE_ValeDeliveryXml value);
        Task<ResultadoTransaccion<BE_ValeDelivery>> ModificarValeDelivery(BE_ValeDeliveryXml value);
    }
}