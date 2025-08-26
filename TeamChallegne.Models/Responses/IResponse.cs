namespace TeamChallenge.Models.Responses
{
    public interface IResponse
    {
        int StatusCode { get; set; }
        bool IsSuccess { get; set; }
        string Message { get; set; }
    }
}
