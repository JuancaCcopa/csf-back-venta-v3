using Net.Business.Entities;
using Net.Connection;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Net.Data
{
    public class MedicoRepository : RepositoryBase<BE_Medico>, IMedicoRepository
    {
        private readonly string _cnx;
        const string DB_ESQUEMA = "";
        const string SP_GET_POR_FILTRO = DB_ESQUEMA + "VEN_ListaMedicosPorFiltroGet";
        const string SP_GET_POR_ATENCION = DB_ESQUEMA + "VEN_ListaMedicosPorAtencionGet";

        public MedicoRepository(IConnectionSQL context, IConfiguration configuration)
            : base(context)
        {
            _cnx = configuration.GetConnectionString("cnnSqlLogistica");
        }
        public async Task<IEnumerable<BE_Medico>> GetListMedicoPorNombre(string nombre)
        {
            using (SqlConnection conn = new SqlConnection(_cnx))
            {
                using (SqlCommand cmd = new SqlCommand(SP_GET_POR_FILTRO, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@nombres", nombre));

                    var response = new List<BE_Medico>();

                    conn.Open();

                    using (IDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        response = (List<BE_Medico>)context.ConvertTo<BE_Medico>(reader);
                    }

                    conn.Close();

                    return response;
                }
            }
        }

        public async Task<IEnumerable<BE_Medico>> GetListMedicoPorAtencion(string codAtencion)
        {
            using (SqlConnection conn = new SqlConnection(_cnx))
            {
                using (SqlCommand cmd = new SqlCommand(SP_GET_POR_ATENCION, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@codatencion", codAtencion));

                    var response = new List<BE_Medico>();

                    conn.Open();

                    using (IDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        response = (List<BE_Medico>)context.ConvertTo<BE_Medico>(reader);
                    }

                    conn.Close();

                    return response;
                }
            }
        }
    }
}
