using System;
using System.Data;

namespace Net.Connection.Attributes
{
    public class DBParameter : Attribute
    {
        public string name { get; set; }
        public int size { get; set; }
        public SqlDbType dbType { get; set; }
        public ParameterDirection direction { get; set; }

        public ActionType actionType;

        public Boolean Key { get; set; }

        public DBParameter(SqlDbType dbType, ActionType actionType, Boolean Key)
        {
            DBParameterMethod(dbType, 0, ParameterDirection.Input, "", actionType, Key);
        }
        public DBParameter(SqlDbType dbType, ActionType actionType)
        {
            DBParameterMethod(dbType, 0, ParameterDirection.Input, "", actionType, false);
        }
        public DBParameter(SqlDbType dbType, ActionType actionType, ParameterDirection direction)
        {
            DBParameterMethod(dbType, 0, direction, "", actionType, false);
        }
        public DBParameter(SqlDbType dbType, int size, ActionType actionType)
        {
            DBParameterMethod(dbType, size, ParameterDirection.Input, "", actionType, false);
        }
        public DBParameter(SqlDbType dbType, int size, ParameterDirection direction, ActionType actionType)
        {
            DBParameterMethod(dbType, size, direction, "", actionType, false);
        }
        public DBParameter(SqlDbType dbType, int size, ParameterDirection direction, string name, ActionType actionType)
        {
            DBParameterMethod(dbType, size, direction, name, actionType, false);
        }

        private void DBParameterMethod(SqlDbType dbType, int size, ParameterDirection direction, string name, ActionType actionType, Boolean Key)
        {
            this.dbType = dbType;
            this.direction = direction;
            this.size = size;
            this.name = name;
            this.actionType = actionType;
            this.Key = Key;
        }
    }
}
