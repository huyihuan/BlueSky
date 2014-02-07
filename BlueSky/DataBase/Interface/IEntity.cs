using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataBase.Interface
{
    public interface IEntity
    {
        string GetTableName();
        string GetKeyName();
    }

    //public interface IClass
    //{ 
    
    //}

    //public class Entity
    //{
    //    protected string GetName()
    //    {
    //        return "";
    //    }
    //}

}
