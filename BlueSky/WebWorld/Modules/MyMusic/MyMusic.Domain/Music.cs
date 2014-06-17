using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebWorld.Modules.MyMusic.Domain
{
    public class Music : BlueSky.Interfaces.IEntity
    {
        public int Id;
        public int UserId;
        public string MusicName;
        public string MusicURL;
        public string MusicType;

        #region IEntity

        public string GetTableName() { return "Music"; }
        public string GetKeyName() { return "Id"; }

        #endregion
    }
}
