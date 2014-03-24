using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Reflection;

namespace DataBase.DBAccess
{
    public class HSqlFactory
    {
        private string _strSqlString = string.Empty;
        private HSqlType _sqlType = HSqlType.Select;
        private string _sqlTableName = string.Empty;
        private string _sqlContent = string.Empty;
        private string _sqlWhere = string.Empty;

        public HSqlFactory()
        { }
        public HSqlFactory(HSqlType __SqlType, string __strTableName, string __strOperContent, string __strWhereString)
        {
            this._sqlType = __SqlType;
            this._sqlTableName = __strTableName;
            this._sqlContent = __strOperContent;
            this._sqlWhere = __strWhereString;
        }

        public string SqlString
        {
            get { return _strSqlString; }
        }
        public string SqlTableName
        {
            get { return _sqlTableName; }
            set { _sqlTableName = value; }
        }
        public string SqlContent
        {
            get { return _sqlContent; }
            set { _sqlContent = value; }
        }
        public string SqlWhereString
        {
            get { return _sqlWhere; }
            set { _sqlWhere = value; }
        }
        public HSqlType SqlType
        {
            get { return _sqlType; }
            set { _sqlType = value; }
        }

        public override string ToString()
        {
            StringBuilder sbSql = new StringBuilder();
            switch (SqlType)
            {
                case HSqlType.Select:
                    sbSql.Append(string.Format("select {0} from {1} ", this.SqlContent, this.SqlTableName));
                    break;
                case HSqlType.Update:
                    sbSql.Append(string.Format("update {0} set {1} ", this.SqlTableName, this.SqlContent));
                    break;
                case HSqlType.Delete:
                    sbSql.Append(string.Format("delete from {0} ", this.SqlTableName));
                    break;
                case HSqlType.Insert:
                    sbSql.Append(string.Format("SET IDENTITY_INSERT [{0}] ON;", SqlTableName));
                    sbSql.Append(string.Format("insert into {0}({1}) values({2});", SqlTableName, SqlContent, SqlWhereString));
                    break;
            }

            if (SqlType != HSqlType.Insert && !string.IsNullOrEmpty(SqlWhereString))
            {
                if (!(SqlType == HSqlType.Select && this.SqlContent.IndexOf("ROW_NUMBER") != -1))
                    sbSql.Append(" where ");
                sbSql.Append(string.Format(" {0}", this.SqlWhereString));
            }
            if(SqlType == HSqlType.Insert)
                sbSql.Append(string.Format("SET IDENTITY_INSERT [{0}] OFF;", SqlTableName));

            _strSqlString = sbSql.ToString();
            return _strSqlString;
        }

    }

    public enum HSqlType
    { 
        Select = 0,
        Update = 1,
        Delete = 2,
        Insert = 3
    }
}
