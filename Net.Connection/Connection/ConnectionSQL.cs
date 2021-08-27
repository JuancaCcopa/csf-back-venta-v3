using Microsoft.Data.SqlClient;
using Net.Connection.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Net.Connection
{
    public class ConnectionSQL : IConnectionSQL
    {
        private string _cnx;

        public void ExecuteSqlNonQuery(string comandSql)
        {
            ExecuteSqlNonQuery(comandSql, null);
        }

        public void ExecuteSqlNonQueryAuto(string procedureName, object parameters)
        {
            ExecuteSqlNonQuery(procedureName, GetParametersSqlQueryAnotation(parameters, ActionType.Update).ToArray());
        }

        public IEnumerable<T> ExecuteSqlViewFindByCondition<T>(string procedureName, object parameters, string cadenaConexion)
        {
            _cnx = cadenaConexion;
            return ExecuteSqlQuery<T>(procedureName, GetParametersSqlQueryAnotation(parameters, ActionType.View).ToArray());
        }

        public T ExecuteSqlViewId<T>(string procedureName, T parameters, string cadenaConexion)
        {
            _cnx = cadenaConexion;
            return ExecuteSqlQuery<T>(procedureName, GetParametersSqlQueryAnotation(parameters, ActionType.Everything).ToArray()).FirstOrDefault();
        }

        public IEnumerable<T> ExecuteSqlViewAll<T>(string procedureName, T parameters)
        {
            return ExecuteSqlQuery<T>(procedureName, GetParametersSqlQueryAnotation(parameters, ActionType.Everything).ToArray());
        }

        public object ExecuteSqlInsert<T>(string procedureName, T parameters)
        {
            return ExecuteSqlNonQuery(procedureName, GetParametersSqlQueryAnotation(parameters, ActionType.Insert).ToArray())[0].valor;
        }
        public object ExecuteSqlUpdate<T>(string procedureName, T parameters)
        {
            return ExecuteSqlNonQuery(procedureName, GetParametersSqlQueryAnotation(parameters, ActionType.Update).ToArray());
        }

        public object ExecuteSqlDelete<T>(string procedureName, T parameters)
        {
            return ExecuteSqlNonQuery(procedureName, GetParametersSqlQueryAnotation(parameters, ActionType.Delete).ToArray());
        }

        public DbParametro[] ExecuteSqlNonQuery(string procedureName, DbParametro[] parameters)
        {
            SqlConnection cnx = new SqlConnection(_cnx);
            SqlCommand cmd = cnx.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = procedureName;

            try
            {
                foreach (DbParametro parametro in parameters)
                {
                    SqlParameter prm = new SqlParameter();
                    prm.ParameterName = parametro.nombre.ToUpper();
                    prm.SqlDbType = parametro.dbType;
                    prm.Size = (parametro.size == 0) ? prm.Size : parametro.size;
                    prm.Direction = parametro.direccion;
                    //prm.IsNullable = parametro.IsNullable;
                    //prm.SourceColumn = parametro.SourceColumn;
                    //prm.SourceVersion = parametro.SourceVersion;
                    prm.Value = parametro.valor;
                    cmd.Parameters.Add(prm);
                }

                if (cmd.Connection.State.HasFlag(ConnectionState.Closed))
                    cmd.Connection.Open();

                cmd.Transaction = cmd.Connection.BeginTransaction();
                cmd.ExecuteNonQuery();
                if (parameters != null)
                {
                    SqlParameterCollection sqlParameters = cmd.Parameters;

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        parameters[i].valor = sqlParameters[parameters[i].nombre.ToUpper().Trim()].Value;
                    }
                }
                cmd.Transaction.Commit();
            }
            catch (SqlException ex)
            {
                if (cmd.Transaction != null)
                {
                    cmd.Transaction.Rollback();
                }
                throw ex;
            }
            catch (Exception ex)
            {
                if (cmd.Transaction != null)
                {
                    cmd.Transaction.Rollback();
                }
                throw ex;
            }
            finally
            {
                if (cmd.Connection != null && cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();
                    cmd.Dispose();
                }
            }
            return parameters;
        }

        public IEnumerable<T> ExecuteSqlQuery<T>(string comandSql)
        {
            return ExecuteSqlQuery<T>(comandSql, null);
        }

        public IEnumerable<T> ExecuteSqlQuery<T>(string procedureName, DbParametro[] parameters)
        {
            IEnumerable<T> queryResult;
            SqlCommand cmd = new SqlCommand(procedureName);
            cmd.CommandText = procedureName;
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                // Asignamos los parámetros
                if (parameters != null)
                {
                    foreach (DbParametro parametro in parameters)
                    {
                        SqlParameter prm = new SqlParameter();
                        prm.ParameterName = parametro.nombre;
                        prm.SqlDbType = parametro.dbType;
                        prm.Size = parametro.size;
                        prm.Direction = parametro.direccion;
                        //prm.IsNullable = parametro.IsNullable;
                        //prm.SourceColumn = parametro.SourceColumn;
                        //prm.SourceVersion = parametro.SourceVersion;
                        prm.Value = parametro.valor;
                        cmd.Parameters.Add(prm);
                    }
                }

                // Creamos la Conexion
                cmd.Connection = new SqlConnection(_cnx);
                if (cmd.Connection.State.HasFlag(ConnectionState.Closed))
                    cmd.Connection.Open();

                DataTable dataTable = new DataTable();
                cmd.Transaction = cmd.Connection.BeginTransaction();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    dataTable.Load(reader);
                    queryResult = ConvertTo<T>(dataTable);
                }
                cmd.Transaction.Commit();

            }
            catch (SqlException ex)
            {
                if (cmd.Transaction != null)
                {
                    cmd.Transaction.Rollback();
                }
                throw ex;
            }
            finally
            {
                if (cmd.Connection != null && cmd.Connection.State == ConnectionState.Open)
                {
                    cmd.Connection.Close();
                    cmd.Connection.Dispose();
                    cmd.Dispose();
                }
            }
            return queryResult;
        }

        private static SqlParameterCollection CreateParametros(string procedureName, string conexion)
        {
            SqlCommand cmd = new SqlCommand(procedureName.Trim());
            try
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = new SqlConnection(conexion);
                if (cmd.Connection.State == ConnectionState.Closed)
                {
                    cmd.Connection.Open();
                }
                SqlCommandBuilder.DeriveParameters(cmd);
                for (int i = 0; i < cmd.Parameters.Count; i++)
                {
                    string Nombre = cmd.Parameters[i].ParameterName;
                    if (Nombre.Substring(0, 1) == "@")
                    {
                        Nombre = Nombre.Substring(1);
                    }
                    cmd.Parameters[i].ParameterName = Nombre.ToUpper();
                }
                return cmd.Parameters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                cmd.Connection.Dispose();
                cmd.Dispose();
            }
        }

        private static IList<DbParametro> GetParametersSqlQuery(Object parameters)
        {
            IList<DbParametro> listaParametro = new List<DbParametro>();

            foreach (PropertyInfo prop in parameters.GetType().GetProperties())
            {
                dynamic valor = prop.GetValue(parameters, null);
                Type type = Nullable.GetUnderlyingType(prop.PropertyType);

                if (valor != null)
                {
                    type = (type == null) ? prop.PropertyType : type;

                    if (type != typeof(System.Collections.Generic.ICollection<>))
                    {
                        listaParametro.Add(new DbParametro() { nombre = prop.Name, valor = valor });
                    }
                }
            }

            return listaParametro;
        }

        private static IList<DbParametro> GetParametersSqlQueryAnotation<T>(T parameters, ActionType actionType)
        {
            IList<DbParametro> listaParametro = new List<DbParametro>();

            foreach (PropertyInfo prop in parameters.GetType().GetProperties())
            {
                dynamic valor = prop.GetValue(parameters, null);
                Type type = Nullable.GetUnderlyingType(prop.PropertyType);

                if (valor != null)
                {
                    type = (type == null) ? prop.PropertyType : type;
                    var attributes = prop.GetCustomAttributes(false);
                    var columnMapping = attributes.FirstOrDefault(a => a.GetType() == typeof(DBParameter));
                    if (columnMapping != null)
                    {
                        DbParametro parametro = new DbParametro() { valor = valor };
                        var mapsto = columnMapping as DBParameter;

                        switch (actionType)
                        {
                            case ActionType.Insert:
                                if (mapsto.actionType == ActionType.InsertDelete || mapsto.actionType == ActionType.InsertUpdate || mapsto.actionType == ActionType.Everything)
                                {
                                    mapsto.actionType = ActionType.Insert;
                                }
                                break;
                            case ActionType.Update:
                                if (mapsto.actionType == ActionType.InsertUpdate || mapsto.actionType == ActionType.UpdateDelete || mapsto.actionType == ActionType.Everything)
                                {
                                    mapsto.actionType = ActionType.Update;
                                }
                                break;
                            case ActionType.Delete:
                                if (mapsto.actionType == ActionType.InsertDelete || mapsto.actionType == ActionType.UpdateDelete || mapsto.actionType == ActionType.Everything)
                                {
                                    mapsto.actionType = ActionType.Delete;
                                }
                                break;
                            case ActionType.View:
                                if (mapsto.actionType == ActionType.View ||
                                    mapsto.actionType == ActionType.InsertDeleteView ||
                                    mapsto.actionType == ActionType.InsertUpdateView ||
                                    mapsto.actionType == ActionType.UpdateDeleteView ||
                                    mapsto.actionType == ActionType.Everything)
                                {
                                    mapsto.actionType = ActionType.View;
                                }
                                break;
                            default:
                                break;
                        }

                        if (mapsto.actionType == actionType)
                        {
                            parametro.nombre = (mapsto.name == "") ? prop.Name : mapsto.name;
                            parametro.direccion = (mapsto.Key == true && mapsto.actionType == ActionType.Insert) ? ParameterDirection.Output : mapsto.direction;
                            parametro.dbType = mapsto.dbType;
                            parametro.size = mapsto.size;
                            listaParametro.Add(parametro);
                        }
                    }
                }
            }

            return listaParametro;
        }

        public IList<T> ConvertTo<T>(IDataReader reader)
        {
            DataTable dataTable = new DataTable();
            IList<T> queryResult;
            dataTable.Load(reader);
            queryResult = ConvertTo<T>(dataTable);
            return queryResult;
        }

        public T Convert<T>(IDataReader reader)
        {
            DataTable dataTable = new DataTable();
            IList<T> queryResult;
            dataTable.Load(reader);
            queryResult = ConvertTo<T>(dataTable);

            T notData = default(T);

            if (queryResult.Count == 0)
            {
                return notData;
            }

            return queryResult[0];
        }

        public static IList<T> ConvertTo<T>(DataTable table)
        {
            if (table == null)
                return null;

            List<DataRow> rows = new List<DataRow>();
            foreach (DataRow row in table.Rows)
                rows.Add(row);

            return ConvertTo<T>(rows);
        }

        public static IList<T> ConvertTo<T>(IList<DataRow> rows)
        {
            IList<T> list = null;
            if (rows != null)
            {
                list = new List<T>();
                foreach (DataRow row in rows)
                {
                    T item = CreateItem<T>(row);
                    list.Add(item);
                }
            }
            return list;
        }

        public static T CreateItem<T>(DataRow row)
        {
            string columnName;
            T obj = default(T);
            if (row != null)
            {
                obj = Activator.CreateInstance<T>();
                foreach (DataColumn column in row.Table.Columns)
                {
                    columnName = column.ColumnName;
                    //Get property with same columnName
                    PropertyInfo prop = obj.GetType().GetProperty(columnName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    try
                    {
                        if (prop != null)
                        {
                            //Get value for the column
                            object value = (row[columnName].GetType() == typeof(DBNull)) ? null : row[columnName];
                            //Set property value
                            prop.SetValue(obj, value, null);
                        }
                    }
                    catch
                    {
                        throw;
                        //Catch whatever here
                    }
                }
            }
            return obj;
        }
    }
}
