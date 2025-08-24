namespace TeamChallenge.Models.Responses
{
    public abstract class BaseResponse : IResponse
    {
        public int StatusCode { get; set; } = 200;
        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = string.Empty;

        public BaseResponse()
        {
            
        }

        public BaseResponse(string message)
        {
            Message = message;
        }

    }
}
