using System.ComponentModel.DataAnnotations;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Requests.Category
{
    public class CreateCategoryRequest
    {
        [Required, StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
        public IEnumerable<ProductEntity> Products { get; set; }
    }
}
