using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.Data;

namespace DataBase.DBAccess
{
    public class HDBOperation
    {
        public static object QueryScalar(string _strSql)
        {
            using (HDataBase hDatabase = new HDataBase())
            {
                using (IDbCommand iCommand = hDatabase.CreateDBCommand(_strSql))
                {
                    object oValue = iCommand.ExecuteScalar();
                    iCommand.Dispose();
                    hDatabase.AutoClose();
                    return oValue;
                }
            }
        }

        public static int QueryNonQuery(string _strSql)
        {
            using (HDataBase hDatabase = new HDataBase())
            {
                using (IDbCommand iCommand = hDatabase.CreateDBCommand(_strSql))
                {
                    object oValue = iCommand.ExecuteNonQuery();
                    iCommand.Dispose();
                    hDatabase.AutoClose();
                    return (int)oValue;
                }
            }
        }

        public static DataSet QueryDataSet(string _strSql)
        {
            using (HDataBase hDatabase = new HDataBase())
            {
                IDataAdapter iDBAdapter = hDatabase.CreateDBAdapter(_strSql);
                DataSet dsFill = new DataSet();
                iDBAdapter.Fill(dsFill);
                hDatabase.AutoClose();
                return dsFill;
            }
        }
    }
}
