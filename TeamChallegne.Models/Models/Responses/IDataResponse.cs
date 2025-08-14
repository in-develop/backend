namespace TeamChallenge.Models.Responses
{
    public interface IDataResponse<T> : IResponse
    {
        T Data { get; set; }
    }
}
