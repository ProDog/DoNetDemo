using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Net;
using System.Web.Security;

namespace HuobiAPI
{
    /// <summary>
    ///HuobiBase 的摘要说明,接口信息
    /// </summary>
    public class HuobiBase
    {
        //火币现货配置信息
        //火币网账户中获取到的 key
        public static String HUOBI_ACCESS_KEY = "XXXXXX";
        public static String HUOBI_SECRET_KEY = "XXXXXX";
        public static String HUOBI_API_URL = "https://api.huobi.com/apiv3";
        protected static int success = 200;

        //火币网请求信息
        public static String BUY = "buy";
        public static String BUY_MARKET = "buy_market";
        public static String CANCEL_ORDER = "cancel_order";
        public static String ACCOUNT_INFO = "get_account_info";
        public static String NEW_DEAL_ORDERS = "get_new_deal_orders";
        public static String ORDER_ID_BY_TRADE_ID = "get_order_id_by_trade_id";
        public static String GET_ORDERS = "get_orders";
        public static String ORDER_INFO = "order_info";
        public static String SELL = "sell";
        public static String SELL_MARKET = "sell_market";

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="PostVars"></param>
        /// <returns></returns>
        public static string Sign(NameValueCollection PostVars)
        {
            Dictionary<string, string> dicMap = new Dictionary<string, string>();
            foreach (var m in PostVars.AllKeys)
            {
                dicMap.Add(m, PostVars[m]);
            }
            var dicMapOrder = dicMap.OrderBy(d => d.Key);

            StringBuilder inputStr = new StringBuilder();
            foreach (var d in dicMapOrder)
            {
                inputStr.Append(d.Key).Append("=").Append(d.Value).Append("&");
            }
            string str = inputStr.ToString();
            string md5Str = FormsAuthentication.HashPasswordForStoringInConfigFile(str.Substring(0, str.Length - 1), "MD5").ToLower();
            return md5Str;
        }

        /// <summary>
        /// 请求
        /// </summary>
        /// <param name="PostVars"></param>
        /// <returns></returns>
        public static string Post(NameValueCollection PostVars)
        {
            WebClient WebClientObj = new WebClient();
            WebClientObj.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            byte[] byRemoteInfo = WebClientObj.UploadValues(HuobiBase.HUOBI_API_URL, "POST", PostVars);
            string sRemoteInfo = System.Text.Encoding.UTF8.GetString(byRemoteInfo);
            return sRemoteInfo;
        }

        /// <summary>
        /// 获得10位时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimestamp()
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (DateTime.Now.Ticks - startTime.Ticks) / 10000000;//除10000000调整为10位      
            return t.ToString();
        }
    }
}