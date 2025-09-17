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
    public class ReviewController : ControllerBase
    {
        private readonly IReviewLogic _reviewLogic;
        public ReviewController(IReviewLogic reviewLogic)
        {
            _reviewLogic = reviewLogic;
        }

        [HttpGet]
        public async Task<IResponse> GetAll()
        {
            return await _reviewLogic.GetAllReviewsAsync();
        }

        [HttpGet("{id}")]
        public async Task<IResponse> GetById([FromRoute] int id)
        {
            return await _reviewLogic.GetReviewByIdAsync(id);
        }

        [HttpPost]
        public async Task<IResponse> Create([FromBody] CreateReviewRequest requestData)
        {
            return await _reviewLogic.CreateReviewAsync(requestData);
        }

        [HttpPut("{id}")]
        public async Task<IResponse> Update([FromRoute] int id, [FromBody] UpdateReviewRequest requestData)
        {
            return await _reviewLogic.UpdateReviewAsync(id, requestData);
        }

        [HttpDelete("{id}")]
        public async Task<IResponse> Delete([FromRoute] int id)
        {
            return await _reviewLogic.DeleteReviewAsync(id);
        }
    }
}
