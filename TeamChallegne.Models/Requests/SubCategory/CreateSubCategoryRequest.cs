using System.ComponentModel.DataAnnotations;

namespace TeamChallenge.Models.Requests.SubCategory
{
    public class CreateSubCategoryRequest
    {
        [Required, StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public List<int> ProductIds { get; set; } = new List<int>();
    }
}
