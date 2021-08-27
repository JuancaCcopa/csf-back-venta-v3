using Net.Business.Entities;
using Net.Connection;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface ISerieRepository : IRepositoryBase<BE_Serie>
    {
        Task<ResultadoTransaccion<BE_Serie>> GetListSeriePorTipoSerie(string tiposerie);
        Task<ResultadoTransaccion<BE_SerieConfig>> GetListConfigDocumentoPorNombreMaquina(string nombremaquina);
        Task<ResultadoTransaccion<BE_Serie>> Registrar(BE_Serie value);
        Task<ResultadoTransaccion<BE_SerieConfig>> GetCorrelativo();
    }
}
