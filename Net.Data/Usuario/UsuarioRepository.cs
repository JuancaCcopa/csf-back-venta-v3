using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Net.Business.Entities;
using Net.Connection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Net.Data
{
    public class UsuarioRepository : RepositoryBase<BE_Usuario>, IUsuarioRepository
    {
        private readonly string _cnx_clinica;
        private readonly string _cnx_logistica;

        private string _aplicacionName;
        private string _metodoName;
        private readonly Regex regex = new Regex(@"<(\w+)>.*");

        const string DB_ESQUEMA = "";
        const string SP_GET_USUARIO_POR_FILTRO = DB_ESQUEMA + "REQ_UsuariosFiltroGet";
       // const string SP_GET_USUARIO_POR_FILTRO_ESTADODIFERENTE = DB_ESQUEMA + "VEN_ObtenerUsuarioByEstadoDif";
       // const string SP_GET_USUARIO_POR_FILTRO_NOMBRE_LIKE = DB_ESQUEMA + "SEG_ObtenerUsuarioByNombre";
        const string SP_GET_USUARIO_POR_FILTRO_NOMBRE_LIKE = DB_ESQUEMA + "SEG_ObtenerUsuarioByNombre";

        public UsuarioRepository(IConnectionSQL context, IConfiguration configuration)
            : base(context)
        {
            _aplicacionName = this.GetType().Name;
            _cnx_clinica = configuration.GetConnectionString("cnnSqlClinica");
            _cnx_logistica = configuration.GetConnectionString("cnnSqlLogistica");
        }

        public async Task<ResultadoTransaccion<BE_Usuario>> GetUsuarioPorFiltro(string codtabla, string buscar, int key, int numerolineas, int orden)
        {
            ResultadoTransaccion<BE_Usuario> vResultadoTransaccion = new ResultadoTransaccion<BE_Usuario>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx_clinica))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_GET_USUARIO_POR_FILTRO, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@codtabla", codtabla));
                        cmd.Parameters.Add(new SqlParameter("@buscar", buscar));
                        cmd.Parameters.Add(new SqlParameter("@key", key));
                        cmd.Parameters.Add(new SqlParameter("@numerolineas", numerolineas));
                        cmd.Parameters.Add(new SqlParameter("@orden", orden));

                        var response = new BE_Usuario();

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            response = context.Convert<BE_Usuario>(reader);

                            if (response == null)
                            {
                                response = new BE_Usuario();
                            }
                        }

                        conn.Close();

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", 1);
                        vResultadoTransaccion.data = response;
                    }
                }
            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return vResultadoTransaccion;
        }

        public async Task<ResultadoTransaccion<BE_UsuarioPersona>> GetUsuarioPersonaPorFiltroNombre(string Nombre)
        {
            ResultadoTransaccion<BE_UsuarioPersona> vResultadoTransaccion = new ResultadoTransaccion<BE_UsuarioPersona>();
            _metodoName = regex.Match(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name).Groups[1].Value.ToString();

            vResultadoTransaccion.NombreMetodo = _metodoName;
            vResultadoTransaccion.NombreAplicacion = _aplicacionName;
            try
            {
                using (SqlConnection conn = new SqlConnection(_cnx_logistica))
                {
                    using (SqlCommand cmd = new SqlCommand(SP_GET_USUARIO_POR_FILTRO_NOMBRE_LIKE, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Nombre", Nombre));

                        List<BE_UsuarioPersona> response = new List<BE_UsuarioPersona>();
                        //var response = new BE_Usuario();

                        conn.Open();

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                             response = (List<BE_UsuarioPersona>)context.ConvertTo<BE_UsuarioPersona>(reader);
                        }

                        conn.Close();

                        vResultadoTransaccion.IdRegistro = 0;
                        vResultadoTransaccion.ResultadoCodigo = 0;
                        vResultadoTransaccion.ResultadoDescripcion = string.Format("Registros Totales {0}", response.Count);
                        vResultadoTransaccion.dataList = response;

                    }
                }
            }
            catch (Exception ex)
            {
                vResultadoTransaccion.IdRegistro = -1;
                vResultadoTransaccion.ResultadoCodigo = -1;
                vResultadoTransaccion.ResultadoDescripcion = ex.Message.ToString();
            }

            return vResultadoTransaccion;
        }

    }
}
