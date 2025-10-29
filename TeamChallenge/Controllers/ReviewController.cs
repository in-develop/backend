using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamChallenge.Logic;
using TeamChallenge.Models.Requests;
using TeamChallenge.Models.Responses;

namespace TeamChallenge.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/reviews")]
    public class ReviewController(IReviewLogic reviewLogic) : ControllerBase
    {
        [HttpGet]
        public async Task<IResponse> GetAll()
        {
            return await reviewLogic.GetAllReviewsAsync();
        }

        [HttpGet("{id}")]
        public async Task<IResponse> GetById([FromRoute] int id)
        {
            return await reviewLogic.GetReviewByIdAsync(id);
        }

        [HttpPost]
        public async Task<IResponse> Create([FromBody] CreateReviewRequest requestData)
        {
            return await reviewLogic.CreateReviewAsync(requestData);
        }

        [HttpPut("{id}")]
        public async Task<IResponse> Update([FromRoute] int id, [FromBody] UpdateReviewRequest requestData)
        {
            return await reviewLogic.UpdateReviewAsync(id, requestData);
        }

        [HttpDelete("{id}")]
        public async Task<IResponse> Delete([FromRoute] int id)
        {
            return await reviewLogic.DeleteReviewAsync(id);
        }
    }
}
