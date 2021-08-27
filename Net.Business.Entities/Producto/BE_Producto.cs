using System.Collections.Generic;

namespace Net.Business.Entities
{
    public class BE_Producto
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
        /// Trabaja con Lote
        /// </summary>
        public bool manbtchnum { get => ManageBatchNumbers.Equals("tYES") ? true : false; }
        public string ManageBatchNumbers { get; set; }
        public string ArTaxCode { get; set; }
        /// <summary>
        /// Laboratorio
        /// </summary>
        public string U_SYP_CS_LABORATORIO { get; set; }
        /// <summary>
        /// Familia
        /// </summary>
        public string U_SYP_CS_FAMILIA { get; set; }
        /// <summary>
        /// Codigo de producto de CSFE
        /// </summary>
        //public string U_SYP_CS_SIC { get; set; }
        /// <summary>
        /// Estado de Abastecimiento
        /// N => Normal
        /// A => Agotado
        /// </summary>
        public string U_SYP_CS_EABAS { get; set; }
        /// <summary>
        /// Tipo clasificación
        /// Alto riesgo => Alto riesgo
        /// Tratamiento => Tratamiento
        /// </summary>
        public string U_SYP_CS_CLASIF { get; set; }
        /// <summary>
        /// Moneda Articulo
        /// S => Soles
        /// D => Dolar
        /// </summary>
        public string U_SYP_MONART { get; set; }
        /// <summary>
        /// Indicador Restringido
        /// True
        /// False
        /// </summary>
        public bool flgrestringido { get; set; }
        /// <summary>
        /// Grupo de Articulos
        /// </summary>
        public int ItemsGroupCode { get; set; }
        /// <summary>
        /// Codigo Articulo -CSFE
        /// </summary>
        //public string codproducto { get => U_SYP_CS_SIC; }
        /// <summary>
        /// Precio Venta al publico (+ IGV)
        /// </summary>
        public decimal valorPVP { get => decimal.Round(valorVVP * (valorIGV / 100 + 1),2); }
        /// <summary>
        /// Valor venta al publico (sin IGV)
        /// </summary>
        public decimal valorVVP { get; set; }
        /// <summary>
        /// Valor IGV
        /// </summary>
        public decimal valorIGV { get; set; }
        /// <summary>
        /// Fraccion Venta
        /// </summary>
        public int? U_SYP_CS_FRVTA { get; set; }
        /// <summary>
        /// Fraccion Venta
        /// </summary>
        public decimal? fraccionVenta { get => U_SYP_CS_FRVTA.Equals(null) ? 0 : U_SYP_CS_FRVTA; }
        /// <summary>
        /// Descuento del producto
        /// </summary>
        public decimal? U_SYP_CS_DCTO { get; set; }
        /// <summary>
        /// Descuento del producto
        /// </summary>
        public decimal? valorDescuento { get => U_SYP_CS_DCTO.Equals(null) ? 0 : U_SYP_CS_DCTO; }
        public bool FlgConvenio { get; set; }
        /// <summary>
        /// Narcotico ?
        /// </summary>
        public string Properties1 { get; set; }
        /// <summary>
        /// Narcotico ?
        /// </summary>
        public bool Narcotico { get => Properties1.Equals("tSI") ? true: false; }

        public bool GastoCubierto { get; set; }
        public bool ProductoRestringido { get; set; }

        /// <summary>
        /// Cantidad del Pedido
        /// </summary>
        public decimal CantidadPedido { get; set; }
        /// <summary>
        /// CodPedido
        /// </summary>
        public string CodPedido { get; set; }
        public decimal ProductoStock { get; set; }
        public string U_SYP_CS_PRODCI { get; set; }

        public List<BE_Stock> ListStockAlmacen { get; set; }

        public List<BE_StockLote> ListStockLote { get; set; }
    }
}

