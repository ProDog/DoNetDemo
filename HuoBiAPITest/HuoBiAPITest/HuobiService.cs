using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;

namespace HuobiAPI
{
    /// <summary>
    ///HuobiService 的摘要说明
    /// </summary>
    public class HuobiService
    {
        public HuobiService()
        {
        }

        /// <summary>
        /// 下单接口
        /// </summary>
        /// <param name="coinType"></param>
        /// <param name="price"></param>
        /// <param name="amount"></param>
        /// <param name="tradePassword"></param>
        /// <param name="tradeid"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public string buy(string coinType, string price, string amount, string tradePassword, string tradeid)
        {
            NameValueCollection PostVars = new NameValueCollection();
            PostVars.Add("method", HuobiBase.BUY);
            PostVars.Add("created", HuobiBase.GetTimestamp());
            PostVars.Add("access_key", HuobiBase.HUOBI_ACCESS_KEY);
            PostVars.Add("secret_key", HuobiBase.HUOBI_SECRET_KEY);
            PostVars.Add("coin_type", coinType);
            PostVars.Add("price", price);
            PostVars.Add("amount", amount);
            string md5 = HuobiBase.Sign(PostVars);
            PostVars.Remove("secret_key");
            PostVars.Add("sign", md5);
            PostVars.Add("trade_password", tradePassword);
            PostVars.Add("trade_id", tradeid);

            return HuobiBase.Post(PostVars);
        }

        /// <summary>
        /// 提交市价单接口
        /// </summary>
        /// <param name="coinType"></param>
        /// <param name="amount"></param>
        /// <param name="tradePassword"></param>
        /// <param name="tradeid"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public string buyMarket(string coinType, string amount, string tradePassword, string tradeid)
        {
            NameValueCollection PostVars = new NameValueCollection();
            PostVars.Add("method", HuobiBase.BUY_MARKET);
            PostVars.Add("created", HuobiBase.GetTimestamp());
            PostVars.Add("access_key", HuobiBase.HUOBI_ACCESS_KEY);
            PostVars.Add("secret_key", HuobiBase.HUOBI_SECRET_KEY);
            PostVars.Add("coin_type", coinType);
            PostVars.Add("amount", amount);
            string md5 = HuobiBase.Sign(PostVars);
            PostVars.Remove("secret_key");
            PostVars.Add("sign", md5);
            PostVars.Add("trade_password", tradePassword);
            PostVars.Add("trade_id", tradeid);
            return HuobiBase.Post(PostVars);
        }

        /// <summary>
        /// 撤销订单
        /// </summary>
        /// <param name="coinType"></param>
        /// <param name="id"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public string cancelOrder(string coinType, string id)
        {
            NameValueCollection PostVars = new NameValueCollection();
            PostVars.Add("method", HuobiBase.CANCEL_ORDER);
            PostVars.Add("created", HuobiBase.GetTimestamp());
            PostVars.Add("access_key", HuobiBase.HUOBI_ACCESS_KEY);
            PostVars.Add("secret_key", HuobiBase.HUOBI_SECRET_KEY);
            PostVars.Add("coin_type", coinType);
            PostVars.Add("id", id);
            string md5 = HuobiBase.Sign(PostVars);
            PostVars.Remove("secret_key");
            PostVars.Add("sign", md5);
            return HuobiBase.Post(PostVars);
        }

        /// <summary>
        /// 获取账号详情
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public string getAccountInfo()
        {
            NameValueCollection PostVars = new NameValueCollection();
            PostVars.Add("method", HuobiBase.ACCOUNT_INFO);
            PostVars.Add("created", HuobiBase.GetTimestamp());
            PostVars.Add("access_key", HuobiBase.HUOBI_ACCESS_KEY);
            PostVars.Add("secret_key", HuobiBase.HUOBI_SECRET_KEY);
            string md5 = HuobiBase.Sign(PostVars);
            PostVars.Remove("secret_key");
            PostVars.Add("sign", md5);
            return HuobiBase.Post(PostVars);
        }

        /// <summary>
        /// 查询个人最新10条成交订单
        /// </summary>
        /// <param name="coinType"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public string getNewDealOrders(string coinType)
        {
            NameValueCollection PostVars = new NameValueCollection();
            PostVars.Add("method", HuobiBase.NEW_DEAL_ORDERS);
            PostVars.Add("created", HuobiBase.GetTimestamp());
            PostVars.Add("access_key", HuobiBase.HUOBI_ACCESS_KEY);
            PostVars.Add("secret_key", HuobiBase.HUOBI_SECRET_KEY);
            PostVars.Add("coin_type", coinType);
            string md5 = HuobiBase.Sign(PostVars);
            PostVars.Remove("secret_key");
            PostVars.Add("sign", md5);
            return HuobiBase.Post(PostVars);
        }

        /// <summary>
        /// 根据trade_id查询oder_id
        /// </summary>
        /// <param name="coinType"></param>
        /// <param name="tradeid"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public string getOrderIdByTradeId(string coinType, string tradeid)
        {
            NameValueCollection PostVars = new NameValueCollection();
            PostVars.Add("method", HuobiBase.ORDER_ID_BY_TRADE_ID);
            PostVars.Add("created", HuobiBase.GetTimestamp());
            PostVars.Add("access_key", HuobiBase.HUOBI_ACCESS_KEY);
            PostVars.Add("secret_key", HuobiBase.HUOBI_SECRET_KEY);
            PostVars.Add("coin_type", coinType);
            PostVars.Add("trade_id", tradeid);
            string md5 = HuobiBase.Sign(PostVars);
            PostVars.Remove("secret_key");
            PostVars.Add("sign", md5);
            return HuobiBase.Post(PostVars);
        }

        /// <summary>
        /// 根据trade_id查询oder_id
        /// </summary>
        /// <param name="coinType"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public string getOrders(string coinType)
        {
            NameValueCollection PostVars = new NameValueCollection();
            PostVars.Add("method", HuobiBase.GET_ORDERS);
            PostVars.Add("created", HuobiBase.GetTimestamp());
            PostVars.Add("access_key", HuobiBase.HUOBI_ACCESS_KEY);
            PostVars.Add("secret_key", HuobiBase.HUOBI_SECRET_KEY);
            PostVars.Add("coin_type", coinType);
            string md5 = HuobiBase.Sign(PostVars);
            PostVars.Remove("secret_key");
            PostVars.Add("sign", md5);
            return HuobiBase.Post(PostVars);
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="coinType"></param>
        /// <param name="id"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public string getOrderInfo(string coinType, string id)
        {
            NameValueCollection PostVars = new NameValueCollection();
            PostVars.Add("method", HuobiBase.ORDER_INFO);
            PostVars.Add("created", HuobiBase.GetTimestamp());
            PostVars.Add("access_key", HuobiBase.HUOBI_ACCESS_KEY);
            PostVars.Add("secret_key", HuobiBase.HUOBI_SECRET_KEY);
            PostVars.Add("coin_type", coinType);
            PostVars.Add("id", id);
            string md5 = HuobiBase.Sign(PostVars);
            PostVars.Remove("secret_key");
            PostVars.Add("sign", md5);
            return HuobiBase.Post(PostVars);
        }

        /// <summary>
        /// 限价卖出
        /// </summary>
        /// <param name="coinType"></param>
        /// <param name="price"></param>
        /// <param name="amount"></param>
        /// <param name="tradePassword"></param>
        /// <param name="tradeid"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public string sell(string coinType, string price, string amount, string tradePassword, string tradeid)
        {
            NameValueCollection PostVars = new NameValueCollection();
            PostVars.Add("method", HuobiBase.SELL);
            PostVars.Add("created", HuobiBase.GetTimestamp());
            PostVars.Add("access_key", HuobiBase.HUOBI_ACCESS_KEY);
            PostVars.Add("secret_key", HuobiBase.HUOBI_SECRET_KEY);
            PostVars.Add("coin_type", coinType);
            PostVars.Add("price", price);
            PostVars.Add("amount", amount);
            string md5 = HuobiBase.Sign(PostVars);
            PostVars.Remove("secret_key");
            PostVars.Add("sign", md5);
            PostVars.Add("trade_password", tradePassword);
            PostVars.Add("trade_id", tradeid);
            return HuobiBase.Post(PostVars);
        }

        /// <summary>
        /// 市价卖出
        /// </summary>
        /// <param name="coinType"></param>
        /// <param name="amount"></param>
        /// <param name="tradePassword"></param>
        /// <param name="tradeid"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public string sellMarket(string coinType, string amount, string tradePassword, string tradeid)
        {
            NameValueCollection PostVars = new NameValueCollection();
            PostVars.Add("method", HuobiBase.SELL_MARKET);
            PostVars.Add("created", HuobiBase.GetTimestamp());
            PostVars.Add("access_key", HuobiBase.HUOBI_ACCESS_KEY);
            PostVars.Add("secret_key", HuobiBase.HUOBI_SECRET_KEY);
            PostVars.Add("coin_type", coinType);
            PostVars.Add("amount", amount);
            string md5 = HuobiBase.Sign(PostVars);
            PostVars.Remove("secret_key");
            PostVars.Add("sign", md5);
            PostVars.Add("trade_password", tradePassword);
            PostVars.Add("trade_id", tradeid);
            return HuobiBase.Post(PostVars);
        }
    }
}