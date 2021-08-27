using Net.Business.Entities;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Net.Http;
using Net.Connection.ServiceLayer;

namespace Net.Data
{
    public class WarehousesRepository: IWarehousesRepository
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ConnectionServiceLayer _connectServiceLayer;

        public WarehousesRepository(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
            _connectServiceLayer = new ConnectionServiceLayer(_configuration, _clientFactory);
        }
        public async Task<IEnumerable<BE_Warehouses>> GetListWarehousesContains(string warehouseName)
        {
            List<BE_Warehouses> data = await _connectServiceLayer.GetAsync<BE_Warehouses>("Warehouses?$select=WarehouseCode, WarehouseName&$filter=contains (WarehouseName , '" + warehouseName + "' )");
            return data;
        }

        public async Task<BE_Warehouses> GetWarehousesPorCodigo(string warehouseCode)
        {
            BE_Warehouses data = await _connectServiceLayer.GetAsyncTo<BE_Warehouses>("Warehouses?$select=WarehouseCode, WarehouseName&$filter=WarehouseCode eq '" + warehouseCode + "'");
            return data;
        }
    }
}
