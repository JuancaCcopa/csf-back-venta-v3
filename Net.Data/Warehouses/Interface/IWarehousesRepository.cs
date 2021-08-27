using Net.Business.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface IWarehousesRepository
    {
        Task<IEnumerable<BE_Warehouses>> GetListWarehousesContains(string warehouseName);
        Task<BE_Warehouses> GetWarehousesPorCodigo(string warehouseCode);
    }
}
