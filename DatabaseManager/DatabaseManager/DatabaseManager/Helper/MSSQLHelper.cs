using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace DatabaseManager
{
    public class FieldMapper
    {
        public string DBFieldType { get; set; }

        //public string CFieldType { get; set; }
        public Type type { get; set; }

        public bool IsNullable { get; set; }

        public FieldMapper(string DBFieldType, Type type)
        {
            this.DBFieldType = DBFieldType;
            //this.CFieldType = CFieldType;
            this.type = type;
            this.IsNullable = true;

            switch (type.Name.ToString())
            {
                case "byte[]":
                case "string":
                case "DbGeography":
                case "DbGeometry":
                    IsNullable = false;
                    break;

                default:
                    IsNullable = true;
                    break;
            }
        }
    }

    public class MSSQLHelper : IDisposable
    {
        public string Message;
        private List<FieldMapper> fieldMapperList = new List<FieldMapper>();

        private SqlConnection sqlCon;
        private SqlTransaction sqlTran;

        private string connetionString = null;
        private string ServerName;
        private string DatabaseName;
        private string UserName;
        private string Password;

        public ConnectionState ConnectionState
        {
            get
            {
                return sqlCon.State;
            }
        }

        public MSSQLHelper(
                    string ServerName,
                    string DatabaseName,
                    string UserName,
                    string Password)
        {
            this.ServerName = ServerName;
            this.DatabaseName = DatabaseName;
            this.UserName = UserName;
            this.Password = Password;
            SetMapper();

            Open();
        }

        public MSSQLHelper(string ConnectionString)
        {
            SetMapper();
            ParceConStr(ConnectionString, ref this.ServerName, ref this.DatabaseName, ref this.UserName, ref this.Password);
            Open();
        }

        private void ParceConStr(string ConnectionString, ref string serverName, ref string databaseName, ref string userName, ref string password)
        {
            serverName = GetStr(ConnectionString, "Server=", ";");
            databaseName = GetStr(ConnectionString, "Database=", ";");
            userName = GetStr(ConnectionString, "User ID=", ";");
            password = GetStr(ConnectionString, "Password=", ";");
        }

        private string GetStr(string connectionString, string key1, string key2)
        {
            string newStr = "";

            int ind1 = connectionString.ToLower(new CultureInfo("en-US", false)).IndexOf(key1.ToLower(new CultureInfo("en-US", false)));
            if (ind1 == -1)
                return "";

            ind1 += key1.Length;

            string str1 = connectionString.Substring(ind1, connectionString.Length - ind1);

            int ind2 = str1.ToLower(new CultureInfo("en-US", false)).IndexOf(key2.ToLower(new CultureInfo("en-US", false)));

            newStr = str1.Substring(0, ind2);
            return newStr;
        }

        private void SetMapper()
        {
            fieldMapperList.Add(new FieldMapper("bigint", typeof(long)));
            fieldMapperList.Add(new FieldMapper("binary", typeof(byte[])));
            fieldMapperList.Add(new FieldMapper("bit", typeof(bool)));
            fieldMapperList.Add(new FieldMapper("char", typeof(string)));
            fieldMapperList.Add(new FieldMapper("date", typeof(DateTime)));
            fieldMapperList.Add(new FieldMapper("datetime", typeof(DateTime)));
            fieldMapperList.Add(new FieldMapper("datetime2", typeof(DateTime)));
            fieldMapperList.Add(new FieldMapper("datetimeoffset", typeof(DateTimeOffset)));
            fieldMapperList.Add(new FieldMapper("decimal", typeof(decimal)));
            fieldMapperList.Add(new FieldMapper("float", typeof(double)));
            fieldMapperList.Add(new FieldMapper("hierarchyid", typeof(string)));
            fieldMapperList.Add(new FieldMapper("image", typeof(byte[])));
            fieldMapperList.Add(new FieldMapper("int", typeof(int)));
            fieldMapperList.Add(new FieldMapper("money", typeof(decimal)));
            fieldMapperList.Add(new FieldMapper("nchar", typeof(string)));
            fieldMapperList.Add(new FieldMapper("ntext", typeof(string)));
            fieldMapperList.Add(new FieldMapper("numeric", typeof(decimal)));
            fieldMapperList.Add(new FieldMapper("nvarchar", typeof(string)));
            fieldMapperList.Add(new FieldMapper("real", typeof(float)));
            fieldMapperList.Add(new FieldMapper("smalldatetime", typeof(DateTime)));
            fieldMapperList.Add(new FieldMapper("smallint", typeof(short)));
            fieldMapperList.Add(new FieldMapper("smallmoney", typeof(decimal)));
            fieldMapperList.Add(new FieldMapper("sql_variant", typeof(string)));
            fieldMapperList.Add(new FieldMapper("sysname", typeof(string)));
            fieldMapperList.Add(new FieldMapper("text", typeof(string)));
            fieldMapperList.Add(new FieldMapper("time", typeof(TimeSpan)));
            fieldMapperList.Add(new FieldMapper("timestamp", typeof(byte[])));
            fieldMapperList.Add(new FieldMapper("tinyint", typeof(byte)));
            fieldMapperList.Add(new FieldMapper("uniqueidentifier", typeof(Guid)));
            fieldMapperList.Add(new FieldMapper("varbinary", typeof(byte[])));
            fieldMapperList.Add(new FieldMapper("varchar", typeof(string)));
            fieldMapperList.Add(new FieldMapper("xml", typeof(string)));
        }

        public void Dispose()
        {
            Close();
        }

        public bool Open()
        {
            connetionString = "Data Source=/*ServerName*/;Initial Catalog=/*DatabaseName*/;User ID=/*UserName*/;Password=/*Password*/";

            connetionString = connetionString.Replace("/*ServerName*/", ServerName);
            connetionString = connetionString.Replace("/*DatabaseName*/", DatabaseName);
            connetionString = connetionString.Replace("/*UserName*/", UserName);
            connetionString = connetionString.Replace("/*Password*/", Password);

            sqlCon = new SqlConnection(connetionString);
            try
            {
                sqlCon.Open();
                Message = "Connection Open !";
                return true;
            }
            catch (Exception ex)
            {
                Message = "Can not open connection ! " + ex.Message;
                return false;
            }
        }

        public bool Close()
        {
            try
            {
                if (sqlCon.State == ConnectionState.Open)
                    sqlCon.Close();

                Message = "Connection Closed !";
                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }

        public bool TranssactionBegin()
        {
            try
            {
                sqlTran = sqlCon.BeginTransaction();
                Message = "";
                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }

        public bool Commit()
        {
            try
            {
                sqlTran.Commit();
                Message = "";
                sqlTran.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }

        public bool RollBack()
        {
            try
            {
                sqlTran.Rollback();
                Message = "";
                sqlTran.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }

        public bool ExecuteNonQuery(string sql)
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand(sql, sqlCon);
                sqlCommand.CommandTimeout = 3600;
                if (sqlCon.State != ConnectionState.Open)
                    sqlCon.Open();

                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }

        public bool ExecuteReader(ref SqlDataReader dr, string sql)
        {
            try
            {
                SqlCommand sqlCommand = new SqlCommand(sql, sqlCon);
                sqlCommand.CommandTimeout = 3600;
                sqlCommand.CommandType = CommandType.Text;
                dr = sqlCommand.ExecuteReader();
                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }

        public SqlDataReader ExecuteReader_TEST(string sql)
        {
            SqlDataReader dr = null;

            try
            {
                SqlCommand sqlCommand = new SqlCommand(sql, sqlCon);
                sqlCommand.CommandTimeout = 3600;
                sqlCommand.CommandType = CommandType.Text;

                dr = sqlCommand.ExecuteReader();

                return dr;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                dr.Close();
                return null;
            }
        }

        public bool ExecuteReader(ref DataTable dt, string sql)
        {
            try
            {
                SqlDataReader dr = null;
                bool retVal = ExecuteReader(ref dr, sql);

                if (!retVal)
                    return false;

                dt = new DataTable();
                dt.Load(dr);

                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }

        public void SQLBulkCopyWriteToServer(string destinationTableName, DataTable dataTable, int recordSize = 1000)
        {
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlCon, SqlBulkCopyOptions.KeepIdentity, sqlTran))
            {
                bulkCopy.DestinationTableName = destinationTableName;

                foreach (DataColumn coll in dataTable.Columns)
                {
                    var collName = coll.ColumnName;
                    //DataRow[] results = dataTable.Select("COLUMN_NAME = '" + collName + "'");
                    //FieldMapper fieldMapper = fieldMapperList.FirstOrDefault(p => p.DBFieldType == results[0].ItemArray[7].ToString());
                    //coll.DataType = fieldMapper.type;
                    bulkCopy.ColumnMappings.Add(collName, collName);
                }
                bulkCopy.BatchSize = recordSize;
                bulkCopy.BulkCopyTimeout = 1800;
                bulkCopy.WriteToServer(dataTable);
            }
        }

        public bool TableList(ref DataTable dt)
        {
            try
            {
                string sql = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' ORDER BY TABLE_NAME";
                SqlDataReader dr = null;
                bool retVal = ExecuteReader(ref dr, sql);

                if (!retVal)
                    return false;

                dt = new DataTable();
                dt.Load(dr);

                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }

        public bool ViewList(ref DataTable dt)
        {
            try
            {
                string sql = "SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='VIEW'";
                SqlDataReader dr = null;
                bool retVal = ExecuteReader(ref dr, sql);

                if (!retVal)
                    return false;

                dt = new DataTable();
                dt.Load(dr);

                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }

        public bool ColumnInfo(ref DataTable dt, string tableName = "", string columnName = "")
        {
            try
            {
                string sql = "SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE 1 = 1 ";

                if (!string.IsNullOrEmpty(tableName))
                    sql += "AND TABLE_NAME = '" + tableName + "'";

                if (!string.IsNullOrEmpty(columnName))
                    sql += "AND COLUMN_NAME = '" + columnName + "'";

                SqlDataReader dr = null;
                bool retVal = ExecuteReader(ref dr, sql);

                if (!retVal)
                    return false;

                dt = new DataTable();
                dt.Load(dr);

                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }

        public bool PrimaryKeyInfo(ref DataTable dt, string tableName = "")
        {
            string sql = @"SELECT
                            KU.table_name as TABLENAME,
                        column_name as PRIMARYKEYCOLUMN
                        FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC
                        INNER JOIN
                            INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KU
                                  ON TC.CONSTRAINT_TYPE = 'PRIMARY KEY' AND
                                     TC.CONSTRAINT_NAME = KU.CONSTRAINT_NAME
                                     AND KU.table_name= /*TABLENAME*/
                        ORDER BY KU.TABLE_NAME, KU.ORDINAL_POSITION;";

            try
            {
                SqlDataReader dr = null;

                if (!string.IsNullOrEmpty(tableName))
                {
                    sql = sql.Replace("/*TABLENAME*/", "'" + tableName + "'");
                }
                else
                {
                    sql = sql.Replace("AND KU.table_name= /*TABLENAME*/", "");
                }

                bool retVal = ExecuteReader(ref dr, sql);

                if (!retVal)
                    return false;

                dt = new DataTable();
                dt.Load(dr);

                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }

        public bool IndexInfo(ref DataTable dt, string tableName = "")
        {
            string sql = @"SELECT
                         TableName = t.name,
                         IndexName = ind.name,
                         IndexId = ind.index_id,
                         ColumnId = ic.index_column_id,
                         ColumnName = col.name,
                         ind.*,
                         ic.*,
                         col.*
                    FROM
                         sys.indexes ind
                    INNER JOIN
                         sys.index_columns ic ON  ind.object_id = ic.object_id and ind.index_id = ic.index_id
                    INNER JOIN
                         sys.columns col ON ic.object_id = col.object_id and ic.column_id = col.column_id
                    INNER JOIN
                         sys.tables t ON ind.object_id = t.object_id
                    WHERE
                         ind.is_primary_key = 0
                         AND ind.is_unique = 0
                         AND ind.is_unique_constraint = 0
                         AND t.is_ms_shipped = 0
	                     AND t.name = /*TABLENAME*/
                    ORDER BY
                         t.name, ind.name, ind.index_id, ic.index_column_id;";

            try
            {
                SqlDataReader dr = null;

                if (!string.IsNullOrEmpty(tableName))
                {
                    sql = sql.Replace("/*TABLENAME*/", "'" + tableName + "'");
                }
                else
                {
                    sql = sql.Replace("AND t.name = /*TABLENAME*/", "");
                }

                bool retVal = ExecuteReader(ref dr, sql);

                if (!retVal)
                    return false;

                dt = new DataTable();
                dt.Load(dr);

                return true;
            }
            catch (Exception ex)
            {
                Message = ex.Message;
                return false;
            }
        }

        private string Tab = "        ";

        public string GetInsertSql(string tableName)
        {
            string sqlStr = "INSERT INTO " + tableName + "(" + Environment.NewLine;

            DataTable dt = null;
            ColumnInfo(ref dt, tableName);
            DataTable dtPri = null;
            PrimaryKeyInfo(ref dtPri, tableName);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow rw = dt.Rows[i];
                string columnName = rw["COLUMN_NAME"].ToString();

                DataRow[] results = dtPri.Select("PRIMARYKEYCOLUMN = '" + columnName + "'");
                if (results.Count() > 0)
                    continue;

                sqlStr += Tab + columnName;

                if (i < dt.Rows.Count - 1)
                    sqlStr += "," + Environment.NewLine;
            }

            sqlStr += ")" + Environment.NewLine;
            sqlStr += "VALUES(" + Environment.NewLine;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow rw = dt.Rows[i];
                string columnName = rw["COLUMN_NAME"].ToString();

                DataRow[] results = dtPri.Select("PRIMARYKEYCOLUMN = '" + columnName + "'");
                if (results.Count() > 0)
                    continue;

                sqlStr += Tab + columnName + " = @" + columnName;

                if (i < dt.Rows.Count - 1)
                    sqlStr += "," + Environment.NewLine;
            }
            sqlStr += ")" + Environment.NewLine;

            return sqlStr;
        }

        public string GetUpdateSql(string tableName)
        {
            string sqlStr = "UPDATE " + tableName + Environment.NewLine;
            sqlStr += " SET";
            sqlStr += Environment.NewLine;

            DataTable dt = null;
            ColumnInfo(ref dt, tableName);
            DataTable dtPri = null;
            PrimaryKeyInfo(ref dtPri, tableName);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow rw = dt.Rows[i];
                string columnName = rw["COLUMN_NAME"].ToString();

                DataRow[] results = dtPri.Select("PRIMARYKEYCOLUMN = '" + columnName + "'");
                if (results.Count() > 0)
                    continue;

                sqlStr += Tab + columnName + " = @" + columnName;

                if (i < dt.Rows.Count - 1)
                    sqlStr += "," + Environment.NewLine;
            }

            sqlStr += Environment.NewLine;

            sqlStr += "WHERE 1 = 1" + Environment.NewLine;

            for (int i = 0; i < dtPri.Rows.Count; i++)
            {
                DataRow rw = dtPri.Rows[i];
                string columnName = rw["PRIMARYKEYCOLUMN"].ToString();
                sqlStr += Tab + " AND " + columnName + " = @" + columnName;

                if (i < dt.Rows.Count - 1)
                    sqlStr += Environment.NewLine;
            }

            return sqlStr;
        }

        public string GetDeleteSql(string tableName)
        {
            string sqlStr = "DELETE " + tableName + Environment.NewLine;

            DataTable dt = null;
            ColumnInfo(ref dt, tableName);
            DataTable dtPri = null;
            PrimaryKeyInfo(ref dtPri, tableName);

            sqlStr += "WHERE 1 = 1" + Environment.NewLine;

            for (int i = 0; i < dtPri.Rows.Count; i++)
            {
                DataRow rw = dtPri.Rows[i];
                string columnName = rw["PRIMARYKEYCOLUMN"].ToString();
                sqlStr += Tab + " AND " + columnName + " = @" + columnName;

                if (i < dt.Rows.Count - 1)
                    sqlStr += Environment.NewLine;
            }
            return sqlStr;
        }

        public string GetModel(string tableName)
        {
            string str = "";
            string fieldsStr = "";

            str = @"public class /*TableName*/
{
/*Fields*/
}";
            str = str.Replace("/*TableName*/", tableName + "Model");

            DataTable dt = null;
            ColumnInfo(ref dt, tableName);
            foreach (DataRow rw in dt.Rows)
            {
                string columnName = rw["COLUMN_NAME"].ToString();
                switch (columnName)
                {
                    case "char":
                    case "decimal":
                    case "float":
                    case "int":
                        columnName = "@" + columnName;
                        break;

                    default:
                        break;
                }

                fieldsStr += Tab + "public /*Type*/ " + columnName + " { get; set; }" + Environment.NewLine;

                string typeStr = "";

                FieldMapper fieldMapper = fieldMapperList.FirstOrDefault(p => p.DBFieldType == rw["DATA_TYPE"].ToString());

                if (fieldMapper == null)
                    continue;

                typeStr += fieldMapper.type.Name.ToString();

                //c# değişkeni null olabilir.
                if (rw["IS_NULLABLE"].ToString().Equals("YES") && fieldMapper.IsNullable)
                {
                    typeStr = "Nullable<" + typeStr + ">";
                }
                typeStr += " ";
                fieldsStr = fieldsStr.Replace("/*Type*/", typeStr);
            }

            str = str.Replace("/*Fields*/", fieldsStr);
            return str;
        }

        public string GetCreateSqlScript(DataTable dt, string tableName = "TableName")
        {
            string sql = "IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='/*TABLENAME*/' and xtype='U')" + Environment.NewLine;
            sql += "BEGIN" + Environment.NewLine;
            sql += "    CREATE TABLE /*TABLENAME*/ (" + Environment.NewLine;

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                DataColumn item = dt.Columns[i];
                sql += "        " + item.ColumnName + " nvarchar(MAX)";

                if (i < dt.Columns.Count - 1)
                    sql += ",";
                sql += Environment.NewLine;
            }

            sql += "    )" + Environment.NewLine;
            sql += "END" + Environment.NewLine;

            sql = sql.Replace("/*TABLENAME*/", tableName);

            return sql;
        }

        public SqlParameterCollection GetSqlParameter(string tableName)
        {
            SqlParameterCollection parameters = null;// = new SqlParameterCollection();

            //SqlParameter param = parameter.Add();
            DataTable dt = null;
            ColumnInfo(ref dt, tableName);
            foreach (DataRow rw in dt.Rows)
            {
                string columnName = rw["COLUMN_NAME"].ToString();
                SqlParameter param = new SqlParameter();
            }
            return parameters;
        }
    }
}