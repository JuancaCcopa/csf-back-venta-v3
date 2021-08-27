using Net.Business.Entities;
using Net.Business.Entities.Informe;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface IInformeRepository
    {
        Task<ResultadoTransaccion<BE_Informe>> GetInformePorFiltro(string buscar, int key, int numerolineas, int orden);
    }
}
