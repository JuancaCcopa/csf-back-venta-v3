using System;

namespace Net.Business.Entities
{
    public class BE_ValeDelivery: EntityBase
    {
		public int idvaledelivery { get; set; }
		public string codatencion { get; set; }
		public string nombrepaciente { get; set; }
		public string telefono { get; set; }
		public string celular { get; set; }
		public string direccion { get; set; }
		public string distrito { get; set; }
		public string referencia { get; set; }
		public DateTime fechaentrega { get; set; }
		public DateTime? fecharegistro { get; set; }
		public string lugarentrega { get; set; }
		public string prioridad_1 { get; set; }
		public string prioridad_2 { get; set; }
		public bool estado { get; set; }
	}
}
