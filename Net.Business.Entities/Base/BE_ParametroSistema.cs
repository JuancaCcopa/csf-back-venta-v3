using Net.Connection.Attributes;
using System;
using System.Data;

namespace Net.Business.Entities
{
    public class BE_ParametroSistema
    {
        /// <summary>
        /// IdParametrosSistema
        /// </summary>
        [DBParameter(SqlDbType.Int, ActionType.Everything, true)]
        public int IdParametrosSistema { get; set; }
        /// <summary>
        /// SendEmail
        /// </summary>
        [DBParameter(SqlDbType.VarChar, 100, ActionType.Everything)]
        public string SendEmail { get; set; }
        /// <summary>
        /// SendEmailPassword
        /// </summary>
        [DBParameter(SqlDbType.Text, 0, ActionType.Everything)]
        public string SendEmailPassword { get; set; }
        /// <summary>
        /// SendEmailPassword
        /// </summary>
        [DBParameter(SqlDbType.VarChar, 30, ActionType.Everything)]
        public string SendEmailPasswordOrigen { get; set; }
        /// <summary>
        /// SendEmailPort
        /// </summary>
        [DBParameter(SqlDbType.Int, 0, ActionType.Everything)]
        public int? SendEmailPort { get; set; }
        /// <summary>
        /// SendEmailEnabledSSL
        /// </summary>
        [DBParameter(SqlDbType.VarChar, 100, ActionType.Everything)]
        public Boolean? SendEmailEnabledSSL { get; set; }
        /// <summary>
        /// SendEmailHost
        /// </summary>
        [DBParameter(SqlDbType.VarChar, 100, ActionType.Everything)]
        public string SendEmailHost { get; set; }
    }
}
