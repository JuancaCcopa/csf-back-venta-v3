using System;
using System.Collections.Generic;
using System.Text;

namespace Net.Business.Entities
{
    public class BE_SerieConfig
    {
        /// <summary>
        /// Boleta
        /// </summary>
        public string boleta { get; set; }
        /// <summary>
        /// Factura
        /// </summary>
        public string factura { get; set; }
        /// <summary>
        /// Nota de Credito - Boleta
        /// </summary>
        public string creditob { get; set; }
        /// <summary>
        /// Nota de Credito - Factura
        /// </summary>
        public string creditof { get; set; }
        /// <summary>
        /// Nota de Debito - Boleta
        /// </summary>
        public string debitob { get; set; }
        /// <summary>
        /// Nota de Debito - Factura
        /// </summary>
        public string debitof { get; set; }
        /// <summary>
        /// Guia por local
        /// </summary>
        public string guiaxlocal { get; set; }
        /// <summary>
        /// Factura
        /// </summary>
        public string flg_electronicof { get; set; }
        /// <summary>
        /// Boleta
        /// </summary>
        public string flg_electronicob { get; set; }
        /// <summary>
        /// Nota de Credito - Boleta
        /// </summary>
        public string flg_electronicocb { get; set; }
        /// <summary>
        /// Nota de Credito - Factura
        /// </summary>
        public string flg_electronicocf { get; set; }
        /// <summary>
        /// Nota de Debito - Boleta
        /// </summary>
        public string flg_electronicodb { get; set; }
        /// <summary>
        /// Nota de Debito - Factura
        /// </summary>
        public string flg_electronicodf { get; set; }
        public int flg_otorgarf { get; set; }
        public int flg_otorgarb { get; set; }
        public int flg_otorgarcb { get; set; }
        public int flg_otorgarcf { get; set; }
        public int flg_otorgardb { get; set; }
        public int flg_otorgardf { get; set; }
        public string flg_electronico { get; set; }
        public string generar_e { get; set; }
    }
}
