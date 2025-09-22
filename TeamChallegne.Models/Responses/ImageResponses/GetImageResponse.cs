using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses
{
    public class GetImageResponse : BaseDataResponse<ImageEntity>
    {
        public GetImageResponse(ImageEntity data) : base(data)
        {
        }
    }
}
