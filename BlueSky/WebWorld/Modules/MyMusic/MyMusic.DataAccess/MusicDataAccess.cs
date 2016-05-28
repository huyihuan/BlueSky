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
            EntityAccess<Music>.Access.Save(_oMusic);
        }

        public static void Delete(Music _oMusic)
        {
            EntityAccess<Music>.Access.Delete(_oMusic);
        }
        public static Music Get(int _nId)
        {
            if (_nId <= 0)
                return null;
            return EntityAccess<Music>.Access.Get(_nId);
        }
        public static Music[] List()
        {
            return EntityAccess<Music>.Access.List();
        }
        public static Music[] List(string _strFilter, string _strSort, int _nPageIndex, int _nPageSize)
        {
            return EntityAccess<Music>.Access.List(_strFilter, _strSort, _nPageIndex, _nPageSize);
        }
        public static int Count(string _strFilter)
        {
            return EntityAccess<Music>.Access.Count(_strFilter);
        }
    }
}
