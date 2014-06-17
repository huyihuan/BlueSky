using System;

namespace BlueSky.Interfaces
{
    public interface IEntity
    {
        string GetTableName();
        string GetKeyName();
    }
}
