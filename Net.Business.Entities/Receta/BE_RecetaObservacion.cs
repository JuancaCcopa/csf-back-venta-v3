using Net.Connection.Attributes;
using System;
using System.Data;

namespace Net.Business.Entities
{
    public class BE_RecetaObservacion: EntityBase
    {
        public int idobs { get; set; }
        [DBParameter(SqlDbType.Int, 0, ActionType.Everything)]
        public int id_receta { get; set; }
        public DateTime fecharegistro { get; set; }
        public int idusuarioregistro { get; set; }
        public string usuarioregistro { get; set; }
        public string comentario { get; set; }
        public int idusuarioanulacion { get; set; }
        public string usuarioanulacion { get; set; }
        public DateTime? fechaanulacion { get; set; }
    }
}
