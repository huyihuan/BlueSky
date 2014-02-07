using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Common;
using System.Data;

namespace DataBase.DBHelper
{
    public class HDBOperation
    {
        public static object QueryScalar(string __sql)
        {
            using (HDataBase db = new HDataBase())
            {
                using (IDbCommand command = db.CreateDBCommand(__sql))
                {
                    object oValue = command.ExecuteScalar();
                    command.Dispose();
                    db.AutoClose();
                    return oValue;
                }
            }
        }

        public static int QueryNonQuery(string __sql)
        {
            using (HDataBase db = new HDataBase())
            {
                using (IDbCommand command = db.CreateDBCommand(__sql))
                {
                    object oValue = command.ExecuteNonQuery();
                    command.Dispose();
                    db.AutoClose();
                    return (int)oValue;
                }
            }
        }

        public static DataSet QueryDataSet(string __sql)
        {
            HDataBase db = new HDataBase();
            IDataAdapter da = db.CreateDBAdapter(__sql);
            DataSet ds = new DataSet();
            da.Fill(ds);
            db.AutoClose();
            return ds;
        }
    }
}
