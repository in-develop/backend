namespace TeamChallenge.Models.Responses
{
    public class BadRequestResponse : ErrorResponse
    {
        public BadRequestResponse(string errorMessage) : base(errorMessage)
        {
            StatusCode = 400;
        }
    }
}
