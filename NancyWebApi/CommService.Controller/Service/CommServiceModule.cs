using Nancy;

namespace CommService.Controller.Service
{
    public class CommServiceModule : NancyModule
    {
        string _jsonString = string.Empty;
        public CommServiceModule() : base("/comm")
        {
            //查询用户信息
            Get[@"/userinfo/{account}"] = x => DoGetUserInfo(x.account);
            //查询用户产品权限
            Get[@"/accrights/{account}"] = x => DoGetAccRights(x.account);
            //查询用户模块权限
            Get[@"/apprights/{account}"] = x => DoGetAppRights(x.account);
            //Get[@"/basedata/{someparameter}"] = x => DoGetBaseData();
        }

        private Response DoGetUserInfo(string account)
        {
            _jsonString = ResponseHandle.ResponseInfo.GetUserInfo(account);
            return Response.AsText(_jsonString, "text/html;charset=UTF-8");
        }

        private Response DoGetAccRights(string account)
        {
            _jsonString = ResponseHandle.ResponseInfo.GetAccRights(account);
            return Response.AsText(_jsonString, "text/html;charset=UTF-8");
        }

        private Response DoGetAppRights(string account)
        {
            _jsonString = ResponseHandle.ResponseInfo.GetAppRights(account);
            return Response.AsText(_jsonString, "text/html;charset=UTF-8");
        }

    }
}
