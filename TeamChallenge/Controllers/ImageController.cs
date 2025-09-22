using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Controllers
{
    [Route("api/images")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageLogic _imageLogic;
        public ImageController(IImageLogic imageLogic)
        {
            _imageLogic = imageLogic;
        }
        [HttpGet]
        public async Task<IResponse> GetAllImagesAsync()
        {
            var response = await _imageLogic.GetAllImagesAsync();
            return response;
        }

        [HttpGet("product/{id}")]
        public async Task<IResponse> GetImagesByProductIdAsync(int id)
        {
            var response = await _imageLogic.GetImagesByProductIdAsync(id);
            return response;
        }

        [HttpGet("{id}")]
        public async Task<IResponse> GetImageAsync(int id)
        {
            var response = await _imageLogic.GetImageAsync(id);
            return response;
        }

        [HttpPost]
        public async Task<IResponse> AddImageAsync([FromForm] CreateImageRequest dto)
        {
            var response = await _imageLogic.UploadImageAsync(dto);
            return response;
        }

        [HttpPut]
        public async Task<IResponse> UpdateImageAsync([FromForm] UpdateImageRequest dto)
        {
            var response = await _imageLogic.UpdateImageAsync(dto);
            return response;
        } 

        [HttpDelete("{id}")]
        public async Task<IResponse> DeleteImageAsync(int id)
        {
            var response = await _imageLogic.DeleteImageAsync(id);
            return response;
        }
    }
}
