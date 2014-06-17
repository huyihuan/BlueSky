using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Data;
using System.Collections;
using BlueSky.Utilities;
using BlueSky.Cache;
using BlueSky.DataAccess;
using BlueSky.Interfaces;

namespace BlueSky.EntityAccess
{
    public class HEntityCommon
    {
        private ActiveEntity _activeEntity;
        public HEntityCommon()
        { }
        public HEntityCommon(object _oE)
        {
            if (!(_oE is IEntity))
                throw new Exception(_oE.GetType().FullName + "is not Entity!");
            this._activeEntity = new ActiveEntity(_oE);
        }
        public static HEntityCommon HEntity(object _oE)
        {
            if (null == _oE)
                return null;
            HEntityCommon oEntity = new HEntityCommon(_oE);
            return oEntity;
        }

        private static object[] ChangeTableToEntitys(HEntityCommon _oE, DataTable _dt)
        {
            if (null == _dt || _dt.Rows.Count == 0)
                return null;
            int nRowNum = _dt.Rows.Count;
            ArrayList alItems = new ArrayList();
            for (int i = 0; i < nRowNum; i++)
            {
                DataRow rowItem = _dt.Rows[i];
                object oInstance = Activator.CreateInstance(_oE._activeEntity.ObjectType);
                foreach (FieldInfo oF in _oE._activeEntity.ObjectFields)
                {
                    if (rowItem[oF.Name] is DBNull)
                        oF.SetValue(oInstance, null);
                    else
                        oF.SetValue(oInstance, rowItem[oF.Name]);
                }
                alItems.Add(oInstance);
            }
            return (object[])alItems.ToArray(_oE._activeEntity.ObjectType);
        }

        private string FormatSetValue(object _oV, FieldInfo _oF)
        {
            TypeCode oTC = Type.GetTypeCode(_oF.FieldType);
            if (oTC == TypeCode.Int32)
            {
                return _oV.ToString();
            }
            else if (oTC == TypeCode.String)
            {
                if (null == _oV)
                {
                    return "null";
                }
                return string.Format("'{0}'", _oV);
            }
            else if (oTC == TypeCode.Double)
            {
                if (_oV.Equals(0.0d))
                {
                    return "0";
                }
                return _oV.ToString();
            }
            else if (oTC == TypeCode.DateTime)
            {
                if (_oV.Equals(DateTime.Parse("0001-01-01 12:00:00")))
                {
                    return "null";
                }
                else if (((DateTime)_oV).CompareTo(DateTime.Parse("1900-01-01 00:00:00")) < 0)
                {
                    return "null";
                }
                return string.Format("'{0}'", ((DateTime)_oV).ToString(Constants.DateTimeFormatStandard));
            }
            else if (oTC == TypeCode.Boolean)
            {
                return _oV.Equals(false) ? "0" : "1";
            }
            else if (oTC == TypeCode.Decimal)
            {
                if (_oV.Equals(0.0m))
                {
                    return "0";
                }
                return _oV.ToString();
            }
            return _oV.ToString();
        }

        private string FormatSelectValue(object _oV, FieldInfo _oF)
        {
            TypeCode oTC = Type.GetTypeCode(_oF.FieldType);
            if (oTC == TypeCode.Int32)
            {
                return _oV.Equals(0) ? null : string.Format("[{0}]={1}", _oF.Name, _oV);
            }
            else if (oTC == TypeCode.String)
            {
                return _oV == null ? null : string.Format("[{0}]='{1}'", _oF.Name, _oV);
            }
            else if (oTC == TypeCode.Double)
            {
                return _oV.Equals(0.0d) ? null : string.Format("[{0}]={1}", _oF.Name, _oV);
            }
            else if (oTC == TypeCode.DateTime)
            {
                return _oV.Equals(DateTime.Parse("0001-01-01 12:00:00")) ? null : string.Format("[{0}]='{1}'", _oF.Name, _oV);
            }
            else if (oTC == TypeCode.Boolean)
            {
                return _oV.Equals(false) ? null : string.Format("[{0}]=1", _oF.Name);
            }
            else if(oTC == TypeCode.Decimal)
            {
                return _oV.Equals(0.0m) ? null : string.Format("[{0}]={1}", _oF.Name, _oV);
            }
            else if (oTC == TypeCode.Byte || oTC == TypeCode.Int16 || oTC == TypeCode.Int64 || oTC == TypeCode.UInt32 || oTC == TypeCode.UInt16 || oTC == TypeCode.UInt64)
            {
                return _oV.Equals(0) ? null : string.Format("[{0}]={1}", _oF.Name, _oV);
            }
            return null;
        }

        private string FormatSelectWhere(HEntityCommon _oE)
        {
            int nLength = _oE._activeEntity.ObjectFields.Length;
            StringBuilder sb = new StringBuilder("1=1");
            for (int i = 0; i < nLength; i++)
            {
                object oValue = _oE._activeEntity.ObjectFields[i].GetValue(_oE._activeEntity.ActiveObject);
                if (null != oValue)
                {
                    string strWhere = FormatSelectValue(oValue, _oE._activeEntity.ObjectFields[i]);
                    if (!string.IsNullOrEmpty(strWhere))
                        sb.Append(string.Format(" and {0}", strWhere));
                }
            }
            return sb.ToString();
        }

        public object Instance()
        {
            object oE = Activator.CreateInstance(this._activeEntity.ObjectType);
            return oE;
        }

        public object EntityGet(object _oKey)
        {
            string strTableName = this._activeEntity.TableName;
            if (EntityCache.IsExistEntity(strTableName, _oKey))
                return EntityCache.GetEntity(strTableName, _oKey);
            object[] entities = EntityList(string.Format("{0}={1}", this._activeEntity.KeyName, _oKey));
            if (null != entities && entities.Length >= 0)
            {
                EntityCache.SetEntity(strTableName, _oKey, entities[0]);
                return entities[0];
            }
            return null;
        }

        public object EntityClone()
        {
            object oClone = this.Instance();
            object oSource = this._activeEntity.ActiveObject;
            foreach (FieldInfo oF in this._activeEntity.ObjectFields)
            {
                object oValue = oF.GetValue(oSource);
                oF.SetValue(oClone, oValue);
            }
            return oClone;
        }

        public int EntityCount()
        {
            string strWhere = FormatSelectWhere(this);
            return this.EntityCount(strWhere);
        }

        public int EntityCount(string _strFilter)
        {
            string strEntityName = this._activeEntity.TableName;
            if (ListCache.IsExistCount(strEntityName, _strFilter))
            {
                return ListCache.GetCount(strEntityName, _strFilter);
            }
            HSqlFactory query = new HSqlFactory();
            query.SqlType = HSqlType.Select;
            query.SqlTableName = strEntityName;
            query.SqlContent = "COUNT(1)";
            query.SqlWhereString = _strFilter;
            int nCount = (int)HDBOperation.QueryScalar(query.ToString());
            ListCache.SetCount(strEntityName, _strFilter, nCount);
            return nCount;
        }

        public object[] EntityList()
        {
            string strWhere = FormatSelectWhere(this);
            return EntityList(strWhere);
        }

        public object[] EntityList(string _strFilter)
        {
            string strEntityName = this._activeEntity.TableName;
            if (ListCache.IsExistList(strEntityName, _strFilter))
            {
                return (object[])ListCache.GetList(strEntityName, _strFilter);
            }
            HSqlFactory query = new HSqlFactory();
            query.SqlType = HSqlType.Select;
            query.SqlTableName = strEntityName;
            query.SqlContent = this._activeEntity.SelectContent;
            query.SqlWhereString = _strFilter;
            DataSet ds = HDBOperation.QueryDataSet(query.ToString());
            object[] oList = ChangeTableToEntitys(this, ds.Tables[0]);
            ListCache.SetList(strEntityName, _strFilter, oList);
            return oList;
        }

        public object[] EntityList(string _strFilter, string _strSort)
        {
            if (!string.IsNullOrEmpty(_strSort))
                _strFilter += string.Format(" order by {0}", _strSort.Trim());
            return EntityList(_strFilter);
        }

        public object[] EntityList(string _strFilter, string _strSort, int _nPageIndex, int _nPageSize)
        {
            string strCacheFilter = string.Format("{0}-{1}-{2}", _strFilter, _nPageIndex, _nPageSize);
            string strEntityName = this._activeEntity.TableName;
            if (ListCache.IsExistList(strEntityName, strCacheFilter))
            {
                return (object[])ListCache.GetList(strEntityName, strCacheFilter);
            }
            HSqlFactory query = new HSqlFactory();
            query.SqlType = HSqlType.Select;
            query.SqlTableName = strEntityName;

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
                query.SqlWhereString += string.Format("{0} not in (select top (({1} - 1) * {2}) {0} from {3}{4} order by {0})", this._activeEntity.KeyName, _nPageIndex, _nPageSize, strEntityName, string.IsNullOrEmpty(_strFilter) ? "" : (" where " + _strFilter));
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
            object[] oList = ChangeTableToEntitys(this, ds.Tables[0]);
            ListCache.SetList(strEntityName, strCacheFilter, oList);
            return oList;
        }

        public int EntitySave()
        {
            bool IsEntity = this._activeEntity.ActiveObject is IEntity;
            string strKeyName = this._activeEntity.KeyName;
            if (!IsEntity || string.IsNullOrEmpty(strKeyName))
                return -1;
            int nKeyValue = TypeUtil.ParseInt(this._activeEntity.KeyValue + "", -1);
            int nCount = this._activeEntity.ObjectFields.Length;
            if (nCount == 0)
                return -1;
            if (nKeyValue <= 0)
            {
                nKeyValue = GetNextKey();
                List<string> ltInsertField = new List<string>();
                List<string> ltInsertValue = new List<string>();
                for (int i = 0; i < nCount; i++)
                {
                    FieldInfo oF = this._activeEntity.ObjectFields[i];
                    string strFieldName = oF.Name;
                    if (strFieldName != strKeyName)
                    {
                        object oValue = oF.GetValue(this._activeEntity.ActiveObject);
                        ltInsertValue.Add(FormatSetValue(oValue, oF));
                    }
                    else
                    {
                        ltInsertValue.Add(nKeyValue.ToString());
                        oF.SetValue(this._activeEntity.ActiveObject, nKeyValue);
                    }
                    ltInsertField.Add(string.Format("[{0}]", strFieldName));
                }
                HSqlFactory query = new HSqlFactory();
                query.SqlType = HSqlType.Insert;
                query.SqlTableName = this._activeEntity.TableName;
                query.SqlContent = string.Join(",", ltInsertField.ToArray());
                query.SqlWhereString = string.Join(",", ltInsertValue.ToArray());
                try
                {
                    HDBOperation.QueryNonQuery(query.ToString());
                }
                catch (Exception ee)
                {
                    return -1;
                }
            }
            else
            {
                List<string> ltUpdateValue = new List<string>();
                for (int i = 0; i < nCount; i++)
                {
                    FieldInfo oF = this._activeEntity.ObjectFields[i];
                    string strName = oF.Name;
                    if (strName == strKeyName)
                        continue;
                    object oValue = oF.GetValue(this._activeEntity.ActiveObject);
                    ltUpdateValue.Add(string.Format("[{0}]={1}", strName, FormatSetValue(oValue, oF)));
                }
                HSqlFactory query = new HSqlFactory();
                query.SqlType = HSqlType.Update;
                query.SqlTableName = this._activeEntity.TableName;
                query.SqlWhereString = string.Format("{0}={1}", strKeyName, nKeyValue);
                query.SqlContent = string.Join(",", ltUpdateValue.ToArray());
                try
                {
                    HDBOperation.QueryNonQuery(query.ToString());
                }
                catch (Exception ee)
                {
                    return -1;
                }
            }
            //清除表缓存
            ListCache.Clear(this._activeEntity.TableName);

            //添加实体缓存
            EntityCache.SetEntity(this._activeEntity.TableName, nKeyValue, this._activeEntity.ActiveObject);

            //添加缓存(缓存的存储格式和EntityList的缓存格式相同，以防当出现"KeyName=KeyValue"类型的条件时两个方法可以存储相同的缓存)
            //string strCacheKey = string.Format("[{0}]={1}", strKeyName ,nKeyValue);
            //ArrayList alCaches = new ArrayList();
            //alCaches.Add(this._activeEntity.ActiveObject);
            //CacheBase.SetValue(strCacheKey, alCaches.ToArray(this._activeEntity.ObjectType));
            return nKeyValue;
        }

        public int EntityDelete()
        {
            bool IsEntity = this._activeEntity.ActiveObject is IEntity;
            if (!IsEntity || string.IsNullOrEmpty(this._activeEntity.KeyName))
                return -1;
            int nKeyValue = TypeUtil.ParseInt(this._activeEntity.KeyValue + "", -1);
            if (nKeyValue <= 0)
                return -1;
            string strEntityName = this._activeEntity.TableName;
            string strUpdateKey = string.Format("[{0}]={1}", this._activeEntity.KeyName, this._activeEntity.KeyValue);
            HSqlFactory query = new HSqlFactory();
            query.SqlType = HSqlType.Delete;
            query.SqlTableName = strEntityName;
            query.SqlWhereString = strUpdateKey;
            HDBOperation.QueryNonQuery(query.ToString());
            //清除表缓存
            ListCache.Clear(strEntityName);
            //清除实体缓存
            EntityCache.Clear(strEntityName, nKeyValue);
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
            return TypeUtil.ParseInt(oValue + "", 0) + 1;
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
            if (SchemaCache.IsExistSchema(strCacheTypeKey))
            {
                htTypeInformation = (Hashtable)SchemaCache.GetSchema(strCacheTypeKey);
            }
            else
            {
                TimeSpan ts = new TimeSpan(24, 0, 0);
                SchemaCache.SetSchema(strCacheTypeKey, htTypeInformation, ts.Ticks);
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
