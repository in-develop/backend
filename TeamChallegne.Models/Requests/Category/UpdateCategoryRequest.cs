using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Requests.Category
{
    public class UpdateCategoryRequest
    {
        [Required, StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
    }
}
