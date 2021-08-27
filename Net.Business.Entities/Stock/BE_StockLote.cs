using System;

namespace Net.Business.Entities
{
    public class BE_StockLote
    {
        /// <summary>
        /// Codigo de Articulo - SAP
        /// </summary>
        public string ItemCode { get; set; }
        /// <summary>
        /// Descripción del Articulo
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// Código del Lote
        /// </summary>
        public string BatchNum { get; set; }
        /// <summary>
        /// Cantidad Stock - Lote
        /// </summary>
        public decimal? QuantityLote { get; set; }
        /// <summary>
        /// Cantidad Solicitada - Inputado por el Usuario
        /// </summary>
        public decimal Quantityinput { get; set; }
        /// <summary>
        /// Cantidad Solicitada Lote (Solo del almacén)
        /// </summary>
        public decimal? IsCommitedLote { get; set; }
        /// <summary>
        /// Cantidad Comprometida Lote (Solo del almacén)
        /// </summary>
        public decimal? OnOrderLote { get; set; }
        /// <summary>
        /// Fecha de Vencimiento del Lote
        /// </summary>
        public DateTime? ExpDate { get; set; }
        /// <summary>
        /// Id Ubicación
        /// </summary>
        public int? BinAbs { get; set; }
        /// <summary>
        /// Ubicación
        /// </summary>
        public string BinCode { get; set; }
        /// <summary>
        /// Stock Por Ubicación
        /// </summary>
        public decimal? OnHandQty { get; set; }
    }
}
