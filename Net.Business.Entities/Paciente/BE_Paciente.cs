using System;

namespace Net.Business.Entities
{
    public class BE_Paciente
    {
        public string codatencion { get; set; }
        public string codpaciente { get; set; }
        public string nombrepaciente { get; set; }
        public string titular { get; set; }
        public DateTime? fechainicio { get; set; }
        public DateTime? fechafin { get; set; }
        public string cama { get; set; }
        public string nombresexo { get; set; }
        public int edad { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public string nombreprovincia { get; set; }
        public string nombredistrito { get; set; }
        public string codplan { get; set; }
        public decimal porcentajeplan { get; set; }
        public string poliza { get; set; }
        public string planpoliza { get; set; }
        public string npoliza { get; set; }
        public string codaseguradora { get; set; }
        public string nombreaseguradora { get; set; }
        public string nombrecontratante { get; set; }
        public string codcia { get; set; }
        public string codmedico { get; set; }
        public string nombremedico { get; set; }
        public string observacionesasegurado { get; set; }
        public string observacionespaciente { get; set; }
        public string observacionatencion { get; set; }
        public DateTime? fechainiciovigencia { get; set; }
        public DateTime? fechafinvigencia { get; set; }
        public int numerodiasatencion { get; set; }
        public string nombretarifa { get; set; }
        public string nombreparentesco { get; set; }
        public double coaseguro { get; set; }
        public double deducible { get; set; }
        public double descuento { get; set; }
        public string nombreconcepto { get; set; }
        public string nombremoneda { get; set; }
        public int activo { get; set; }
        public string pagacoaseguro { get; set; }
        public string familiar { get; set; }
        public string traslado { get; set; }
        public int? exclusiones { get; set; }
        public string codatenciondestino { get; set; }
        public int idegctipoatencionmae { get; set; }
        public string descripcionpaquete { get; set; }

    }
}
