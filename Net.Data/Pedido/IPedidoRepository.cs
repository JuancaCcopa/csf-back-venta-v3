using Net.Business.Entities;
using Net.Connection;
using System;
using System.Threading.Tasks;
namespace Net.Data
{
    public interface IPedidoRepository : IRepositoryBase<BE_Pedido>
    {
        Task<ResultadoTransaccion<BE_Pedido>> GetListaPedidosSeguimientoPorFiltro(DateTime fechainicio, DateTime fechaFin,string ccosto, int opcion);

        Task<ResultadoTransaccion<BE_Pedido>> GetListPedidosPorAtencion(string codatencion, string codtercero);
        Task<ResultadoTransaccion<BE_PedidoDetalle>> GetListPedidoDetallePorPedido(string codpedido);
        Task<ResultadoTransaccion<BE_Pedido>> GetListPedidosPorFiltro(DateTime fechainicio, DateTime fechafin, string codtipopedido, string codpedido);
        Task<ResultadoTransaccion<BE_Pedido>> GetListPedidosPorPedido(string codpedido);
        Task<ResultadoTransaccion<BE_Pedido>> GetDatosPedidoPorPedido(string codpedido);
    }
}