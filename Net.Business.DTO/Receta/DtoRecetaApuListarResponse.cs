using Net.Business.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Net.Business.DTO
{
    public class DtoRecetaApuListarResponse
    {
        public IEnumerable<DtoRecetaApuResponse> ListaReceta { get; set; }

        public DtoRecetaApuListarResponse RetornarListaReceta(IEnumerable<BE_Receta> listaReceta)
        {
            IEnumerable<DtoRecetaApuResponse> lista = (
                from value in listaReceta
                select new DtoRecetaApuResponse
                {
                    ide_receta = value.ide_receta,
                    ide_historia = value.ide_historia,
                    fec_registra = value.fec_registra,
                    cod_atencion = value.cod_atencion,
                    paciente = value.paciente,
                    key = value.key
                }
            );

            return new DtoRecetaApuListarResponse() { ListaReceta = lista };
        }
    }
}
