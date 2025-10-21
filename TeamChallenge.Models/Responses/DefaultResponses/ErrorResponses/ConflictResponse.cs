namespace TeamChallenge.Models.Responses
{
    public class ConflictResponse : ErrorResponse
    {
        public ConflictResponse(string errorMessage) : base(errorMessage)
        {
            StatusCode = 409;
        }
    }
}
