namespace TeamChallenge.Models.Responses
{

    public abstract class BaseDataResponse<T> : BaseResponse, IDataResponse<T>
    {
        public T Data { get; set; }
        public BaseDataResponse(T data)
        {
            Data = data;
        }
    }
}
