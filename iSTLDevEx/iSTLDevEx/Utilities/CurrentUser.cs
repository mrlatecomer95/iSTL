using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using iSTLDevEx.DAL;
namespace iSTLDevEx.Utilities
{
    //public static class UserLogin
    //{
    //    public static string CurrentUser
    //}
    public class CurrentUser
    {
        private MembershipUser _user;
        private SMSSysEntities _db;
        public CurrentUser(SMSSysEntities db, MembershipUser user)
        {
            _db = new SMSSysEntities();
            _user = user;
        }

        public string OperatorID
        {
            get
            {
                var strUserName = _user.UserName;
                var userProfile = _db.UserProfiles.SingleOrDefault(x => x.UserName == strUserName);
                return userProfile.OperatorID;
            }
        }


        public string OperatorName
        {
            get
            {
                var strUserName = _user.UserName;
                var userProfileInDB = _db.UserProfiles.SingleOrDefault(x => x.UserName == strUserName);
                var OperatorInDb = _db.M_Operator.SingleOrDefault(x => x.OperatorID == userProfileInDB.OperatorID);
                return OperatorInDb.OperatorName;
            }
        }
    }
}