using Microsoft.AspNetCore.Http;

namespace TeamChallenge.Models.Requests
{
    public class CreateImageRequest
    {
        public int ProductId { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
