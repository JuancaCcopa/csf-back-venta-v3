using Net.Business.Entities;
using Net.Connection;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface IComprobanteElectronicoRepository : IRepositoryBase<BE_TCIComprobanteElectronico>
    {
        Task<ResultadoTransaccion<BE_TCIComprobanteElectronico>> GetListComprobanteElectronicoPorFiltro(string codempresa, string codcomprobante, string codcomprobante_e, string codsistema, string tipocomp_sunat, int orden);
        Task<int> Registrar(string codcomprobante, string tipoCodigo_BarraHash, string tipoOtorgamiento, string Xml);
        Task<ResultadoTransaccion<BE_TCIComprobanteElectronico>> ModificarComprobanteElectronico(string campo, string nuevoValor, string XML, string codigo);
    }
}