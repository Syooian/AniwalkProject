namespace AniwalkServer.Data
{
    public class Result
    {
        /// <summary>
        /// 回傳結果
        /// </summary>
        public ResultType Type { get; set; }
        /// <summary>
        /// 回傳訊息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="Message"></param>
        public Result(ResultType Type = ResultType.Success, string Message = "")
        {
            this.Type = Type;
            this.Message = Message;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ResultType
    {
        Success = 0,
        Fail
    }
}
