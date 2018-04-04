namespace CommService.Model
{
    public static class ErrorHelper
    {
        public static ErrorInfo ErrNone = new ErrorInfo { ErrorId = 0, ErrorMsg = "OK" };
        public static ErrorInfo ErrNullData = new ErrorInfo { ErrorId = 1, ErrorMsg = "数据为空" };
        public static ErrorInfo ErrDbError = new ErrorInfo { ErrorId = 2, ErrorMsg = "数据库执行错误" };
        public static ErrorInfo ErrJsonConvert = new ErrorInfo { ErrorId = 3, ErrorMsg = "数据格式转换错误" };
        public static ErrorInfo ErrNullAccount = new ErrorInfo { ErrorId = 4, ErrorMsg = "传入参数为空" };
        public static ErrorInfo ErrGetFundid = new ErrorInfo { ErrorId = 5, ErrorMsg = "获取FUNDID出错" };
        public static ErrorInfo ErrDealApprights = new ErrorInfo { ErrorId = 6, ErrorMsg = "产品权限处理过程出错" };
    }
}