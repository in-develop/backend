using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Logic
{
    public interface IImageLogic
    {
        Task<IResponse> UploadImageAsync(CreateImageRequest dto);
        Task<IResponse> UpdateImageAsync(UpdateImageRequest dto);
        Task<IResponse> DeleteImageAsync(int id);
        Task<IResponse> GetImageAsync(int id);
        Task<IResponse> GetAllImagesAsync();
        Task<IResponse> GetImagesByProductIdAsync(int productId);

    }
}
