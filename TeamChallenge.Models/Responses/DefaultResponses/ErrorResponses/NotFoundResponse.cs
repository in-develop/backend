namespace TeamChallenge.Models.Responses
{
    public class NotFoundResponse : ErrorResponse
    {
        public NotFoundResponse(string errorMessage) : base(errorMessage)
        {
            StatusCode = 404;
        }
    }
}
