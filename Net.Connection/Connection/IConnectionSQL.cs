using System.Collections.Generic;
using System.Data;

namespace Net.Connection
{
    public interface IConnectionSQL
    {
        //public void ExecuteSqlNonQuery(string comandSql, string cadenaConexion);
        //public void ExecuteSqlNonQueryAuto(string procedureName, object parameters, string cadenaConexion);
        //public object ExecuteSqlInsert<T>(string procedureName, T parameters, string cadenaConexion);
        //public object ExecuteSqlUpdate<T>(string procedureName, T parameters, string cadenaConexion);
        //public object ExecuteSqlDelete<T>(string procedureName, T parameters, string cadenaConexion);
        //public DbParametro[] ExecuteSqlNonQuery(string procedureName, DbParametro[] parameters, string cadenaConexion);
        public T ExecuteSqlViewId<T>(string procedureName, T parameters, string cadenaConexion);
        public IEnumerable<T> ExecuteSqlViewFindByCondition<T>(string procedureName, object parameters, string cadenaConexion);
        //public IEnumerable<T> ExecuteSqlViewAll<T>(string procedureName, T parameters, string cadenaConexion);
        //public IEnumerable<T> ExecuteSqlQuery<T>(string comandSql, string cadenaConexion);
        //public IEnumerable<T> ExecuteSqlQuery<T>(string procedureName, DbParametro[] parameters, string cadenaConexion);
        public IList<T> ConvertTo<T>(IDataReader reader);

        public T Convert<T>(IDataReader reader);

    }
}
