using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BlueSky.Interfaces;
using System.Data.Common;

namespace BlueSky.DataAccess
{
    public abstract class DbBase : IDatabase
    {
        private string _ConnectionString;
        private string _ConnectionName;
        private DatabaseType _DbType;
        private DbProviderFactory _DbFactory;
        protected abstract string DefaultConncetionName { get; }
        public virtual string ConnectionString
        {
            get
            {
                return this._ConnectionString;
            }
            set
            {
                this._ConnectionString = value;
            }
        }
        public virtual string ConnectionName
        {
            get
            {
                return this._ConnectionName;
            }
            set
            {
                this._ConnectionName = value;
            }
        }
        public virtual DatabaseType DbType
        {
            get
            {
                return this._DbType;
            }
            set
            {
                this._DbType = value;
            }
        }
        public virtual DbProviderFactory DbFactory
        {
            get
            {
                return this._DbFactory;
            }
            set
            {
                this._DbFactory = value;
            }
        }
    }
}
