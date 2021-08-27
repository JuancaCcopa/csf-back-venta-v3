using Net.Business.Entities;
using Net.Connection;
using System;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface IRecetaRepository : IRepositoryBase<BE_Receta>
    {
        Task<ResultadoTransaccion<BE_Receta>> GetListRecetasPorFiltro(DateTime fechainicio, DateTime fechafin, string codtipoconsultamedica, int ide_receta, string nombrespaciente, string sbaestadoreceta);
        Task<ResultadoTransaccion<BE_Receta>> GetListRecetasPorReceta(int ide_receta);
        Task<ResultadoTransaccion<BE_RecetaDetalle>> GetListRecetaDetallePorReceta(int ide_receta);
        Task<ResultadoTransaccion<BE_RecetaObservacion>> GetListRecetasObservacionPorReceta(int ide_receta);
        Task<ResultadoTransaccion<BE_RecetaObservacion>> RegistrarRecetasObservacion(BE_RecetaObservacionXml value);
        Task<ResultadoTransaccion<BE_RecetaObservacion>> ModificarRecetasObservacion(BE_RecetaObservacion value);
    }
}
