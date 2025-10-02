using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses
{
    public class GetImagesResponse: BaseDataResponse<ImageEntity>
    {
        public GetImagesResponse(ImageEntity data) : base(data)
        {
        }

        public string? Base64Image { get; set; }
        public int ProductId { get; set; }
    }
}
