using Net.Business.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface IVentasNotaRepository
    {
        Task<ResultadoTransaccion<BE_VentasNota>> GetNotaPorFiltro(string buscar, int key, int numerolineas, int orden, string tipo);
    }
}
