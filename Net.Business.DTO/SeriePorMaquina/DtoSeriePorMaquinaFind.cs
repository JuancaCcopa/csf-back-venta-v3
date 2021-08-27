using Net.Business.Entities;

namespace Net.Business.DTO
{
    public class DtoSeriePorMaquinaFind
    {
        public int id { get; set; }
        public string nombremaquina { get; set; }

        public BE_SeriePorMaquina RetornaSeriePorMaquina()
        {
            return new BE_SeriePorMaquina
            {
                id = this.id,
                nombremaquina = this.nombremaquina
            };
        }
    }
}
