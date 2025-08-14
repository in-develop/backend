namespace TeamChallenge.Models.Responses
{
    public class ErrorResponse : BaseResponse
    {
        public ErrorResponse(string errorMessage)
        {
            Message = errorMessage;
            IsSuccess = false;
        }
    }
}
