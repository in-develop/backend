using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Requests
{
    public class UpdateImageRequest
    {
        public int Id { get; set; }
        [Required]
        public IFormFile? ImageFile { get; set; }
    }
}
