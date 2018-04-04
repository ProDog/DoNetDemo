using System.Collections.Generic;
using System.Data;
using System.Web.Script.Serialization;
using CommService.Model;

namespace CommService.Common
{
    public class ConverToJson
    {
        public static string DataTableToJson(DataTable dt)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            foreach (DataRow row in dt.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    if (row[col].ToString() == ""||row[col]==null)
                        row[col] = "";
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }
            return jsSerializer.Serialize(parentRow);
        }

        public static string RspInfoToJson(RspInfoModel Rsp)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow = new Dictionary<string, object>();
            childRow.Add("ErrorId", Rsp.ErrInfo.ErrorId);
            childRow.Add("ErrorMsg", Rsp.ErrInfo.ErrorMsg);
            childRow.Add("Data",Rsp.RspInfo);
            parentRow.Add(childRow);
            return jsSerializer.Serialize(childRow);
        }

        public static List<Dictionary<string, object>> DataTableToList(DataTable dt)
        {
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            foreach (DataRow row in dt.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    if (row[col].ToString() == "" || row[col] == null)
                        row[col] = "";
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }
            return parentRow;
        }

        public static string DataSetToJson(DataSet ds)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            foreach (DataTable dt in ds.Tables)
            {
                DataRow row = dt.Rows[0];
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }
            return jsSerializer.Serialize(parentRow);
        }
    }
}
