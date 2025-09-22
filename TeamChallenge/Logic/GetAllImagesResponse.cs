using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Logic
{
    public class GetAllImagesResponse : DataListResponse<ImageEntity>
    {
        public GetAllImagesResponse(IEnumerable<ImageEntity> data) : base(data)
        {
        }
    }
}