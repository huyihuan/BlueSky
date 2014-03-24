using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebSystemBase.Utilities;

namespace WebWorld.Modules.MyMusic.Services
{
    public class MusicServices
    {
        public static string MusicRootPath;
        static MusicServices()
        {
            MusicRootPath = SystemUtil.ResovleModuleUploadPath("MyMusic");
            if (!System.IO.Directory.Exists(MusicRootPath))
                System.IO.Directory.CreateDirectory(MusicRootPath);
        }

        public static string GetUserMusicPath(int _nUserId)
        {
            string strUserMusicPath = string.Format("{0}\\{1}\\", MusicRootPath, _nUserId);
            if (!System.IO.Directory.Exists(strUserMusicPath))
                System.IO.Directory.CreateDirectory(strUserMusicPath);
            return strUserMusicPath;
        }

        public static void Save(Music _oEntity)
        {
            using (BlueSkyEntities oEntities = new BlueSkyEntities())
            {
                oEntities.AddToMusic(_oEntity);
                oEntities.SaveChanges();
            }
        }
    }
}
