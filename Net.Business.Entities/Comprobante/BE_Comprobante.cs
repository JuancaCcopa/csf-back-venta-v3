using System;
using System.Collections.Generic;								 

namespace Net.Business.Entities
{
    public class BE_Comprobante
    {
		 public BE_Comprobante()
        {
            this.cuadreCaja = new List<BE_CuadreCaja>();
        }
        public string codcomprobante { get; set; }
        public string codventa { get; set; }
        public string codtipocliente { get; set; }
        public string codcliente { get; set; }
        public string anombrede { get; set; }
        public string direccion { get; set; }
        public string ruc { get; set; }
        public DateTime fechagenera { get; set; }
        public DateTime fechaemision { get; set; }
        public DateTime? fechacancelacion { get; set; }
        public DateTime? fechaanulacion { get; set; }
        public decimal porcentajeimpuesto { get; set; }
        public string codimpuesto { get; set; }
        public decimal montototal { get; set; }
        public decimal montoigv { get; set; }
        public string moneda { get; set; }
        public decimal tipodecambio { get; set; }
        public string coduser { get; set; }
		public int idusuario { get; set; }							  
        public string estado { get; set; }
        public string numeroplanilla { get; set; }
        public string tipdocidentidad { get; set; }
        public string docidentidad { get; set; }
        public string codcentro { get; set; }
        public string codcomprobantee { get; set; }
        public bool flg_electronico { get; set; }
        public bool flg_gratuito { get; set; }
        public double montototaldolares { get; set; }
        public double montoigvdolares { get; set; }
        public string cardcode { get; set; }
        public int regcreateidusuario { get; set; }
        public DateTime regcreate { get; set; }
        public int regupdateidusuario { get; set; }
        public DateTime regupdate { get; set; }
        public int flgeliminado { get; set; }
        public string nombreestado { get; set; }
        public string nombretipocliente { get; set; }
        public string codatencion { get; set; }
		public  IList<BE_CuadreCaja> cuadreCaja { get; set; }
        //extra
        public string codplan { get; set; }
        public decimal porcentajedctoplan { get; set; }
        public decimal porcentajecoaseguro { get; set; }
        public string nombreplan { get; set; }
        public string nombretipdocidentidad { get; set; }
        public string correo { get; set; }
        public string tipoafectacionigv { get; set; }													 
    }
}
