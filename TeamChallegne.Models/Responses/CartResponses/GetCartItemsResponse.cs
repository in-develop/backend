namespace TeamChallenge.Models.Responses
{
    public class GetCartItemsResponse : BaseDataListResponse<GetCartItemsResponseModel>
    {
        public GetCartItemsResponse(IEnumerable<GetCartItemsResponseModel> data) : base(data)
        {
        }
    }
}
