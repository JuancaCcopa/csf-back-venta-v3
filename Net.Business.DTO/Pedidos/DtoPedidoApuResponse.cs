using System;

namespace Net.Business.DTO
{
    public class DtoPedidoApuResponse
    {
        public string codpedido { get; set; }
        public string codatencion { get; set; }
        public string cama { get; set; }
        public DateTime? fechagenera { get; set; }
        public DateTime fechaatencion { get; set; }
        public string codtipopedido { get; set; }
        public string tipopedido { get; set; }
        public string key { get; set; }
    }
}
