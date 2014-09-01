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
    public class EntityAccess<TEntity> where TEntity : IEntity, new()
    {
        public static IEntityMeta<TEntity> Meta { get; set; }
        static EntityAccess()
        {
            if (null == Meta)
            {
                Meta = new EntityMeta<TEntity>();
            }
        }
        public IDbSession DbSession;
        public EntityAccess()
        {
            DbSession = (IDbSession)Activator.CreateInstance(DbSessionFactory.Map(Meta.DbType));
            if (!string.IsNullOrEmpty(Meta.ConnectionName))
            {
                DbSession.Database.ConnectionName = Meta.ConnectionName;
            }
        }
        public static EntityAccess<TEntity> Access
        {
            get
            {
                return new EntityAccess<TEntity>();
            }
        }
        private TEntity[] ToEntitys(DataTable _dtEntities)
        {
            if (null == _dtEntities || _dtEntities.Rows.Count == 0)
                return null;
            List<TEntity> ltEntities = new List<TEntity>();
            foreach (DataRow row in _dtEntities.Rows)
            {
                ltEntities.Add(ToEntity(row));
            }
            return ltEntities.ToArray();
        }
        private TEntity ToEntity(DataRow _drEntity)
        {
            TEntity oEntity = new TEntity();
            foreach (EntityField ef in Meta.EntityFields)
            {
                ef.SetValue(oEntity, _drEntity[ef.FieldName] is DBNull ? null : _drEntity[ef.FieldName]);
            }
            return oEntity;
        }
        private string FormatSetValue(object _oV, IEntityField _oEF)
        {
            TypeCode oTC = Type.GetTypeCode(_oEF.Type);
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
        private string FormatSelectValue(object _oV, IEntityField _oEF)
        {
            TypeCode oTC = Type.GetTypeCode(_oEF.Type);
            if (oTC == TypeCode.Int32)
            {
                return _oV.Equals(0) ? null : string.Format("[{0}]={1}", _oEF.FieldName, _oV);
            }
            else if (oTC == TypeCode.String)
            {
                return _oV == null ? null : string.Format("[{0}]='{1}'", _oEF.FieldName, _oV);
            }
            else if (oTC == TypeCode.Double)
            {
                return _oV.Equals(0.0d) ? null : string.Format("[{0}]={1}", _oEF.FieldName, _oV);
            }
            else if (oTC == TypeCode.DateTime)
            {
                return _oV.Equals(DateTime.Parse("0001-01-01 12:00:00")) ? null : string.Format("[{0}]='{1}'", _oEF.FieldName, _oV);
            }
            else if (oTC == TypeCode.Boolean)
            {
                return _oV.Equals(false) ? null : string.Format("[{0}]=1", _oEF.FieldName);
            }
            else if(oTC == TypeCode.Decimal)
            {
                return _oV.Equals(0.0m) ? null : string.Format("[{0}]={1}", _oEF.FieldName, _oV);
            }
            else if (oTC == TypeCode.Byte || oTC == TypeCode.Int16 || oTC == TypeCode.Int64 || oTC == TypeCode.UInt32 || oTC == TypeCode.UInt16 || oTC == TypeCode.UInt64)
            {
                return _oV.Equals(0) ? null : string.Format("[{0}]={1}", _oEF.FieldName, _oV);
            }
            return null;
        }
        private string FormatSelectWhere(TEntity _Entity)
        {
            StringBuilder sbWhere = new StringBuilder();
            bool bFirst = true;
            foreach (EntityField oEF in Meta.EntityFields)
            {
                object oValue = oEF.FieldValue(_Entity);
                if (null != oValue)
                {
                    string strCause = this.FormatSelectValue(oValue, oEF);
                    if (!string.IsNullOrEmpty(strCause))
                    {
                        sbWhere.Append((bFirst ? "" : " and ") + strCause);
                        bFirst = false;
                    }
                }
            }
            return sbWhere.ToString();
        }
        public TEntity Instance()
        {
            return new TEntity();
        }
        public TEntity Get(object _oKey)
        {
            if (Meta.EnableCache && EntityCache<TEntity>.Exist(_oKey))
            {
                return EntityCache<TEntity>.Find(_oKey);
            }
            TEntity[] oEntities = List(string.Format("[{0}]={1}", Meta.KeyField.FieldName, _oKey));
            if (null != oEntities && oEntities.Length >= 2)
            {
                throw new Exception(string.Format("TableName:{0},Key:{1},Value:{2} Exist Mutil Records!", Meta.TableName, Meta.KeyField.FieldName, _oKey));
            }
            if (null != oEntities && oEntities.Length == 1)
            {
                if (Meta.EnableCache)
                {
                    EntityCache<TEntity>.Add(_oKey, oEntities[0]);
                }
                return oEntities[0];
            }
            return default(TEntity);
        }
        public TEntity Clone(TEntity _Entity)
        {
            TEntity oClone = this.Instance();
            foreach (EntityField oEF in Meta.EntityFields)
            {
                oEF.SetValue(oClone, oEF.FieldValue(_Entity));
            }
            return oClone;
        }
        public int Count()
        {
            return this.Count("");
        }
        public int Count(TEntity _Entity)
        {
            return this.Count(this.FormatSelectWhere(_Entity));
        }
        public int Count(string _strFilter)
        {
            if (Meta.EnableCache && EntityListCache<TEntity>.ExistCount(_strFilter))
            {
                return EntityListCache<TEntity>.FindCount(_strFilter);
            }
            string strQuery = string.Format("SELECT COUNT(1) FROM {0}", Meta.TableName);
            if (!string.IsNullOrEmpty(_strFilter))
                strQuery += " WHERE " + _strFilter;
            int nCount = this.DbSession.ExecuteScale<int>(strQuery);
            if (Meta.EnableCache)
            {
                EntityListCache<TEntity>.AddCount(_strFilter, nCount);
            }
            return nCount;
        }
        public TEntity[] List()
        {
            return List("");
        }
        public TEntity[] List(TEntity _Entity)
        {
            return List(this.FormatSelectWhere(_Entity));
        }
        public TEntity[] List(string _strFilter)
        {
            if (Meta.EnableCache && EntityListCache<TEntity>.Exist(_strFilter))
            {
                return EntityListCache<TEntity>.Find(_strFilter);
            }
            string strQuery = string.Format("SELECT {0} FROM {1}", Meta.Selects, Meta.TableName);
            if (!string.IsNullOrEmpty(_strFilter))
                strQuery += " WHERE " + _strFilter;
            DataSet ds = this.DbSession.Query(strQuery);
            TEntity[] oList = this.ToEntitys(ds.Tables[0]);
            if (Meta.EnableCache)
            {
                EntityListCache<TEntity>.Add(_strFilter, oList);
            }
            return oList;
        }
        public TEntity[] List(string _strFilter, string _strSort)
        {
            if (!string.IsNullOrEmpty(_strSort))
                _strFilter += string.Format(" ORDER BY {0}", _strSort.Trim());
            return List(_strFilter);
        }
        public TEntity[] List(string _strFilter, string _strSort, int _nPageIndex, int _nPageSize)
        {
            string strCacheFilter = string.Format("{0}-{1}-{2}", _strFilter, _nPageIndex, _nPageSize);
            if (Meta.EnableCache && EntityListCache<TEntity>.Exist(strCacheFilter))
            {
                return EntityListCache<TEntity>.Find(strCacheFilter);
            }
            if (!string.IsNullOrEmpty(_strFilter))
                _strFilter = string.Format("({0})", _strFilter);
            string strQuery = "";
            if (_nPageIndex == 1)
            {
                #region 如果查询第一页的数据采用top方式，提高效率
                strQuery = string.Format("SELECT TOP {0} {1} FROM {2}", _nPageSize, Meta.Selects, Meta.TableName);
                if (!string.IsNullOrEmpty(_strFilter))
                    strQuery += " WHERE " + _strFilter;
                if (!string.IsNullOrEmpty(_strSort))
                    strQuery += " ORDER BY " + _strSort;
                #endregion
            }
            else if (_nPageIndex * _nPageSize <= 1000000)
            {
                #region 分页方案(not in),在百万条数据内效率高
                strQuery = string.Format("SELECT TOP {0} {1} FROM {2}", _nPageSize, Meta.Selects, Meta.TableName);
                strQuery += string.Format(" WHERE {0} NOT IN (SELECT TOP (({1} - 1) * {2}) {0} FROM {3}{4} ORDER BY {0})", Meta.KeyField.FieldName, _nPageIndex, _nPageSize, Meta.TableName, string.IsNullOrEmpty(_strFilter) ? "" : (" WHERE " + _strFilter));
                if (!string.IsNullOrEmpty(_strFilter))
                    strQuery += string.Format(" AND {0}", _strFilter);
                if (!string.IsNullOrEmpty(_strSort))
                    strQuery += " ORDER BY " + _strSort;
                #endregion
            }
            else
            {
                #region 分页方案(ROW_NUMBER()),在百万条数据以上效率高
                strQuery = string.Format("SELECT {0} FROM (SELECT ROW_NUMBER() OVER(ORDER BY {1}) AS nRow,{0} FROM {2}", Meta.Selects, Meta.KeyField.FieldName, Meta.TableName);
                if (!string.IsNullOrEmpty(_strFilter))
                    strQuery += string.Format(" WHERE {0} ", _strFilter);
                strQuery += string.Format(") tTemp WHERE nRow >= (({0} - 1) * {1} + 1) and nRow <= ({0}*{1})", _nPageIndex, _nPageSize);
                if (!string.IsNullOrEmpty(_strSort))
                    strQuery += " ORDER BY " + _strSort;
                #endregion
            }

            DataSet ds = this.DbSession.Query(strQuery);
            TEntity[] oList = this.ToEntitys(ds.Tables[0]);
            if (Meta.EnableCache)
            {
                EntityListCache<TEntity>.Add(strCacheFilter, oList);
            }
            return oList;
        }
        public int Save(TEntity _Entity)
        {
            string strKeyName = Meta.KeyField.FieldName;
            int nKeyValue = TypeUtil.ParseInt(Meta.KeyValue(_Entity) + "", -1);
            if (nKeyValue <= 0)
            {
                nKeyValue = GetNextKey();
                List<string> ltInsertField = new List<string>();
                List<string> ltInsertValue = new List<string>();
                foreach (IEntityField oEF in Meta.EntityFields)
                {
                    if (oEF.FieldName != strKeyName)
                    {
                        ltInsertValue.Add(FormatSetValue(oEF.FieldValue(_Entity), oEF));
                    }
                    else
                    {
                        ltInsertValue.Add(nKeyValue.ToString());
                        oEF.SetValue(_Entity, nKeyValue);
                    }
                    ltInsertField.Add(string.Format("[{0}]", oEF.FieldName));
                }
                string strOperate = string.Format("SET IDENTITY_INSERT {0} ON;INSERT INTO {0}({1}) VALUES({2});SET IDENTITY_INSERT {0} OFF;", Meta.TableName, string.Join(",", ltInsertField.ToArray()), string.Join(",", ltInsertValue.ToArray()));
                try
                {
                    this.DbSession.Execute(strOperate);
                    if (Meta.EnableCache)
                    {
                        EntityListCache<TEntity>.ClearCount();
                    }
                }
                catch (Exception ee)
                {
                    return -1;
                }
            }
            else
            {
                List<string> ltUpdateValue = new List<string>();
                foreach (IEntityField oEF in Meta.EntityFields)
                {
                    if (oEF.FieldName == strKeyName)
                        continue;
                    ltUpdateValue.Add(string.Format("[{0}]={1}", oEF.FieldName, FormatSetValue(oEF.FieldValue(_Entity), oEF)));
                }
                string strOperate = string.Format("UPDATE {0} SET {1} WHERE [{2}]={3};", Meta.TableName, string.Join(",", ltUpdateValue.ToArray()), Meta.KeyField.FieldName, Meta.KeyField.FieldValue(_Entity));
                try
                {
                    this.DbSession.Execute(strOperate);
                }
                catch (Exception ee)
                {
                    return -1;
                }
            }
            if (Meta.EnableCache)
            {
                //清除表缓存
                EntityListCache<TEntity>.Clear();

                //添加实体缓存
                EntityCache<TEntity>.Add(nKeyValue, _Entity);
            }

            //添加缓存(缓存的存储格式和EntityList的缓存格式相同，以防当出现"KeyName=KeyValue"类型的条件时两个方法可以存储相同的缓存)
            //string strCacheKey = string.Format("[{0}]={1}", strKeyName ,nKeyValue);
            //ArrayList alCaches = new ArrayList();
            //alCaches.Add(this._activeEntity.ActiveObject);
            //CacheBase.SetValue(strCacheKey, alCaches.ToArray(this._activeEntity.ObjectType));
            return nKeyValue;
        }
        public int Delete(TEntity _Entity)
        {
            int nKeyValue = TypeUtil.ParseInt(Meta.KeyField.FieldValue(_Entity) + "", -1);
            if (nKeyValue <= 0)
                return -1;
            string strQuery = string.Format("DELETE FROM [{0}] WHERE [{1}]={2}", Meta.TableName, Meta.KeyField.FieldName, nKeyValue);
            this.DbSession.Execute(strQuery);
            if (Meta.EnableCache)
            {
                EntityListCache<TEntity>.Clear();
                EntityListCache<TEntity>.ClearCount();
                EntityCache<TEntity>.Remove(nKeyValue);
            }
            return nKeyValue;
        }
        public int GetNextKey()
        {
            string strQuery = string.Format("SELECT MAX([{0}]) FROM [{1}]", Meta.KeyField.FieldName, Meta.TableName);
            int nMax = this.DbSession.ExecuteScale<int>(strQuery);
            return ++nMax;
        }
    }
}
