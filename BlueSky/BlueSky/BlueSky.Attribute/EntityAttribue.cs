using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlueSky.DataAccess;

namespace BlueSky.Attribute
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class EntityAttribue : System.Attribute
    {
        private string _TableName;
        private string _ConectionName;
        private DatabaseType _DbType;
        private bool _EnableCache = true;
        public string TableName
        {
            get
            {
                return this._TableName;
            }
            set
            {
                this._TableName = value;
            }
        }
        public string ConectionName
        {
            get
            {
                return this._ConectionName;
            }
            set
            {
                this._ConectionName = value;
            }
        }
        public DatabaseType DbType { get; set; }
        public bool EnableCache
        {
            get
            {
                return this._EnableCache;
            }
            set
            {
                this._EnableCache = value;
            }
        }

        public EntityAttribue()
        { }
        public EntityAttribue(string _TableName)
        {
            this.TableName = _TableName;
        }
        public EntityAttribue(string _TableName, string _ConectionName)
        {
            this.TableName = _TableName;
            this.ConectionName = _ConectionName;
        }
        public EntityAttribue(string _TableName, string _ConectionName, DatabaseType _DbType)
        {
            this.TableName = _TableName;
            this.ConectionName = _ConectionName;
            this.DbType = _DbType;
        }
        public EntityAttribue(string _TableName, string _ConectionName, DatabaseType _DbType, bool _EnableCache)
        {
            this.TableName = _TableName;
            this.ConectionName = _ConectionName;
            this.DbType = _DbType;
            this.EnableCache = _EnableCache;
        }

    }
}
