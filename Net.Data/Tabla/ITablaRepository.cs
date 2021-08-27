using Net.Business.Entities;
using Net.Connection;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface ITablaRepository : IRepositoryBase<BE_Tabla>
    {
        Task<ResultadoTransaccion<BE_Tabla>> GetTablasClinicaPorFiltros(string codtabla, string buscar, int key, int numerolineas, int orden);
        Task<ResultadoTransaccion<BE_Tabla>> GetTablaLogisticaPorFiltros(string codtabla, string buscar, int key, int numerolineas, int orden);
        Task<ResultadoTransaccion<BE_Tabla>> GetListTablaClinicaPorFiltros(string codtabla, string buscar, int key, int numerolineas, int orden);
        Task<ResultadoTransaccion<BE_Tabla>> GetListTablaLogisticaPorFiltros(string codtabla, string buscar, int key, int numerolineas, int orden);
        Task<ResultadoTransaccion<BE_Tabla>> GetTablasTCIWebService(string codtabla);
    }
}
