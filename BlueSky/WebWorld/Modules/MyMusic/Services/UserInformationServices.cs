using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace WebWorld.Modules.MyMusic.Services
{
    public class UserInformationServices
    {
        public static UserInformation GetObjectByKey(int _nUserId)
        {
            using (BlueSkyEntities oEntities = new BlueSkyEntities())
            {
                EntityKey oKey = new EntityKey("BlueSkyEntities.UserInformation", "Id", _nUserId);
                return oEntities.GetObjectByKey(oKey) as UserInformation;
            }
        }

        public static void Save(UserInformation _oEntity)
        {
            using (BlueSkyEntities oEntities = new BlueSkyEntities())
            {
                oEntities.AddToUserInformation(_oEntity);
                oEntities.SaveChanges();
            }
        }
    }
}
