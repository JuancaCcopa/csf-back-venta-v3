using Net.Business.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Net.Data
{
   public interface ITerminalRepository
    {
        Task<ResultadoTransaccion<BE_Terminal>> GetTerminal();

    }
}
