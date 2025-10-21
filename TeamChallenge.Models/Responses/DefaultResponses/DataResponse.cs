namespace TeamChallenge.Models.Responses
{
    public class DataResponse<T> : BaseDataResponse<T>
    {
        public DataResponse(T data) : base(data)
        {
        }
    }
}
