namespace Net.Data
{
    public interface IRepositoryWrapper
    {
        IEmailSenderRepository EmailSender { get; }
        IVentaRepository Venta { get; }
        ITipoCambioRepository TipoCambio { get; }
        IPlanesRepository Planes { get; }
        IWarehousesRepository Warehouses { get; }
        IAtencionRepository Atencion { get; }
        IClienteRepository Cliente { get; }
        IPersonalClinicaRepository PersonalClinica { get; }
        IMedicoRepository Medico { get; }
        IPacienteRepository Paciente { get;  }
        IComprobanteRepository Comprobante { get; }
        IPedidoRepository Pedido { get; }
        IProductoRepository Producto { get; }
        ISeriePorMaquinaRepository SeriePorMaquina { get; }
        IVentaConfiguracion VentaConfiguracion { get; }
        ITablaRepository Tabla { get; }
        IHospitalRepository Hospital { get; }
        IRecetaRepository Receta { get; }
        ISerieRepository Serie { get; }
        ICentroRepository CentroCosto { get; }
        IPickingRepository Picking { get; }
        IConsolidadoRepository Consolidado { get; }
        IConveniosRepository Convenios { get; }
        IAseguradoraxProductoRepository AseguradoraxProducto { get; }
        IStockRepository Stock { get; }
        IGenericoRepository Generico { get; }
        ISapDocumentsRepository SapDocuments { get; }
        IVentaDevolucionRepository VentaDevolucion { get; }
        ISeguimientoRepository Seguimiento { get; }
        IValeDeliveryRepository ValeDelivery { get; }
        IComprobanteElectronicoRepository ComprobanteElectronico { get; }
        IPlanillaRepository Planilla { get; }
        ICuadreCajaRepository CuadreCaja { get; }
        IUsuarioRepository Usuario { get; }
        IPerfilUsuarioRepository PerfilUsuario { get; }
        IInformeRepository Informe { get; }
        IVentaCajaRepository VentaCaja { get; }
        ITipoComprobanteRepository TipoComprobante { get; }
        ITerminalRepository Terminal { get; }
    }
}
