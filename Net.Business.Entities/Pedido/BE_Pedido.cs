using System;

namespace Net.Business.Entities
{
    public class BE_Pedido
    {
        public string estimpresion { get; set; }
        public string tiplistado { get; set; }
        public string estimpresiondsc { get; set; }
        public string tiplistadodsc { get; set; }
        public string codventa { get; set; }
        public string codpedido { get; set; }
        public string codcentro { get; set; }
        public string codalmacen { get; set; }
        public string codmedico { get; set; }
        public string codatencion { get; set; }
        public string cama { get; set; }
        public string codpoliza { get; set; }
        public DateTime? fechagenera { get; set; }
        public DateTime fechaemision { get; set; }
        public DateTime? fechapedido { get; set; }
        public string tipopedido { get; set; }
        public DateTime? fecha_envio { get; set; }
        public DateTime? fecha_entrega { get; set; }
        public string codtipopedido { get; set; }
        public DateTime fechaatencion { get; set; }
        public string estado { get; set; }
        public string listado { get; set; }
        public string nompaciente { get; set; }
        public string nomobservacion { get; set; }
        public string nomusuario { get; set; }
        public string nomcentro { get; set; }
        public string nomalmacen { get; set; }
        public string orden { get; set; }
        public bool TieneVenta { get; set; }
        public string flg_paquete { get; set; }
        public string tipomovimiento { get; set; }
        public string codalmacenventa { get; set; }
        public string codprestacion { get; set; }
        public string key { get => codventa == null ? string.Empty : codventa + codpedido + codatencion; }
    }
}
