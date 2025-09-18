namespace TeamChallenge.Models.Models.Responses
{
    public class CategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SubCategoryResponse.SubCategoryResponse> SubCategories { get; set; }
    }

}
