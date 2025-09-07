using TeamChallenge.Models.Responses.SubCategoryResponses;

namespace TeamChallenge.Models.Responses.CategoryResponses
{
    public class CategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SubCategoryResponse> SubCategories { get; set; }
    }
}
