using Net.Business.Entities;
using Net.Connection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface IPickingRepository : IRepositoryBase<BE_Picking>
    {
        Task<ResultadoTransaccion<BE_Picking>> GetListPickingPorPedido(string codpedido);
        Task<ResultadoTransaccion<BE_Picking>> GetListPickingPorReceta(int id_receta);
        Task<ResultadoTransaccion<BE_Picking>> GetPickingPorId(int idpicking);
        Task<ResultadoTransaccion<BE_Picking>> Eliminar(BE_Picking value);
        Task<ResultadoTransaccion<BE_Picking>> Registrar(BE_Picking value);
        Task<ResultadoTransaccion<BE_Picking>> ModificarEstadoPedido(BE_Picking value);
        Task<ResultadoTransaccion<BE_Picking>> ModificarEstadoReceta(BE_Picking value);
    }
}