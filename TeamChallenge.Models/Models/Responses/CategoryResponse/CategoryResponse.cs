using TeamChallenge.Models.Entities;
using TeamChallenge.Models.Responses;
using TeamChallenge.Models.Responses.SubCategoryResponses;

namespace TeamChallenge.Models.Models.Responses
{
    public class CategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SubCategoryResponse.SubCategoryResponse> SubCategories { get; set; }
    }

}
