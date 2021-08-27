using System.Data;

namespace Net.Connection
{
    public class DbParametro
    {
        public string nombre { get; set; }
        public object valor { get; set; }

        public int size { get; set; }
        public ParameterDirection direccion { get; set; }
        public SqlDbType dbType { get; set; }

        public DbParametro()
        {
            nombre = "";
            valor = null;
            direccion = ParameterDirection.Input;
        }

        public DbParametro(string Nombre, object Valor, ParameterDirection Direccion)
        {
            nombre = Nombre;
            valor = Valor;
            direccion = Direccion;
        }

        public DbParametro(string Nombre, object Valor)
        {
            nombre = Nombre;
            valor = Valor;
            direccion = ParameterDirection.Input;
        }
    }
}
