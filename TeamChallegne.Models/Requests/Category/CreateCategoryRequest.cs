using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Requests.Category
{
    public class CreateCategoryRequest
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
        public string Name { get; set; }
        public List<int>? SubCategoryIds { get; set; }
    }
}
