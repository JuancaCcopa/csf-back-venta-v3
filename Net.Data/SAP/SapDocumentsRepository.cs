using Net.Business.Entities;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Net.Http;
using Net.Connection.ServiceLayer;
using System.Text.RegularExpressions;
using System;
using Microsoft.Data.SqlClient;
using Net.Connection;
using System.Linq;

namespace Net.Data
{
    public class SapDocumentsRepository : RepositoryBase<SapDocument>, ISapDocumentsRepository
    {
        private readonly string _cnx;
        private string _aplicacionName;
        private string _metodoName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private readonly ConnectionServiceLayer _connectServiceLayer;

        const string DB_ESQUEMA = "";
        const string SP_SET_CREATE_DOCUMENT = DB_ESQUEMA + "";

        public SapDocumentsRepository(IHttpClientFactory clientFactory, IConfiguration configuration, IConnectionSQL context)
             : base(context)
        {
            _cnx = configuration.GetConnectionString("cnnSqlLogistica");
            _aplicacionName = this.GetType().Name;
            _configuration = configuration;
            _clientFactory = clientFactory;
            _connectServiceLayer = new ConnectionServiceLayer(_configuration, _clientFactory);
        }

        public async Task<ResultadoTransaccion<SapDocument>> SetCreateDocument(BE_VentasCabecera venta)
        {
            ResultadoTransaccion<SapDocument> vResultadoTransaccion = new ResultadoTransaccion<SapDocument>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;

            List<SapDocumentLines> sapDocumentLines = new List<SapDocumentLines>();
            List<SapBatchNumbers> sapBatchNumbers = new List<SapBatchNumbers>();
            List<SapBinAllocations> sapBinAllocations = new List<SapBinAllocations>();

            try
            {

                VentaRepository ventaRepository = new VentaRepository(_clientFactory, context, _configuration);

                ResultadoTransaccion<BE_VentasCabecera> resultadoTransaccionVenta = await ventaRepository.GetVentaPorCodVenta(venta.codventa);

                if (resultadoTransaccionVenta.ResultadoCodigo == -1)
                {
                    vResultadoTransaccion.IdRegistro = -1;
                    vResultadoTransaccion.ResultadoCodigo = -1;
                    vResultadoTransaccion.ResultadoDescripcion = resultadoTransaccionVenta.ResultadoDescripcion;

                    return vResultadoTransaccion;
                }

                BE_VentasCabecera value = resultadoTransaccionVenta.data;

                var document = new SapDocument
                {
                    DocType = "dDocument_Items",
                    DocDate = value.fechaemision,
                    DocDueDate = value.fechaemision,
                    CardCode = "10087354186",
                    DocCurrency = "S/",
                    TaxDate = value.fechaemision,
                    Comments = value.observacion,
                    DocObjectCode = "oDeliveryNotes",
                    DocumentLines = new List<SapDocumentLines>()
                };

                foreach (BE_VentasDetalle item in value.listaVentaDetalle)
                {
                    var linea = new SapDocumentLines
                    {
                        ItemCode = item.codproducto,
                        ItemDescription = item.nombreproducto,
                        Quantity = item.cantidad,
                        UnitPrice =   item.preciounidad,
                        TaxCode = "IGV",
                        AccountCode  = "20111001",
                        WarehouseCode = value.codalmacen,
                        CostingCode = "CAMA",
                        CostingCode2 = "AMBU",
                        CostingCode3 = "FARM",
                        CostingCode4 = "GESO",
                        BatchNumbers = new List<SapBatchNumbers>(),
                        DocumentLinesBinAllocations = new List<SapBinAllocations>()
                    };

                    if (item.listVentasDetalleLotes != null)
                    {
                        if (item.listVentasDetalleLotes.Count > 0)
                        {
                            foreach (BE_VentasDetalleLote itemLote in item.listVentasDetalleLotes)
                            {
                                var lineaLote = new SapBatchNumbers
                                {
                                    Quantity = itemLote.cantidad,
                                    BatchNumber = itemLote.lote
                                };

                                sapBatchNumbers.Add(lineaLote);
                            }

                            linea.BatchNumbers = sapBatchNumbers;
                        }
                    }

                    document.DocumentLines.Add(linea);
                }
                var cadena = "DeliveryNotes";
                SapDocument data = await _connectServiceLayer.PostAsync<SapDocument>(cadena, document);
                //if (data.DocEntry > 0)
                //{

                //    foreach (var resulLinea in item.DocumentLines)
                //    {
                //        ResultadoTransaccion resultadoTransaccionSendSAP = await consolidadoRepository.UpdateDocEntrySAP(dataConsolidado.IdConsolidado, resulLinea.ItemCode, data.DocEntry, IdUsuario);

                //        if (resultadoTransaccionSendSAP.ResultadoCodigo == -1)
                //        {
                //            data.Exito = false;
                //            data.Mensaje = resultadoTransaccionSendSAP.ResultadoDescripcion;
                //        }
                //    }
                //}

            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return vResultadoTransaccion;
        }
    }
}
