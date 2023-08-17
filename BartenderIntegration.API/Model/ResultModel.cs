namespace BartenderIntegration.API.Model
{
    public class ResultModel<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ResultModel(bool isSuccess, string message, T data)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public ResultModel(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public ResultModel(bool isSuccess, T data)
        {
            IsSuccess = isSuccess;
            Data = data;
        }
    }
}
