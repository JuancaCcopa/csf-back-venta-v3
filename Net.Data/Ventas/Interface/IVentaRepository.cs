using Net.Business.Entities;
using Net.Connection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface IVentaRepository : IRepositoryBase<BE_VentasCabecera>
    {
        Task<ResultadoTransaccion<BE_VentasCabecera>> GetAll(string codcomprobante, string codventa, DateTime fecinicio, DateTime fecfin);
        Task<ResultadoTransaccion<BE_VentasCabecera>> GetAllSinStock(string codcomprobante, string codventa, DateTime fecinicio, DateTime fecfin);
        Task<ResultadoTransaccion<BE_VentasCabecera>> GetVentaPorCodVenta(string codventa);
        Task<ResultadoTransaccion<BE_VentasCabecera>> GetVentaCabeceraPendientePorFiltro(DateTime fecha);
        Task<ResultadoTransaccion<BE_VentasDetalle>> GetVentaChequea1MesPorFiltro(string codpaciente, int cuantosmesesantes);
        Task<ResultadoTransaccion<BE_VentasCabecera>> ModificarVentaCabeceraEnvioPiso(BE_VentasCabecera value);
        Task<ResultadoTransaccion<BE_VentasCabecera>> ValidacionRegistraVentaCabecera(BE_VentasCabecera value);
        Task<ResultadoTransaccion<BE_VentasGenerado>> RegistrarVentaCabecera(BE_VentasCabecera value, bool ventaAutomatica);
        Task<ResultadoTransaccion<BE_VentasGenerado>> RegistrarVentaDevolucion(BE_VentaXml value);
        Task<ResultadoTransaccion<Boolean>> GetGastoCubiertoPorFiltro(string codaseguradora, string codproducto, int tipoatencion);
        Task<ResultadoTransaccion<BE_VentasDetalle>> GetVentasChequea1MesAntes(string codpaciente, int cuantosmesesantes);
        Task<ResultadoTransaccion<BE_VentasCabecera>> ValidacionAnularVenta(BE_VentasCabecera value);
        Task<ResultadoTransaccion<BE_VentasCabecera>> RegistrarAnularVenta(BE_VentasCabecera value);
        Task<ResultadoTransaccion<bool>> GeneraVentaAutomatica(string codpedido);
        Task<ResultadoTransaccion<MemoryStream>> GenerarValeVentaPrint(string codventa);
        Task<ResultadoTransaccion<MemoryStream>> GenerarValeVentaLotePrint(string codventa);
        Task<ResultadoTransaccion<bool>> UpdateSAPVenta(BE_VentasCabecera value);
        Task<ResultadoTransaccion<bool>> UpdateSinStockVenta(BE_VentasCabecera value);
    }
}
