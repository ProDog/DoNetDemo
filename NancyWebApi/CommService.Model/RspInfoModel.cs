namespace CommService.Model
{
    public class RspInfoModel
    {
        public ErrorInfo ErrInfo { get; set; }
        public dynamic RspInfo { get; set; }

    }

    public class ErrorInfo
    {
        private int _msgMaxLength = 512;

        public ErrorInfo()
        {

        }

        public int ErrorId { get; set; }

        private string _errorMsg;

        public string ErrorMsg
        {
            get { return _errorMsg; }
            set
            {
                _errorMsg = value;
                if (_errorMsg.Length > _msgMaxLength)
                {
                    _errorMsg = _errorMsg.Substring(0, _msgMaxLength);
                }
            }
        }

        public ErrorInfo(int errorId, string errorMsg)
        {
            ErrorId = errorId;
            ErrorMsg = errorMsg;
        }
    }
}
