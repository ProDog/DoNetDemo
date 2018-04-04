using CommService.Db;

namespace CommService.Controller.ResponseHandle
{
    public class ResponseInfo
    {
        UserRightsProvider _accountInfo = new UserRightsProvider();
        public static string GetUserInfo(string account)
        {
            string userInfo = UserRightsProvider.GetUserInfo(account);
            return userInfo;
        }

        public static string GetAccRights(string account)
        {
            string accRights = UserRightsProvider.GetAccRights(account);
            return accRights;
        }

        public static string GetAppRights(string account)
        {
            string appRights = UserRightsProvider.GetAppRights(account);
            return appRights;
        }
    }
}
