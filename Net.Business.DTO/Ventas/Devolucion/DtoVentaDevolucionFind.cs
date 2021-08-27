using Net.Business.Entities;

namespace Net.Business.DTO
{
    public class DtoVentaDevolucionFind
    {
        public string codatencion { get; set; }

        public BE_VentasDevolucion RetornaModelo()
        {
            return new BE_VentasDevolucion
            {
                codatencion = this.codatencion
            };
        }
    }
}
