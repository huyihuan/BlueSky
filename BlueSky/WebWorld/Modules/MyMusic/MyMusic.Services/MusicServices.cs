﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBase.Utilities;
using WebWorld.Modules.MyMusic.DataAccess;
using WebWorld.Modules.MyMusic.Domain;

namespace WebWorld.Modules.MyMusic.Services
{
    public class MusicServices
    {
        public static string MusicRootPath;
        static MusicServices()
        {
            MusicRootPath = SystemUtil.ResovleModuleUploadPath("MyMusic");
            string strPhysicPath = SystemUtil.Server.MapPath(MusicRootPath);
            if (!System.IO.Directory.Exists(strPhysicPath))
                System.IO.Directory.CreateDirectory(strPhysicPath);
        }

        public static string GetUserMusicPath(int _nUserId)
        {
            string strUserMusicPath = string.Format("{0}\\{1}\\", MusicRootPath, _nUserId);
            string strPhysicPath = SystemUtil.Server.MapPath(strUserMusicPath);
            if (!System.IO.Directory.Exists(strPhysicPath))
                System.IO.Directory.CreateDirectory(strPhysicPath);
            return strUserMusicPath;
        }

        public static void Save(Music _oMusic)
        {
            MusicDataAccess.Save(_oMusic);
        }

        public static void Delete(Music _oMusic)
        {
            MusicDataAccess.Delete(_oMusic);
        }

        public static void Delete(int _nId)
        {
            Music oDelete = MusicServices.Get(_nId);
            if(null != oDelete)
                MusicDataAccess.Delete(oDelete);
        }

        public static Music Get(int _nId)
        {
            return MusicDataAccess.Get(_nId);
        }

        public static Music[] List()
        {
            return MusicDataAccess.List();
        }

        public static Music[] List(string _strFilter, string _strSort, int _nPageIndex, int _nPageSize)
        {
            return MusicDataAccess.List(_strFilter, "", _nPageIndex, _nPageSize);
        }

        public static int Count(string _strFilter)
        {
            return MusicDataAccess.Count(_strFilter);        
        }
    }
}
