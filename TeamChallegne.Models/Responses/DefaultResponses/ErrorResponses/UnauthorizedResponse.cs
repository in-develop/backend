namespace TeamChallenge.Models.Responses
{
    public class UnauthorizedResponse : ErrorResponse
    {
        public UnauthorizedResponse(string errorMessage) : base(errorMessage)
        {
            StatusCode = 401;
        }
    }
}
