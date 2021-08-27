using System.Collections.Generic;

namespace Net.Business.Entities
{
    public class ResultadoTransaccion<T>
    {
        /// <summary>
        /// ID de un nuevo registro o Id de registro actualizar
        /// </summary>
        public int IdRegistro { get; set; }
        /// <summary>
        /// Resultado de Codigo, Obtenido de la Base de Datos
        /// 0 => Correcto
        /// -1 => Error
        /// </summary>
        public int ResultadoCodigo { get; set; }
        /// <summary>
        /// Descripcion de la acción al finalizar el metodo invocado
        /// </summary>
        public string ResultadoDescripcion { get; set; }
        public string NombreAplicacion { get; set; }
        public string NombreMetodo { get; set; }
        public IEnumerable<T> dataList { get; set; }
        public T data { get; set; }
    }
}
