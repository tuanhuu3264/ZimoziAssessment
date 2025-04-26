namespace Zimozi.Assessment.BusinessModels.ResponseModels
{
    public interface IBusinessResult<T> where T : class
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public T Data { get; set; }

    }

    public class BusinessResult<T> : IBusinessResult<T> where T : class
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public T Data { get; set; }
        public BusinessResult(string Message, int StatusCode, T Data) 
        {
            this.Message = Message;
            this.StatusCode = StatusCode;
            this.Data = Data;
        }
        public BusinessResult()
        {

        }
    }
}
