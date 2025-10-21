namespace TeamChallenge.Models.Responses
{
    public class ForbiddenResponse : ErrorResponse
    {
        public ForbiddenResponse(string errorMessage) : base(errorMessage)
        {
            StatusCode = 403;
        }
    }
}
