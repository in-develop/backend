using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Requests
{
    public class UpdateCategoryRequest
    {
        [Required, StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        public List<int> SubCategoryIds { get; set; } = new List<int>();
    }
}
