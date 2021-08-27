using Net.Connection;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace Net.Data
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly IConnectionSQL _repoContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;

        private IEmailSenderRepository _EmailSender;
        private IVentaRepository _Venta;
        private ITipoCambioRepository _TipoCambio;
        private IPlanesRepository _Planes;
        private IWarehousesRepository _Warehouses;
        private IAtencionRepository _Atencion;
        private IClienteRepository _Cliente;
        private IPersonalClinicaRepository _PersonalClinica;
        private IMedicoRepository _Medico;
        private IPacienteRepository _Paciente;
        private IComprobanteRepository _Comprobante;
        private IPedidoRepository _Pedido;
        private IProductoRepository _Producto;
        private ISeriePorMaquinaRepository _SeriePorMaquina;
        private IVentaConfiguracion _VentaConfiguracion;
        private ITablaRepository _Tabla;
        private IHospitalRepository _Hospital;
        private IRecetaRepository _Receta;
        private ISerieRepository _Serie;
        private ICentroRepository _CentroCosto;
        private IPickingRepository _Picking;
        private IConsolidadoRepository _Consolidado;
        private IConveniosRepository _Convenios;
        private IAseguradoraxProductoRepository _AseguradoraxProducto;
        private IStockRepository _Stock;
        private IGenericoRepository _Generico;
        private ISapDocumentsRepository _SapDocuments;
        private IVentaDevolucionRepository _VentaDevolucion;
        private ISeguimientoRepository _Seguimiento;
        private IValeDeliveryRepository _ValeDelivery;
        private IComprobanteElectronicoRepository _ComprobanteElectronico;
        private IPlanillaRepository _Planilla;
        private ICuadreCajaRepository _CuadreCaja;
        private IUsuarioRepository _Usuario;
        private IPerfilUsuarioRepository _PerfilUsuario;
        private IInformeRepository _Informe;
        private IVentaCajaRepository _VentaCaja;
        private ITipoComprobanteRepository _TipoComprobante;
        private ITerminalRepository _Terminal;

        public RepositoryWrapper(IConnectionSQL repoContext, IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _repoContext = repoContext;
            _configuration = configuration;
            _clientFactory = clientFactory;
        }
        public IEmailSenderRepository EmailSender
        {
            get
            {
                if (_EmailSender == null)
                {
                    _EmailSender = new EmailSenderRepository(_repoContext, _configuration);
                }
                return _EmailSender;
            }
        }
        public IVentaRepository Venta
        {
            get
            {
                if (_Venta == null)
                {
                    _Venta = new VentaRepository(_clientFactory,_repoContext, _configuration);
                }
                return _Venta;
            }
        }
        public ITipoCambioRepository TipoCambio
        {
            get
            {
                if (_TipoCambio == null)
                {
                    _TipoCambio = new TipoCambioRepository(_clientFactory, _configuration);
                }
                return _TipoCambio;
            }
        }
        public IPlanesRepository Planes
        {
            get
            {
                if(_Planes == null)
                {
                    _Planes = new PlanesRepository(_repoContext, _configuration);
                }
                return _Planes;
            }
        }
        public IWarehousesRepository Warehouses
        {
            get
            {
                if (_Warehouses == null)
                {
                    _Warehouses = new WarehousesRepository(_clientFactory, _configuration);
                }
                return _Warehouses;
            }
        }
        public IAtencionRepository Atencion
        {
            get
            {
                if (_Atencion == null)
                {
                    _Atencion = new AtencionRepository(_repoContext, _configuration);
                }
                return _Atencion;
            }
        }
        public IClienteRepository Cliente
        {
            get
            {
                if (_Cliente == null)
                {
                    _Cliente = new ClienteRepository(_clientFactory, _configuration);
                }
                return _Cliente;
            }
        }
        public IPersonalClinicaRepository PersonalClinica
        {
            get
            {
                if (_PersonalClinica == null)
                {
                    _PersonalClinica = new PersonalClinicaRepository(_repoContext, _configuration);
                }
                return _PersonalClinica;
            }
        }
        public IMedicoRepository Medico
        {
            get
            {
                if (_Medico == null)
                {
                    _Medico = new MedicoRepository(_repoContext, _configuration);
                }
                return _Medico;
            }
        }
        public IPacienteRepository Paciente
        {
            get
            {
                if (_Paciente == null)
                {
                    _Paciente = new PacienteRepository(_repoContext, _configuration);
                }
                return _Paciente;
            }
        }
        public IComprobanteRepository Comprobante
        {
            get
            {
                if (_Comprobante == null)
                {
                    _Comprobante = new ComprobanteRepository(_repoContext, _configuration);
                }
                return _Comprobante;
            }
        }
        public IPedidoRepository Pedido
        {
            get
            {
                if (_Pedido == null)
                {
                    _Pedido = new PedidoRepository(_repoContext, _configuration);
                }
                return _Pedido;
            }
        }
        public IProductoRepository Producto
        {
            get
            {
                if (_Producto == null)
                {
                    _Producto = new ProductoRepository(_clientFactory, _configuration, _repoContext);
                }
                return _Producto;
            }
        }
        public ISeriePorMaquinaRepository SeriePorMaquina
        {
            get
            {
                if (_SeriePorMaquina == null)
                {
                    _SeriePorMaquina = new SeriePorMaquinaRepository(_repoContext, _configuration);
                }
                return _SeriePorMaquina;
            }
        }
        public IVentaConfiguracion VentaConfiguracion
        {
            get
            {
                if (_VentaConfiguracion == null)
                {
                    _VentaConfiguracion = new VentaConfiguracion(_repoContext, _configuration);
                }
                return _VentaConfiguracion;
            }
        }
        public ITablaRepository Tabla
        {
            get
            {
                if (_Tabla == null)
                {
                    _Tabla = new TablaRepository(_repoContext, _configuration);
                }
                return _Tabla;
            }
        }
        public IHospitalRepository Hospital
        {
            get
            {
                if (_Hospital == null)
                {
                    _Hospital = new HospitalRepository(_repoContext, _configuration);
                }
                return _Hospital;
            }
        }
        public IRecetaRepository Receta
        {
            get
            {
                if (_Receta == null)
                {
                    _Receta = new RecetaRepository(_repoContext, _configuration);
                }
                return _Receta;
            }
        }
        public ISerieRepository Serie
        {
            get
            {
                if (_Serie == null)
                {
                    _Serie = new SerieRepository(_repoContext, _configuration);
                }
                return _Serie;
            }
        }
        public ICentroRepository CentroCosto
        {
            get
            {
                if (_CentroCosto == null)
                {
                    _CentroCosto = new CentroRepository(_repoContext, _configuration);
                }
                return _CentroCosto;
            }
        }
        public IPickingRepository Picking
        {
            get
            {
                if (_Picking == null)
                {
                    _Picking = new PickingRepository(_repoContext, _configuration);
                }
                return _Picking;
            }
        }
        public IConsolidadoRepository Consolidado
        {
            get
            {
                if (_Consolidado == null)
                {
                    _Consolidado = new ConsolidadoRepository(_repoContext, _configuration);
                }
                return _Consolidado;
            }
        }
        public IConveniosRepository Convenios
        {
            get
            {
                if (_Convenios == null)
                {
                    _Convenios = new ConveniosRepository(_clientFactory,_repoContext, _configuration);
                }
                return _Convenios;
            }
        }
        public IAseguradoraxProductoRepository AseguradoraxProducto
        {
            get
            {
                if (_AseguradoraxProducto == null)
                {
                    _AseguradoraxProducto = new AseguradoraxProductoRepository(_repoContext, _configuration);
                }
                return _AseguradoraxProducto;
            }
        }
        public IStockRepository Stock
        {
            get
            {
                if (_Stock == null)
                {
                    _Stock = new StockRepository(_clientFactory, _configuration);
                }
                return _Stock;
            }
        }
        public IGenericoRepository Generico
        {
            get
            {
                if (_Generico == null)
                {
                    _Generico = new GenericoRepository(_clientFactory, _configuration);
                }
                return _Generico;
            }
        }
        public ISapDocumentsRepository SapDocuments
        {
            get
            {
                if (_SapDocuments == null)
                {
                    _SapDocuments = new SapDocumentsRepository(_clientFactory, _configuration, _repoContext);
                }
                return _SapDocuments;
            }
        }
        public IVentaDevolucionRepository VentaDevolucion
        {
            get
            {
                if (_VentaDevolucion == null)
                {
                    _VentaDevolucion = new VentaDevolucionRepository(_repoContext, _configuration);
                }
                return _VentaDevolucion;
            }
        }
        public ISeguimientoRepository Seguimiento
        {
            get
            {
                if (_Seguimiento == null)
                {
                    _Seguimiento = new SeguimientoRepository(_repoContext, _configuration);
                }
                return _Seguimiento;
            }
        }
        public IValeDeliveryRepository ValeDelivery
        {
            get
            {
                if (_ValeDelivery == null)
                {
                    _ValeDelivery = new ValeDeliveryRepository(_repoContext, _configuration);
                }
                return _ValeDelivery;
            }
        }
        public IComprobanteElectronicoRepository ComprobanteElectronico
        {
            get
            {
                if (_ComprobanteElectronico == null)
                {
                    _ComprobanteElectronico = new ComprobanteElectronicoRepository(_repoContext, _configuration);
                }
                return _ComprobanteElectronico;
            }
        }

        public IPlanillaRepository Planilla
        {
            get
            {
                if (_Planilla == null)
                {
                    _Planilla = new PlanillaRepository(_repoContext, _configuration);
                }
                return _Planilla;
            }
        }
        public ICuadreCajaRepository CuadreCaja
        {
            get
            {
                if (_CuadreCaja == null)
                {
                    _CuadreCaja = new CuadreCajaRepository(_repoContext, _configuration);
                }
                return _CuadreCaja;
            }
        }
        public IUsuarioRepository Usuario
        {
            get
            {
                if (_Usuario == null)
                {
                    _Usuario = new UsuarioRepository(_repoContext, _configuration);
                }
                return _Usuario;
            }
        }
        public IPerfilUsuarioRepository PerfilUsuario
        {
            get
            {
                if (_PerfilUsuario == null)
                {
                    _PerfilUsuario = new PerfilUsuarioRepository(_repoContext, _configuration);
                }
                return _PerfilUsuario;
            }
        }
        public IInformeRepository Informe
        {
            get
            {
                if (_Informe == null)
                {
                    _Informe = new InformeRepository(_repoContext, _configuration);
                }
                return _Informe;
            }
        }
        public IVentaCajaRepository VentaCaja
        {
            get
            {
                if (_VentaCaja == null)
                {
                    _VentaCaja = new VentaCajaRepository(_clientFactory ,_repoContext, _configuration);
                }
                return _VentaCaja;
            }
        }
        public ITipoComprobanteRepository TipoComprobante
        {
            get
            {
                if (_TipoComprobante == null)
                {
                    _TipoComprobante = new TipoComprobanteRepository(_clientFactory, _configuration);
                }
                return _TipoComprobante;
            }
        }

        public ITerminalRepository Terminal
        {
            get
            {
                if (_Terminal == null)
                {
                    _Terminal = new TerminalRepository(_clientFactory, _repoContext, _configuration);
                }
                return _Terminal;
            }
        }
    }
}
