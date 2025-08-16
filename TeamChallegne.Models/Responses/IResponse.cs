namespace TeamChallenge.Models.Responses
{
    public interface IResponse
    {
        bool IsSuccess { get; set; }
        string Message { get; set; }
    }
}
