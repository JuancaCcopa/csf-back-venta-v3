using Net.Business.Entities;

namespace Net.Business.DTO
{
    public class DtoVentaAnular : EntityBase
    {
        public string codventa { get; set; }
        public string codpresotor { get; set; }
        public string codatencion { get; set; }
        public string codtipocliente { get; set; }
        public string tipomovimiento { get; set; }
        public bool tienedevolucion { get; set; }
        public string motivoanulacion { get; set; }
        public string usuario { get; set; }

        public BE_VentasCabecera RetornaVentaAnular()
        {
            return new BE_VentasCabecera
            {
                codventa = this.codventa,
                codpresotor = this.codpresotor,
                codatencion = this.codatencion,
                codtipocliente = this.codtipocliente,
                tipomovimiento = this.tipomovimiento,
                tienedevolucion = this.tienedevolucion,
                usuario = this.usuario,
                motivoanulacion = this.motivoanulacion,
                RegIdUsuario = this.RegIdUsuario
            };
        }
    }
}
