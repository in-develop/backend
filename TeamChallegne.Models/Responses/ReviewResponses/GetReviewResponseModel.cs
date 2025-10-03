using System.Text.Json.Serialization;
using TeamChallenge.Models.Entities;

namespace TeamChallenge.Models.Responses
{
    public class GetReviewResponseModel
    {
        [JsonConstructor]
        public GetReviewResponseModel()
        {
            
        }
        public GetReviewResponseModel(ReviewEntity entity)
        {
            Id = entity.Id;
            UserId = entity.UserId;
            ProductId = entity.ProductId;
            Rating = entity.Rating;
            Comment = entity.Comment;
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
