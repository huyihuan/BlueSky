using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Text;
using DataBase.DBAccess;
using System.Data;
using System.Collections;

namespace DataBase
{
    public class HEntityCommon
    {
        static string[] alInt = new string[] { "int32", "int", "double", "long" };
        static string[] alString = new string[] { "string", "datetime" };

        private ActiveEntity _activeEntity;

        public HEntityCommon()
        { }

        public HEntityCommon(object _oE)
        {
            if (!(_oE is Interface.IEntity))
                return;
            this._activeEntity = new ActiveEntity(_oE);
        }

        public static HEntityCommon HEntity(object _oE)
        {
            if (null == _oE)
                return null;
            HEntityCommon oEntity = new HEntityCommon(_oE);
            return oEntity;
        }

        #region 私有方法

        private static object[] ChangeTableToEntitys(HEntityCommon _oE, DataTable __dt)
        {
            if (null == __dt || __dt.Rows.Count == 0)
                return null;
            int nRowNum = __dt.Rows.Count;
            ArrayList alItems = new ArrayList();
            for (int i = 0; i < nRowNum; i++)
            {
                DataRow rowItem = __dt.Rows[i];
                object oInstance = Activator.CreateInstance(_oE._activeEntity.ObjectType);
                foreach (FieldInfo f in _oE._activeEntity.ObjectFields)
                {
                    if (rowItem[f.Name] is DBNull)
                        f.SetValue(oInstance, null);
                    else
                        f.SetValue(oInstance, rowItem[f.Name]);
                }
                alItems.Add(oInstance);
            }
            return (object[])alItems.ToArray(_oE._activeEntity.ObjectType);
        }

        private string _GetParamter(object _oValue, FieldInfo _field)
        {
            string strName = _field.Name;
            string strTpName = _field.FieldType.Name.ToLower();
            //数据库中int默认为0，所以当int型字段为0时跳过，不添加入参数列表
            if (strTpName == "int32" && (int)_oValue == 0)
            {
                return null;
            }
            if (alInt.Contains(strTpName))
            {
                return string.Format("[{0}]={1}", strName, _oValue);
            }
            else if (alString.Contains(strTpName))
            {
                return string.Format("[{0}]='{1}'", strName, _oValue);
            }
            return "";
        }

        private string _CreateSelectWhere(HEntityCommon _oE)
        {
            int nFieldNum = _oE._activeEntity.ObjectFields.Length;
            StringBuilder sb = new StringBuilder("1=1");
            for (int i = 0; i < nFieldNum; i++)
            {
                object oValue = _oE._activeEntity.ObjectFields[i].GetValue(_oE._activeEntity.ActiveObject);
                if (null != oValue)
                {
                    string strSingleWhere = _GetParamter(oValue, _oE._activeEntity.ObjectFields[i]);
                    if (!string.IsNullOrEmpty(strSingleWhere))
                        sb.Append(string.Format(" and {0}", strSingleWhere));
                }
            }
            return sb.ToString();
        }

        #endregion

        public object Instance()
        {
            object oI = Activator.CreateInstance(this._activeEntity.ObjectType);
            return oI;
        }

        public object EntityClone()
        {
            object oClone = Activator.CreateInstance(this._activeEntity.ObjectType);
            foreach (FieldInfo f in this._activeEntity.ObjectFields)
            {
                object oValue = f.GetValue(this._activeEntity.ActiveObject);
                f.SetValue(oClone, oValue);
            }
            return oClone;
        }

        public int EntityCount()
        {
            string strWhere = _CreateSelectWhere(this);
            string strCacheKey = string.Format("{0}-count-{1}", this._activeEntity.TableName, strWhere);
            if (CacheUtil.IsExistCache(strCacheKey))
            {
                object oCache = CacheUtil.GetValue(strCacheKey);
                if (null != oCache)
                    return (int)oCache;
            }
            HSqlFactory query = new HSqlFactory();
            query.SqlType = HSqlType.Select;
            query.SqlTableName = this._activeEntity.TableName;
            query.SqlContent = "count(1)";
            query.SqlWhereString = strWhere;
            object oCount = HDBOperation.QueryScalar(query.ToString());
            CacheUtil.SetValue(strCacheKey, oCount);
            return (int)oCount;
        }

        public int EntityCount(string _strFilter)
        {
            string strCacheKey = string.Format("{0}-count-{1}", this._activeEntity.TableName, _strFilter);
            if (CacheUtil.IsExistCache(strCacheKey))
            {
                object oCache = CacheUtil.GetValue(strCacheKey);
                if (null != oCache)
                    return (int)oCache;
            }
            HSqlFactory query = new HSqlFactory();
            query.SqlType = HSqlType.Select;
            query.SqlTableName = this._activeEntity.TableName;
            query.SqlContent = "count(1)";
            query.SqlWhereString = _strFilter;
            object oCount = HDBOperation.QueryScalar(query.ToString());
            CacheUtil.SetValue(strCacheKey, oCount);
            return (int)oCount;
        }

        public object[] EntityList()
        {
            string strWhere = _CreateSelectWhere(this);
            string strCacheKey = string.Format("{0}-{1}", this._activeEntity.TableName, strWhere);
            if (CacheUtil.IsExistCache(strCacheKey))
            {
                object oCache = CacheUtil.GetValue(strCacheKey);
                //if (null != oCache)
                return (object[])oCache;
            }
            HSqlFactory query = new HSqlFactory();
            query.SqlType = HSqlType.Select;
            query.SqlTableName = this._activeEntity.TableName;
            query.SqlContent = this._activeEntity.SelectContent;
            query.SqlWhereString = strWhere;
            DataSet ds = HDBOperation.QueryDataSet(query.ToString());
            object[] alObjects = ChangeTableToEntitys(this, ds.Tables[0]);
            CacheUtil.SetValue(strCacheKey, alObjects);
            return alObjects;
        }

        public object[] EntityList(string _strFilter)
        {
            string strCacheKey = string.Format("{0}-{1}", this._activeEntity.TableName, _strFilter);
            if (CacheUtil.IsExistCache(strCacheKey))
            {
                object oCache = CacheUtil.GetValue(strCacheKey);
                if (null != oCache)
                    return (object[])oCache;
            }
            HSqlFactory query = new HSqlFactory();
            query.SqlType = HSqlType.Select;
            query.SqlTableName = this._activeEntity.TableName;
            query.SqlContent = this._activeEntity.SelectContent;
            query.SqlWhereString = _strFilter;
            DataSet ds = HDBOperation.QueryDataSet(query.ToString());
            object[] alObjects = ChangeTableToEntitys(this, ds.Tables[0]);
            CacheUtil.SetValue(strCacheKey, alObjects);
            return alObjects;
        }

        public object[] EntityList(string _strFilter, string _strSort)
        {
            if (!string.IsNullOrEmpty(_strSort))
                _strFilter += string.Format(" order by {0}", _strSort.Trim());
            return EntityList(_strFilter);
        }

        public object[] EntityList(string _strFilter, string _strSort, int _nPageIndex, int _nPageSize)
        {
            string strCacheKey = string.Format("{0}-{1}-{2}-{3}", this._activeEntity.TableName, _strFilter, _nPageIndex, _nPageSize);
            if (CacheUtil.IsExistCache(strCacheKey))
            {
                object oCache = CacheUtil.GetValue(strCacheKey);
                if (null != oCache)
                    return (object[])oCache;
            }
            HSqlFactory query = new HSqlFactory();
            query.SqlType = HSqlType.Select;
            query.SqlTableName = this._activeEntity.TableName;

            if (!string.IsNullOrEmpty(_strFilter))
                _strFilter = string.Format("({0})", _strFilter);
            if (_nPageIndex == 1)
            {
                #region 如果查询第一页的数据采用top方式，提高效率
                query.SqlContent = string.Format("top {0} {1}", _nPageSize, this._activeEntity.SelectContent);
                if (!string.IsNullOrEmpty(_strFilter))
                    query.SqlWhereString = _strFilter;
                if (!string.IsNullOrEmpty(_strSort))
                    query.SqlWhereString += " order by " + _strSort;
                #endregion
            }
            else if (_nPageIndex * _nPageSize <= 1000000)
            {
                #region 分页方案(not in),在百万条数据内效率高
                query.SqlContent = string.Format("top {0} {1}", _nPageSize, this._activeEntity.SelectContent);
                if (!string.IsNullOrEmpty(_strFilter))
                    query.SqlWhereString += _strFilter + " and ";
                query.SqlWhereString += string.Format("{0} not in (select top (({1} - 1) * {2}) {0} from {3}{4} order by {0})", this._activeEntity.KeyName, _nPageIndex, _nPageSize, this._activeEntity.TableName, string.IsNullOrEmpty(_strFilter) ? "" : (" where " + _strFilter));
                if (!string.IsNullOrEmpty(_strSort))
                    query.SqlWhereString += " order by " + _strSort;
                #endregion
            }
            else
            {
                #region 分页方案(ROW_NUMBER()),在百万条数据以上效率高
                query.SqlContent = string.Format("{0} from (select ROW_NUMBER() over(order by {1}) as nRow,{0}", this._activeEntity.SelectContent, this._activeEntity.KeyName);
                if (!string.IsNullOrEmpty(_strFilter))
                    query.SqlWhereString += string.Format("where {0} ", _strFilter);
                query.SqlWhereString += string.Format(") xTemp where nRow >= (({0} - 1) * {1} + 1) and nRow <= ({0}*{1})", _nPageIndex, _nPageSize);
                if (!string.IsNullOrEmpty(_strSort))
                    query.SqlWhereString += " order by " + _strSort;
                #endregion
            }

            DataSet ds = HDBOperation.QueryDataSet(query.ToString());
            object[] alObjects = ChangeTableToEntitys(this, ds.Tables[0]);
            CacheUtil.SetValue(strCacheKey, alObjects);
            return alObjects;
        }

        public int EntitySave()
        {
            bool IsEntity = this._activeEntity.ActiveObject is DataBase.Interface.IEntity;
            if (!IsEntity || string.IsNullOrEmpty(this._activeEntity.KeyName))
                return -1;
            int nKeyValue = Util.ParseInt(this._activeEntity.KeyValue + "", -1);
            int nNum = this._activeEntity.ObjectFields.Length;
            if (nNum == 0)
                return -1;
            if (nKeyValue <= 0)
            {
                //insert
                int nMaxId = GetNextKey();
                StringBuilder sbContent = new StringBuilder();
                StringBuilder sbWhere = new StringBuilder();
                string strSingle = "";
                for (int i = 0; i < nNum; i++)
                {
                    string strFieldName = this._activeEntity.ObjectFields[i].Name;
                    if (strFieldName != this._activeEntity.KeyName)
                    {
                        object oValue = this._activeEntity.ObjectFields[i].GetValue(this._activeEntity.ActiveObject);
                        if (null == oValue) oValue = "null";
                        strSingle = alInt.Contains(this._activeEntity.ObjectFields[i].FieldType.Name.ToLower()) ? oValue + "" : string.Format("'{0}'", oValue);
                    }
                    else
                    {
                        strSingle = nMaxId + "";
                        this._activeEntity.ObjectFields[i].SetValue(this._activeEntity.ActiveObject, nMaxId);
                    }
                    if (0 != i)
                    {
                        sbContent.Append(",");
                        sbWhere.Append(",");
                    }
                    sbContent.Append(string.Format("[{0}]", strFieldName));
                    sbWhere.Append(strSingle);
                }
                HSqlFactory query = new HSqlFactory();
                query.SqlType = HSqlType.Insert;
                query.SqlTableName = this._activeEntity.TableName;
                query.SqlContent = sbContent.ToString();
                query.SqlWhereString = sbWhere.ToString();
                try
                {
                    HDBOperation.QueryNonQuery(query.ToString());
                }
                catch (Exception ee)
                {
                    return -1;
                }
                nKeyValue = nMaxId;
            }
            else
            {
                //old obj to update
                string strUpdateKey = string.Format("{0}={1}", this._activeEntity.KeyName, this._activeEntity.KeyValue);
                StringBuilder sb = new StringBuilder();
                string strSingle = "";
                for (int i = 0; i < nNum; i++)
                {
                    string strFieldName = this._activeEntity.ObjectFields[i].Name;
                    if (strFieldName == this._activeEntity.KeyName)
                        continue;
                    object oValue = this._activeEntity.ObjectFields[i].GetValue(this._activeEntity.ActiveObject);
                    if (null == oValue) oValue = "null";
                    strSingle = alInt.Contains(this._activeEntity.ObjectFields[i].FieldType.Name.ToLower()) ? string.Format("[{0}]={1}", strFieldName, oValue) : string.Format("[{0}]='{1}'", strFieldName, oValue);
                    if (sb.Length != 0)
                        sb.Append(",");
                    sb.Append(strSingle);
                }
                HSqlFactory query = new HSqlFactory();
                query.SqlType = HSqlType.Update;
                query.SqlTableName = this._activeEntity.TableName;
                query.SqlWhereString = strUpdateKey;
                query.SqlContent = sb.ToString();
                try
                {
                    HDBOperation.QueryNonQuery(query.ToString());
                }
                catch (Exception ee)
                {
                    return -1;
                }
            }
            //清除整表缓存
            CacheUtil.Clear(this._activeEntity.TableName);
            //清除该表相关条件缓存
            CacheUtil.ClearContainsKey(this._activeEntity.TableName + "-");

            //添加缓存(缓存的存储格式和EntityList的缓存格式相同，以防当出现"KeyName=KeyValue"类型的条件时两个方法可以存储相同的缓存)
            string strCacheKey = string.Format("{0}-[{1}]={2}", this._activeEntity.TableName, this._activeEntity.KeyName ,nKeyValue);
            ArrayList alCaches = new ArrayList();
            alCaches.Add(this._activeEntity.ActiveObject);
            CacheUtil.SetValue(strCacheKey, alCaches.ToArray(this._activeEntity.ObjectType));
            return nKeyValue;
        }

        public int EntityDelete()
        {
            bool IsEntity = this._activeEntity.ActiveObject is DataBase.Interface.IEntity;
            if (!IsEntity || string.IsNullOrEmpty(this._activeEntity.KeyName))
                return -1;
            int nKeyValue = Util.ParseInt(this._activeEntity.KeyValue + "", -1);
            if (nKeyValue <= 0)
                return -1;
            string strUpdateKey = string.Format("[{0}]={1}", this._activeEntity.KeyName, this._activeEntity.KeyValue);
            HSqlFactory query = new HSqlFactory();
            query.SqlType = HSqlType.Delete;
            query.SqlTableName = this._activeEntity.TableName;
            query.SqlWhereString = strUpdateKey;
            HDBOperation.QueryNonQuery(query.ToString());
            //清除整表缓存
            CacheUtil.Clear(this._activeEntity.TableName);
            //清除该表相关条件缓存
            CacheUtil.ClearContainsKey(this._activeEntity.TableName + "-");
            return nKeyValue;
        }

        public int GetNextKey()
        {
            HSqlFactory query = new HSqlFactory();
            query.SqlType = HSqlType.Select;
            query.SqlTableName = this._activeEntity.TableName;
            query.SqlContent = string.Format("Max([{0}])", this._activeEntity.KeyName);
            query.SqlWhereString = "";
            object oValue = HDBOperation.QueryScalar(query.ToString());
            return Util.ParseInt(oValue + "", 0) + 1;
        }

    }

    sealed class ActiveEntity
    {
        public object ActiveObject;
        public Type ObjectType;
        public FieldInfo[] ObjectFields;
        public string KeyName;
        public object KeyValue;
        public string TableName;
        public string SelectContent;

        private static Hashtable htTypeInformation = new Hashtable();
        private static string strCacheTypeKey = "BlueSky_EntityTypeList";

        static ActiveEntity()
        {
            if (CacheUtil.IsExistCache(strCacheTypeKey))
            {
                htTypeInformation = (Hashtable)CacheUtil.GetValue(strCacheTypeKey);
            }
            else
            {
                CacheUtil.SetValue(strCacheTypeKey, htTypeInformation, 3600);
            }
        }

        public ActiveEntity()
        { }

        public ActiveEntity(object _oE)
        {
            this.ActiveObject = _oE;
            this.ObjectType = _oE.GetType();

            if (htTypeInformation.ContainsKey(this.ObjectType.Name))
            {
                Hashtable htType = (Hashtable)htTypeInformation[this.ObjectType.Name];
                this.ObjectFields = (FieldInfo[])htType["ObjectFields"];
                this.KeyName = (string)htType["KeyName"];
                this.TableName = (string)htType["TableName"];
                this.SelectContent = (string)htType["SelectContent"];
            }
            else
            {
                FieldInfo[] alAllFields = this.ObjectType.GetFields();
                List<FieldInfo> ltFields = new List<FieldInfo>(alAllFields);
                List<string> ltSelect = new List<string>();
                foreach (FieldInfo field in alAllFields)
                {
                    if (field.IsStatic)
                        ltFields.Remove(field);
                    else
                        ltSelect.Add(string.Format("[{0}]", field.Name));
                }
                Hashtable htType = new Hashtable();
                this.ObjectFields = ltFields.ToArray();
                this.KeyName = this.ObjectType.GetMethod("GetKeyName").Invoke(_oE, null) + "";
                this.TableName = this.ObjectType.GetMethod("GetTableName").Invoke(_oE, null) + "";
                this.SelectContent = string.Join(",", ltSelect.ToArray());
                //add to cache
                htType["ObjectFields"] = this.ObjectFields;
                htType["KeyName"] =  this.KeyName;
                htType["TableName"] =  this.TableName;
                htType["SelectContent"] = this.SelectContent;
                htTypeInformation[this.ObjectType.Name] = htType;
            }
            FieldInfo key = this.ObjectType.GetField(this.KeyName);
            this.KeyValue = key.GetValue(this.ActiveObject);
        }
    }
}
