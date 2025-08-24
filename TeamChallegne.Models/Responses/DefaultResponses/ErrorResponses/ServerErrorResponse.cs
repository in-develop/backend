namespace TeamChallenge.Models.Responses
{
    public class ServerErrorResponse : ErrorResponse
    {
        public ServerErrorResponse(string errorMessage) : base(errorMessage)
        {
            StatusCode = 500;
        }
    }
}
