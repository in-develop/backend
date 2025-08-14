namespace TeamChallenge.Models.Responses
{
    public abstract class BaseDataListResponse<T> : BaseResponse, IDataListResponse<T>
    {
        public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();
        public BaseDataListResponse(IEnumerable<T> data)
        {
            Data = data;
        }
    }
}
