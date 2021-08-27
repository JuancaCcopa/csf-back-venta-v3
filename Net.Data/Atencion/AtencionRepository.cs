using Net.Business.Entities;
using Net.Connection;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Net.Data
{
    public class AtencionRepository : RepositoryBase<BE_Atencion>, IAtencionRepository
    {
        private readonly string _cnx;
        const string DB_ESQUEMA = "";
        const string SP_GET = DB_ESQUEMA + "VEN_ListaAtencionPorFiltrosGet";

        public AtencionRepository(IConnectionSQL context, IConfiguration configuration)
            : base(context)
        {
            _cnx = configuration.GetConnectionString("cnnSqlLogistica");
        }
        public async Task<IEnumerable<BE_Atencion>> GetListPacientePorFiltros(string opcion, string codpaciente, string nombres)
        {
            using (SqlConnection conn = new SqlConnection(_cnx))
            {
                using (SqlCommand cmd = new SqlCommand(SP_GET, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@opcion", opcion));
                    cmd.Parameters.Add(new SqlParameter("@codpaciente", codpaciente));
                    cmd.Parameters.Add(new SqlParameter("@nombres", nombres));

                    var response = new List<BE_Atencion>();

                    conn.Open();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToValue(reader));
                        }
                    }

                    conn.Close();

                    return response;
                }
            }
        }

        public BE_Atencion MapToValue(SqlDataReader reader) => new BE_Atencion()
        {
            nombrepaciente = (string)reader["nombrepaciente"].ToString(),
            atencion = (string)reader["atencion"].ToString(),
            estadopaciente = (string)reader["estadopaciente"].ToString(),
            finicio = (string)reader["finicio"].ToString(),
            codpaciente = (string)reader["codpaciente"].ToString()
        };
    }
}
