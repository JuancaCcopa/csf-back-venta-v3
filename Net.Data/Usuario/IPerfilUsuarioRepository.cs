using Net.Business.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Net.Data
{
    public interface IPerfilUsuarioRepository
    {
        Task<ResultadoTransaccion<BE_PerfilUsuario>> GetPerfilUsuario(int idemodulo, int ideusuario);
    }
}
