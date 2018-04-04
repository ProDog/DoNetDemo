using System.Data;
using CommService.Common;
using CommService.Model;

namespace CommService.Db
{
    public class UserRightsProvider
    {
        //响应对象格式
        public static RspInfoModel Res = new RspInfoModel();
        //用户信息
        public static string GetUserInfo(string account)
        {
            Res.ErrInfo = ErrorHelper.ErrNone;
            Res.RspInfo = null;
            DataTable table = new DataTable();
            string sql = "select * from table";
            try
            {
                table = DbHelper.ExecuteDbQueryDt(sql);
                if (table.Rows.Count == 0)
                    Res.ErrInfo = ErrorHelper.ErrNullData;
            }
            catch
            {
                Res.ErrInfo = ErrorHelper.ErrDbError;
            }
            if (table?.Rows.Count > 0)
            {
                Res.RspInfo = ConverToJson.DataTableToList(table);
            }

            return ConverToJson.RspInfoToJson(Res);
        }

        
        public static string GetAccRights(string account)
        {
            Res.ErrInfo = ErrorHelper.ErrNone;
            Res.RspInfo = null;
            DataTable table = null;
            string sql = "select * from table";
                    
            try
            {
                table = DbHelper.ExecuteDbQueryDt(sql);
                if (table.Rows.Count == 0 || string.IsNullOrEmpty(table.Rows[0][0].ToString()))
                    Res.ErrInfo = ErrorHelper.ErrNullData;
            }
            catch
            {
                Res.ErrInfo = ErrorHelper.ErrDbError;
            }
            if (table?.Rows.Count > 0 && !string.IsNullOrEmpty(table.Rows[0][0].ToString()))
            {
                Res.RspInfo = ConverToJson.DataTableToList(table);
            }
            return ConverToJson.RspInfoToJson(Res);
        }

        public static string GetAppRights(string account)
        {
            Res.ErrInfo = ErrorHelper.ErrNone;
            Res.RspInfo = null;
            DataTable table = null;
            string sql = "select * from table";
            try
            {
                table = DbHelper.ExecuteDbQueryDt(sql);
                if (table.Rows.Count == 0 || string.IsNullOrEmpty(table.Rows[0][0].ToString()))
                    Res.ErrInfo = ErrorHelper.ErrNullData;
            }
            catch
            {
                Res.ErrInfo = ErrorHelper.ErrDbError;
            }
            if (table?.Rows.Count > 0 && !string.IsNullOrEmpty(table.Rows[0][0].ToString()))
            {
                Res.RspInfo = ConverToJson.DataTableToList(table);
            }
            return ConverToJson.RspInfoToJson(Res);
        }

    }
}
