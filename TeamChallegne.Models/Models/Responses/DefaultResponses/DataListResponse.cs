
namespace TeamChallenge.Models.Responses
{
    public class DataListResponse<T> : BaseDataListResponse<T>
    {
        public DataListResponse(IEnumerable<T> data) : base(data)
        {
        }
    }
}
