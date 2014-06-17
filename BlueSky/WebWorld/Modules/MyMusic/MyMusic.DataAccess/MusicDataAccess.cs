using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebWorld.Modules.MyMusic.Domain;
using BlueSky.EntityAccess;

namespace WebWorld.Modules.MyMusic.DataAccess
{
    public class MusicDataAccess
    {
        public static void Save(Music _oMusic)
        {
            HEntityCommon.HEntity(_oMusic).EntitySave();
        }

        public static void Delete(Music _oMusic)
        {
            HEntityCommon.HEntity(_oMusic).EntityDelete();
        }

        public static Music Get(int _nId)
        {
            if (_nId <= 0)
                return null;
            Music oGet = new Music();
            Music[] alist = (Music[])HEntityCommon.HEntity(oGet).EntityList("Id=" + _nId);
            if (null == alist || alist.Length == 0)
                return null;
            if (alist.Length > 1)
                throw new Exception(string.Format("{0}-{1}:{2} exist mutil records", oGet.GetTableName(), oGet.GetKeyName(), _nId));
            return alist[0];
        }

        public static Music[] List()
        {
            Music[] alist = (Music[])HEntityCommon.HEntity(new Music()).EntityList();
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static Music[] List(string _strFilter, string _strSort, int _nPageIndex, int _nPageSize)
        {
            Music oList = new Music();
            Music[] alist = (Music[])HEntityCommon.HEntity(oList).EntityList(_strFilter, _strSort, _nPageIndex, _nPageSize);
            if (null == alist || alist.Length == 0)
                return null;
            return alist;
        }

        public static int Count(string _strFilter)
        {
            return HEntityCommon.HEntity(new Music()).EntityCount(_strFilter);
        }
    }
}
