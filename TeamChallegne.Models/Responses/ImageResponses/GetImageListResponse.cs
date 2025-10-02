using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses
{
    public class GetImageListResponse : BaseDataListResponse<ImageEntity>
    {
        public GetImageListResponse(IEnumerable<ImageEntity> data) : base(data)
        {
        }
    }
}
